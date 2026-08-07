using BlueprintCore.Blueprints.Configurators.DialogSystem;
using BlueprintCore.Blueprints.Configurators.Globalmap;
using BlueprintCore.Utils;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gun;
using Kingmaker.Blueprints;
using BlueprintCore.Actions.Builder;
using Kingmaker.ElementsSystem;
using BlueprintCore.Actions.Builder.KingdomEx;
using BlueprintCore.Blueprints.References;
using Kingmaker.Crusade.GlobalMagic;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.BasicEx;
using Kingmaker.Globalmap.View;
using BlueprintCore.Blueprints.Configurators.RandomEncounters;
using Kingmaker.Globalmap.State;
using BlueprintCore.Conditions.Builder;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;

namespace gun.Plot
{
    internal static class Act2
    {
        public const string CowgirlMeetingLocation = "293f6e4355864253b7e99c92e4622d14";
        public const string CowgirlMeetingEvent = "df3e9749e87e477db611face07558f30";
        public static string[] Pages = { 
            "e2cacd5222f142e08508fd336cb845e6",
            "e86fa0a069cb433aa79cb16842fc9578",
            "e04a858a34bc4c309b6d63dfe87a52ea",
            "b5f47ee9b0744aadaaf4732fa6a7a3c0",
            "bd45db8b5b014c13b97a71178f2f2214",
            "91f5f54f18974c66b6d4ff6fc69a25d5"
        };
        public static string[] cueIDs = { 
            "6b0369c28ea14c5199c037e737e29761",
            "31509b6331be48449d7f6a038e61a4ed",
            "4e28f64ba28b4b96b14b9c261c552405",
            "e064da30edfd4a15ab7a18a2d7d9e46b",
            "993c25fdae6e47f4bd56724cd2e0a3d7",
            "6ed10c24b6bd4cabaf1193481d48f777",
            "5a4bbd6dda044682964cb684a71f8e3a",
            "246e779199f54e8aa55fc88e7e9a7ce3",
            "0f6a43c73fd64cffb30f1ddc6526579b",
            "439ee89166ce42a28342dd43274d30ca",
            "ff628a1ecec2464f867e20603e6cb42e",
            "1f7054eccd164aa3a1ef86607ec8f013",
            "de8236ff99314836a905121fccf01613",
            "eb4355283611418da8cafd6fe6bfdc9d",
            "5a7a8ac4b5994414a7782de001a14e07",
            "a9bd2b81f848427da724d3d4677403fc",
            "36bb80b798c4491d88cce9099bae6430",
            "940af6fce5aa472f9aa65d35141784b3",
            "cb812f7c039a47d298e138a8d1e9b9c1",
            "5d81baf65e5444028f2a287cf7306f28",
            "ed4a265821d147f783864cc262aad61c",
            "8b1d6a04ea284fc49cf25d3ff6795f7f",
            "80a25d5a3fea4862aa21cab812546dd5",
            "82bc27de117b422ebb1d1f5ac7779016",
            "a64fe972a0914b17a16621721a4e55a7",
            "8a404e655830485e8d2ec0cad80fab09",
            "2f8aaaa9977a46f7bb03dcfccc1dc235",
            "14acf86e922e47cbb99fbb7c225f44ed",
            "ca678c0f39bd4e0eb97370772e91e1cb",
            "a162e1a4b0bd4ba4848999eddce27036" 
        };
        public static string[] AnswerIDs = { 
            "817cd952f6be4680a5ce8402edb0ea23",
            "d68c23917a454da4b907721673ab2664",
            "9bf9ff3b7642475797d4308f560271a3",
            "5395017e7db34dd586a312d43450dfbd",
            "9581946380fa476ab47040b7f49e29c4",
            "6b73e2cbd3214e3485c4e3033489397a",
            "7de984245e8648918ccd099c6d0bd11c",
            "cdd36f16c9f641fd963f336cfb8345fc",
            "f06a758b383644719d3d52e9e00dfc9d",
            "5e231bb96fd04030b6853cc5c9f399cd",
            "b38f1decf99348e0955df4590e8e5cb2",
            "25420bb320d24e6987f23ed0b27476ce",
            "dc149ebfce4740a2ae31064a801062e8",
            "dfd0112c9a714c6191267a01163cda61",
            "5279a256249542b4bea1539b87e19045",
            "b94db7f9cc5c4d8cb505cf3f67c83f8b",
            "b9a6564021ba4af7b2ea59e9efb9ea74",
            "1d545e7d31604847a017efbe797a6e9f"
        };
        public static string[] CheckIDs = { 
            "36c758e16cba484eaf10dde526ec9db1",
            "4d59bbd2e89b400fbdd9b0ff91f93973",
            "0a02fdebc909494e961a4b8d63795f97",
            "005328423b46409096930346ee1a2dd6",
            "1d8279fe89a64471b24de9a947ab57f6",
            "0d2baad19ad44efeb6c8a2ddef792b73",
            "2a09028d33d64cdd890785bf6fb26906"
        };
        public static void Configure()
        {
            SetMap();
            SetupDialogue();
            Page0();
            Page1();
            Page2();
            Page3();
            Page4();

        }

