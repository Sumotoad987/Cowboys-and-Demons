using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gun.Firearms
{
    public static class RapidReload
    {
        public const string RapidReloadGUID = "737cc264baee4d02a33921f099dd82da";
        public static void Configure()
        {
            //creates an Icon
            byte[] data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/Rapid Reload.png");
            Texture2D texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite icon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));

            FeatureConfigurator.New("RapidReload", RapidReloadGUID, new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat })
                .SetDescription(LocalizationTool.GetString("Feats.RapidReload.Description"))
                .SetDisplayName(LocalizationTool.GetString("Feats.RapidReload.Name"))
                .SetIcon(icon)
                .AddPrerequisiteFeature(FirearmProficiency.FirearmProficiencyGUID)
                .AddComponent(new CheckLoadingFeature())
                .Configure()
                ;
        }
    }
    public class HasRapidReload : BlueprintComponent, IAbilityCasterRestriction
    {
        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            return caster.GetFeature(BlueprintTool.Get<BlueprintFeature>(RapidReload.RapidReloadGUID)) != null;
        }

        public string GetAbilityCasterRestrictionUIText()
        {
            return "Requires Rapid Reload";
        }

    }

    public static class ReloadSpeedCalc 
    {
        public static void Update (UnitEntityData unit)
        {
            bool isAdvanced = unit.GetFirstWeapon().GetEnchantment(BlueprintTool.Get<BlueprintItemEnchantment>(BaseFirearm.AdvancedClipGUID)) != null;
            //bool isEarly = unit.GetFirstWeapon().GetEnchantment(BlueprintTool.Get<BlueprintItemEnchantment>(BaseFirearm.ClipGUID)) != null;
            bool hasRapidReload = unit.GetFeature(BlueprintTool.Get<BlueprintFeature>(RapidReload.RapidReloadGUID)) != null;
            Main.Log.Log("Adding Reload");
            if (isAdvanced)
            {
                Main.Log.Log("Advanced Weapon");
                if (!hasRapidReload)
                {
                    unit.AddFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadAdvancedGUID));//then its a move action to reload
                    Main.Log.Log("Not Rapid");
                }

            } else
            {
                Main.Log.Log("Early Weapon");
                bool TwoHanded = unit.GetFirstWeapon().Blueprint.IsTwoHanded;


                if (!TwoHanded && hasRapidReload)
                {//if its a one handed weapon with rapid reload

                    unit.AddFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadMoveGUID));//then its a move action to reload
                    Main.Log.Log("Reload (Move)");
                }
                else if (TwoHanded == hasRapidReload)
                {//(!TwoHanded && !hasRapidReload) || (TwoHanded && hasRapidReload)
                    //if its a one handed weapon without rapid reload or a two handed weapoin with it
                    Main.Log.Log("Reload (Standard)");
                    unit.AddFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadStandardGUID));//its a standard action
                }
                else
                {
                    Main.Log.Log("Reload (Full Round)");
                    unit.AddFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadFullRoundGUID));//its a full round action
                }
            }
        }

        public static void Clear (UnitEntityData unit)
        {
            unit.RemoveFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadAdvancedGUID));
            unit.RemoveFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadMoveGUID));
            unit.RemoveFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadStandardGUID));
            unit.RemoveFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadFullRoundGUID));

        }
    }

    public class CheckLoadingEquipment : ItemEnchantmentComponentDelegate<ItemEntity>
    {
        public override void OnActivate()
        {
            ReloadSpeedCalc.Update(base.Owner.Wielder);
        }


        public override void OnDeactivate()
        {
            ReloadSpeedCalc.Clear(base.Owner.Wielder);
        }
    }

    public class CheckLoadingFeature : UnitFactComponentDelegate
    {
        public override void OnActivate()
        {
            ReloadSpeedCalc.Update(base.Owner);
        }


        public override void OnDeactivate()
        {
            ReloadSpeedCalc.Clear(base.Owner);
        }
    }


}
