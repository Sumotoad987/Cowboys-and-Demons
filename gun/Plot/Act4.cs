using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.AreaEx;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.StoryEx;
using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.Configurators.AI;
using BlueprintCore.Blueprints.Configurators.Area;
using BlueprintCore.Blueprints.Configurators.AreaLogic.Etudes;
using BlueprintCore.Blueprints.Configurators.DialogSystem;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Assets;
using gun;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Controllers.Units;
using Kingmaker.Designers;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.TempMapCode.Ambush;
using Kingmaker.DialogSystem;
using Kingmaker.Dungeon.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence.Scenes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.GameModes;
using Kingmaker.PubSubSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using static Kingmaker.GameModes.GameModeType;
using static Kingmaker.Kingdom.KingdomStats;
using static Kingmaker.Kingdom.Settlements.SettlementGridTopology;

namespace gun.Plot
{
    internal static class Act4
    {
        public const string WeaversLairArea = "b88ef2fc7c35427c9e62aeb0070e451a";
        public const string WeaversLairAreaEntrance = "e6a2af762d7147ff9d8731aa112f17a6";
        public const string WeaverLairPortal = "f5c02cfb8d594f81a13d262331b89be9";
        public const string WeaverLairPortalEffect = "82daa8afd5004487a135310603c5115f";
        public const string LobotomisedMarilith = "3fe4d22aa66048b0ac05918745bc5249";
        public const string LobotomisedFeature = "a6641c0cf0954dd18bae45275b72dcfd";
        public const string LobotomisedNalfeshnee = "586bb5d98fbc48359b23c1f2de14a55e";
        public const string LobotomisedVrolikai = "2984aff65a424f9bb2a820f166b3df67";

        public static void Configure()
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(System.IO.Path.Combine("D:\\Games\\Pathfinder\\Pathfinder Wrath of the Righteous\\Bundles\\weaverlair_mainmechanics.scenes"));
            CampingSettings WeaverLairCamp = new CampingSettings();
            WeaverLairCamp.CampingAllowed = false;
            AreaConfigurator.New("WeaversLairArea", WeaversLairArea).CopyFrom("7141102c64f0a924c8b458831247bcb5")
                //.SetDynamicScene(new SceneReference ("WeaversLair_MainMechanics"))
                .SetSetting(Kingmaker.Enums.AreaSetting.Abyss)
                .SetCampingSettings(WeaverLairCamp)
                .SetArtSetting(BlueprintArea.SettingType.DemonicIndoor)
                .SetAreaName("Plot.CowgirlQuest2.Area.Name")
                .Configure();//need some kind of reload for the bundles so it loads properly. Do Something with a flag so when the map is first loaded it plays the storybook about walking through 4d space then at the end of the storybook reloads the map
            EventBus.Subscribe(new WeaverLairSetup());
            AreaEnterPointConfigurator.New("WeaversLairAreaEntrance", WeaversLairAreaEntrance)
                .SetArea(WeaversLairArea)
                .SetAreaPart(WeaversLairArea)
                .Configure();

            FusedDemon.Configure();
            LairPortals();
            LobotomisedMonsters();
            DistortedPortal.Configure();
            BrainweaverDialog.Configure();




        }