        private static void SetMap()
        {
            //creates the map point for the act 2 encounter
            GlobalMapPointConfigurator Act2EncounterPoint = GlobalMapPointConfigurator.New("CowgirlMeetMap", CowgirlMeetingLocation);
            Act2EncounterPoint.SetType(Kingmaker.Globalmap.Blueprints.GlobalMapPointType.Location);
            Act2EncounterPoint.SetBookEvent(CowgirlMeetingEvent);
            Act2EncounterPoint.Configure();
        }

        private static void SetupDialogue()
        {
            //creates the dialogue which will hold the act 2 encounter
            DialogConfigurator CowgirlMeetingDialog = DialogConfigurator.New("CowgirlMeetStorybook", CowgirlMeetingEvent);
            CowgirlMeetingDialog.SetType(DialogType.Book);
            CowgirlMeetingDialog.SetFirstCue(Utilities.MakeCueSelection(Pages[0]));
            CowgirlMeetingDialog.SetStartActions(Flags.IncrementFlag(1,Flags.MetCowgirl));
            CowgirlMeetingDialog.Configure();


        }

        private static void Page0()
        {
            //creates the first page
            BookPageConfigurator.New("CowgirlMeetBook0", Pages[0])
                .SetTitle(LocalizationTool.GetString("Plot.CowgirlMeeting.Title"))
                .SetCues(cueIDs[0], cueIDs[1], cueIDs[2], cueIDs[3], cueIDs[4], cueIDs[5], cueIDs[6], cueIDs[7])
                .SetAnswers(AnswerIDs[0], AnswerIDs[1], AnswerIDs[2], AnswerIDs[3])
                .Configure();

            //create the cues for the first page
            CueConfigurator.New("CowgirlMeet0", cueIDs[0])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.0"))
                .Configure();
            CueConfigurator.New("CowgirlMeet1", cueIDs[1])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.1"))
                .Configure();

            #region Answer0Perception
            CheckConfigurator.New("CowgirlMeetC0", CheckIDs[0])
                .SetDC(22)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillPerception)
                .SetSuccess(Pages[0])
                .SetFail(Pages[0])
                .Configure();



            AnswerConfigurator.New("CowgirlMeetA0", AnswerIDs[0])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.0"))
                .SetNextCue(Utilities.MakeCueSelection(CheckIDs[0]))
                .SetShowConditions(Utilities.MakeSeenCue(true, cueIDs[2], cueIDs[3]))
                .Configure();

            CueConfigurator.New("CowgirlMeet2", cueIDs[2])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.2"))
                .SetShowOnce()
                .SetConditions(Utilities.MakePassedCheck(true, CheckIDs[0]))
                .Configure();

            CueConfigurator.New("CowgirlMeet3", cueIDs[3])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.3"))
                .SetShowOnce()
                .SetConditions(Utilities.MakePassedCheck(false, CheckIDs[0]))
                .Configure();

            #endregion

