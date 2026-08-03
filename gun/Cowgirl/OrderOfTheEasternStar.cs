using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace gun.Cowgirl
{
    internal static class OrderOfTheEasternStar
    {
        public static string GUID = "1d1093c2a7524107a0fccdc66c30f823";
        public static string EasternStarChallengeGUID = "f42b138f0323429680d7fc58678e62e4";
        public static string EasternStarChallengeBuffGUID = "424afa9aa49d499f900923abe092bb23";
        public static string EasternStarSkillsGUID = "216bb1604b344675abf992b81837c583";
        public static string EasternStarGuardedGUID = "8e50761b81874889841893d0d7280fc0";
        public static string EasternStarGuardedBuffGUID = "e940973bb0034179b9ba3b2fa0edb9ee";
        public static string EasternStarGuardedArmourCheckGUID = "17be28bedb254f06a036d671ba20772a";
        public static string EasternStarPierceTheGuardGUID = "d89e1f05131e49fa9067fa95d7bfd3e6";
        public static string EasternStarPierceTheGuardBuffGUID = "43a121f09ba44efcacf1b942bbd7502c";
        public static string EasternStarPierceOnePurposeGUID = "d7fab1e1e3f241769ca2729350526004";
        private static ProgressionConfigurator Progression;

        public static void Configure()
        {
            EasternStarChallenge();
            EasternStarSkills();
            EasternStarGuarded();
            EasternStarPierceTheGuard();
            EasternStarOnePurpose();
            Progression = ProgressionConfigurator.New("CavalierOrderOfTheEasternStarProgression", GUID);
            Progression.SetDisplayName(LocalizationTool.GetString("OrderOfTheEasternStar.Name"));
            Progression.SetDescription(LocalizationTool.GetString("OrderOfTheEasternStar.Description"));
            Progression.SetIcon(Utilities.MakeIcon("OrderOfTheEasternStar.png"));
            Progression.SetIsClassFeature(true);
            Progression.SetClasses([BlueprintTool.GetRef<BlueprintCharacterClassReference>("3adc3439f98cb534ba98df59838f02c7")]);
            Progression.SetLevelEntry(1, EasternStarChallengeGUID, EasternStarSkillsGUID);
            Progression.SetLevelEntry(2, EasternStarGuardedGUID);
            Progression.SetLevelEntry(8, EasternStarPierceTheGuardGUID);
            Progression.SetLevelEntry(15, EasternStarPierceOnePurposeGUID);
            Progression.Configure();

            BlueprintFeatureSelection OrderSelection = BlueprintTool.Get<BlueprintFeatureSelection>("d710e30ea20240247ad87ad86bcd50f2");
            OrderSelection.m_Features.AddItem(BlueprintTool.GetRef<BlueprintFeatureReference>(GUID));
            OrderSelection.m_AllFeatures.AddItem(BlueprintTool.GetRef<BlueprintFeatureReference>(GUID));
            FeatureSelectionConfigurator.For("d710e30ea20240247ad87ad86bcd50f2").AddToAllFeatures(GUID).Configure();

        }

        public static void EasternStarChallenge ()
        {
            BuffConfigurator challengeBuff = BuffConfigurator.New("CavalierEasternStarChallengeBuff", EasternStarChallengeBuffGUID);
            challengeBuff.SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi);
            challengeBuff.SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Replace);

            ContextRankConfig buffrank = new ContextRankConfig();
            buffrank.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
            buffrank.m_Progression = ContextRankProgression.OnePlusDivStep;
            buffrank.m_StepLevel = 4;
            buffrank.m_Class = [BlueprintTool.GetRef<BlueprintCharacterClassReference>("3adc3439f98cb534ba98df59838f02c7")];

            ContextValue buffValue = new ContextValue();
            buffValue.ValueType = ContextValueType.Rank;

            SavingThrowContextBonusAgainstFact SaveBonus = new SavingThrowContextBonusAgainstFact();

            SaveBonus.m_CheckedFact = BlueprintTool.GetRef<BlueprintFeatureReference>("4f0218323ad379248b69de8a9501159f");//cavalier challenge target
            SaveBonus.Descriptor = ModifierDescriptor.Insight;
            SaveBonus.Bonus = buffValue;


            challengeBuff.AddContextRankConfig(buffrank);
            challengeBuff.AddACContextBonusAgainstFactOwner(bonus:buffValue, checkedFact: BlueprintTool.GetRef<BlueprintUnitFactReference>("4f0218323ad379248b69de8a9501159f"), descriptor: Kingmaker.Enums.ModifierDescriptor.Dodge);
            challengeBuff.AddComponent(SaveBonus);
            challengeBuff.Configure();


            FeatureConfigurator challenge = FeatureConfigurator.New("CavalierEasternStarChallenge", EasternStarChallengeGUID);
            challenge.SetDisplayName(LocalizationTool.GetString("CavalierEasternStarChallenge.Name"));
            challenge.SetDescription(LocalizationTool.GetString("CavalierEasternStarChallenge.Description"));
            challenge.AddBuffOnLightOrNoArmor(BlueprintTool.GetRef<BlueprintBuffReference>(EasternStarChallengeBuffGUID));
            challenge.SetIsClassFeature(true);
            challenge.Configure();


        }

        public static void EasternStarSkills ()
        {
            FeatureConfigurator Skills = FeatureConfigurator.New("CavalierEasternStarSkills", EasternStarSkillsGUID);
            Skills.SetIsClassFeature (true);
            Skills.SetReapplyOnLevelUp(true);
            Skills.SetDisplayName(LocalizationTool.GetString("CavalierEasternStarSkills.Name"));
            Skills.SetDescription(LocalizationTool.GetString("CavalierEasternStarSkills.Description"));
            Skills.AddClassSkill(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana);

            ContextRankConfig SkillBoost = new ContextRankConfig();
            SkillBoost.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
            SkillBoost.m_Progression = ContextRankProgression.Div2;
            SkillBoost.m_Class = [BlueprintTool.GetRef<BlueprintCharacterClassReference>("3adc3439f98cb534ba98df59838f02c7")];
            SkillBoost.m_Min = 1;
            SkillBoost.m_UseMin = true;

            ContextValue SkillValue = new ContextValue();
            SkillValue.ValueType = ContextValueType.Rank;
            
            Skills.AddContextStatBonus(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana, SkillValue);

            Skills.AddContextRankConfig(SkillBoost);

            Skills.Configure();
        }

        public static void EasternStarGuarded()
        {
            BuffConfigurator GuardedBuff = BuffConfigurator.New("CavalierEasternStarGuardedBuff", EasternStarGuardedBuffGUID);

            ContextRankConfig GuardedBuffRank = new ContextRankConfig();
            GuardedBuffRank.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
            GuardedBuffRank.m_Progression = ContextRankProgression.OnePlusDivStep;
            GuardedBuffRank.m_StepLevel = 4;
            GuardedBuffRank.m_StartLevel = 2;
            GuardedBuffRank.m_Class = [BlueprintTool.GetRef<BlueprintCharacterClassReference>("3adc3439f98cb534ba98df59838f02c7")];

            ContextValue GuardedBuffValue = new ContextValue();
            GuardedBuffValue.ValueType = ContextValueType.Rank;
            GuardedBuff.AddDamageResistancePhysical(value: GuardedBuffValue);
            
            GuardedBuff.AddStatBonus(ModifierDescriptor.Morale, stat: Kingmaker.EntitySystem.Stats.StatType.SaveFortitude, value: 2);
            GuardedBuff.AddStatBonus(ModifierDescriptor.Morale, stat: Kingmaker.EntitySystem.Stats.StatType.SaveReflex, value: 2);
            GuardedBuff.AddStatBonus(ModifierDescriptor.Morale, stat: Kingmaker.EntitySystem.Stats.StatType.SaveWill, value: 2);
            GuardedBuff.AddContextRankConfig(GuardedBuffRank);
            GuardedBuff.SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi);
            GuardedBuff.Configure();


            BuffConfigurator GuardedArmorCheck = BuffConfigurator.New("CavalierEasternStarGuardedArmorCheck", EasternStarGuardedArmourCheckGUID);
            GuardedArmorCheck.AddBuffExtraEffects(
                checkedBuff: BlueprintTool.GetRef<BlueprintBuffReference>("6ffd93355fb3bcf4592a5d976b1d32a9"),//defensive fighting or
                checkedBuffList: [BlueprintTool.GetRef<BlueprintBuffReference>("e81cd772a7311554090e413ea28ceea1")], //combat expertise
                extraEffectBuff: BlueprintTool.GetRef<BlueprintBuffReference>(EasternStarGuardedBuffGUID));//then give the guarded bonous
            GuardedArmorCheck.SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi);
            GuardedArmorCheck.Configure();


            FeatureConfigurator Guarded = FeatureConfigurator.New("CavalierEasternStarGuarded", EasternStarGuardedGUID);
            Guarded.AddBuffOnLightOrNoArmor(BlueprintTool.GetRef<BlueprintBuffReference>(EasternStarGuardedArmourCheckGUID));
            Guarded.SetDisplayName(LocalizationTool.GetString("CavalierEasternStarGuarded.Name"));
            Guarded.SetDescription(LocalizationTool.GetString("CavalierEasternStarGuarded.Description"));
            Guarded.Configure();
        }

        public static void EasternStarPierceTheGuard()
        {
            BuffConfigurator PierceBuff = BuffConfigurator.New("CavalierEasterStarPierceTheGuardBuff", EasternStarPierceTheGuardBuffGUID);
            PierceBuff.SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi);
            PierceBuff.AddIgnoreTargetDR(checkCaster:true);
            PierceBuff.AddUniqueBuff();
            PierceBuff.Configure();

            FeatureConfigurator Pierce = FeatureConfigurator.New("CavalierEasterStarPierceTheGuard", EasternStarPierceTheGuardGUID);

            //add alignment to physical damage against extraplaner beings
            Pierce.AddOutgoingPhysicalDamageProperty(addAlignment:true,myAlignment:true,againstFactOwner:true,unitFact:BlueprintTool.GetRef<BlueprintUnitFactReference>("136fa0343d5b4b348bdaa05d83408db3"));

            ActionsBuilder actions = ActionsBuilder.New();

            ConditionsBuilder conditions = ConditionsBuilder.New();
            ConditionsBuilderContextEx.HasFact(conditions, BlueprintTool.GetRef<BlueprintUnitFactReference>("136fa0343d5b4b348bdaa05d83408db3"));//if they have the extraplaner type

            ActionsBuilder IfTrue = ActionsBuilder.New();

            IfTrue.ApplyBuffPermanent(BlueprintTool.GetRef<BlueprintBuffReference>(EasternStarPierceTheGuardBuffGUID), true, false, false, false, false, false, false);

            actions.Conditional(conditions,IfTrue);

            Pierce.AddAbilityUseTrigger(ability: BlueprintTool.GetRef<BlueprintAbilityReference>("9d5d58ff40e39ff4681670463abe99c9"),//cavalier challenge
                actionsOnTarget:true,forOneSpell:true,action:actions
                );

            Pierce.SetDisplayName(LocalizationTool.GetString("CavalierEasterStarPierceTheGuard.Name"));
            Pierce.SetDescription(LocalizationTool.GetString("CavalierEasterStarPierceTheGuard.Description"));
            Pierce.SetIsClassFeature(true);
            Pierce.Configure();

        }

        public static void EasternStarOnePurpose()
        {
            FeatureConfigurator OnePurpose = FeatureConfigurator.New("CavalierEasternStarOnePurpose", EasternStarPierceOnePurposeGUID);
            OnePurpose.AddShareBuffsWithPet([BlueprintTool.GetRef<BlueprintBuffReference>(EasternStarGuardedBuffGUID),
                BlueprintTool.GetRef<BlueprintBuffReference>(EasternStarChallengeBuffGUID),
                BlueprintTool.GetRef<BlueprintBuffReference>("0529edfc06abe58439f36a7b0f933359")
            ]);
            OnePurpose.SetDisplayName(LocalizationTool.GetString("CavalierEasternStarOnePurpose.Name"));
            OnePurpose.SetDescription(LocalizationTool.GetString("CavalierEasternStarOnePurpose.Description"));
            OnePurpose.Configure();



        }
    }

    public class SavingThrowContextBonusAgainstFact : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {

        public BlueprintFeatureReference m_CheckedFact;

        public ModifierDescriptor Descriptor;

        public ContextValue Bonus;

        public BlueprintFeature CheckedFact => m_CheckedFact?.Get();

        public void OnEventAboutToTrigger(RuleSavingThrow evt)
        {

            UnitDescriptor unitDescriptor = evt.Reason.Caster?.Descriptor;
            if (unitDescriptor != null && unitDescriptor.HasFact(CheckedFact))
            {
                int Value = Bonus.Calculate(base.Context);
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(Value, base.Runtime, Descriptor));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(Value, base.Runtime, Descriptor));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(Value, base.Runtime, Descriptor));
            }
        }

        public void OnEventDidTrigger(RuleSavingThrow evt)
        {
        }
    }
}