        private static void LobotomisedMonsters()
        {
            FeatureConfigurator.New("LobotomisedFeature", LobotomisedFeature)
                .AddComponent(new Lobotomised())
                .AddConditionImmunity(UnitCondition.Dazed)
                .AddConditionImmunity(UnitCondition.Confusion)
                .AddConditionImmunity(UnitCondition.Cowering)
                .AddConditionImmunity(UnitCondition.Frightened)
                .AddConditionImmunity(UnitCondition.Shaken)
                .AddConditionImmunity(UnitCondition.Sleeping)
                .AddConditionImmunity(UnitCondition.Stunned)
                .AddConditionImmunity(UnitCondition.Unconscious)
                .AddStatBonus(ModifierDescriptor.Inherent,false,StatType.BaseAttackBonus,10)
                .AddStatBonus(ModifierDescriptor.Inherent, false, StatType.AdditionalDamage, 10)
                .AddBuffDescriptorImmunity(descriptor: new SpellDescriptorWrapper(SpellDescriptor.MindAffecting))
                .AddSpellImmunityToSpellDescriptor(descriptor: new SpellDescriptorWrapper(SpellDescriptor.Compulsion | SpellDescriptor.MindAffecting | SpellDescriptor.Emotion | SpellDescriptor.Charm | SpellDescriptor.Daze | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Stun))
                .AddImmunityToAbilityScoreDamage(true,statTypes: [StatType.Intelligence, StatType.Wisdom, StatType.Charisma])
                .SetDisplayName("Lobotomised.Name")
                .SetDescription("Lobotomised.Description")
                .Configure();

            UnitConfigurator.New("LobotomisedMarilith", LobotomisedMarilith)
                .CopyFrom("4addf73a9a68c694f9919137f800be50")
                .AddClassLevels(characterClass: "92ab5f2fe00631b44810deffcc1a97fd",levels:16,levelsStat: StatType.Constitution)
                .AddExperience(cR:17,encounter: Kingmaker.Blueprints.Classes.Experience.EncounterType.Mob)
                .AddFacts([
                    "561041cdb5887464883c55c75219a9dc",//demon of strength
                    LobotomisedFeature
                    ])
                .SetBrain("ce05259ea078468aa394a3e1db057baa")//copied from gelatinous cube this brain has only attack on it
                .SetType("41df5dccedbdfc840a4264038f92911d")
                .SetDisplayName("Lobotomised.Marilith.Name")
                .SetSkills(new BlueprintUnit.UnitSkills())
                .SetFaction("0f539babafb47fe4586b719d02aff7c4")
                .SetStrength(25)
                .SetDexterity(19)
                .SetConstitution(32)
                .SetIntelligence(10)
                .SetWisdom(10)
                .SetCharisma(10)
                .SetAlignment(Alignment.TrueNeutral)
                .SetPrefab(BlueprintTool.Get<BlueprintUnit>("4addf73a9a68c694f9919137f800be50").Prefab)
                .SetBody(BlueprintTool.Get<BlueprintUnit>("4addf73a9a68c694f9919137f800be50").Body)
                .SetVisual(BlueprintTool.Get<BlueprintUnit>("4addf73a9a68c694f9919137f800be50").Visual)
                .SetSpeed(new Feet(30))
                .Configure();

            UnitConfigurator.New("LobotomisedNalfeshnee", LobotomisedNalfeshnee)
                .CopyFrom("45207a7715f2b2c46bdcf5af6d21c528", (BlueprintComponent comp) =>
                {
                    return true;
                })
                .AddFacts([LobotomisedFeature])
                .SetDisplayName("Lobotomised.Nalfeshnee.Name")
                .SetBrain("ce05259ea078468aa394a3e1db057baa")//copied from gelatinous cube this brain has only attack on it
                .Configure();
            UnitConfigurator.New("LobotomisedVrolikai", LobotomisedVrolikai)
                .CopyFrom("eda9ddab61fd61e4eb2a860315d317c5", (BlueprintComponent comp) =>
                {
                    return true;
                })
                .AddFacts([LobotomisedFeature])
                .SetDisplayName("Lobotomised.Vrolikai.Name")
                .SetBrain("ce05259ea078468aa394a3e1db057baa")//copied from gelatinous cube this brain has only attack on it
                .Configure();
        }