            #region Answer1KnowledgeWorld
            CheckConfigurator.New("CowgirlMeetC1", CheckIDs[1])
                .SetDC(24)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeWorld)
                .SetSuccess(Pages[0])
                .SetFail(Pages[0])
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA1", AnswerIDs[1])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.1"))
                .SetNextCue(Utilities.MakeCueSelection(CheckIDs[1]))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("CowgirlMeet4", cueIDs[4])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.4"))
                .SetConditions(Utilities.MakePassedCheck(true, CheckIDs[1]))
                .Configure();

            CueConfigurator.New("CowgirlMeet5", cueIDs[5])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.5"))
                .SetConditions(Utilities.MakePassedCheck(false, CheckIDs[1]))
                .Configure();
            #endregion

            #region Answer1KnowledgeArcana
            CheckConfigurator.New("CowgirlMeetC2", CheckIDs[2])
                .SetDC(20)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana)
                .SetSuccess(Pages[0])
                .SetFail(Pages[0])
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA2", AnswerIDs[2])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.2"))
                .SetNextCue(Utilities.MakeCueSelection(CheckIDs[2]))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("CowgirlMeet6", cueIDs[6])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.6"))
                .SetConditions(Utilities.MakePassedCheck(true, CheckIDs[2]))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("CowgirlMeet7", cueIDs[7])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.7"))
                .SetConditions(Utilities.MakePassedCheck(false, CheckIDs[2]))
                .SetShowOnce()
                .Configure();
            #endregion

            AnswerConfigurator.New("CowgirlMeetA3", AnswerIDs[3])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.3"))
                .SetNextCue(Utilities.MakeCueSelection(Pages[1]))
                .SetShowOnce()
                .Configure();
        }

        private static void Page1()
        {
            BookPageConfigurator.New("CowgirlMeetBook1", Pages[1])
                .SetTitle(LocalizationTool.GetString("Plot.CowgirlMeeting.Title"))
                .SetCues(cueIDs[8], cueIDs[9],cueIDs[10], cueIDs[11], cueIDs[12], cueIDs[13])
                .SetAnswers(AnswerIDs[4], AnswerIDs[5], AnswerIDs[17])
                .Configure();

            CueConfigurator.New("CowgirlMeet8", cueIDs[8])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.8"))
                .Configure();

            CueConfigurator.New("CowgirlMeet9", cueIDs[9])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.9"))
                .Configure();

            ActionsBuilder AddFatigue = ActionsBuilder.New();
            AddFatigue.ApplyBuffPermanent(BlueprintTool.GetRef<BlueprintBuffReference>("e6f2fc5d73d88064583cb828801212f4"));

            ActionsBuilder AddFatigueParty = ActionsBuilder.New();
            AddFatigueParty.OnPartyUnits(AddFatigue,Kingmaker.Player.CharactersList.PartyCharacters);

            #region Answer3Athletics
            CheckConfigurator.New("CowgirlMeetC3", CheckIDs[3])
                .SetDC(22)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillAthletics)
                .SetSuccess(Pages[1])
                .SetFail(Pages[1])
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA4", AnswerIDs[4])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.4"))
                .SetShowConditions(Utilities.MakeSelectedAnswer(AnswerIDs[5]))
                .SetNextCue(Utilities.MakeCueSelection(CheckIDs[3]))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("CowgirlMeet10", cueIDs[10])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.10"))
                .SetConditions(Utilities.MakePassedCheck(true, CheckIDs[3]))
                .SetOnShow(Flags.IncrementFlag(1,Flags.CowgirlRespect))
                .Configure();

            CueConfigurator.New("CowgirlMeet11", cueIDs[11])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.11"))
                .SetConditions(Utilities.MakePassedCheck(false, CheckIDs[3]))
                .SetOnShow(AddFatigueParty)
                .Configure();
            #endregion

            #region Answer4Mobility
            CheckConfigurator.New("CowgirlMeetC4", CheckIDs[4])
                .SetDC(22)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillMobility)
                .SetSuccess(Pages[1])
                .SetFail(Pages[1])
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA5", AnswerIDs[5])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.5"))
                .SetShowConditions(Utilities.MakeSelectedAnswer(AnswerIDs[4]))
                .SetNextCue(Utilities.MakeCueSelection(CheckIDs[4]))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("CowgirlMeet12", cueIDs[12])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.12"))
                .SetConditions(Utilities.MakePassedCheck(true, CheckIDs[4]))
                .SetOnShow(Flags.IncrementFlag(1,Flags.CowgirlRespect))
                .Configure();

            CueConfigurator.New("CowgirlMeet13", cueIDs[13])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.13"))
                .SetConditions(Utilities.MakePassedCheck(false, CheckIDs[4]))
                .SetOnShow(AddFatigueParty)
                .Configure();
            #endregion

            AnswerConfigurator.New("CowgirlMeetA17", AnswerIDs[17])
                .SetText(LocalizationTool.GetString("Plot.Continue"))
                .SetShowConditions(Utilities.MakeSeenAny(false, cueIDs[13], cueIDs[12], cueIDs[11], cueIDs[10]))
                .SetNextCue(Utilities.MakeCueSelection(Pages[2]))
                .SetShowOnce()
                .Configure();
        }

        private static void Page2()
        {
            BookPageConfigurator.New("CowgirlMeetBook2", Pages[2])
                .SetTitle(LocalizationTool.GetString("Plot.CowgirlMeeting.Title"))
                .SetCues(cueIDs[14], cueIDs[15], cueIDs[16], cueIDs[17])
                .SetAnswers(AnswerIDs[6], AnswerIDs[7])
                .Configure();

            CueConfigurator.New("CowgirlMeet14", cueIDs[14])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.14"))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("CowgirlMeet15", cueIDs[15])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.15"))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("CowgirlMeet16", cueIDs[16])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.16"))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("CowgirlMeet17", cueIDs[17])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.17"))
                .SetShowOnce()
                .Configure();
            
            AnswerConfigurator.New("CowgirlMeetA6", AnswerIDs[6])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.6"))
                .SetNextCue(Utilities.MakeCueSelection(Pages[3]))
                .SetOnSelect(Flags.IncrementFlag(1,Flags.CowgirlApproval))
                .SetShowOnce()
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA7", AnswerIDs[7])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.7"))
                .SetNextCue(Utilities.MakeCueSelection(Pages[3]))
                .SetShowOnce()
                .Configure();

        }

        private static void Page3()
        {
            BookPageConfigurator.New("CowgirlMeetBook3", Pages[3])
                .SetTitle(LocalizationTool.GetString("Plot.CowgirlMeeting.Title"))
                .SetCues(cueIDs[18], cueIDs[19], cueIDs[20], cueIDs[21])
                .SetAnswers(AnswerIDs[8], AnswerIDs[9], AnswerIDs[10], AnswerIDs[11], AnswerIDs[12])
                .Configure();

            CueConfigurator.New("CowgirlMeet18", cueIDs[18])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.18"))
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA8", AnswerIDs[8])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.8"))
                .SetNextCue(Utilities.MakeCueSelection(Pages[3]))
                .Configure();

            CueConfigurator.New("CowgirlMeet19", cueIDs[19])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.19"))
                .SetConditions(Utilities.MakeSelectedAnswer(AnswerIDs[8]))
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA9", AnswerIDs[9])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.9"))
                .SetNextCue(Utilities.MakeCueSelection(Pages[3]))
                .Configure();

            CueConfigurator.New("CowgirlMeet20", cueIDs[20])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.20"))
                .SetConditions(Utilities.MakeSelectedAnswer(AnswerIDs[9]))
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA10", AnswerIDs[10])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.10"))
                .SetNextCue(Utilities.MakeCueSelection(Pages[3]))
                .Configure();

            CueConfigurator.New("CowgirlMeet21", cueIDs[21])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.21"))
                .SetConditions(Utilities.MakeSelectedAnswer(AnswerIDs[10]))
                .SetContinueValue(Utilities.MakeCueSelection(CheckIDs[5]))
                .Configure();

            CheckConfigurator.New("CowgirlMeetC5", CheckIDs[5])
                .SetDC(25)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillPerception)
                .SetSuccess(cueIDs[22])
                .SetFail(cueIDs[21])
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("CowgirlMeet22", cueIDs[22]) //Note this cue will be one to check in normal dialgue with her as having seen it will unlock an extra option to ask her about it
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.22"))
                .SetConditions(Utilities.MakePassedCheck(true, CheckIDs[5]))
                .SetOnShow(Flags.IncrementFlag(1,Flags.SuspectsCowgirl))
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA11", AnswerIDs[11])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.11"))
                .SetNextCue(Utilities.MakeCueSelection(Pages[4]))
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("CowgirlMeet23", cueIDs[23])//need to move this to next page
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.23"))
                .SetShowOnce()
                .SetConditions(Utilities.MakeSelectedAnswer(AnswerIDs[11]))
                .SetOnShow(ActionsBuilder.New().AddAll(Flags.IncrementFlag(1, Flags.CowgirlApproval).Build()).AddAll(Flags.IncrementFlag(1, Flags.OfferedToHelpCowgirl).Build()))
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA12", AnswerIDs[12])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.12"))
                .SetNextCue(Utilities.MakeCueSelection(Pages[4]))
                .Configure();

            CueConfigurator.New("CowgirlMeet24", cueIDs[24])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.24"))
                .SetShowOnce()
                .SetConditions(Utilities.MakeSelectedAnswer(AnswerIDs[12]))
                .SetOnShow(Flags.IncrementFlag(1, Flags.AskedCowgirlToJoinCrusade))
                .Configure();
        }

        private static void Page4()
        {
            BookPageConfigurator.New("CowgirlMeetBook4", Pages[4])
               .SetTitle(LocalizationTool.GetString("Plot.CowgirlMeeting.Title"))
               .SetCues(cueIDs[23], cueIDs[24], cueIDs[25])
               .SetAnswers(AnswerIDs[14], AnswerIDs[13])
               .Configure();

            CueConfigurator.New("CowgirlMeet25", cueIDs[25])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.25"))
                .SetShowOnce()
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA13", AnswerIDs[13])
                .SetText(LocalizationTool.GetString("Plot.Leave"))//hoping listing no next cue will end the dialogue
                .SetShowOnce()
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA14", AnswerIDs[14])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.14"))
                .SetNextCue(Utilities.MakeCueSelection(Pages[5]))
                .SetShowOnce()
                .Configure();

           

            
        }

        public static void Page5()
        {
            BookPageConfigurator.New("CowgirlMeetBook5", Pages[5])
               .SetTitle(LocalizationTool.GetString("Plot.CowgirlMeeting.Title"))
               .SetCues(cueIDs[26], cueIDs[27], cueIDs[28])
               .SetAnswers(AnswerIDs[15], AnswerIDs[16])
               .Configure();

            CueConfigurator.New("CowgirlMeet26", cueIDs[26])
               .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.26"))
               .Configure();

            AnswerConfigurator.New("CowgirlMeetA15", AnswerIDs[15])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Answer.15"))
                .SetNextCue(Utilities.MakeCueSelection(CheckIDs[6]))
                .SetShowOnce()
                .SetOnSelect(Flags.IncrementFlag(1, Flags.Madness))
                .Configure();

            CheckConfigurator.New("CowgirlMeetC6", CheckIDs[6])
                .SetDC(24)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana)
                .SetSuccess(Pages[5])
                .SetFail(Pages[5])
                .Configure();

            CueConfigurator.New("CowgirlMeet27", cueIDs[27])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.27"))
                .SetConditions(Utilities.MakeSelectedAnswer(AnswerIDs[15]))
                .Configure();

            CueConfigurator.New("CowgirlMeet28", cueIDs[28])
                .SetText(LocalizationTool.GetString("Plot.CowgirlMeeting.Cue.28"))
                .SetConditions(Utilities.MakePassedCheck(true, CheckIDs[6]))
                .SetOnShow(UpdateMythosCount.MythosActionBuilder(1))
                .Configure();

            AnswerConfigurator.New("CowgirlMeetA16", AnswerIDs[16])
                .SetText(LocalizationTool.GetString("Plot.Leave"))
                .Configure();
        }

    }
}
