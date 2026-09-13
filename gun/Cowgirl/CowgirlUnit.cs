using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.StoryEx;
using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.Configurators.DialogSystem;
using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Customization;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder.StoryEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Assets;
using gun.Classes.Gunslinger;
using gun.Classes.Spellscar_Drifter;
using gun.Firearms;
using gun.Plot;
using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.CharGen;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.ContextData;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.Dungeon;
using Kingmaker.Dungeon.Units;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.GameModes;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.Localization;
using Kingmaker.PubSubSystem;
using Kingmaker.QA.Statistics;
using Kingmaker.ResourceLinks;
using Kingmaker.ResourceManagement;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Components;
using Kingmaker.UnitLogic.Customization;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Kingmaker.View;
using Kingmaker.Visual.CharacterSystem;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using static Kingmaker.UnitLogic.Class.LevelUp.DollState;
using static Kingmaker.UnitLogic.Class.LevelUp.LevelUpController;
 

namespace gun.Cowgirl
{
    internal static class CowgirlUnit
    {
        public const string GUID = "16bd150344a748a6b41dddcb808b27b3";
        public const string FeaturesGUID = "3239ec314a7148f082f8fd187635c54f";
        public const string HorseFeaturesGUID = "ce173170541b42839ac437dc98d0dcd6";
        public const string Dialog = "5cf54737ebe34892b8f17e0e6bb02a90";
        public static string[] RecruitmentDialogCue = {
            "01f55957aab847348589aae553063395",
            "2fc88e83b94c4f44af66a7c11862b720",
            "0fd1588e036f4adfa7f28fc7768d9d16",
        };
        public static string[] RecruitmentDialogAnswer = {
            "a4c7b81592c24a3688bfdc0af4bbb9dc",
            "a4de8b190727433a93aace3fe0faa16d",
            "e312bde944ab4b4e88e74f5e9286b390",
        };
        public static string[] DialogCue = {
            "e00f21c6fd2649248cbfb1961487b245",
            "4494b20f113a4368a043b47688595869",
            "c41dbe2e08ad49abbc6329c1feb2a0f8",
            "ac8a59addec44152a12c0ec6dafd1c71",
            "b6ba0a8332d64fb586e51c9be52479d2",
            "777725173d614368aca0dac51d7b327c",
            "f42a7c2b4e1540b8b1787ebf14d1599a",
            "f4a0b6184d4448878e0a7b52ed6a51ac",
            "ab6550d752fc4f6695862ce0d6c3078d",
            "3a32ca31d74b4f7eb76c1c299160584a",
            "4246b89d8f104e4f9e3da93e0a71b871",
            "113749104d6d4b9fb23793abb203cf96",
            "6ff925ca895a4f8e9c05c87792158b26",
            "ad4c2f8c0597421fad25ec414b879b4b",
            "023dfc1ece28454a9ed24dc3e20c4ff3",
            "9e47b3bf723e4eaea8807b118daddfd7",
            "0d9f338f4ff64a8b99ddfa9ed975494d",
            "8c8850c4f866414787839dd8cb409f72",
            "09e33bb218af44b2a76b8014c5d19bf1",
            "c70aa07272614b8097c3c23a46261e70",
            "c3f1b57c307045018197bcfb0dddcfde",
            "f4474df48c924f3585983b36e847df0e",
            "e428790a2fa24114ad31f60aa0aaca99",
            "c4a79847f1b34b94b26a77338ae1c1aa",
            "6c3db6f754e54afebd77f4fa2fd7c9f0",
            "528860d4401b4369bb95fdbc6cadbcf1",
            "69e86b98817140459112d4c67f5dc9e4",
            "0d8665104a7c4181a85aaa8a3112aa78",
            "1a74941e9bbd48edba3e1d5019c4a971",
            "f5876e2339ec48669d2c62652d04a07b",
            "16372b32cd184878bc72958cd46c7ff2",
            "5194e54d73ca42cb9c9f027f746b51c6",
            "48088c72213b40d885e284c60292a586",
            "828d0db93f18485da72030543ea9e72e",
            "217fe791d1704c3cb3ec1e5f05b2b248",
            "831e63976a5e44c796228897a7a2fccb",
        };
        public static string[] DialogAnswer = {
            "815c3e4841684ba78544e2f09013b295",
            "139103f72a10475da70aa4370351a860",
            "e5f2396341b24ac1ac8804d5798d6d40",
            "76a56ab1a3c54eaca5c3f292cab879e9",
            "89283716565d40379d8d63de2e53b010",
            "fea07db4f8404ad388bad7affff91342",
            "6289ce8ed0c24f1da164dc7389241031",
            "90acd16e10c64c9f8fec38ea49dd4c30",
            "5a0f89bb82ae4aa59273f3a72cc5abe4",
            "9efe1c6296344cf097f8321ca4432923",
            "a183194428e4425195e1226f99d62168",
            "1eeca65c613c441793c72e5333a02b0c",
            "37bf70df453a416e89553bf06423eefc",
            "1391356ff86c4a7eab40b6ff1a94e13a",
            "cb54234d7f754974bb24d0b695f4f4c9",
        };
        public static string[] DialogCheck = {
            "83aed4ca32cb4e3db007576c0529fb2e",
            "a5acd0dd24094ff988aecad63d875865",
        };
        public static string[] CompanionStoryGUID =
        {
            "5216b6b6ec2143eaacd9b8f4a0dee196"
        };

