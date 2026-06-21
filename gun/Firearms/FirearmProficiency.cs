using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gun.Firearms
{
    internal static class FirearmProficiency
    {
        public const string FirearmProficiencyGUID = "5af8e018e2f34276a1c421de80674ff0";
        public static void Configure()
        {
            Sprite icon = BlueprintTool.Get<BlueprintFeature>("203992ef5b35c864390b4e4a1e200629").Icon;//steal the icon from martial weapon proficiency

            FeatureConfigurator.New("WeaponProficencyFirearms", FirearmProficiencyGUID, new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat })
                .SetDescription(LocalizationTool.GetString("Feats.FirearmProficiency.Description"))
                .SetDisplayName(LocalizationTool.GetString("Feats.FirearmProficiency.Name"))
                .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.BaseAttackBonus, 1)
                .AddProficiencies(weaponProficiencies:new Kingmaker.Enums.WeaponCategory[]{ BaseFirearm.FirearmCategory})//hope this works
                .SetIcon(icon)
                .Configure();
        }
    }

    [AllowMultipleComponents]
    [TypeId("3229cdc69f0b421098fe4755ed528d6d")]
    public class EquipmentRestrictionFirearm : EquipmentRestriction
    {

        public int MinValue;

        public override bool CanBeEquippedBy(UnitDescriptor unit)
        {
            return unit.GetFeature(BlueprintTool.Get<BlueprintFeature>(FirearmProficiency.FirearmProficiencyGUID)) != null;
        }
    }
}
