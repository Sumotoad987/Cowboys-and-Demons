using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils;
using gun.Deeds;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.Rendering.DebugUI;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder.StoryEx;
using BlueprintCore.Conditions.Builder.ContextEx;

namespace gun.Classes.Gunslinger
{
    internal static class Buccaneer
    {
        public const string SeaDogFeature = "c1b1bb40c6fa45beac3425d77f92bab5";
        public const string SeaDogAbility = "ebbcca9b890c4f22891c9451a7e3ce54";
        public const string SeaDogBuff = "f3a036f94efd4cac90a9b92580cd091f";
        public const string JargonFeature = "d90f8e526a1e476283d7e6eff7a2213f";
        public const string JargonBuff = "82a17b4d87024400b448cebe21687546";
        public const string JargonAbility = "490a9aef160e48bca8ae40d078a78e67";
        public const string GrogBuff = "f2ab92a0fbe84084b6d91abfacc89db1";
        public const string GrogAbility = "8575a215acaf4043906f03f8db4b5d4d";
        public const string GrogFeature = "0e99979b79b34457961c9db6e636e38b";
        public const string GUID = "2ce38ce1165d47de87dce2404e725e02";
        public static void Configure()
        {
            
            SeaDog();
            PirateJargon();
            Grog();
            ArchetypeConfigurator.New("GunslingerBuccaneer",GUID,Gunslinger.GunslingerClassGUID)
                .SetLocalizedName("Buccaneer.Name")
                .SetLocalizedDescription("Buccaneer.Description")
                .AddToRemoveFeatures(DefineRemoveFeatures())
                .AddToAddFeatures(DefineAddFeatures())
                .RemoveFromRecommendedAttributes(Kingmaker.EntitySystem.Stats.StatType.Wisdom)
                .AddToRecommendedAttributes(Kingmaker.EntitySystem.Stats.StatType.Constitution, Kingmaker.EntitySystem.Stats.StatType.Charisma)
                .Configure();
        }

        private static LevelEntry[] DefineRemoveFeatures()
        {
            LevelEntry level1 = new LevelEntry();
            level1.Level = 1;
            level1.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(DeedConfigurator.QuickClearFeatureGUID),
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(Grit.GritFeatureGUID)//get cha grit instead
            ];

            LevelEntry level2 = new LevelEntry();
            level2.Level = 2;
            level2.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(Nimble.NimbleUnlockGUID),
            ];