        public static void Configure()
        {
            Secrets.Configure();
            CompanionStory();
            UnitConfigurator Cowgirl = UnitConfigurator.New("CowgirlUnit", GUID).CopyFrom(UnitHelper.CustomCompanion());
            Cowgirl.AddUnitIsStoryCompanion();
            Cowgirl.SetDisplayName(LocalizationTool.GetString("Cowgirl.Name"));
            Cowgirl.SetGender(Gender.Female);
            //Cowgirl.SetColor(); Not sure what to do with this one
            Cowgirl.SetRace(BlueprintTool.GetRef<BlueprintRaceReference>("0a5d473ead98b0646b94495af250fdc4"));//human (sort of)
            //will need to make a portrait
            CowgirlPortrait.Configure();
            Cowgirl.SetPortrait(CowgirlPortrait.GUID);

            Cowgirl.SetStrength(8);
            Cowgirl.SetDexterity(18);
            Cowgirl.SetConstitution(10);
            Cowgirl.SetIntelligence(12);
            Cowgirl.SetWisdom(14);
            Cowgirl.SetCharisma(16);
            


            Cowgirl.SetSpeed(Kingmaker.Utility.FeetExtension.Feet(30));
            Cowgirl.SetAlignment(Kingmaker.Enums.Alignment.ChaoticGood);

            SharedStringAsset Name = SharedStringAsset.CreateInstance<SharedStringAsset>();
            Name.String = LocalizationTool.GetString("Cowgirl.Name");
            Cowgirl.SetLocalizedName(Name);
            AssetLink<UnitViewLink> prefab = "851e563d9b9640e489faa376a3b722f4";
            Cowgirl.SetPrefab(prefab);

            FeatureConfigurator CowgirlFeatures = FeatureConfigurator.New("CowgirlFeatuers", FeaturesGUID);
            CowgirlFeatures.SetHideInUI(true);
            CowgirlFeatures.SetHideNotAvailibleInUI(true);
            CowgirlFeatures.SetHideInCharacterSheetAndLevelUp(true);
            CowgirlFeatures.AddFacts([Secrets.MonsterTemplateGUID]);

            CowgirlFeatures.AddClassLevels(archetypes: [SpellscarDrifter.SpellscarDrifterGUID], characterClass: "3adc3439f98cb534ba98df59838f02c7", doNotApplyAutomatically: false, levels: 10, levelsStat: Kingmaker.EntitySystem.Stats.StatType.Dexterity, raceStat: Kingmaker.EntitySystem.Stats.StatType.Charisma, selections: DefineClassChoices(), skills: [StatType.SkillKnowledgeArcana, StatType.SkillMobility, StatType.SkillPerception, StatType.SkillUseMagicDevice, StatType.SkillPersuasion, StatType.SkillLoreReligion]);
            CowgirlFeatures.Configure();



            FeatureConfigurator HorseFeatures = FeatureConfigurator.New("CowgirlHorseFeatuers", HorseFeaturesGUID);
            HorseFeatures.SetHideInUI(true);
            HorseFeatures.SetHideNotAvailibleInUI(true);
            HorseFeatures.SetHideInCharacterSheetAndLevelUp(true);
            //HorseFeatures.addcomponent
            HorseFeatures.AddClassLevels(characterClass: "26b10d4340839004f960f9816f6109fe", doNotApplyAutomatically: false, levels: 10, levelsStat: Kingmaker.EntitySystem.Stats.StatType.Strength, raceStat: Kingmaker.EntitySystem.Stats.StatType.Strength, selections: DefineHorseChoices(), skills: [StatType.SkillMobility, StatType.SkillPerception]);
            HorseFeatures.Configure();

            List<BlueprintItemReference> StartingEquipment = new List<BlueprintItemReference>()
            {
                BlueprintTool.GetRef<BlueprintItemReference>(Rifle.BasicItemIDs[3]),
                BlueprintTool.GetRef<BlueprintItemReference>("fb4e768611f820b4186a004cbd70ead6")
            }
            ;



            Cowgirl.AddFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(FeaturesGUID)]);
            Cowgirl.AddFeatureToPet(HorseFeaturesGUID);

