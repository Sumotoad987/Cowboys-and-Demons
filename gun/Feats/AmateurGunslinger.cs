using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using gun.Firearms;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gun.Feats
{
    internal static class AmateurGunslinger
    {
        public static string BaseGUID = "b8f2b2563237487e8ac892d0e9566d8b";
        public static string DrifterGUID = "9f4718bc57354bbe8143b246514bc555";
        public static void Configure()
        {
            FeatureConfigurator.New("AmateurGunslinger", BaseGUID, new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat })
                .SetDisplayName(LocalizationTool.GetString("Feats.AmateurGunslinger.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.AmateurGunslinger.Description"))
                .SetDescriptionShort(LocalizationTool.GetString("Feats.AmateurGunslinger.Description.Short"))
                .AddAbilityResources(resource: Deeds.Grit.GritResource, restoreAmount: true)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(Kingmaker.EntitySystem.Stats.StatType.Wisdom, min: 1))
                .AddIncreaseResourceAmountBySharedValue(resource: Deeds.Grit.GritResource, value: ContextValues.Rank())
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RestoreResource(Deeds.Grit.GritResource, value: 1), actionsOnInitiator: true, criticalHit: true, category: BaseFirearm.FirearmCategory)//firearms will all use the heavy crossbow category for now
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RestoreResource(Deeds.Grit.GritResource, value: 1), actionsOnInitiator: true, reduceHPToZero: true, category: BaseFirearm.FirearmCategory)//firearms will all use the heavy crossbow category for now
                .AddPrerequisiteNoFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(Deeds.Grit.GritFeatureGUID))
                .AddPrerequisiteNoFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(Deeds.Grit.ChaGritFeatureGUID))
                .AddPrerequisiteNoFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(DrifterGUID))
                .AddFeatureOnApply(BlueprintTool.GetRef<BlueprintFeatureReference>(Deeds.DeedConfigurator.QuickClearFeatureGUID))
                .Configure();

            FeatureConfigurator.New("AmateurGunslingerDrifter", DrifterGUID)
                .SetDisplayName(LocalizationTool.GetString("Feats.AmateurGunslinger.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.AmateurGunslinger.Drifter.Description"))
                .SetDescriptionShort(LocalizationTool.GetString("Feats.AmateurGunslinger.Description.Short"))
                .AddAbilityResources(resource: Deeds.Grit.GritResource, restoreAmount: true)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(Kingmaker.EntitySystem.Stats.StatType.Charisma, min: 1))
                .AddIncreaseResourceAmountBySharedValue(resource: Deeds.Grit.GritResource, value: ContextValues.Rank())
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RestoreResource(Deeds.Grit.GritResource, value: 1), actionsOnInitiator: true, criticalHit: true, category: BaseFirearm.FirearmCategory)//firearms will all use the heavy crossbow category for now
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RestoreResource(Deeds.Grit.GritResource, value: 1), actionsOnInitiator: true, reduceHPToZero: true, category: BaseFirearm.FirearmCategory)//firearms will all use the heavy crossbow category for now
                .AddPrerequisiteNoFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(Deeds.Grit.GritFeatureGUID))
                .AddFeatureOnApply(BlueprintTool.GetRef<BlueprintFeatureReference>(Deeds.DeedConfigurator.QuickClearFeatureGUID))
                .SetIsClassFeature(true)
                .Configure();
        }
    }
}