            LevelEntry level5 = new LevelEntry();
            level5.Level = 5;
            level5.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(GunTraining.GunTrainingGUID),//get this at level 13 instead
            ];

            return new LevelEntry[] { level1, level2, level5 };
        }

        private static LevelEntry[] DefineAddFeatures()
        {
            LevelEntry level1 = new LevelEntry();
            level1.Level = 1;
            level1.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(SeaDogFeature),
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(Grit.ChaGritFeatureGUID)
            ];

            LevelEntry level2 = new LevelEntry();
            level2.Level = 2;
            level2.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(GrogFeature),
            ];

            LevelEntry level3 = new LevelEntry();
            level3.Level = 3;
            level3.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(JargonFeature),
            ];

            LevelEntry level5 = new LevelEntry();
            level5.Level = 5;
            level5.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>("363cab72f77c47745bf3a8807074d183"),//get a familiar
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>("576933720c440aa4d8d42b0c54b77e80")//and evasion since the familairs distance from you is arbitrary
            ];

            LevelEntry level13 = new LevelEntry();
            level13.Level = 13;
            level13.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(GunTraining.GunTrainingGUID),
            ];

            return new LevelEntry[] { level1, level2, level3, level5, level13 };
        }
        private static void SeaDog()
        {
            BuffConfigurator.New("SeaDogBuff", SeaDogBuff)
                .SetDisplayName("Buccaneer.SeaDog.Name")
                .SetDescription("Buccaneer.SeaDog.Ability.Description")
                .SetIcon(Utilities.MakeIcon("SeaDog.png"))
                .AddConditionImmunity(UnitCondition.DifficultTerrain)
                .Configure();

            AbilityConfigurator.New("SeaDogAbility", SeaDogAbility)
                .SetDisplayName("Buccaneer.SeaDog.Name")
                .SetDescription("Buccaneer.SeaDog.Ability.Description")
                .SetIcon(Utilities.MakeIcon("SeaDog.png"))
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuffWithDurationSeconds(SeaDogBuff, 6))
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: Grit.GritResource)
                .Configure();

            FeatureConfigurator.New("SeaDogFeature", SeaDogFeature)
                .SetDisplayName("Buccaneer.SeaDog.Name")
                .SetDescription("Buccaneer.SeaDog.Description")
                .SetIcon(Utilities.MakeIcon("SeaDog.png"))
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, stat: Kingmaker.EntitySystem.Stats.StatType.SkillMobility)
                .AddFacts([SeaDogAbility])
                .Configure();
        }

        private static void PirateJargon()
        {
            BuffConfigurator.New("JargonBuff", JargonBuff)
                .SetDisplayName("Buccaneer.Jargon.Name")
                .SetDescription("Buccaneer.Jargon.Ability.Description")
                .SetIcon(BlueprintTool.Get<BlueprintBuff>("886c7407dc629dc499b9f1465ff382df").Icon)
                .AddCondition(UnitCondition.Confusion)
                .Configure();
            //not sure if this is right but we'l go with it for now
            AbilityConfigurator.New("JargonAbility", JargonAbility)
                .SetDisplayName("Buccaneer.Jargon.Name")
                .SetDescription("Buccaneer.Jargon.Ability.Description")
                .SetIcon(BlueprintTool.Get<BlueprintBuff>("886c7407dc629dc499b9f1465ff382df").Icon)
                .AddContextCalculateAbilityParamsBasedOnClass(Gunslinger.GunslingerClassGUID, statType: Kingmaker.EntitySystem.Stats.StatType.Charisma)
                .AddAbilityEffectRunAction(ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Will, onResult: ActionsBuilder.New().ConditionalSaved(ActionsBuilder.New().ApplyBuffWithDurationSeconds(JargonBuff, 6))))
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Close)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: Grit.GritResource)
                .SetCanTargetEnemies(true)
                .AddSpellDescriptorComponent(new SpellDescriptorWrapper(SpellDescriptor.MindAffecting | SpellDescriptor.Confusion))
                .Configure();

            FeatureConfigurator.New("JargonFeature", JargonFeature)
                .SetDisplayName("Buccaneer.Jargon.Name")
                .SetDescription("Buccaneer.Jargon.Description")
                .SetIcon(BlueprintTool.Get<BlueprintBuff>("886c7407dc629dc499b9f1465ff382df").Icon)//copy from confused
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, stat: Kingmaker.EntitySystem.Stats.StatType.CheckBluff)
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, stat: Kingmaker.EntitySystem.Stats.StatType.CheckIntimidate)
                .AddFacts([JargonAbility])
                .Configure();
        }

        private static void Grog()
        {

            ContextValue Context1 = new ContextValue();
            Context1.ValueType = ContextValueType.Simple;
            Context1.Value = 1;

            BuffConfigurator.New("GrogBuff", GrogBuff)
                .SetDisplayName("Buccaneer.Grog.Buff.Name")
                .SetDescription("Buccaneer.Grog.Description")
                .SetIcon(BlueprintTool.Get<BlueprintFeature>("300e212868bca984687c92bcb66d381b").Icon)//copy from cayden caelian
                .SetRanks(200)//Setting this crazy high to be on the safe side since the actual limiter is set in the ability that applies the buff
                .SetStacking(StackingType.Rank)
                .AddComponent(new Grog())
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.Dodge,stat:Kingmaker.EntitySystem.Stats.StatType.AC,value:1)
                .AddSavingThrowBonusAgainstDescriptor(Context1,modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Morale, spellDescriptor: SpellDescriptor.Fear)
                .Configure();

            

            ContextValue Con = new ContextValue();
            Con.ValueType = ContextValueType.CasterProperty;
            Con.Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusConstitution;

  
            AbilityConfigurator.New("GrogAbility", GrogAbility)
                .SetDisplayName("Buccaneer.Grog.Name")
                .SetDescription("Buccaneer.Grog.Description")
                .SetIcon(BlueprintTool.Get<BlueprintFeature>("300e212868bca984687c92bcb66d381b").Icon)//copy from cayden caelian
                .AddContextCalculateAbilityParamsBasedOnClass(Gunslinger.GunslingerClassGUID, statType: Kingmaker.EntitySystem.Stats.StatType.Charisma)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New()
                    .BuffRank(GrogBuff,rankValue: Con)//check if we have as much Grog as our con mod or more
                    ,ifFalse:ActionsBuilder.New()//if we don't
                        .ApplyBuffWithDurationSeconds(GrogBuff,3600)//gain 1 grog
                        .RestoreResource(Grit.GritResource, value: Context1)))//gain 1 grit
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetCanTargetSelf(true)
                .Configure();

            FeatureConfigurator.New("GrogFeature", GrogFeature)
                .SetDisplayName("Buccaneer.Grog.Name")
                .SetDescription("Buccaneer.Grog.Description")
                .SetIcon(BlueprintTool.Get<BlueprintFeature>("300e212868bca984687c92bcb66d381b").Icon)//copy from cayden caelian
                .AddFacts([GrogAbility])
                .Configure();
        }
    }

    public class Grog : UnitFactComponentDelegate, IUnitAbilityResourceHandler, IResourceAmountBonusHandler, IUnitSubscriber, ISubscriber
    {

        private BlueprintAbilityResource Resource => Grit.GritResource;


        public void CalculateMaxResourceAmount(BlueprintAbilityResource resource, ref int bonus)
        {
            if (base.Fact.Active && resource == Resource)
            {
                bonus += base.Fact.GetRank();
            }
        }
        public void HandleAbilityResourceChange(UnitEntityData unit, UnitAbilityResource resource, int oldAmount)
        {
            if (unit == this.Owner && resource.Blueprint == Resource)
            {//if grit has been spent
                int spent = oldAmount - resource.Amount;
                while (spent > 0 && this.Owner.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(Buccaneer.GrogBuff)).Rank > 0)
                {//lose as much grog as spent grit to a maximum of all grog
                    this.Owner.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(Buccaneer.GrogBuff)).RemoveRank();
                    spent--;
                }
            }
        }
    }

}