        private static void LairPortals()
        {
            AbilityAreaEffectConfigurator.New("WeaverLairPortalEffect", WeaverLairPortalEffect)
                .CopyFrom("e122151e93e44e0488521aed9e51b617")
                .AddAbilityAreaEffectRunAction(unitEnter: ActionsBuilder.New().TranslocateUnit(unit: new ContextTargetUnit(), translocatePositionEvaluator: new WeaverLairPorterPos()))
                .Configure();


            ContextDurationValue InfinateDuration = new ContextDurationValue();
            InfinateDuration.Rate = DurationRate.Days;
            InfinateDuration.m_IsExtendable = true;
            InfinateDuration.BonusValue = new ContextValue();
            InfinateDuration.BonusValue.Value = 99999;
            InfinateDuration.DiceCountValue = new ContextValue();
            AbilityConfigurator.New("WeaverLairPortal", WeaverLairPortal)
                .CopyFrom("1407fb5054d087d47a4c40134c809f12")
                .AddAbilityEffectRunAction(ActionsBuilder.New().SpawnAreaEffect(WeaverLairPortalEffect, InfinateDuration))
                .AddAbilityAoERadius(true, radius: new Feet(20))
                .AddIncreaseSpellDC(bonusDC: 99999)
                .Configure();
        }
    }

    internal static class DistortedPortal
    {
        public static string[] cues = {
            "5a5dcf65ea6d47e88a71fa971600b47e",
            "2559913a68154a57825b19b11d56062f",
            "cf40058780914766ba3c6550f2e1b8f8",
            "df0eb62e211942008c25004cc70dd5b8",
            "40e1cb2cfcca4b61a5248d533b8f122e",
            "f2769208ba6147a091dfd3ad00aad367",
            "88ee94e539ef471984d0faef0bf41a17",
            "5c3aa451eef742c5b78c4812f34ada6c",
            "d0fe379dccb84016bc737ebbfeb11ced",
            "9b699547c3f64f0185fcec2898699a5a",
            "be4abc2993164bd480fbe7c202242c65",
            "0aecc51d28e14880ba8225c2528fd6bb",
            "766ea200d2f541ea866286b2e5b509ed"
        };
        public static string[] answers = {
            "7ee9c2ce828a4173896650dbf83fe9bd",
            "d5e19c4b24c842188683e675f3887e37",
            "161d5013bd434c3185b89974efbb9c34",
            "cfd6713b574b49dd91bc37cdf771c7a2" 
        };
        public static string[] checks = {
        "dcdd605ba1e547fcbfba61f3fa432ade",
        "1b12df09d73e47f7a063f7f03a355c6c",
        "5778b0dba0c04f6c93e09189acdaf98a",
        "7337c3201a6847a8989908bdced3d8e9"
        };
        public static string[] pages = {
        "6000b0baa99543b2b5569e75f0c24082",
        "53ad0980042c48be92dba4a509e83a8f"
        };
        public static void Configure()
        {
            Page0();
            Page1();
        }

        private static void Page0()
        {
            BookPageConfigurator.New("CowgirlDistortedPortalPage0", pages[0])
               .SetTitle(LocalizationTool.GetString("Plot.DistortedPortal.Title"))
               .SetCues(cues[0], cues[3], cues[4], cues[5])
               .SetAnswers(answers[0], answers[1], answers[2], answers[3])
               .Configure();

            CueConfigurator.New("CowgirlDistortedPortalCue0", cues[0])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Cue.0"))
                .SetContinueValue(Utilities.MakeCueSelection(checks[0]))
                .Configure();



            CheckConfigurator.New("CowgirlMeetC0", checks[0])
                .SetDC(35)
                .SetExperience(DialogExperience.NormalExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SaveWill)
                .SetSuccess(pages[0])
                .SetFail(pages[0])
                .Configure();

            DiceFormula wisdrain = new DiceFormula(2, diceType: DiceType.One);

            CueConfigurator.New("CowgirlDistortedPortalCue3", cues[3])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Cue.3"))
                .SetConditions(Utilities.MakePassedCheck(false, checks[0]))
                .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.Madness,true, Utilities.MakeIntConstant(1)).DealStatDamage(wisdrain,StatType.Wisdom, new PlayerCharacter(),isDrain:true))
                .Configure();

            CueConfigurator.New("CowgirlDistortedPortalCue4", cues[4])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Cue.4"))
                .SetConditions(Utilities.MakePassedCheck(true, checks[0]))
                .Configure();

            CueConfigurator.New("CowgirlDistortedPortalCue5", cues[5])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Cue.5"))
                .Configure();
            #region Answer0Perception

            AnswerConfigurator.New("CowgirlDistortedPortalAnswer0", answers[0])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Answer.0"))
                .SetNextCue(Utilities.MakeCueSelection(checks[1]))
                .Configure();

            CheckConfigurator.New("CowgirlMeetC1", checks[1])
                .SetDC(32)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillPerception)
                .SetSuccess(pages[1])
                .SetFail(pages[1])
                .Configure();

            CueConfigurator.New("CowgirlDistortedPortalCue6", cues[6])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Cue.6"))
                .SetConditions(Utilities.MakePassedCheck(true, checks[1]))
                .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.SuspectsCowgirl,true,Utilities.MakeIntConstant(1)))
                .Configure();

            CueConfigurator.New("CowgirlDistortedPortalCue7", cues[7])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Cue.7"))
                .SetConditions(Utilities.MakePassedCheck(false, checks[1]))
                .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.Madness, true, Utilities.MakeIntConstant(1)))
                .Configure();


            #endregion

            #region Answer1Survival
            AnswerConfigurator.New("CowgirlDistortedPortalAnswer1", answers[1])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Answer.1"))
                .SetNextCue(Utilities.MakeCueSelection(checks[2]))
                .Configure();

            CheckConfigurator.New("CowgirlDistortedPortalC2", checks[2])
                .SetDC(36)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillLoreNature)
                .SetSuccess(pages[1])
                .SetFail(pages[1])
                .Configure();

            CueConfigurator.New("CowgirlDistortedPortalCue8", cues[8])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Cue.6"))
                .SetConditions(Utilities.MakePassedCheck(true, checks[2]))
                .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.SuspectsCowgirl, true, Utilities.MakeIntConstant(1)).IncrementFlagValue(Flags.CowgirlRespect,true,Utilities.MakeIntConstant(1)))
                .Configure();

            CueConfigurator.New("CowgirlDistortedPortalCue9", cues[9])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Cue.7"))
                .SetConditions(Utilities.MakePassedCheck(false, checks[2]))
                .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.Madness, true, Utilities.MakeIntConstant(1)))
                .Configure();

            #endregion

            #region Answer2Arcana
            AnswerConfigurator.New("CowgirlDistortedPortalAnswer2", answers[2])
                .SetText(LocalizationTool.GetString("Plot.CowgirlDistortedPortal.Answer.2"))
                .SetNextCue(Utilities.MakeCueSelection(checks[3]))
                .Configure();

            CheckConfigurator.New("CowgirlDistortedPortaltC3", checks[3])
                .SetDC(30)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana)
                .SetSuccess(pages[1])
                .SetFail(pages[1])
                .Configure();

            CueConfigurator.New("CowgirlDistortedPortalCue10", cues[10])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Cue.10"))
                .SetConditions(Utilities.MakePassedCheck(true, checks[3]))
                .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.Madness, true, Utilities.MakeIntConstant(1)).IncrementFlagValue(Flags.Mythos, true, Utilities.MakeIntConstant(1)))
                .Configure();

            CueConfigurator.New("CowgirlDistortedPortalCue11", cues[11])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Cue.11"))
                .SetConditions(Utilities.MakePassedCheck(false, checks[2]))
                .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.Madness, true, Utilities.MakeIntConstant(2)))
                .Configure();
            #endregion

            //answer 3 trust
            AnswerConfigurator.New("CowgirlDistortedPortalAnswer3", answers[3])
                .SetText(LocalizationTool.GetString("Plot.CowgirlDistortedPortal.Answer.3"))
                .SetNextCue(Utilities.MakeCueSelection(pages[1]))
                .SetOnSelect(ActionsBuilder.New().IncrementFlagValue(Flags.CowgirlApproval,true,Utilities.MakeIntConstant(1)))
                .Configure();
        }

        public static void Page1()
        {
            BookPageConfigurator.New("CowgirlDistortedPortalPage1", pages[1])
               .SetTitle(LocalizationTool.GetString("Plot.DistortedPortal.Title"))
               .SetCues(cues[6], cues[7], cues[8], cues[9], cues[10], cues[11], cues[12])
               .SetAnswers()
               .Configure();

            CueConfigurator.New("CowgirlDistortedPortalCue12", cues[12])
                .SetText(LocalizationTool.GetString("Plot.DistortedPortal.Cue.12"))
                .Configure();


        }
    }

    internal static class BrainweaverDialog
    {
        public static string[] cues = {
            "eb89b9753d534e7a84179a9dbdc30294",
            "522de34d731b4b428c0ddeb5a6dd6544",
            "3f82fdf2a65e40228ee77c8d91c7cd7b",
            "e5a23f28fa0e44118491b465ab8f438e",
            "382f9499b01a4cf79c02380df00dbebb",
            "c586577a824348228b5ee72cad8c06d1",
            "a27db0089a8049289c88f8e6251eeea0",
            "a578740540f84c66ac7f99f6548ac6f5",
            "44dc5783bf85466fb13ae7131d9f00fd",
            "3576a5a07af64be595bbb9819c491c16",
            "3ca4077d943d4252a034d7b27afcae7c",
            "f8eb00a1c1f544eeaef04237952ad69d",
            "822d6ffa62e049de902b8d2a41c41e12",
            "fcde6ce992014a7e95d1748ef960ec84"
        };
        public static string[] answers =
        {
            "e4d7fd981ddd4cfebb294b4f0dade90f",
            "1e1140c05b8f49f99e7647ae069aea81",
            "f80bfcd13a774e0685fa87d30b18397b",
            "7d83b80259794b758f5bb3b93ed742ae",
            "b2a77d57fc6e4019b3c9f877da0d73e2"

        };
        public static string[] checks =
        {
            "68e3bf73886d4e49b8ffc660e9075f14"
        };
        public const string Dialog = "0deb7b34602745fdaecb15160287e0a9";

        public static void Configure()
        {
            DialogConfigurator.New("BrainweaverAct4Dialog", Dialog)
                .SetType(Kingmaker.DialogSystem.Blueprints.DialogType.Common)
                .SetFirstCue(Utilities.MakeCueSelection(cues[0]))
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue0", cues[0])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.0"))
                .SetContinueValue(Utilities.MakeCueSelection(checks[0]))
                //set speaker as brainweaver
                .Configure();

            CheckConfigurator.New("BrainweaverAct4Check0", checks[0])
                .SetDC(35)
                .SetExperience(DialogExperience.NormalExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillPerception)
                .SetSuccess(cues[1])
                .SetFail(cues[2])
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue1", cues[1])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.1"))
                .SetContinueValue(Utilities.MakeCueSelection(cues[2]))
                //set speaker as brainweaver
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue2", cues[2])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.2"))
                .SetAnswers(answers[0], answers[1], answers[2], answers[3], answers[4])
                //set speaker as brainweaver
                .Configure();

            //what were those things
            AnswerConfigurator.New("BrainweaverAct4Answer0", answers[0])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Answer.0"))
                .SetNextCue(Utilities.MakeCueSelection(cues[3]))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue3", cues[3])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.3"))
                .SetContinueValue(Utilities.MakeCueSelection(cues[4]))
                .SetAnswers(answers[0], answers[1], answers[2], answers[3], answers[4])
                //set speaker as brainweaver
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue4", cues[4])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.4"))
                .SetContinueValue(Utilities.MakeCueSelection(cues[5]))
                .SetSpeaker(Utilities.GetSpeaker("a352873d37ec6c54c9fa8f6da3a6b3e1"))//arueshalae
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue5", cues[5])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.5"))
                .SetContinueValue(Utilities.MakeCueSelection(cues[6]))
                //set speaker as brainweaver
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue6", cues[6])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.6"))
                .SetSpeaker(Cowgirl.CowgirlUnit.GetSpeaker())
                .SetAnswers(answers[0], answers[1], answers[2], answers[3], answers[4])
                .Configure();

            //what are you?
            AnswerConfigurator.New("BrainweaverAct4Answer1", answers[1])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Answer.1"))
                .SetNextCue(Utilities.MakeCueSelection(cues[7]))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue7", cues[7])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.7"))
                .SetAnswers(answers[0], answers[1], answers[2], answers[3], answers[4])
                //set speaker as brainweaver
                .Configure();

            AnswerConfigurator.New("BrainweaverAct4Answer2", answers[2])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Answer.2"))
                .SetNextCue(Utilities.MakeCueSelection(cues[8]))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue8", cues[8])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.8"))
                .SetAnswers(answers[0], answers[1], answers[2], answers[3], answers[4])
                //set speaker as brainweaver
                .Configure();

            AnswerConfigurator.New("BrainweaverAct4Answer3", answers[3])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Answer.3"))
                .SetNextCue(Utilities.MakeCueSelection(cues[9]))
                .SetMythicRequirement(Kingmaker.DialogSystem.Blueprints.Mythic.PlayerIsAeon)
                .SetShowOnce()
                .Configure();

            AnswerConfigurator.New("BrainweaverAct4Answer4", answers[4])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Answer.4"))
                .SetNextCue(Utilities.MakeCueSelection(cues[10]))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue9", cues[9])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.9"))
                .SetContinueValue(Utilities.MakeCueSelection(cues[10]))
                //set speaker as brainweaver
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue10", cues[10])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.10"))
                .SetContinueValue(Utilities.MakeCueSelection(cues[11]))
                //set speaker as brainweaver
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue11", cues[11])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.11"))
                .SetContinueValue(Utilities.MakeCueSelection(cues[12]))
                .SetSpeaker(Cowgirl.CowgirlUnit.GetSpeaker())
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue12", cues[12])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.12"))
                .SetContinueValue(Utilities.MakeCueSelection(cues[13]))
                //set speaker as brainweaver
                .Configure();

            CueConfigurator.New("BrainweaverAct4Cue13", cues[13])
                .SetText(LocalizationTool.GetString("Plot.BrainweaverAct4.Cue.13"))
                .SetSpeaker(Cowgirl.CowgirlUnit.GetSpeaker())
                .Configure();
        }
    }



    internal static class FusedDemon
    {
        
        public const string GUID = "c905ed9391524cb4b0e1d364d36c360e";
        public const string ManyMinds = "c65ecf006f9142a09e6e45d0b89ce209";
        public const string DistributedPsyche = "9d8ed9bba00f4207bcae78342b0d90bf";
        public const string ExposedBrains = "c450d74a45434f718f79f0ede126a42d";
        public const string Brain = "f80f4e3574a145de9b11f890e5ceb9d7";
        public static void Configure()
        {
            FusedDemonFeatures();
            ContextValue casterLevel = new ContextValue();
            casterLevel.Value = 20;

            BrainConfigurator.New("FusedDemonBrain", Brain)
                .SetActions(
                "866ffa6c34000cd4a86fb1671f86c7d8",//attack
                "73cb501b62786d642804ec2a87c706dc",//call lightning
                "72238dde5de8fa449a0944bd37fcb36e",//glab mirror image
                "287b4dac4f921414b9b4a866019a7a01",//glab confusion
                "95d38ddd262c4167aa74c8291730323e"//song of discord
                )
                .Configure();
            UnitConfigurator.New("FusedDemon", GUID)
                .CopyFrom("d13545bd34bf2c3438f909417b9c5314")
                .SetPrefab("9a8c9b1216b0a2744876e54a777d1c8c")//switch the prefab out for the new brain covered monster
                .RemoveComponents((BlueprintComponent comp) => { return true; })
                .AddComponent(new FusedDemonFix())
                .AddComponent(new BrainweaverSetupCaster())
                .AddClassLevels(characterClass: "92ab5f2fe00631b44810deffcc1a97fd",levels: 20,selections:FusedDemonSelections())
                .AddExperience(cR:25,encounter:Kingmaker.Blueprints.Classes.Experience.EncounterType.Boss)
                .AddMobCaster()
                .SetBrain(Brain)
                .SetDisplayName("FusedDemon.Name")
                .SetDescription("FusedDemon.Description")
                .SetType("563108bc9d0b1ae4fbfd4bc67a3a4cc1")
                .SetGender(Gender.Male)
                .SetSize(Size.Huge)
                .SetAlignment(Alignment.ChaoticEvil)
                .SetVisual(BlueprintTool.Get<BlueprintUnit>("d13545bd34bf2c3438f909417b9c5314").Visual)
                .SetFaction("0f539babafb47fe4586b719d02aff7c4")
                .SetBody(BlueprintTool.Get<BlueprintUnit>("d13545bd34bf2c3438f909417b9c5314").Body)
                .SetStrength(36)
                .SetDexterity(12)
                .SetIntelligence(74)
                .SetWisdom(34)
                .SetConstitution(32)
                .SetCharisma(20)
                .AddFacts(FusedDemonFacts(),20)
                .SetSkills(BlueprintTool.Get<BlueprintUnit>("d13545bd34bf2c3438f909417b9c5314").Skills)//just use glab skills cause they don't matter
                .Configure();
        }

        public static void FusedDemonFeatures()
        {
            FeatureConfigurator.New("ManyMinds", ManyMinds)
                .AddAutoMetamagic(allowedAbilities: Kingmaker.Designers.Mechanics.Facts.AutoMetamagic.AllowedType.SpellOrSpellLike, metamagic: Kingmaker.UnitLogic.Abilities.Metamagic.Quicken)              
                .SetDisplayName("FusedDemon.ManyMinds.Name")
                .SetDescription("FusedDemon.ManyMinds.Description")
                .SetIcon(Utilities.MakeIcon("ManyMinds.png"))
                .Configure();

            FeatureConfigurator.New("DistributedPsyche", DistributedPsyche)
                .AddSpellImmunityToSpellDescriptor(descriptor: new SpellDescriptorWrapper(SpellDescriptor.MindAffecting))
                .SetDisplayName("FusedDemon.DistributedPsyche.Name")
                .SetDescription("FusedDemon.DistributedPsyche.Description")
                .SetIcon(Utilities.MakeIcon("DistributedPsyche.png"))
                .Configure();

            FeatureConfigurator.New("ExposedBrains", ExposedBrains)
                .AddComponent(new ExposedBrainsStatMod())
                .SetDisplayName("FusedDemon.ExposedBrains.Name")
                .SetDescription("FusedDemon.ExposedBrains.Description")
                .SetIcon(Utilities.MakeIcon("ExposedBrains.png"))
                .Configure();
        }

        public static List<Blueprint<BlueprintUnitFactReference>> FusedDemonFacts()
        {
            List<Blueprint<BlueprintUnitFactReference>> facts = new List<Blueprint<BlueprintUnitFactReference>>()
            {
                "561041cdb5887464883c55c75219a9dc",//demon of strength
                "1b466705276e3124ab43f865e282c6e8",//demon of magic
                "65c289f08343f5349b6dafbc0240d6ef",//natural armor 20
                "dc960a234d365cb4f905bdc5937e623a",//subtype demon
                "136fa0343d5b4b348bdaa05d83408db3",//subtype extraplaner
                "205205053a2915d4782cf48dc0cc3c09",//spell resistance 11 + CR
                "09b4b69169304474296484c74aa12027",//true seeing
                "b555e9c8da67a7344ae0bba48b706f53",//glabrezu rend
                "5aca73bc5f1b3bb4abd948aa016c0772",//glab remove polymorph on touch
                "9c0fa9b438ada3f43864be8dd8b3e741",//mage shield
                "a92acdf18049d784eaa8f2004f5d2304",//mage armor
                "443dcf9797ba30345a9d54a9ed5b87d9",//DR15/good
                "3c05c7a39f7a5574796f49476e850fb7",//fast healing 15
                "2a9ef0e0b5822a24d88b16673a267456",//call lightning
                "2a9ef0e0b5822a24d88b16673a267456",//call lightning
                "2a9ef0e0b5822a24d88b16673a267456",//call lightning
                "2a9ef0e0b5822a24d88b16673a267456",//call lightning
                "2a9ef0e0b5822a24d88b16673a267456",//call lightning
                "2a9ef0e0b5822a24d88b16673a267456",//call lightning I hope having it here a few times will give it multiple castings
                "3e4ab69ada402d145a5e0ad3ad4b8564",//mirror image
                "cf6c901fb7acc904e85c63b342e9c949",//confusion
                "d38aaf487e29c3d43a3bffa4a4a55f8f",//song of discord
                ManyMinds,
                DistributedPsyche,
                ExposedBrains
            };
            return facts;
        }
        public static SelectionEntry[] FusedDemonSelections()
        {
            
            SelectionEntry BasicFeats = new SelectionEntry();
            BasicFeats.m_Selection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>("247a4068296e8be42890143f451b4b45");
            BasicFeats.m_Features = new BlueprintFeatureReference[] {
                BlueprintTool.GetRef<BlueprintFeatureReference>("9972f33f977fc724c838e59641b2fca5"),//power attack
                BlueprintTool.GetRef<BlueprintFeatureReference>("f4201c85a991369408740c6888362e20"),//improved crit
                BlueprintTool.GetRef<BlueprintFeatureReference>("d809b6c4ff2aaff4fa70d712a70f7d7b"),//cleave
                BlueprintTool.GetRef<BlueprintFeatureReference>("cc9c862ef2e03af4f89be5088851ea35"),//great cleave
                BlueprintTool.GetRef<BlueprintFeatureReference>("59bd93899149fa44687ff4121389b3a9"),//cleaving finish
                BlueprintTool.GetRef<BlueprintFeatureReference>("ffa1b373190af4f4db7a5501904a1983"),//improved cleaving finish
            };

            SelectionEntry ParameterizedFeats = new SelectionEntry();
            ParameterizedFeats.m_Selection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>("247a4068296e8be42890143f451b4b45");
            ParameterizedFeats.m_ParametrizedFeature = BlueprintTool.GetRef<BlueprintParametrizedFeatureReference>("f4201c85a991369408740c6888362e20");
            ParameterizedFeats.m_Features = new BlueprintFeatureReference[] {
                BlueprintTool.GetRef<BlueprintFeatureReference>("9972f33f977fc724c838e59641b2fca5"),//power attack
                BlueprintTool.GetRef<BlueprintFeatureReference>("f4201c85a991369408740c6888362e20"),//improved crit
                BlueprintTool.GetRef<BlueprintFeatureReference>("0f8939ae6f220984e8fb568abbdfba95"),//combat reflexes
                BlueprintTool.GetRef<BlueprintFeatureReference>("cc9c862ef2e03af4f89be5088851ea35"),//great cleave
                BlueprintTool.GetRef<BlueprintFeatureReference>("197306972c98bb843af738dc7529a7ac"),//agile manuevers
                BlueprintTool.GetRef<BlueprintFeatureReference>("f4201c85a991369408740c6888362e20"),//improved crit
            };
            ParameterizedFeats.ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.OtherNaturalWeapons;

            SelectionEntry[] Output = new SelectionEntry[]{
                BasicFeats,
                ParameterizedFeats
            };
            return Output;
        }
    }
}

