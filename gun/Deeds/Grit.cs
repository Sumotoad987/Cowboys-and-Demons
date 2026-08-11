using BepInEx.AssemblyPublicizer;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using gun.Classes.Gunslinger;
using gun.Firearms;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Newtonsoft.Json;
using Owlcat.QA.Validation;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace gun.Deeds
{
    internal static class Grit
    {
        public const string GritResourceGUID = "121f2748b31245a49c4da692a38f2c43";
        public const string GritFeatureGUID = "b6472099ad7d43f3a270dc441b6c9c55";
        public const string ChaGritFeatureGUID = "cc6fd36c2f7c4e01a7675fba10df9871";
        public const string TrueGritFeatureGUID = "06594d741aff436ba6af6fc65f9c593b";
        public static BlueprintAbilityResourceReference GritResource;
        public static BlueprintFeature gritFeature;
        public static void Configure()
        {
            AbilityResourceConfigurator.New("GritResource", GritResourceGUID)
                .SetLocalizedName(LocalizationTool.GetString("Grit.Name"))
                .SetMaxAmount(ResourceAmountBuilder.New(0).IncreaseByStat(Kingmaker.EntitySystem.Stats.StatType.Wisdom))
                .SetMin(0)
                .SetUseMax(true)
                .Configure();

            GritResource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(GritResourceGUID);

            FeatureConfigurator.New("Grit", GritFeatureGUID)//looked at the swashbuckler mod to get a sense of what was needed here
                .SetDisplayName(LocalizationTool.GetString("Grit.Name"))
                .SetDescription(LocalizationTool.GetString("Grit.Description"))
                .SetDescriptionShort(LocalizationTool.GetString("Grit.Description.Short"))
                //.SetIcon(GetIcon())
                .AddAbilityResources(resource: GritResource, restoreAmount:true)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(Kingmaker.EntitySystem.Stats.StatType.Wisdom,min:1))
                .AddIncreaseResourceAmountBySharedValue(resource: GritResource, value: ContextValues.Rank())
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RestoreResource(GritResource, value: 1), actionsOnInitiator: true, criticalHit: true, category: BaseFirearm.FirearmCategory)//firearms will all use the heavy crossbow category for now
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RestoreResource(GritResource, value: 1), actionsOnInitiator: true, reduceHPToZero: true, category: BaseFirearm.FirearmCategory)//firearms will all use the heavy crossbow category for now
                .SetIsClassFeature(true)
                .Configure();

            gritFeature = BlueprintTool.Get<BlueprintFeature>(GritFeatureGUID);

            ActionsBuilder recoverGrit = ActionsBuilder.New();
            recoverGrit.RestoreResource(GritResource, value: 1);


            FeatureConfigurator.New("TrueGrit", TrueGritFeatureGUID)
                .SetDisplayName(LocalizationTool.GetString("TrueGrit.Name"))
                .SetDescription(LocalizationTool.GetString("TrueGrit.Description"))
                //.SetIcon(GetIcon())
                .AddAbilityResourceTrigger(recoverGrit, resource: GritResource)
                .Configure()
                ;
            ChaGrit();
        }

        private static void ChaGrit()
        {

            FeatureConfigurator.New("ChaGrit", ChaGritFeatureGUID)//looked at the swashbuckler mod to get a sense of what was needed here
                .SetDisplayName(LocalizationTool.GetString("Grit.Name"))
                .SetDescription(LocalizationTool.GetString("Grit.Cha.Description"))
                .SetDescriptionShort(LocalizationTool.GetString("Grit.Cha.Description.Short"))
                .AddAbilityResources(resource: GritResource, restoreAmount: true)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(Kingmaker.EntitySystem.Stats.StatType.Charisma, min: 1))
                .AddIncreaseResourceAmountBySharedValue(resource: GritResource, value: ContextValues.Rank())
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RestoreResource(GritResource, value: 1), actionsOnInitiator: true, criticalHit: true, category: BaseFirearm.FirearmCategory)//firearms will all use the heavy crossbow category for now
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RestoreResource(GritResource, value: 1), actionsOnInitiator: true, reduceHPToZero: true, category: BaseFirearm.FirearmCategory)//firearms will all use the heavy crossbow category for now
                .SetIsClassFeature(true)
                .Configure();
        }
    }

    //Will need a class here like the light armor unlock with can grant features if grit is at least 1
    [ComponentName("Add feature if owner has grit")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("5615a454-981d-483a-8b52-ac0674a585e7")]
    public class GritUnlock : UnitFactComponentDelegate<LightArmorUnlockData>, IGlobalSubscriber, ISubscriber
    {//this is the system copied from monks no armor setup not sure it will work properly here since I don't know how to get it to update when a resource is spent
        [SerializeField]
        [FormerlySerializedAs("NewFact")]
        public BlueprintUnitFactReference m_NewFact;

        public BlueprintUnitFact NewFact => m_NewFact?.Get();

        public override void OnActivate()
        {
            CheckEligibility();
        }

        public override void OnDeactivate()
        {
            RemoveFact();
        }

        public void UpdateBuffStateIfCorrectUnit(UnitEntityData unit)
        {
            if (!(unit != Owner))
            {
                CheckEligibility();
            }
        }

        public void CheckEligibility()
        {//this is the only fact of the class I changed
            if (Owner.Resources.HasEnoughResource(Grit.GritResource,1))
            {
                AddFact();
            }
            else
            {
                RemoveFact();
            }
        }

        public void AddFact()
        {
            if (Data.AppliedFact == null)
            {
                Data.AppliedFact = Owner.AddFact(NewFact);
            }
        }
        public void RemoveFact()
        {
            if (Data.AppliedFact != null)
            {
                Owner.RemoveFact(Data.AppliedFact);
                Data.AppliedFact = null;
            }
        }
    }

    [ComponentName("Condition/HasGrit")]
    [AllowMultipleComponents]
    [TypeId("e7834349f2324baf8ea9af2f4d5eff3e")]
    public class HasGrit : Condition
    {
        public HasGrit(int quantity)
        {
            Quantity = quantity;
        }
        [ValidateNotNull]
        [SerializeReference]
        public UnitEvaluator Target;

        [ValidateNotNull]
        [SerializeField]
        public int Quantity = 1;
        public override string GetConditionCaption()
        {
            return $"Has Grit";
        }
        public override bool CheckCondition()
        {
            if (!Target || !Target.TryGetValue(out var value))
            {
                return false;
            }
            return value.Resources.HasEnoughResource(Grit.GritResource, Quantity);
        }
    }
}