            DialogOnClick CowgirlInteraction = new DialogOnClick();
            CowgirlInteraction.m_Dialog = BlueprintTool.GetRef<BlueprintDialogReference>(Dialog);
            CowgirlInteraction.TriggerOnApproach = false;
            CowgirlInteraction.Conditions = ConditionsBuilder.New().AddTrue().Build();
            Cowgirl.AddComponent(CowgirlInteraction);
            Cowgirl.Configure();
            GeneralDialogue();
            RecruitmentDialog();
        }

        private static void CompanionStory()
        {
            CompanionStoryConfigurator.New("CowgirlStory0", CompanionStoryGUID[0])
                .SetCompanion(GUID)
                .SetGender(Gender.Female)
                .SetTitle("Cowgirl.CompanionStory.0.Title")
                .SetDescription("Cowgirl.CompanionStory.0.Text")
                .Configure();
        }
        private static void GeneralDialogue()
        {//this one is what she normal has as dialogue in Drezen (might move this to another class since it's not exclusive to Act 3
            DialogConfigurator CowgirlDialog = DialogConfigurator.New("CowgirlDialgoue", Dialog);
            CowgirlDialog.SetType(DialogType.Common);
            CowgirlDialog.SetFirstCue(Utilities.MakeCueSelection(Act3.CowgirlQuest1DialogueCues[0], DialogCue[0], RecruitmentDialogCue[0]));
            CowgirlDialog.Configure();

            CueConfigurator.New("CowgirlGeneral0", DialogCue[0])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.0"))
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.CowgirlInParty, 999, 1))//only shows up if she has been recruited
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA0", DialogAnswer[0])//entrance to ask about her
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.0"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[1]))
                .Configure();
            #region AskAboutHer

            CueConfigurator.New("CowgirlGeneral1", DialogCue[1])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.1"))
               .SetAnswers(DialogAnswer[1], DialogAnswer[5], DialogAnswer[6], DialogAnswer[7])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA1", DialogAnswer[1])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.1"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[2]))
                .Configure();

            CueConfigurator.New("CowgirlGeneral2", DialogCue[2])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.2"))
               .SetAnswers(DialogAnswer[1], DialogAnswer[2], DialogAnswer[3], DialogAnswer[5], DialogAnswer[6], DialogAnswer[7])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA2", DialogAnswer[2])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.2"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[3]))
                .Configure();

            CueConfigurator.New("CowgirlGeneral3", DialogCue[3])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.3"))
               .SetAnswers(DialogAnswer[1], DialogAnswer[2], DialogAnswer[3], DialogAnswer[5], DialogAnswer[6], DialogAnswer[7])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA3", DialogAnswer[3])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.3"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[4]))
                .Configure();

            CueConfigurator.New("CowgirlGeneral4", DialogCue[4])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.4"))
               .SetAnswers(DialogAnswer[1], DialogAnswer[2], DialogAnswer[3], DialogAnswer[4], DialogAnswer[5], DialogAnswer[6], DialogAnswer[7])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA4", DialogAnswer[4])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.4"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[5]))
                .Configure();

            CueConfigurator.New("CowgirlGeneral5", DialogCue[5])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.5"))
               .SetAnswers(DialogAnswer[1], DialogAnswer[2], DialogAnswer[3], DialogAnswer[5], DialogAnswer[6], DialogAnswer[7])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA5", DialogAnswer[5])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.5"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[6]))
                .Configure();

            CueConfigurator.New("CowgirlGeneral6", DialogCue[6])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.6"))
               .SetAnswers(DialogAnswer[1], DialogAnswer[5], DialogAnswer[6], DialogAnswer[7])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA6", DialogAnswer[6])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.6"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[7]))
                .Configure();

            CueConfigurator.New("CowgirlGeneral7", DialogCue[7])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.7"))
               .SetAnswers(DialogAnswer[1], DialogAnswer[5], DialogAnswer[6], DialogAnswer[7])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA7", DialogAnswer[7])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.7"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[8]))
                .Configure();



            #endregion

            CueConfigurator.New("CowgirlGeneral8", DialogCue[8])//exit from ask about her
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.8"))
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA8", DialogAnswer[8])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.8"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[9]))
                .Configure();

            CueConfigurator.New("CowgirlGeneral9", DialogCue[9])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.9"))
               .SetContinueValue(Utilities.MakeCueSelection(DialogCheck[0]))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .Configure();

            CheckConfigurator.New("CowgirlGeneralC0", DialogCheck[0])
                .SetDC(30)
                .SetExperience(DialogExperience.NormalExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillPerception)
                .SetSuccess(DialogCue[10])
                .SetFail(DialogCue[11])
                .Configure();

            CueConfigurator.New("CowgirlGeneral10", DialogCue[10])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.10"))
               .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.SuspectsCowgirl,true,Utilities.MakeIntConstant(1)))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();

            CueConfigurator.New("CowgirlGeneral11", DialogCue[11])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.11"))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA9", DialogAnswer[9])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.9"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[12]))
                .Configure();

            CueConfigurator.New("CowgirlGeneral12", DialogCue[12])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.12"))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA10", DialogAnswer[10])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.10"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[13]))
                .SetShowConditions(ConditionsBuilder.New().FlagInRange(Flags.SuspectsCowgirl,999,1))
                .Configure();

            CueConfigurator.New("CowgirlGeneral13", DialogCue[13])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.13"))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA11", DialogAnswer[11])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.11"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[14], DialogCue[15]))
                .Configure();

            CueConfigurator.New("CowgirlGeneral14", DialogCue[14])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.14"))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.TreatedFleshwarps,999,1))
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();

            CueConfigurator.New("CowgirlGeneral15", DialogCue[15])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.15"))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA12", DialogAnswer[12])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.12"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[16]))
                .SetMythicRequirement(Mythic.PlayerIsAeon)
                .Configure();

            CueConfigurator.New("CowgirlGeneral16", DialogCue[16])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.16"))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetContinueValue(Utilities.MakeCueSelection(DialogCheck[1]))
               .Configure();

            CheckConfigurator.New("CowgirlGeneralC1", DialogCheck[1])
                .SetDC(32)
                .SetExperience(DialogExperience.NormalExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillPerception)
                .SetSuccess(DialogCue[17])
                .SetFail(DialogCue[18])
                .Configure();

            CueConfigurator.New("CowgirlGeneral17", DialogCue[17])
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.17"))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();

            CueConfigurator.New("CowgirlGeneral18", DialogCue[18])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();

            AnswerConfigurator.New("CowgirlGeneralA13", DialogAnswer[13])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.13"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[19]))
                .Configure();
            #region AskAboutMythic

            CueConfigurator.New("CowgirlGeneral19", DialogCue[19])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.19"))
               .SetContinueValue(Utilities.MakeCueSelection(DialogCue[20]))
              
               .Configure();

            #region AngelAzataGoldDragon
            CueConfigurator.New("CowgirlGeneral20", DialogCue[20])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.20"))
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Good)//if you are good aligned and
                .AddOrAndLogic(ConditionsBuilder.New().UseOr()//either
                    .EtudeStatus(etude:BlueprintTool.GetRef<BlueprintEtudeReference>("d3b47e973d65c6c46af1cce815d1f6ce"),playing:true)//azata
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("3a82aba4de71b89458ac82949ed957c4"), playing: true)//angel
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("9b193d30c89a20b409fd3dda9bd109bf"), playing: true)//or Gold Dragon
               ))
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            CueConfigurator.New("CowgirlGeneral21", DialogCue[21])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.21"))
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Good,true).AlignmentCheck(AlignmentComponent.Evil, true)//if you are neither good nor evil aligned and
                .AddOrAndLogic(ConditionsBuilder.New().UseOr()//either
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("d3b47e973d65c6c46af1cce815d1f6ce"), playing: true)//azata
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("3a82aba4de71b89458ac82949ed957c4"), playing: true)//angel
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("9b193d30c89a20b409fd3dda9bd109bf"), playing: true)//or Gold Dragon
               ))
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            CueConfigurator.New("CowgirlGeneral22", DialogCue[22])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Evil)//if you are evil aligned and
                .AddOrAndLogic(ConditionsBuilder.New().UseOr()//either
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("d3b47e973d65c6c46af1cce815d1f6ce"), playing: true)//azata
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("3a82aba4de71b89458ac82949ed957c4"), playing: true)//angel
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("9b193d30c89a20b409fd3dda9bd109bf"), playing: true)//or Gold Dragon
               ))
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.22"))
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            #endregion

            #region Aeon
            CueConfigurator.New("CowgirlGeneral23", DialogCue[23])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.23"))
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Good)//if you are good aligned and
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("3a040afde22f4b742a2f607354ab17e7"), playing: true))//aeon
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            CueConfigurator.New("CowgirlGeneral24", DialogCue[24])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.24"))
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Good,true).AlignmentCheck(AlignmentComponent.Evil, true)//if you are neither good nor evil aligned and
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("3a040afde22f4b742a2f607354ab17e7"), playing: true))//aeon
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            CueConfigurator.New("CowgirlGeneral25", DialogCue[25])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Evil)//if you are evil aligned and
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("3a040afde22f4b742a2f607354ab17e7"), playing: true))//aeon
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.25"))
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            #endregion

            #region Trickster
            CueConfigurator.New("CowgirlGeneral26", DialogCue[26])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.26"))
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Good)//if you are good aligned and
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("9f486a9c0c9abfc4a952bb22e88a7e96"), playing: true))//Trickster
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            CueConfigurator.New("CowgirlGeneral27", DialogCue[27])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.27"))
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Good, true).AlignmentCheck(AlignmentComponent.Evil, true)//if you are neither good nor evil aligned and
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("9f486a9c0c9abfc4a952bb22e88a7e96"), playing: true))//Trickster
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            CueConfigurator.New("CowgirlGeneral28", DialogCue[28])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Evil)//if you are evil aligned and
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("9f486a9c0c9abfc4a952bb22e88a7e96"), playing: true))//Trickster
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.28"))
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            #endregion

            #region Demon
            CueConfigurator.New("CowgirlGeneral29", DialogCue[29])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.29"))
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Good)//if you are good aligned and
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("9a3739370f84b0b4196d0e4d326ea3a8"), playing: true))//Demon
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            CueConfigurator.New("CowgirlGeneral30", DialogCue[30])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.30"))
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Good, true).AlignmentCheck(AlignmentComponent.Evil, true)//if you are neither good nor evil aligned and
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("9a3739370f84b0b4196d0e4d326ea3a8"), playing: true))//Demon
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            CueConfigurator.New("CowgirlGeneral31", DialogCue[31])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Evil)//if you are evil aligned and
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("9a3739370f84b0b4196d0e4d326ea3a8"), playing: true))//Demon
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.31"))
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            #endregion

            #region Lich
            CueConfigurator.New("CowgirlGeneral32", DialogCue[32])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.32"))
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Good)//if you are good aligned and
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("11fc5662e0ce8074ea145a022282b879"), playing: true))//Lich
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            CueConfigurator.New("CowgirlGeneral33", DialogCue[33])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.33"))
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Good, true).AlignmentCheck(AlignmentComponent.Evil, true)//if you are neither good nor evil aligned and
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("11fc5662e0ce8074ea145a022282b879"), playing: true))//Lich
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            CueConfigurator.New("CowgirlGeneral34", DialogCue[34])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetConditions(ConditionsBuilder.New().AlignmentCheck(AlignmentComponent.Evil)//if you are evil aligned and
                    .EtudeStatus(etude: BlueprintTool.GetRef<BlueprintEtudeReference>("11fc5662e0ce8074ea145a022282b879"), playing: true))//Lich
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.34"))
               .SetAnswers(DialogAnswer[0], DialogAnswer[8], DialogAnswer[9], DialogAnswer[10], DialogAnswer[11], DialogAnswer[12], DialogAnswer[13], DialogAnswer[14])
               .Configure();
            #endregion


            #endregion

            AnswerConfigurator.New("CowgirlGeneralA14", DialogAnswer[14])
                .SetText(LocalizationTool.GetString("Plot.Cowgirl.Answer.14"))
                .SetNextCue(Utilities.MakeCueSelection(DialogCue[35]))
                .Configure();

            CueConfigurator.New("CowgirlGeneral35", DialogCue[35])
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetText(LocalizationTool.GetString("Plot.Cowgirl.Cue.35"))
               .Configure();

        }

        private static void RecruitmentDialog()
        {
            CueConfigurator.New("CowgirlRecruitment0", RecruitmentDialogCue[0])
               .SetText(LocalizationTool.GetString("Plot.CowgirlRecruitment.Cue.0"))
               .SetAnswers(RecruitmentDialogAnswer[0], RecruitmentDialogAnswer[1], RecruitmentDialogAnswer[2])
               .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.CompletedGatewayToInsanity,999,1))//only shows up if her starting quest is complete
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetShowOnce()
               .Configure();
            AnswerConfigurator.New("CowgirlRecruitmentA0", RecruitmentDialogAnswer[0])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.49"))
                .SetNextCue(Utilities.MakeCueSelection(RecruitmentDialogCue[1]))
                .SetOnSelect(Flags.IncrementFlag(1, Flags.CowgirlInParty))
                .Configure();
            AnswerConfigurator.New("CowgirlRecruitmentA1", RecruitmentDialogAnswer[1])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.50"))
                .SetNextCue(Utilities.MakeCueSelection(RecruitmentDialogCue[1]))
                .SetOnSelect(Flags.IncrementFlag(1, Flags.CowgirlInParty))
                .Configure();
            AnswerConfigurator.New("CowgirlRecruitmentA2", RecruitmentDialogAnswer[2])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.51"))
                .SetNextCue(Utilities.MakeCueSelection(RecruitmentDialogCue[2]))
                .SetOnSelect(Flags.IncrementFlag(1, Flags.RejectedCowgirl))
                .Configure();

            CueConfigurator.New("CowgirlRecruitment1", RecruitmentDialogCue[1])
               .SetText(LocalizationTool.GetString("Plot.CowgirlRecruitment.Cue.1"))
               .SetOnShow(ActionsBuilder.New().Add(new RecruitSpeaker()).UnlockCompanionStory(CompanionStoryGUID[0]))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .Configure();

            CueConfigurator.New("CowgirlRecruitment2", RecruitmentDialogCue[2])
               .SetText(LocalizationTool.GetString("Plot.CowgirlRecruitment.Cue.2"))
               .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.CowgirlInDrezen,true,Utilities.MakeIntConstant(-1)))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .Configure();
        }

        public static SelectionEntry[] DefineClassChoices()
        {
            SelectionEntry OrderSelection = new SelectionEntry();
            OrderSelection.m_Selection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>("d710e30ea20240247ad87ad86bcd50f2");//cavalier order selection
            OrderSelection.m_Features = [BlueprintTool.GetRef<BlueprintFeatureReference>(OrderOfTheEasternStar.GUID)];
            SelectionEntry Horse = new SelectionEntry();
            Horse.m_Selection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>("0605927df6e2fdd42af6ee2424eb89f2");//cavalier order selection
            Horse.m_Features = [BlueprintTool.GetRef<BlueprintFeatureReference>("9dc58b5901677c942854019d1dd98374")];

            SelectionEntry BasicFeats = new SelectionEntry();
            BasicFeats.m_Selection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>("247a4068296e8be42890143f451b4b45");
            BasicFeats.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureReference>("9c928dc570bb9e54a9649b3ebfe47a41"),//1 Rapid Shot
                BlueprintTool.GetRef<BlueprintFeatureReference>("f47df34d53f8c904f9981a3ee8e84892"),//1 (human Bonus) Deadly Aim
                BlueprintTool.GetRef<BlueprintFeatureReference>("0da0c194d6e1d43419eb8d990b28e0ab"),//3 Point-Blank Shot
                BlueprintTool.GetRef<BlueprintFeatureReference>("8f3d1e6b4be006f4d896081f2f889665"),//5 Precise Shot
                BlueprintTool.GetRef<BlueprintFeatureReference>("175d1577bb6c9a04baf88eec99c66334"),//7 Iron Will
                BlueprintTool.GetRef<BlueprintFeatureReference>("7115a6c08bd101247b70d72a4ff99453"),//9 Snap Shot
                BlueprintTool.GetRef<BlueprintFeatureReference>("0f8939ae6f220984e8fb568abbdfba95"),//11 Combat Reflexes
                BlueprintTool.GetRef<BlueprintFeatureReference>("c3453e7e215c1f149b938be27ac754c6"),//13 Improved Snap Shot
                BlueprintTool.GetRef<BlueprintFeatureReference>("f308a03bea0d69843a8ed0af003d47a9"),//15 Mounted Combat
                BlueprintTool.GetRef<BlueprintFeatureReference>("68e814f1f3ce55942a52c1dd536eaa5b"),//17 Indomitable Mount
                BlueprintTool.GetRef<BlueprintFeatureReference>("f7de245bb20f12f47864c7cb8b1d1abb"),//19 Clustered Shots
                ];

            SelectionEntry BonusFeats = new SelectionEntry();
            BonusFeats.m_Selection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>("dd17090d14958ef48ba601688b611970");
            BonusFeats.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureReference>("4c44724ffa8844f4d9bedb5bb27d144a"),//6 combat expertise
                BlueprintTool.GetRef<BlueprintFeatureReference>("46f970a6b9b5d2346b10892673fe6e74"),//12 Improved Precise Shot
                BlueprintTool.GetRef<BlueprintFeatureReference>("67b09c86234cecc4c8309f22f7d33973"),//18 Greater Snap Shot
                ];

            SelectionEntry Background = new SelectionEntry();
            Background.m_Selection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>("f926dabeee7f8a54db8f2010b323383c");
            Background.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureReference>("7d300f497584d9245ac24c062dce0bd6")//None (she has the child of yog thing instead)
                ];

            SelectionEntry Deity = new SelectionEntry();
            Deity.m_Selection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>("59e7a76987fe3b547b9cce045f4db3e4");
            Deity.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureReference>("c1c4f7f64842e7e48849e5e67be11a1b")//Sarenrae
                ];

            SelectionEntry AnimalArchetype = new SelectionEntry();
            AnimalArchetype.m_Selection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>("65af7290b4efd5f418132141aaa36c1b");
            AnimalArchetype.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureReference>("e7d53709a33bd9b45a89536ed6766264")//Default
                ];

            SelectionEntry[] CowgirlSelections = { OrderSelection, BasicFeats, BonusFeats, Background, Deity, Horse, AnimalArchetype };
            return CowgirlSelections;
        }

        public static SelectionEntry[] DefineHorseChoices()
        {

            SelectionEntry BasicFeats = new SelectionEntry();
            BasicFeats.m_Selection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>("247a4068296e8be42890143f451b4b45");
            BasicFeats.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureReference>("175d1577bb6c9a04baf88eec99c66334"),//1 Iron Will
                BlueprintTool.GetRef<BlueprintFeatureReference>("3ea2215150a1c8a4a9bfed9d9023903e"),//3 Improved Iron Will
                BlueprintTool.GetRef<BlueprintFeatureReference>("97e216dbb46ae3c4faef90cf6bbe6fd5"),//5 Dodge
                BlueprintTool.GetRef<BlueprintFeatureReference>("2a6091b97ad940943b46262600eaeaeb"),//7 combat mobility
                BlueprintTool.GetRef<BlueprintFeatureReference>("0f8939ae6f220984e8fb568abbdfba95"),//9 Combat Reflexes
                ];


            SelectionEntry[] HorseSelection = { BasicFeats };
            return HorseSelection;
        }

        public static DialogSpeaker GetSpeaker()
        {
            DialogSpeaker CowgirlSpeaker = new DialogSpeaker();
            CowgirlSpeaker.m_Blueprint = BlueprintTool.GetRef<BlueprintUnitReference>(GUID);
            CowgirlSpeaker.m_SpeakerPortrait = BlueprintTool.GetRef<BlueprintUnitReference>(GUID);
            return CowgirlSpeaker;
        }

    }
    public class RecruitSpeaker : GameAction
    {
        public override string GetCaption()
        {
            return "Recruiting Cowgirl Speaker";
        }

        public override void RunAction()
        {
            UnitEntityData Speaker = new DialogCurrentSpeaker().GetValue();
            UnitEntityData companion = GameHelper.RecruitNPC(Speaker, BlueprintTool.Get<BlueprintUnit>(CowgirlUnit.GUID));
            AddItemToPlayer CowgirlEquipment = new AddItemToPlayer();
            CowgirlEquipment.m_ItemToGive = BlueprintTool.GetRef<BlueprintItemReference>(Rifle.BasicItemIDs[3]);
            CowgirlEquipment.Equip = true;
            CowgirlEquipment.Silent = true;
            CowgirlEquipment.Identify = true;
            CowgirlEquipment.EquipOn = new EvaluatorUnit(companion);
            CowgirlEquipment.PreferredWeaponSet = 0;
            CowgirlEquipment.RunAction();
            CowgirlEquipment.m_ItemToGive = BlueprintTool.GetRef<BlueprintItemReference>("fb4e768611f820b4186a004cbd70ead6");
            CowgirlEquipment.RunAction();
            
            int experience = Game.Instance.Player.MainCharacter.Value.Descriptor.Progression.Experience;
            companion.Descriptor.Progression.AdvanceExperienceTo(experience, log: false);
           


        }
        public static void RecruitCompanion(UnitEntityData unit)//copied this from toybox
        {
            var currentMode = Game.Instance.CurrentMode;
            unit = GameHelper.RecruitNPC(unit, unit.Blueprint);

            if (currentMode == GameModeType.Default || currentMode == GameModeType.Pause)
            {
                var pets = unit.Pets;
                unit.IsInGame = true;
                unit.LeaveCombat();
                //unit.GroupId = Game.Instance.Player.MainCharacter.Value.GroupId;
                //Game.Instance.Player.CrossSceneState.AddEntityData(unit);
                if (unit.IsDetached)
                {
                    Game.Instance.Player.AttachPartyMember(unit);
                }
                foreach (var pet in pets)
                {
                    pet
                            .Entity!
                            .Position = unit.Position;
                }
            }
        }
    }
    public class RecruitCowgirlOld : GameAction
    {
        public override string GetCaption()
        {
            return "RecruitingCowgirl";
        }

        public override void RunAction()
        {
            //iset the flags for her being in drezen and in the party
            ActionsBuilder.New().IncrementFlagValue(gun.Plot.Flags.CowgirlInParty, true, Utilities.MakeIntConstant(1)).IncrementFlagValue(gun.Plot.Flags.CowgirlInDrezen, true, Utilities.MakeIntConstant(1)).Build().Run();

            //spawn a CowgirlUnit

            UnitEntityData Cowgirl = Game.Instance.EntityCreator.SpawnUnit(BlueprintTool.Get<BlueprintUnit>(gun.Cowgirl.CowgirlUnit.GUID), GameHelper.GetPlayerCharacter().View.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)), Game.Instance.State.GetStateForArea(BlueprintTool.Get<BlueprintArea>("2183cc056a7b5d647ad475c8bc6c2074")).MainState);
            Cowgirl.Body.TryInsertItem(BlueprintTool.Get<BlueprintItem>(Rifle.BasicItemIDs[2]), Cowgirl.Body.EquipmentSlots[0]);

            Game.Instance.EntityCreator.Tick();
            Game.Instance.EntityCreator.Tick();


            //and add it as a companion (I sure hope this works)
            Game.Instance.Player.AddCompanion(Cowgirl);
            Game.Instance.Player.PartyAndPets.Add(Cowgirl.GetPet(PetType.AnimalCompanion));
            EventBus.RaiseEvent(delegate (IPartyHandler h)
            {
                h.HandleAddCompanion(Cowgirl.GetPet(PetType.AnimalCompanion));
            });
            int experience = Game.Instance.Player.MainCharacter.Value.Descriptor.Progression.Experience;
            Cowgirl.Descriptor.Progression.AdvanceExperienceTo(experience, log: false);

            //give her some starting equipment
            AddItemToPlayer Weapon = new AddItemToPlayer();
            Weapon.m_ItemToGive = BlueprintTool.GetRef<BlueprintItemReference>(Rifle.BasicItemIDs[3]);
            Weapon.Equip = true;
            Weapon.EquipOn = new EvaluatorUnit(Cowgirl);
            Weapon.RunAction();

            AddItemToPlayer Armor = new AddItemToPlayer();
            Armor.m_ItemToGive = BlueprintTool.GetRef<BlueprintItemReference>("f251cb7bbf176e340893b9e40f9a7c29");
            Armor.Equip = true;
            Armor.EquipOn = new EvaluatorUnit(Cowgirl);
            Armor.RunAction();
           
            EventBus.Subscribe(this);

        }

        public static ActionsBuilder Builder()
        {
            return ActionsBuilder.New().Add(new RecruitCowgirlOld());
        }
    }
}

public class EvaluatorUnit : UnitEvaluator
{
    public UnitEntityData UnitData { get; set; }
    public override UnitEntityData GetValueInternal()
    {
        return UnitData;
    }

    public override string GetCaption()
    {
        return "Scripted Unit";
    }

    public EvaluatorUnit (UnitEntityData unit)
    {
        UnitData = unit;
    }
}