public class FusedDemonFix : UnitFactComponentDelegate
{

    public override void OnActivate()
    {
        Main.Log.Log("In Task");

        //Game.Instance.EntityCreator.Tick();




        Fix(base.Owner);
        
    }

    public override void OnDeactivate()
    {

    }

    public async Task Fix(UnitEntityData FusedDemon)
    {
        Main.Log.Log("In Task");
        while (FusedDemon.View == null || Game.Instance.IsLoadingSave)
        {
            await Task.Delay(1000);//wait to make sure everything has loaded
        }
        Main.Log.Log("Spawned Alt");
        UnitEntityData StandardGlab = Game.Instance.EntityCreator.SpawnUnit(BlueprintTool.Get<BlueprintUnit>("323197d05aa21db4cb1c9460359cce76"), new Vector3(999, 999, 999), new Quaternion(), Game.Instance.State.LoadedAreaState.MainState);
        Main.Log.Log("Waited Time");
        Main.Log.Log(FusedDemon.CharacterName);
        Main.Log.Log(FusedDemon.View.name);
        Main.Log.Log(FusedDemon.View.gameObject.name);
        Main.Log.Log(FusedDemon.View.gameObject.GetComponentInChildren<UnitAnimationManager>().name);
        Main.Log.Log(FusedDemon.View.gameObject.GetComponentInChildren<UnitAnimationManager>().AnimationSet.name);
        FusedDemon.View.gameObject.GetComponentInChildren<UnitAnimationManager>().AnimationSet = StandardGlab.View.gameObject.GetComponentInChildren<UnitAnimationManager>().AnimationSet;
        Main.Log.Log("changed anim set");
        Main.Log.Log(FusedDemon.View.gameObject.GetComponentInChildren<UnitAnimationManager>().AnimationSet.name);
        FusedDemon.View.AnimationManager.WalkSpeedType = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionLocoMotion.WalkSpeedType.Normal;
        FusedDemon.View.AnimationManager.AnimationRace = StandardGlab.View.gameObject.GetComponentInChildren<UnitAnimationManager>().AnimationRace;
        FusedDemon.View.AnimationManager.ResetLocoMotion();
        FusedDemon.View.AnimationManager.Start();
    }
}

