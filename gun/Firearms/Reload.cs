using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Kingmaker.Blueprints.BlueprintUnit;

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

    public class ReloadEffect : ContextAction
    {

        public static BlueprintBuff Buff = BlueprintTool.Get<BlueprintBuff>(BaseFirearm.RoundsGUID);
        public override string GetCaption()
        {
            return "Reloading";
        }

        public override void RunAction()
        {
            MechanicsContext mechanicsContext = ContextData<MechanicsContext.Data>.Current?.Context;
            if (mechanicsContext == null)
            {
                PFLog.Default.Error(this, "Unable to apply buff: no context found");
                return;
            }

            UnitEntityData buffTarget = mechanicsContext.MaybeCaster;
            if (buffTarget == null)
            {
                PFLog.Default.Error(this, "Can't apply buff: target is null");
                return;
            }
            Buff CurrentAmmo = buffTarget.Descriptor.Buffs.GetBuff(Buff);

            int cap = checkCapacity(buffTarget);
            bool Advanced = isAdvanced(buffTarget);
            if (CurrentAmmo != null)
            {//if there is already a round in the clip
                if (Advanced)//and its an advanced weapon
                {
                    CurrentAmmo.Rank = cap;//set the clip to its maximum capacity
                }
                else//if the is a round and its an early firearm
                {
                    if (CurrentAmmo.Rank < cap)//if there is still room for more rounds
                    {
                        CurrentAmmo.Rank++;//add one round to the clip
                    }
                    //otherwise do nothing
                }
            }
            else
            {
                CurrentAmmo = buffTarget.Descriptor.AddBuff(Buff, mechanicsContext, null);
                CurrentAmmo.IsFromSpell = false;
                CurrentAmmo.IsNotDispelable = true;
                if (Advanced)//if its an advanced weapon
                {
                    CurrentAmmo.Rank = cap;//set the clip to its maximum capacity
                }
            }
               
        }

        public static bool isAdvanced(UnitEntityData unit)
        {
            ItemEntity mainhand = unit.Body.PrimaryHand.MaybeWeapon;
            if (mainhand != null)
            {
                return mainhand.Enchantments.Any((ItemEnchantment enhancement) =>
                {
                    return enhancement.Blueprint == BlueprintTool.Get<BlueprintItemEnchantment>(BaseFirearm.AdvancedClipGUID);
                }
                );
            }
            ItemEntity offhand = unit.Body.SecondaryHand.MaybeWeapon;
            if (offhand != null && offhand != mainhand)
            {
                return offhand.Enchantments.Any((ItemEnchantment enhancement) =>
                {
                    return enhancement.Blueprint == BlueprintTool.Get<BlueprintItemEnchantment>(BaseFirearm.AdvancedClipGUID);
                }
                );
            }
            return false;
        }
        public static int checkCapacity(UnitEntityData unit)
        {
            int count = 0;
            ItemEntity mainhand = unit.Body.PrimaryHand.MaybeWeapon;
            if (mainhand != null)
            {
                mainhand.Enchantments.ForEach((ItemEnchantment enhancement) =>
                {
                    if (enhancement.GetComponent<Capacity>() != null)
                    {//if it has a capacity enhancement
                        count += enhancement.GetComponent<Capacity>().count;
                    }
                }
                );
            }
            ItemEntity offhand = unit.Body.SecondaryHand.MaybeWeapon;
            if (offhand != null && offhand != mainhand)
            {
                offhand.Enchantments.ForEach((ItemEnchantment enhancement) =>
                {
                    if (enhancement.GetComponent<Capacity>() != null)
                    {//if it has a capacity enhancement
                        count += enhancement.GetComponent<Capacity>().count;
                    }
                }
                );
            }
            return count;
        }
    }

    public static class ReloadSpeedCalc 
    {

        public static int Calculate (UnitEntityData unit)
        {
            bool isAdvanced = unit.GetFirstWeapon().GetEnchantment(BlueprintTool.Get<BlueprintItemEnchantment>(BaseFirearm.AdvancedClipGUID)) != null;
            //bool isEarly = unit.GetFirstWeapon().GetEnchantment(BlueprintTool.Get<BlueprintItemEnchantment>(BaseFirearm.ClipGUID)) != null;
            bool hasRapidReload = unit.GetFeature(BlueprintTool.Get<BlueprintFeature>(RapidReload.RapidReloadGUID)) != null;
            //Main.Log.Log("Adding Reload");
            if (isAdvanced)
            {
                //Main.Log.Log("Advanced Weapon");
                if (!hasRapidReload)
                {
                    return 0;
                    //unit.AddFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadAdvancedGUID));//then its a move action to reload
                    //Main.Log.Log("Not Rapid");
                }

            }
            else
            {
                //Main.Log.Log("Early Weapon");
                bool TwoHanded = unit.GetFirstWeapon().Blueprint.IsTwoHanded;


                if (!TwoHanded && hasRapidReload)
                {//if its a one handed weapon with rapid reload
                    return 1;
                    //unit.AddFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadMoveGUID));//then its a move action to reload
                    //Main.Log.Log("Reload (Move)");
                }
                else if (TwoHanded == hasRapidReload)
                {//(!TwoHanded && !hasRapidReload) || (TwoHanded && hasRapidReload)
                    //if its a one handed weapon without rapid reload or a two handed weapoin with it
                    //Main.Log.Log("Reload (Standard)");
                    return 2;
                   // unit.AddFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadStandardGUID));//its a standard action
                }
                else
                {
                    return 3;
                    //Main.Log.Log("Reload (Full Round)");
                    //unit.AddFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadFullRoundGUID));//its a full round action
                }
            }
            return -1;//reload free
        }
        public static void Update (UnitEntityData unit)
        {
            switch (Calculate(unit)) {
                case 0://reload advanced
                    unit.AddFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadAdvancedGUID));
                    break;
                case 1://reload move
                    unit.AddFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadMoveGUID));
                    break;
                case 2://reload standard
                    unit.AddFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadStandardGUID));
                    break;
                case 3://reload Full Round
                    unit.AddFact(BlueprintTool.Get<BlueprintAbility>(BaseFirearm.ReloadFullRoundGUID));
                    break;
                case -1:
                    Clear(unit);
                    break;

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