public class ExposedBrainsStatMod : UnitFactComponentDelegate, ISubscriber, ITickEachRound
{




    public override void OnActivate()
    {

    }

    public override void OnTurnOn()
    {

        int HPLoss = base.Owner.MaxHP - base.Owner.HPLeft;
        HPLoss *= -1;
        base.Owner.Stats.GetStat(StatType.Intelligence).AddModifierUnique(HPLoss/5, base.Runtime, ModifierDescriptor.StatDrain);
        base.Owner.Stats.GetStat(StatType.Wisdom).AddModifierUnique(HPLoss/10, base.Runtime, ModifierDescriptor.StatDrain);
    }

    public override void OnTurnOff()
    {
        base.Owner.Stats.GetStat(StatType.Intelligence).RemoveModifiersFrom(base.Runtime);
        base.Owner.Stats.GetStat(StatType.Wisdom).RemoveModifiersFrom(base.Runtime);
    }

    public void OnNewRound()
    {
        base.Owner.Stats.GetStat(StatType.Intelligence).RemoveModifiersFrom(base.Runtime);
        base.Owner.Stats.GetStat(StatType.Wisdom).RemoveModifiersFrom(base.Runtime);
        int HPLoss = base.Owner.MaxHP - base.Owner.HPLeft;
        HPLoss *= -1;
        base.Owner.Stats.GetStat(StatType.Intelligence).AddModifierUnique(HPLoss / 5, base.Runtime, ModifierDescriptor.StatDrain);
        base.Owner.Stats.GetStat(StatType.Wisdom).AddModifierUnique(HPLoss / 10, base.Runtime, ModifierDescriptor.StatDrain);
    }
}

public class BrainweaverSetupCaster : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
{


    public int level;

    public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;


    public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
    {
        if (evt.Spell != null)
        {
            evt.ReplaceCasterLevel = level;
            evt.ReplaceStat = StatType.Intelligence;
        }
    }

    public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
    {
    }
}

public class Lobotomised : UnitFactComponentDelegate
{
    public override void OnTurnOn()
    {
        base.Owner.Stats.Intelligence.RetainDisabled();
        base.Owner.Stats.Wisdom.RetainDisabled();
        base.Owner.Stats.Charisma.RetainDisabled();
    }

    public override void OnTurnOff()
    {
        base.Owner.Stats.Intelligence.ReleaseDisabled();
        base.Owner.Stats.Wisdom.ReleaseDisabled();
        base.Owner.Stats.Charisma.ReleaseDisabled();
    }
}
