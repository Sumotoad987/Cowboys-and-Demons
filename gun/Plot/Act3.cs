using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.KingdomEx;
using BlueprintCore.Actions.Builder.MiscEx;
using BlueprintCore.Actions.Builder.StoryEx;
using BlueprintCore.Blueprints.Configurators.DialogSystem;
using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.Configurators.Quests;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder.StoryEx;
using BlueprintCore.Utils;
using gun.Cowgirl;
using gun.Firearms;
using Kingmaker;
using Kingmaker.AreaLogic.Cutscenes;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Kingdom.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace gun.Plot
{
    internal static class Act3
    {
        //vauge plan. Once act 3 begins and the drezen map is loaded it will check if is after some arbitrary date and if the CowgirlAsksForHelp etude is false. in that case it will add a quest to the journal then spawn her actor on the map at the entrace to the keep for you to talk to and begin the quest
        
        public static string[] CowgirlQuest1DialogueCues = { 
            "1cefbaa101224b1cb09459808481cd90",
            "93b8c8286e854c97ad837bfe3d2e4b27",
            "e4c17ee19f8c434eb2837450050c7adb",
            "d1d0ab6a0fac4ae5b95c0ee6f5c09801",
            "3b8758a1912442249899274c08abc040",
            "1f7d770802474c7d922648b5209db741",
            "8c8ef1889fce4163a2fd870b77bde3aa",
            "c99f52092c3e4474958bb3bc67ddea59",
            "626b74b1bc3040999f09c9d593a44c56",
            "b08be90f223e49a08d1262b50366d1d9"
        };
        public static string[] CowgirlQuest1DialogueAnswers = {
            "80ebafc665ca4e5b8f2817f3918fb1b9",
            "b60298177e9e493d8b1564ae693b1826",
            "a6a4e3a73c0c4bdebbdd37d6b0936a73",
            "819f5781922a450fb2ce70d04c9675a0",
            "1f1ab76766e94c3bb6fe3a35e3bff1c4",
            "a514b75de1644498a4075ddbe1359032",
            "6a752d90a4544a569b91a6e577eb3a9a",
            "6a363d0ccba642c3ad167a045677dd15"
        };
        public const string GatewayToInsanityQuestGUID = "845666dad2424f129f6a922588d446cc";
        public const string ReachTheGatewayToInsanityGUID = "110e190f9c8943129cb185612baf4f0d";
        public const string HelpTheFleshwarpsQuestGUID = "21dffe6963844a14a5ba0ef74a41a528";
        public const string GatewayToInsanityStorybookGUID = "70bf9e5a3bda498bae2c3443d8fe28a3";
        public static string[] GatewayToInsanityPages = { 
            "246d23e2a9414f7e9107d152c8c2ddac",
            "8117bb6a554a49b7bfa546c98cb92c40",
            "7899dcd127774e0183788636455e06d5",
            "8443599923c24758a4fc16138c2910f5",
            "8e0f5f6f3e164268802154c78f9bbf30",
            "10f00ea4cbb04fa3bbf0afcc862606a7",
            "9c61ef119ae44cdfb1791f19c24f77e5",
            "79c57ff84d2d4e8d872cc3bea77e0314",
            "f2e9dc4334364672a36286445b9f6443",
            "5c4cbaec1c7948d6b79bedd74874d65e",
            "322204c0b9224877ac2bed19f4ac97f8",
            "79a4b710b7f543fc9385456e2ee3500b",
            "abc706a3540841a5a278dce731c2627a",
            "27ec89baaf084ec29dc9a38f857fd2f9",
            "e0d09308edbe40e8914863855a3fa6cf",
            "b7c0d7d127a8455293092af79912f873", 
        };
        public static string[] GatewayToInsanityCues = { 
            "c9c912246f0e46068574bb949523745b",
            "ca5001675b284792a77544d9ea9ec0fe",
            "c788259dbac34bf78a37172e4201ce5d",
            "3040fe19dfdd440e83780e734e1c06ec",
            "186da6fa78834878b741346f6f33eb1e",
            "6101e2e9c1e24697bb38c4e4c3ed992b",
            "d6c8105c53d34e579ec016b8a65e45b6",
            "ec1120e778184c0fb423f4101af59cc3",
            "25a4420c92e74df7aaa2c31006a25d18",
            "bcb4842f78234a9aa491526a80d907ea",
            "b829e39f10a44503b8ee1fcafe660b19",
            "453fa072437e4d11be07a517b6bfe262",
            "f718d9c3b1d94a34b510235986c19eec",
            "9ada343263f14b87a658ccef3279a56c",
            "411657b7f573461a93af50245e0c8a85",
            "596db5095dcd4b8a9ec0c606be3ab3b0",
            "da1b3cd4e2cd4e5d81badc1156b37bf5",
            "32e050ee1de04dc39455a2f295c648e3",
            "d0afc7a3e9f1428ab89340262cba430c",
            "d5c34532bba54f9e9925dd40e6dfb580",
            "64dba9a184c8491eaa7e98a096f4fa44",
            "77076fa7ab59487fab3de4543a336b92",
            "5c97a77e5e414061a5a69beb8036828b",
            "d10dfcb45da1408098734497db06559c",
            "9b3728d9b67c415e94e418ba23d1ecb6",
            "ba2a3739160e4706924f32f44a31b37f",
            "35db2cc649034d93b3e4798d5cdd6223",
            "5b146fe0f12b4ff492d18e6cdad6e87b",
            "90d4cd6042a846dd8b0a5d4e509be304",
            "7322d0bd42f14cfebe517e1d3f8c36f2",
            "fe32c26d8755421981e36c4779a33cd7",
            "f5d6b41452ca4246857f3fb9d17318aa",
            "2712d9defe39462386c248cae2cd0a96",
            "61a0ccdec38747bb8505d67f9aa24eed",
            "4e5c3e401b61432faf8e7064228fb225",
            "bcafa4bb991541659cbf29bfedede9ee",
            "05da82e96f854bbebc389819363bc645",
            "d927bf073ff44479b57638850e5574a9",
            "8ff7e94d662f47d7844eacd3a37170cb",
            "d43924c5b77549b891f882d372d3a9b7",
            "4d6dc0d253ed47248ec8000f1b6c8f1f",
            "d656454d36d24edb964d4efbedb57de5",
            "99f07c0503234e4182b652dbdbfe782e",
            "a765e49fb145444b8efaf7ece05e9b53",
            "b50d36af3a5846c1a3b1c1b9578a76d3",
            "8f5193c0d3714d21a5f6f99dd17159b9",
            "714d52bac7474702a3aa87a3ca792d90",
            "0013dbdaffe14b9f97ba7a9649333494",
            "8ef714d4062f424d9b3a51135db472c8",
            "37db03a328e548758bc58245ceb236a1",
            "02b3d58220d34bf6b82ef66c9a762fd1",
            "b72d4bf5a1364703b2d16a20287e7005",
            "25064d21331b4a2a9e2570ec1414e72d",
            "3f73dca8b2cc460187db26208f5e1cef",
            "76b25c888dc84b8d84e030673b1634e4",
            "4d5bfd550a3546d387193061a5cef81c",
            "bd3dea8531fc45aab105a081c3a7b787",
            "1eb81497336a4907a20c03a69e3fd525",
            "b7b7b04e16b74f5ba40f5ea084ec74b4",
            "f1fa0161737846b6a66f8734ea6bbaf6",
            "3279f8feca1049c895d28eb4f352ac1b",
            "d068afd19174462ba72495a5787a6ab2",
            "8bd9abd038d344b0b2e109c7c290b90a",
            "1dd13b85f8dc4b34965070a55d16c9e4",
            "c93f13ab7dcd4b38ae10f692d62869cf",
            "50429597608b4e75a72c02eaaec26a8f",
            "a52e262353df4e6a9f08b8b77cc703bf",
            "1c4a87d40edb43f29294c6a38d7adcf2",
            "cb39b36eb2d3407ebda5d03506070afa",
            "f4bc6cb118044d8c96e5844f914e8607",
            "305fa13863e34f83bd97d1d5a5e08a89",
            "2aea1da2a2654f75bdc00c47702c4643",
            "8967ce1d752b4361b13da22f6dc8cfa9",
            "d43f555ae73c4ec68e9edfaf283a0671",
            "e39d3045dff243eb962dc5fb0efb1a1e",
            "ea822d4b0fe74b3087b41756c474ff3d",
            "c337a7df09c9404392f774aa9fe036aa",
            "b93f4825afe14f3cbe67de4e4df0a3a8",
            "c32a12f516fc4643bf9ac100a0848949",
            "02eeb312b6a44d15957b6e6036ac0b39",
            "ded08385ccc44ec89d318edd638fdbca",
            "25c5c79464e94649b0e6c6a770f46e82",
            "78d7e5dedc4749aab79bb455e83fb523",
            "69395b14ca604ad3b5926f22fd9efd24",
            "ab2d546dec5946d083925c1eb71d0da9",
            "03a801a5ab4646929006deade1904f8c",
            "eb40361f8b98499392915387c7ce64d9",
            "9e9d8d10961c4bb09f38e6234f0f5e56",
            "d24b5b0e935246ef98678bfe4a823404",
            "dedce3b6d9bc4b8aa0de42b31fd98515",
            "401a54b34c6a40e688ce2d689c985045",
            "e5ab77471e72450a82d4d32d118710f3",
            "64dba14004f84895a2b2f9647978b452",
            "53d561f404434dcdb35e29acc3440a77",
            "3dc6232dd5a44c70881539b666de8218",
            "2ae2dd19e5224c65878b17f177412ff1",
            "46f55a0404a14f4eb8261dd163b571d6",
            "dd6616887721492c81fef30e66c8c2fb",
            "201bd378f527416daf129d978586c4ad",
            "8c70e3258e4c461f8d233eb66f4aeb99",
            "0d2ad72c5c3745679a5ee84529a5ffb0",
            "30f1303127d94cc3b394fdda2c12a118"
        };
        public static string[] GatewayToInsanityAnswers = { 
            "93165d1c51e247c8ab3db8126d77110a",
            "8d9d3496fad241e5bb53d641067d1775",
            "6745f65fe80c4440845a3ea6e388a4ea",
            "d52fb9930aae4f92b703cb4e52712962",
            "937d77a3dca14cc68666421021254a5a",
            "b3d1f1fcd60a4d0eab70ce20c9ad7681",
            "4918c924aa494a8589c5dd7ee55fad6b",
            "6fc25b1d93b24393801761add4d7df38",
            "20ca97bcf0c34f0e8321307c16846a32",
            "0b497784ab1942e3b81ce4d59bc2f789",
            "af601c1775ea43ada67dd950c6c18851",
            "6d02fa606bb24aa383299b9ff053ec78",
            "911a1706bffd4a298690428703ea4e8b",
            "6cb15a4b085a4f7fb845374070ca7819",
            "ca67c978f03640ae812a6e35ee5e8c80",
            "b35248c75c4e4e28bcb8b54daba6d7c6",
            "d84b1583303e4bd98c0e0951edccbe40",
            "71577d51b45d4d47bbdca7e90a1e751f",
            "3c29a591f6f54701928f30dcae50aaf1",
            "41325b6cb5364e7ea4c88b5dacc13fbc",
            "5cbcf930d805401eb3220e8cef4b7214",
            "aaef6089530e4970a6492edea0982ce7",
            "e2e430faaeb24c1bb0085342d9cff5c8",
            "1cfeaf6c993f42ccadfaed579afddeb5",
            "acfaa23fa8724f46b99fd4a7fc920ffc",
            "172732cf98da41a6a11328bad581dad2",
            "078e79bb84f64d7f96958523b81473d1",
            "958eefda705040da8b5577a7a5bb8ad9",
            "af6c76d0daf64beaa4aa5b342f9cf1cb",
            "23280dd6395d4310af35918e986df335",
            "ed8397be1e29465dba0ae4d07649f4da",
            "bc667f4a78cf465589171b48af98b34a",
            "1caf9d952940458a9bc620868b593bc9",
            "df20202166c3400aa6a5f03e0da7cc90",
            "14c864af87204705b98c9c803d987776",
            "d8775628c61748aea7cbca55807d4abd",
            "5b08dd0082f344049775d1cabc4910c3",
            "a06485c86567471ab5ac7bde51f5065f",
            "c7ec8a0bf96a4e6d90070a88893459a2",
            "ed6e40ae664a439a82d6c7e6e315fd96",
            "fd1433826d0040408edf2bc0a3c3106d",
            "9a76f426a8a841e9a8cbb0dee394c270",
            "bd4f1d39406b452fa51c25e8009e15e7",
            "4bd0998c779042b7b58a840f77dcd29e",
            "44d22ebb843b4afc9f7ca9d3492fecf1",
            "996733b43ffe4080b9fd4f0e2f5a93dc",
            "87fb6768370347b799318b21af0976db",
            "749ad593eaaf497c9046f14d56dc7ab9",
            "34509f9939164ee19e7786a0ec137968",
            "c9c4f7a1caa045cdb47ff789cba9b1fd",
            "8da8440983b144f2858ec6d1e620eb1a",
            "3b33d72335f64a95980b04de18b81b16",
            "672827bd527c49f69a42899470f06ad6",
            "612765e7f32f48f388ccb318f82cdfdd",
            "5c75abf4f688455986b86c67c0c4b0ea",
            "a060e0003ca04c6cb671d9797a820b91",
            "5213f49e13584cceb5ebb714b771895b",
            "c906776757104bb9a17d35defbc09e2a",
            "8429c2f7bcc04591a15683593d2a24de",
            "3f5694abc3074eed8e047d9ac8bb7158",
            "d57f7e4047de4a52b90974e42b1bc7d6",
            "d6b5a7c40ff7422285caf66850c10cea"
        };
        public static string[] GatewayToInsanityChecks = { 
            "824f9837b249402c98077b7798d5b568",
            "8a6cac30dc0c405cacf447cef840e122",
            "ff7912e656364e4681656ed4b175b9f8",
            "e6dfcf0206de47228a8970ce2a3b5601",
            "dd1598e18de144fc91a8294b07b31b79",
            "b0e1d68ee38f448f915a41ac130d267c",
            "b07d48411e7f4488947c893c746bf529",
            "e1d56953f45f4b23953b0fb69bed8612",
            "abcb161c74644da4a6b53a72d31048a4",
            "4454d022a5344d08bec17fda8ae8395a",
            "de4a380bcb244d8b9259ed2f2dc0a0c0",
            "761dc996340844f1ab1ff6a97113b33e",
            "c40c64d7939340b190e66f1d4ca08287",
            "ac30aba5c2604a61926a52374f1532b0",
            "439ffd2bf5f54b2c84b6b0dbb4b6a190",
            "0613fe7856034862b36393378edda1b6",
            "20173207e11345f4b3085b0c582f37a2",
            "d8887b60cd7b483dbf19eaa25da11052",
            "1c1a109470594a9a85f052a445b13c92",
            "7dfa10228d47472a8214cba84e531738",
            "00c76b3d115f408fb12280b876e30b47",
            "0d62390d92a544359d6cbf01043260d9",
            "7fdb1df72d544748a670100c70294192",
            "4ef78e2f73504a2d900737f99b13d68e",
            "5a67fb019e4d4f4f982f39962e9a9834",
            "287b9629ab4c4663bdc84206eb195764",
            "3b7aef5d58e24878be6187db97b9363d",
            "1497a8a27ca24407abd43b5b65607985"
        };
        public const string AberrantBileGUID = "214eed495b8d47fd9eb7bc5b1f8cbbcf";
        public static void Configure()
        {
            CowgirlDrezenSpawn();
            IntroductionDialogue();
            GatewaytoInsanity();
            Fleshwarps();
            DemonsHeresy();
        }

        private static void CowgirlDrezenSpawn()
        {//this one handles setting up the spawner and such for putting Bell on the map in Drezen and controlling what dialogue is assosiated with her
            EventBus.Subscribe(new JuryRiggedUnitSpawner(
                "2570015799edf594daf2f076f2f975d8",//outdoors of Drezen (hopefully)
                CowgirlUnit.GUID,
                new UnityEngine.Vector3(41.9f, 66.3f, -53.4f),
                new UnityEngine.Vector3(0,0,0),//and rotation (probably use toybox to help find these)
                Flags.CowgirlInDrezen,
                3)//is for chapter 3 assuming this start at 1 not 0 and lines up with in game acts
                );
        }

        private static void DemonsHeresy()
        {
            CueConfigurator.New("CowgirlDemonsHeresy", "ad2bc52ec54544efa1aac3583880d700")
                .SetText(LocalizationTool.GetString("Plot.DemonsHerey.Cowgirl.1"))
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .SetShowOnce()
                .Configure();
            CueSequenceConfigurator.For("698a1c39b353b5843a9da22297885457").AddToCues("ad2bc52ec54544efa1aac3583880d700").Configure();

            CueConfigurator.New("CowgirlDemonsHeresy2", "3914ab87d2044c04b2f15525a03e57a5")
                .SetText(LocalizationTool.GetString("Plot.DemonsHerey.Cowgirl.2"))
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .SetShowOnce()
                .Configure();
            CueSequenceConfigurator.For("4f3d48e00c4a2104bb1502bf92171913").AddToCues("3914ab87d2044c04b2f15525a03e57a5").Configure();

            CueConfigurator.New("CowgirlDemonsHeresy3", "daf0ddd7bd0b44fcb3fc981bf3a7522a")
                .SetText(LocalizationTool.GetString("Plot.DemonsHerey.Cowgirl.3"))
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .SetShowOnce()
                .Configure();
            CueSequenceConfigurator.For("cb78d8dde11091b45a6bc24a3ccc95a7").AddToCues("daf0ddd7bd0b44fcb3fc981bf3a7522a").Configure();
            
        }

        private static void KnowTheyEnemy()
        {
            CueConfigurator.New("CowgirlKnowThyEnemy", "2dcf9624734f484db17cc60d95ecc303")
               .SetText(LocalizationTool.GetString("Plot.KnowThyEnemy.Cowgirl.1"))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetShowOnce()
               .Configure();
            CueSequenceConfigurator.For("48d9dddefe6db3148be8ba512a65d257").AddToCues("2dcf9624734f484db17cc60d95ecc303").Configure();

            CueConfigurator.New("CowgirlKnowThyEnemy2", "17783c0023924c278a0045961e38c40b")
               .SetText(LocalizationTool.GetString("Plot.KnowThyEnemy.Cowgirl.2"))
               .SetSpeaker(CowgirlUnit.GetSpeaker())
               .SetShowOnce()
               .Configure();
            CueSequenceConfigurator.For("23d431e01ba69d74a921d1c897c5b203").AddToCues("17783c0023924c278a0045961e38c40b").Configure();
        }
        private static void IntroductionDialogue()
        {//This one is the dialogue she has when you first meet her in drezen and she asks for your help
            
            CueConfigurator.New("CowgirlQuest1Intro0", CowgirlQuest1DialogueCues[0])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Cue.0"))
                .SetContinueValue(Utilities.MakeCueSelection(CowgirlQuest1DialogueCues[1], CowgirlQuest1DialogueCues[2]))
                .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.BeganGatewayToInsanity,maxValue:0).FlagUnlocked(Flags.BeganGatewayToInsanity,negate:true).UseOr())//only shows up if the quest has not be started
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .SetShowOnce()
                .Configure();

            CueConfigurator.New("CowgirlQuest1Intro1", CowgirlQuest1DialogueCues[1])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Cue.1"))
                .SetConditions(Utilities.MakeFlagCheck(Flags.OfferedToHelpCowgirl,1,10))//is it one or more so picked 10 to be safe
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .SetContinueValue(Utilities.MakeCueSelection(CowgirlQuest1DialogueCues[2]))
                .Configure();

            CueConfigurator.New("CowgirlQuest1Intro2", CowgirlQuest1DialogueCues[2])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Cue.2"))
                .SetAnswers(CowgirlQuest1DialogueAnswers[0], CowgirlQuest1DialogueAnswers[1], CowgirlQuest1DialogueAnswers[2])
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .Configure();

            AnswerConfigurator.New("CowgirlQuest1IntroA0", CowgirlQuest1DialogueAnswers[0])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Answer.0"))
                .SetNextCue(Utilities.MakeCueSelection(CowgirlQuest1DialogueCues[7]))
                .SetOnSelect(Flags.IncrementFlag(2, Flags.CowgirlApproval))
                .Configure();

            AnswerConfigurator.New("CowgirlQuest1IntroA1", CowgirlQuest1DialogueAnswers[1])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Answer.1"))
                .SetNextCue(Utilities.MakeCueSelection(CowgirlQuest1DialogueCues[3], CowgirlQuest1DialogueCues[4]))
                .Configure();

            AnswerConfigurator.New("CowgirlQuest1IntroA2", CowgirlQuest1DialogueAnswers[2])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Answer.2"))
                .SetOnSelect(Flags.IncrementFlag(-2, Flags.CowgirlApproval))
                .SetNextCue(Utilities.MakeCueSelection(CowgirlQuest1DialogueCues[6]))
                .Configure();

            CueConfigurator.New("CowgirlQuest1Intro3", CowgirlQuest1DialogueCues[3])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Cue.3"))
                .SetConditions(Utilities.MakeFlagCheck(Flags.AskedCowgirlToJoinCrusade, 1, 10))//is it one or more so picked 10 to be safe
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .SetContinueValue(Utilities.MakeCueSelection(CowgirlQuest1DialogueCues[5]))
                .Configure();

            CueConfigurator.New("CowgirlQuest1Intro4", CowgirlQuest1DialogueCues[4])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Cue.3"))
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .SetAnswers(CowgirlQuest1DialogueAnswers[3], CowgirlQuest1DialogueAnswers[2])
                .Configure();

            CueConfigurator.New("CowgirlQuest1Intro5", CowgirlQuest1DialogueCues[5])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Cue.5"))
                .SetAnswers(CowgirlQuest1DialogueAnswers[3], CowgirlQuest1DialogueAnswers[2])
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .Configure();

            AnswerConfigurator.New("CowgirlQuest1IntroA3", CowgirlQuest1DialogueAnswers[3])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Answer.3"))
                .SetOnSelect(Flags.IncrementFlag(1,Flags.CowgirlApproval))
                .SetNextCue(Utilities.MakeCueSelection(CowgirlQuest1DialogueCues[7]))
                .Configure();

            CueConfigurator.New("CowgirlQuest1Intro6", CowgirlQuest1DialogueCues[6])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Cue.6"))
                .SetAnswers(CowgirlQuest1DialogueAnswers[4], CowgirlQuest1DialogueAnswers[5])
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .Configure();

            AnswerConfigurator.New("CowgirlQuest1IntroA4", CowgirlQuest1DialogueAnswers[4])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Answer.4"))
                .SetNextCue(Utilities.MakeCueSelection(CowgirlQuest1DialogueCues[9]))
                .Configure();

            AnswerConfigurator.New("CowgirlQuest1IntroA5", CowgirlQuest1DialogueAnswers[5])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Answer.5"))
                .SetNextCue(Utilities.MakeCueSelection(CowgirlQuest1DialogueCues[7]))
                .Configure();

            CueConfigurator.New("CowgirlQuest1Intro7", CowgirlQuest1DialogueCues[7])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Cue.7"))
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .SetAnswers(CowgirlQuest1DialogueAnswers[6])
                .Configure();

            AnswerConfigurator.New("CowgirlQuest1IntroA6", CowgirlQuest1DialogueAnswers[6])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Answer.6"))
                .SetNextCue(Utilities.MakeCueSelection(CowgirlQuest1DialogueCues[8]))
                .Configure();

            CueConfigurator.New("CowgirlQuest1Intro8", CowgirlQuest1DialogueCues[8])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Cue.8"))
                .SetAnswers(CowgirlQuest1DialogueAnswers[7])
                .SetOnStop(ActionsBuilder.New()
                    .GiveObjective(BlueprintTool.GetRef<BlueprintQuestObjectiveReference>(ReachTheGatewayToInsanityGUID)).IncrementFlagValue(Flags.BeganGatewayToInsanity,true,new EvaluatorInt(1))//add the quest
                    .AddAll(Flags.IncrementFlag(-1,Flags.CowgirlInDrezen).Build()))//Remove Bell from Drezen for now
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .Configure();



            AnswerConfigurator.New("CowgirlQuest1IntroA7", CowgirlQuest1DialogueAnswers[7])
                .SetText(LocalizationTool.GetString("Plot.Leave"))
                .Configure();

            CueConfigurator.New("CowgirlQuest1Intro9", CowgirlQuest1DialogueCues[9])
                .SetText(LocalizationTool.GetString("Plot.CowgirlQuest1Intro.Cue.9"))
                .SetOnStop(Flags.IncrementFlag(1, Flags.RejectedCowgirl).AddAll(Flags.IncrementFlag(-1, Flags.CowgirlInDrezen).Build()))
                .SetSpeaker(CowgirlUnit.GetSpeaker())
                .SetAnswers(CowgirlQuest1DialogueAnswers[7])
                .Configure();
        }

        private static void GatewaytoInsanity()
        {//this is the parent for all the storybook stuff for this act
            QuestConfigurator GatewayToInsanityQuest = QuestConfigurator.New("GatewayToInsanityQuest", GatewayToInsanityQuestGUID);
            GatewayToInsanityQuest.SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"));
            GatewayToInsanityQuest.SetDescription(LocalizationTool.GetString("Plot.GatewayToInsanity.Description"));
            GatewayToInsanityQuest.SetCompletionText(LocalizationTool.GetString("Plot.GatewayToInsanity.Completion"));
            GatewayToInsanityQuest.SetGroup(Kingmaker.Enums.QuestGroupId.CompanionQuests);
            GatewayToInsanityQuest.SetDescriptionPriority(0);//not sure what this is
            GatewayToInsanityQuest.SetType(Kingmaker.Enums.QuestType.Normal);
            GatewayToInsanityQuest.SetLastChapter(3);//hoping 3 is chapter 3 again
            GatewayToInsanityQuest.SetQuestMarkerColorType(Kingmaker.Blueprints.Root.QuestMarkerColors.QuestMarkerColorType.Red);
            GatewayToInsanityQuest.SetObjectives(ReachTheGatewayToInsanityGUID,HelpTheFleshwarpsQuestGUID);
            GatewayToInsanityQuest.Configure();

            QuestObjectiveConfigurator ReachTheGatewayToInsanity = QuestObjectiveConfigurator.New("ReachTheGatewayToInsanity", ReachTheGatewayToInsanityGUID);
            ReachTheGatewayToInsanity.SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Reach.Title"));
            ReachTheGatewayToInsanity.SetDescription(LocalizationTool.GetString("Plot.GatewayToInsanity.Reach.Description"));
            ReachTheGatewayToInsanity.AddExperience(cR: 10, encounter: Kingmaker.Blueprints.Classes.Experience.EncounterType.QuestMain, dummy: false, modifier: 1.0f);
            ReachTheGatewayToInsanity.SetQuest(BlueprintTool.Get<BlueprintQuest>(GatewayToInsanityQuestGUID));
            ReachTheGatewayToInsanity.Configure();

            WorldMapEncounter GatewayToInsanityMapPoint = new WorldMapEncounter(-51,9,-50,10, Flags.CompletedGatewayToInsanity, GatewayToInsanityStorybookGUID,"X.png", Flags.BeganGatewayToInsanity);
            EventBus.Subscribe(GatewayToInsanityMapPoint);


            DialogConfigurator GatewayToInsanityStorybook = DialogConfigurator.New("GatewayToInsanityStorybook", GatewayToInsanityStorybookGUID);
            GatewayToInsanityStorybook.SetType(DialogType.Book);
            GatewayToInsanityStorybook.SetFirstCue(Utilities.MakeCueSelection(GatewayToInsanityPages[0]));
            GatewayToInsanityStorybook.Configure();
            GatewayArrival();
            GatewayMeetup();
            GatewayFrontDoor();
            GatewayThroughTheEye();
            GatewayTriggeredTraps();
            GatewayEntranceCombat();
            GatewayEntranceCleared();
            GatewayWallWriting();
            GatewayBeingRead();
            GatewayPage9();
            GatewayLab();
            GatewayFleshwarpsAsMonsters();
            GatewayFleshwarpVictims();
            GatewayWarpedHallway();
            GatewayEnd();
            GatewayReward();

        }

        private static void GatewayArrival()
        {
            BookPageConfigurator.New("GatewayToInsanityPage0", GatewayToInsanityPages[0])
                .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
                .SetCues(GatewayToInsanityCues[0], GatewayToInsanityCues[1], GatewayToInsanityCues[2], GatewayToInsanityCues[3], GatewayToInsanityCues[4], GatewayToInsanityCues[5], GatewayToInsanityCues[6])
                .SetAnswers(GatewayToInsanityAnswers[0], GatewayToInsanityAnswers[1], GatewayToInsanityAnswers[2], GatewayToInsanityAnswers[3], GatewayToInsanityAnswers[4])
                .Configure();

            CueConfigurator.New("GatewayToInsanity0", GatewayToInsanityCues[0])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.0"))
                .Configure();

            CueConfigurator.New("GatewayToInsanity1", GatewayToInsanityCues[1])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.1"))
                .Configure();


            #region Aeon
            AnswerConfigurator.New("GatewayToInsanityA0", GatewayToInsanityAnswers[0])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.0"))
                .SetShowConditions(ConditionsBuilder.New()
                    .CheckPassed(GatewayToInsanityChecks[0], true)
                    .CheckPassed(GatewayToInsanityChecks[1], true)
                    )
                .SetMythicRequirement(Mythic.PlayerIsAeon)
                //plays if the player is Aeon and the player has not passed the check for one of the other options
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[0]))
                .SetShowOnce()
                .SetExperience(DialogExperience.SmallExperience)
                .Configure();

            CueConfigurator.New("GatewayToInsanity2", GatewayToInsanityCues[2])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.2"))
                .SetConditions(ConditionsBuilder.New().AnswerSelected(BlueprintTool.GetRef<BlueprintAnswerReference>(GatewayToInsanityAnswers[0])))
                //only shows if the player chose the aeon answer
                .Configure();
            #endregion

            #region Answer1KnowledgeArcana
            CheckConfigurator.New("GatewayToInsanityC0", GatewayToInsanityChecks[0])
                .SetDC(28)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana)
                .SetSuccess(GatewayToInsanityPages[0])
                .SetFail(GatewayToInsanityPages[0])
                .Configure();

            AnswerConfigurator.New("GatewayToInsanityA1", GatewayToInsanityAnswers[1])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.1"))
                .SetShowConditions(ConditionsBuilder.New()
                    .CheckPassed(GatewayToInsanityChecks[0],true)
                    .CheckPassed(GatewayToInsanityChecks[1],true)
                    )
                .SetShowOnce()
                //plays if the player has not selected the aeon answer or passed another check
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[0]))
                .Configure();

            CueConfigurator.New("GatewayToInsanity3", GatewayToInsanityCues[3])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.3"))
                .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[0]))
                //shows if you passed the arcana check
                .Configure();

            CueConfigurator.New("GatewayToInsanity4", GatewayToInsanityCues[4])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.4"))
                .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[0]))
                //shows if you failed the arcana check
                .SetOnShow(UpdateMadness.MadnessActionBuilder(1))//gain 1 madness
                .Configure();
            #endregion

            #region Answer2Mobility
            CheckConfigurator.New("GatewayToInsanityC1", GatewayToInsanityChecks[1])
                .SetDC(28)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillMobility)
                .SetSuccess(GatewayToInsanityPages[0])
                .SetFail(GatewayToInsanityPages[0])
                .Configure();

            AnswerConfigurator.New("GatewayToInsanityA2", GatewayToInsanityAnswers[2])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.2"))
                .SetShowConditions(ConditionsBuilder.New()
                    .CheckPassed(GatewayToInsanityChecks[0], true)
                    .CheckPassed(GatewayToInsanityChecks[1], true)
                    )
                .SetShowOnce()
                //plays if the player has not selected the aeon answer or passed another check
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[1]))
                .Configure();

            CueConfigurator.New("GatewayToInsanity5", GatewayToInsanityCues[5])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.5"))
                .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[1]))
                
                //shows if you passed the mobility check
                .Configure();



            DamageDescription FailedMobilityDamage = new DamageDescription();
            FailedMobilityDamage.SetDice(new ModifiableDiceFormula(new Kingmaker.RuleSystem.DiceFormula(4, Kingmaker.RuleSystem.DiceType.D6)));
            

            CueConfigurator.New("GatewayToInsanity6", GatewayToInsanityCues[6])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.6"))
                .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[1]))
                //shows if you failed the mobility check
                .SetOnShow(ActionsBuilder.New().DamageParty(damage:FailedMobilityDamage, new PlayerCharacter()))//deals 4d6 damage to party
                .Configure();
            #endregion

            #region Answer3Giveup
            IntConstant HourstoAdd = new Kingmaker.Designers.EventConditionActionSystem.Evaluators.IntConstant();
            HourstoAdd.Value = 12;

            AnswerConfigurator.New("GatewayToInsanityA3", GatewayToInsanityAnswers[3])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.3"))
                .SetShowConditions(ConditionsBuilder.New()
                    .CheckPassed(GatewayToInsanityChecks[0], true)
                    .CheckPassed(GatewayToInsanityChecks[1], true)
                    )
                .SetShowOnce()
                //plays if the player has not selected the aeon answer or passed another check
                .SetOnSelect(ActionsBuilder.New().AddFatigueHours(HourstoAdd, new PlayerCharacter())
                    )//gain 12 hours of fatigue
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[1]))
                .Configure();
            #endregion

            

            #region Answer4Continue
            AnswerConfigurator.New("GatewayToInsanityA4", GatewayToInsanityAnswers[4])
                .SetText(LocalizationTool.GetString("Plot.Continue"))
                .SetShowConditions(ConditionsBuilder.New()
                    .CheckPassed(GatewayToInsanityChecks[0])
                    .CheckPassed(GatewayToInsanityChecks[1])
                    .UseOr()
                    )
                .SetShowOnce()
                //plays if the player has selected the aeon answer or passed another check
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[1]))
                .Configure();
            #endregion
        }

        private static void GatewayMeetup()
        {
            BookPageConfigurator.New("GatewayToInsanityPage1", GatewayToInsanityPages[1])
                .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
                .SetCues(GatewayToInsanityCues[7], GatewayToInsanityCues[8], GatewayToInsanityCues[9], GatewayToInsanityCues[10], GatewayToInsanityCues[11], GatewayToInsanityCues[12])
                .SetAnswers(GatewayToInsanityAnswers[5], GatewayToInsanityAnswers[6], GatewayToInsanityAnswers[7], GatewayToInsanityAnswers[8])
                .Configure();

            CueConfigurator.New("GatewayToInsanity7", GatewayToInsanityCues[7])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.7"))
                .Configure();

            CueConfigurator.New("GatewayToInsanity8", GatewayToInsanityCues[8])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.8"))
                .Configure();

            CueConfigurator.New("GatewayToInsanity9", GatewayToInsanityCues[9])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.9"))
                .Configure();

            AnswerConfigurator.New("GatewayToInsanityA5", GatewayToInsanityAnswers[5])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.5"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[1]))
                .Configure();

            CueConfigurator.New("GatewayToInsanity10", GatewayToInsanityCues[10])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.10"))
                .SetConditions(Utilities.MakeSelectedAnswer(GatewayToInsanityAnswers[5]))
                .Configure();

            AnswerConfigurator.New("GatewayToInsanityA6", GatewayToInsanityAnswers[6])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.6"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[1]))
                .Configure();

            CueConfigurator.New("GatewayToInsanity11", GatewayToInsanityCues[11])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.11"))
                .SetConditions(Utilities.MakeSelectedAnswer(GatewayToInsanityAnswers[6]))
                .Configure();

            AnswerConfigurator.New("GatewayToInsanityA7", GatewayToInsanityAnswers[7])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.7"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[1]))
                .Configure();

            CueConfigurator.New("GatewayToInsanity12", GatewayToInsanityCues[12])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.12"))
                .SetConditions(Utilities.MakeSelectedAnswer(GatewayToInsanityAnswers[7]))
                .Configure();

            AnswerConfigurator.New("GatewayToInsanityA8", GatewayToInsanityAnswers[8])
                .SetText(LocalizationTool.GetString("Plot.Continue"))
                .SetShowConditions(Utilities.MakeSelectedAnswer(GatewayToInsanityAnswers[7]))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[2]))
                .Configure();
        }

        private static void GatewayFrontDoor()
        {
            BookPageConfigurator.New("GatewayToInsanityPage2", GatewayToInsanityPages[2])
                .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
                .SetCues(GatewayToInsanityCues[13])
                .SetAnswers(GatewayToInsanityAnswers[9], GatewayToInsanityAnswers[10], GatewayToInsanityAnswers[11])
                .Configure();

            CueConfigurator.New("GatewayToInsanity13", GatewayToInsanityCues[13])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.13"))
               .Configure();

            #region Answer9Perception
            AnswerConfigurator.New("GatewayToInsanityA9", GatewayToInsanityAnswers[9])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.9"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[2]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC2", GatewayToInsanityChecks[2])
                .SetDC(31)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillPerception)
                .SetSuccess(GatewayToInsanityPages[3])//go to through the eye
                .SetFail(GatewayToInsanityPages[4])//go to traps
                .Configure();

            CueConfigurator.New("GatewayToInsanity14", GatewayToInsanityCues[14])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.14"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[2]))
               .Configure();

            CueConfigurator.New("GatewayToInsanity15", GatewayToInsanityCues[15])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.15"))
               .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[2]))
               .Configure();
            #endregion

            #region Answer10Trickery
            AnswerConfigurator.New("GatewayToInsanityA10", GatewayToInsanityAnswers[10])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.10"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[3]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC3", GatewayToInsanityChecks[3])
                .SetDC(28)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillThievery)
                .SetSuccess(GatewayToInsanityPages[5])//go to entrance hall
                .SetFail(GatewayToInsanityPages[4])//go to trigger traps
                .Configure();

            CueConfigurator.New("GatewayToInsanity16", GatewayToInsanityCues[16])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.16"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[3]))
               .Configure();

            CueConfigurator.New("GatewayToInsanity17", GatewayToInsanityCues[17])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.17"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[3]))
              .Configure();
            #endregion

            #region Answer11Fortitude
            AnswerConfigurator.New("GatewayToInsanityA11", GatewayToInsanityAnswers[11])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.11"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[4]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC4", GatewayToInsanityChecks[4])
                .SetDC(23)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SaveFortitude)
                .SetSuccess(GatewayToInsanityPages[5])//go to entrance hall
                .SetFail(GatewayToInsanityPages[4])//go to trigger traps
                .Configure();

            CueConfigurator.New("GatewayToInsanity18", GatewayToInsanityCues[18])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.18"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[4]))
               .Configure();
            DamageDescription FailedFortDamage = new DamageDescription();
            FailedFortDamage.SetDice(new ModifiableDiceFormula(new Kingmaker.RuleSystem.DiceFormula(2, Kingmaker.RuleSystem.DiceType.D6)));



            CueConfigurator.New("GatewayToInsanity19", GatewayToInsanityCues[19])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.19"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[4]))
              .SetOnShow(ActionsBuilder.New().DamageParty(damage: FailedFortDamage, new PlayerCharacter()))//deals 2d6 damage to party
              .Configure();
            #endregion
        }

        private static void GatewayThroughTheEye()
        {
            BookPageConfigurator.New("GatewayToInsanityPage3", GatewayToInsanityPages[3])
                .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
                .SetCues(GatewayToInsanityCues[15], GatewayToInsanityCues[20], GatewayToInsanityCues[21])
                .SetAnswers(GatewayToInsanityAnswers[12], GatewayToInsanityAnswers[13])
                .Configure();

            #region Answer12Deception

            AnswerConfigurator.New("GatewayToInsanityA12", GatewayToInsanityAnswers[12])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.12"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[5]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC5", GatewayToInsanityChecks[5])
                .SetDC(26)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.CheckBluff)
                .SetSuccess(GatewayToInsanityPages[6])//go to entrance hall cleared
                .SetFail(GatewayToInsanityPages[5])//go to entrance hall fight
                .Configure();

            CueConfigurator.New("GatewayToInsanity20", GatewayToInsanityCues[20])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.20"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[5]))
               .Configure();

            CueConfigurator.New("GatewayToInsanity21", GatewayToInsanityCues[21])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.21"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[5]))
              .Configure();

            #endregion

            #region Answer13Stealth

            AnswerConfigurator.New("GatewayToInsanityA13", GatewayToInsanityAnswers[13])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.13"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[6]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC6", GatewayToInsanityChecks[6])
                .SetDC(26)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillStealth)
                .SetSuccess(GatewayToInsanityPages[6])//go to entrance hall cleared
                .SetFail(GatewayToInsanityPages[5])//go to entrance hall fight
                .Configure();

            CueConfigurator.New("GatewayToInsanity22", GatewayToInsanityCues[22])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.22"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[6]))
               .Configure();

            CueConfigurator.New("GatewayToInsanity23", GatewayToInsanityCues[23])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.23"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[6]))
              .Configure();
            #endregion

        }

        private static void GatewayTriggeredTraps()
        {
            BookPageConfigurator.New("GatewayToInsanityPage4", GatewayToInsanityPages[4])
                .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
                .SetCues(GatewayToInsanityCues[16], GatewayToInsanityCues[18], GatewayToInsanityCues[20], GatewayToInsanityCues[24])
                .SetAnswers(GatewayToInsanityAnswers[14], GatewayToInsanityAnswers[15])
                .Configure();

            CueConfigurator.New("GatewayToInsanity24", GatewayToInsanityCues[24])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.24"))
              .Configure();

            
            BuffConfigurator.New("AberrantBile", AberrantBileGUID)
                .SetDisplayName(LocalizationTool.GetString("AberrantBile.Name"))
                .SetDescription(LocalizationTool.GetString("AberrantBile.Description"))
                .AddBuffPoisonStatDamage(descriptor: Kingmaker.Enums.ModifierDescriptor.None, saveType: Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, stat: Kingmaker.EntitySystem.Stats.StatType.Constitution, succesfullSaves: 3, ticks:600, value: new Kingmaker.RuleSystem.DiceFormula(2, Kingmaker.RuleSystem.DiceType.One))
                .Configure();

            #region Answer14Mobility
            AnswerConfigurator.New("GatewayToInsanityA14", GatewayToInsanityAnswers[14])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.14"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[7]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC7", GatewayToInsanityChecks[7])
                .SetDC(28)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillMobility)
                .SetSuccess(GatewayToInsanityPages[5])//go to entrance hall fight
                .SetFail(GatewayToInsanityPages[5])//go to entrance hall fight
                .Configure();

            CueConfigurator.New("GatewayToInsanity25", GatewayToInsanityCues[25])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.25"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[7]))
               .SetOnShow(ActionsBuilder.New().ApplyBuffPermanent(BlueprintTool.GetRef<BlueprintBuffReference>(AberrantBileGUID)))
               .Configure();

            CueConfigurator.New("GatewayToInsanity26", GatewayToInsanityCues[26])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.26"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[7]))
              .Configure();
            #endregion

            #region Answer15Fortitude
            AnswerConfigurator.New("GatewayToInsanityA15", GatewayToInsanityAnswers[15])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.15"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[8]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC8", GatewayToInsanityChecks[8])
                .SetDC(30)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SaveFortitude)
                .SetSuccess(GatewayToInsanityPages[5])//go to entrance hall fight
                .SetFail(GatewayToInsanityPages[5])//go to entrance hall fight
                .Configure();

            CueConfigurator.New("GatewayToInsanity27", GatewayToInsanityCues[27])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.27"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[8]))
               .SetOnShow(ActionsBuilder.New().ApplyBuffPermanent(BlueprintTool.GetRef<BlueprintBuffReference>(AberrantBileGUID)))
               .Configure();

            CueConfigurator.New("GatewayToInsanity28", GatewayToInsanityCues[28])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.28"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[8]))
              .Configure();
            #endregion

        }

        private static void GatewayEntranceCombat()
        {
            BookPageConfigurator.New("GatewayToInsanityPage5", GatewayToInsanityPages[5])
                .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
                .SetCues(GatewayToInsanityCues[17], GatewayToInsanityCues[19], GatewayToInsanityCues[20], GatewayToInsanityCues[22], GatewayToInsanityCues[25], GatewayToInsanityCues[26], GatewayToInsanityCues[27], GatewayToInsanityCues[28], GatewayToInsanityCues[29], GatewayToInsanityCues[30])
                .SetAnswers(GatewayToInsanityAnswers[16], GatewayToInsanityAnswers[17], GatewayToInsanityAnswers[18])
                .Configure();

            CueConfigurator.New("GatewayToInsanity29", GatewayToInsanityCues[29])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.29"))
              .SetConditions(ConditionsBuilder.New().CueSeen(GatewayToInsanityCues[13],negate:true))//only shows if you have not seen the ambush slide
              .Configure();

            CueConfigurator.New("GatewayToInsanity30", GatewayToInsanityCues[30])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.30"))
              .Configure();

            ContextDiceValue IntDrain = new ContextDiceValue();
            IntDrain.DiceType = Kingmaker.RuleSystem.DiceType.One;
            IntDrain.DiceCountValue = new ContextValue();
            IntDrain.DiceCountValue.ValueType = ContextValueType.Simple;
            IntDrain.DiceCountValue.Value = 4;

            #region Answer16UseMagicDevice
            AnswerConfigurator.New("GatewayToInsanityA16", GatewayToInsanityAnswers[16])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.16"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[9]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC9", GatewayToInsanityChecks[9])
                .SetDC(26)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillUseMagicDevice)
                .SetSuccess(GatewayToInsanityPages[6])//go to entrance hall cleared
                .SetFail(GatewayToInsanityPages[6])//go to entrance hall cleared
                .Configure();


            CueConfigurator.New("GatewayToInsanity31", GatewayToInsanityCues[31])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.31"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[9]))
               .SetOnShow(ActionsBuilder.New().DealAbilityDamage(new PlayerCharacter(),new DiceFormula(), 4, Kingmaker.EntitySystem.Stats.StatType.Intelligence).IncrementFlagValue(BlueprintTool.GetRef<BlueprintUnlockableFlagReference>(Flags.GatewayToInsanityAlarm),true, new EvaluatorInt(1)))
               .Configure();

            CueConfigurator.New("GatewayToInsanity32", GatewayToInsanityCues[32])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.32"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[9]))
              .SetOnShow(ActionsBuilder.New().IncrementFlagValue(BlueprintTool.GetRef<BlueprintUnlockableFlagReference>(Flags.GatewayToInsanityAlarm), true, new EvaluatorInt(1)))
              .Configure();

            #endregion

            #region Answer17Intimidation
            AnswerConfigurator.New("GatewayToInsanityA17", GatewayToInsanityAnswers[17])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.17"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[10]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC10", GatewayToInsanityChecks[10])
                .SetDC(24)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.CheckIntimidate)
                .SetSuccess(GatewayToInsanityPages[6])//go to entrance hall cleared
                .SetFail(GatewayToInsanityPages[6])//go to entrance hall cleared
                .Configure();


            CueConfigurator.New("GatewayToInsanity33", GatewayToInsanityCues[33])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.33"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[10]))
               .SetOnShow(ActionsBuilder.New().DealAbilityDamage(new PlayerCharacter(), new DiceFormula(), 4, Kingmaker.EntitySystem.Stats.StatType.Intelligence).IncrementFlagValue(BlueprintTool.GetRef<BlueprintUnlockableFlagReference>(Flags.GatewayToInsanityAlarm), true, new EvaluatorInt(1)))
               .Configure();

            CueConfigurator.New("GatewayToInsanity34", GatewayToInsanityCues[34])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.34"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[10]))
              .SetOnShow(ActionsBuilder.New().IncrementFlagValue(BlueprintTool.GetRef<BlueprintUnlockableFlagReference>(Flags.GatewayToInsanityAlarm), true, new EvaluatorInt(1)))
              .Configure();

            #endregion

            #region Answer18Athletics
            AnswerConfigurator.New("GatewayToInsanityA18", GatewayToInsanityAnswers[18])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.18"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[11]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC11", GatewayToInsanityChecks[11])
                .SetDC(24)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillAthletics)
                .SetSuccess(GatewayToInsanityPages[6])//go to entrance hall cleared
                .SetFail(GatewayToInsanityPages[6])//go to entrance hall cleared
                .Configure();


            CueConfigurator.New("GatewayToInsanity35", GatewayToInsanityCues[35])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.35"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[11]))
               .SetOnShow(ActionsBuilder.New().DealAbilityDamage(new PlayerCharacter(), new DiceFormula(), 4, Kingmaker.EntitySystem.Stats.StatType.Intelligence).IncrementFlagValue(BlueprintTool.GetRef<BlueprintUnlockableFlagReference>(Flags.GatewayToInsanityAlarm), true, new EvaluatorInt(1)))
               .Configure();

            CueConfigurator.New("GatewayToInsanity36", GatewayToInsanityCues[36])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.36"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[11]))
              .SetOnShow(ActionsBuilder.New().DealAbilityDamage(new PlayerCharacter(), new DiceFormula(), 4, Kingmaker.EntitySystem.Stats.StatType.Intelligence))
              .Configure();

            #endregion



        }

        private static void GatewayEntranceCleared()
        {
            BookPageConfigurator.New("GatewayToInsanityPage6", GatewayToInsanityPages[6])
                .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
                .SetCues(GatewayToInsanityCues[21], GatewayToInsanityCues[23], GatewayToInsanityCues[31], GatewayToInsanityCues[32], GatewayToInsanityCues[33], GatewayToInsanityCues[34], GatewayToInsanityCues[35], GatewayToInsanityCues[36], GatewayToInsanityCues[37])
                .SetAnswers(GatewayToInsanityAnswers[19], GatewayToInsanityAnswers[20])
                .Configure();

            CueConfigurator.New("GatewayToInsanity37", GatewayToInsanityCues[37])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.37"))
              .Configure();

            AnswerConfigurator.New("GatewayToInsanityA19", GatewayToInsanityAnswers[19])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.19"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[9]))
                .Configure();

            AnswerConfigurator.New("GatewayToInsanityA20", GatewayToInsanityAnswers[20])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.20"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[7]))
                .Configure();

        }

        private static void GatewayWallWriting()
        {
            BookPageConfigurator.New("GatewayToInsanityPage7", GatewayToInsanityPages[7])
                .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
                .SetCues(GatewayToInsanityCues[38])
                .SetAnswers(GatewayToInsanityAnswers[21], GatewayToInsanityAnswers[22])
                .Configure();

            CueConfigurator.New("GatewayToInsanity38", GatewayToInsanityCues[38])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.38"))
               .Configure();

            AnswerConfigurator.New("GatewayToInsanityA21", GatewayToInsanityAnswers[21])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.21"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[9]))
                .Configure();

            AnswerConfigurator.New("GatewayToInsanityA22", GatewayToInsanityAnswers[22])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.22"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[12]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC12", GatewayToInsanityChecks[12])
                .SetDC(32)
                .SetExperience(DialogExperience.NormalExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana)
                .SetSuccess(GatewayToInsanityPages[8])
                .SetFail(GatewayToInsanityPages[8])
                .Configure();


            CueConfigurator.New("GatewayToInsanity39", GatewayToInsanityCues[39])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.39"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[12]))
               .Configure();

            CueConfigurator.New("GatewayToInsanity40", GatewayToInsanityCues[40])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.40"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[12]))
              .SetOnShow(UpdateMythosCount.MythosActionBuilder(1))
              .SetShowOnce()
              .Configure();

        }

        private static void GatewayBeingRead()
        {
            BookPageConfigurator.New("GatewayToInsanityPage8", GatewayToInsanityPages[8])
               .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
               .SetCues(GatewayToInsanityCues[39], GatewayToInsanityCues[40], GatewayToInsanityCues[41], GatewayToInsanityChecks[13])
               .SetAnswers(GatewayToInsanityAnswers[23])
               .Configure();

            CueConfigurator.New("GatewayToInsanity41", GatewayToInsanityCues[41])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.41"))
              .Configure();

            CheckConfigurator.New("GatewayToInsanityC13", GatewayToInsanityChecks[13])
                .SetDC(33)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SaveWill)
                .SetSuccess(GatewayToInsanityCues[42])
                .SetFail(GatewayToInsanityCues[43])
                .Configure();

            AnswerConfigurator.New("GatewayToInsanityA23", GatewayToInsanityAnswers[23])
                .SetText(LocalizationTool.GetString("Plot.Continue"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[9]))
                .Configure();

            CueConfigurator.New("GatewayToInsanity42", GatewayToInsanityCues[42])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.42"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[13]))
              .SetOnShow(UpdateMadness.MadnessActionBuilder(1))
              .Configure();

            CueConfigurator.New("GatewayToInsanity43", GatewayToInsanityCues[43])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.43"))
              .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[13]))
              .SetOnShow(ActionsBuilder.New().TimeSkip(minutesToSkip: new EvaluatorInt(10)).Conditional(ConditionsBuilder.New().FlagInRange(Flags.GatewayToInsanityAlarm,999,1), Flags.IncrementFlag(1,Flags.GatewayToInsanityLongTime)))
              //10 mins pass and if alarm raised mark long time
              .SetContinueValue(Utilities.MakeCueSelection(GatewayToInsanityCues[44]))
              .Configure();

            CueConfigurator.New("GatewayToInsanity44", GatewayToInsanityCues[44])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.44"))
              .SetContinueValue(Utilities.MakeCueSelection(GatewayToInsanityCues[45]))
              .Configure();

            CueConfigurator.New("GatewayToInsanity45", GatewayToInsanityCues[45])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.45"))
              .SetOnShow(UpdateMadness.MadnessActionBuilder(3))
              .Configure();

        }

        private static void GatewayPage9()
        {
            BookPageConfigurator.New("GatewayToInsanityPage9", GatewayToInsanityPages[9])
               .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
               .SetCues(GatewayToInsanityCues[46], GatewayToInsanityCues[47], GatewayToInsanityCues[48], GatewayToInsanityCues[50], GatewayToInsanityCues[51], GatewayToInsanityCues[52], GatewayToInsanityCues[53], GatewayToInsanityCues[54], GatewayToInsanityCues[55], GatewayToInsanityCues[49])
               .SetAnswers(GatewayToInsanityAnswers[24], GatewayToInsanityAnswers[25], GatewayToInsanityAnswers[26], GatewayToInsanityAnswers[27])
               .Configure();

            CueConfigurator.New("GatewayToInsanity46", GatewayToInsanityCues[46])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.46"))
              .Configure();

            CueConfigurator.New("GatewayToInsanity47", GatewayToInsanityCues[47])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.47"))
              .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.GatewayToInsanityAlarm,999,1))
              .Configure();

            CueConfigurator.New("GatewayToInsanity48", GatewayToInsanityCues[48])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.48"))
              .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.GatewayToInsanityAlarm, 999, 1,true))
              .Configure();

            CueConfigurator.New("GatewayToInsanity49", GatewayToInsanityCues[49])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.49"))
              .SetConditions(ConditionsBuilder.New()
                .CheckPassed(GatewayToInsanityChecks[14]).CheckPassed(GatewayToInsanityChecks[15]).CheckPassed(GatewayToInsanityChecks[16])//shows if they passed any of the checks
                .CheckFailed(GatewayToInsanityChecks[14]).CheckFailed(GatewayToInsanityChecks[15]).CheckFailed(GatewayToInsanityChecks[16])//or failed any of the checks
                .UseOr())
              .SetOnShow(Utilities.LootBuilder(Pistol.GatewayLootPistolID, Musket.GatewayLootMusketID).GiveItemToPlayer("f2bc0997c24e573448c6c91d2be88afa",quantity:602))
              .Configure();

            #region Answer24Deception
            AnswerConfigurator.New("GatewayToInsanityA24", GatewayToInsanityAnswers[24])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.24"))
                .SetShowOnce()
                .SetShowConditions(ConditionsBuilder.New().FlagInRange(Flags.GatewayToInsanityAlarm,999,1,true).AddOrAndLogic(ConditionsBuilder.New()
                .CheckPassed(GatewayToInsanityChecks[14]).CheckPassed(GatewayToInsanityChecks[15]).CheckPassed(GatewayToInsanityChecks[16])//shows if they passed any of the checks
                .CheckFailed(GatewayToInsanityChecks[14]).CheckFailed(GatewayToInsanityChecks[15]).CheckFailed(GatewayToInsanityChecks[16])//or failed any of the checks
                .UseOr(),true))//shows if the alarm is not raised and no check has been passed or failed
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[14]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC14", GatewayToInsanityChecks[14])
                .SetDC(30)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.CheckBluff)
                .SetSuccess(GatewayToInsanityPages[9])
                .SetFail(GatewayToInsanityPages[9])
                .Configure();


            CueConfigurator.New("GatewayToInsanity50", GatewayToInsanityCues[50])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.50"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[14]))
               .SetOnShow(ActionsBuilder.New().IncrementFlagValue(BlueprintTool.GetRef<BlueprintUnlockableFlagReference>(Flags.GatewayToInsanityAlarm), true, new EvaluatorInt(1)))
               .Configure();

            CueConfigurator.New("GatewayToInsanity51", GatewayToInsanityCues[51])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.51"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[14]))
              .Configure();
            #endregion

            #region Answer25Diplomacy
            AnswerConfigurator.New("GatewayToInsanityA25", GatewayToInsanityAnswers[25])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.25"))
                .SetShowOnce()
                .SetShowConditions(ConditionsBuilder.New().FlagInRange(Flags.GatewayToInsanityAlarm, 999, 1,true).AddOrAndLogic(ConditionsBuilder.New()
                .CheckPassed(GatewayToInsanityChecks[14]).CheckPassed(GatewayToInsanityChecks[15]).CheckPassed(GatewayToInsanityChecks[16])//shows if they passed any of the checks
                .CheckFailed(GatewayToInsanityChecks[14]).CheckFailed(GatewayToInsanityChecks[15]).CheckFailed(GatewayToInsanityChecks[16])//or failed any of the checks
                .UseOr(), true))//shows if the alarm is not raised
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[15]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC15", GatewayToInsanityChecks[15])
                .SetDC(28)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.CheckDiplomacy)
                .SetSuccess(GatewayToInsanityPages[9])
                .SetFail(GatewayToInsanityPages[9])
                .Configure();


            CueConfigurator.New("GatewayToInsanity52", GatewayToInsanityCues[52])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.52"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[15]))
               .SetOnShow(ActionsBuilder.New().IncrementFlagValue(BlueprintTool.GetRef<BlueprintUnlockableFlagReference>(Flags.GatewayToInsanityAlarm), true, new EvaluatorInt(1)))
               .Configure();

            CueConfigurator.New("GatewayToInsanity53", GatewayToInsanityCues[53])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.53"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[15]))
              .Configure();
            #endregion

            #region Answer26Athletics
            AnswerConfigurator.New("GatewayToInsanityA26", GatewayToInsanityAnswers[26])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.26"))
                .SetShowOnce()
                .SetShowConditions(ConditionsBuilder.New().FlagInRange(Flags.GatewayToInsanityAlarm, 999, 1,true).AddOrAndLogic(ConditionsBuilder.New()
                .CheckPassed(GatewayToInsanityChecks[14]).CheckPassed(GatewayToInsanityChecks[15]).CheckPassed(GatewayToInsanityChecks[16])//shows if they passed any of the checks
                .CheckFailed(GatewayToInsanityChecks[14]).CheckFailed(GatewayToInsanityChecks[15]).CheckFailed(GatewayToInsanityChecks[16])//or failed any of the checks
                .UseOr(), true))//shows if the alarm is not raised
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[16]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC16", GatewayToInsanityChecks[16])
                .SetDC(26)
                .SetExperience(DialogExperience.SmallExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillAthletics)
                .SetSuccess(GatewayToInsanityPages[9])
                .SetFail(GatewayToInsanityPages[9])
                .Configure();


            CueConfigurator.New("GatewayToInsanity54", GatewayToInsanityCues[54])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.54"))
               .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[16]))
               .SetOnShow(ActionsBuilder.New().IncrementFlagValue(BlueprintTool.GetRef<BlueprintUnlockableFlagReference>(Flags.GatewayToInsanityAlarm), true, new EvaluatorInt(1)))
               .Configure();

            CueConfigurator.New("GatewayToInsanity55", GatewayToInsanityCues[55])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.55"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[16]))
              .Configure();
            #endregion

            AnswerConfigurator.New("GatewayToInsanityA27", GatewayToInsanityAnswers[27])
                .SetText(LocalizationTool.GetString("Plot.Continue"))
                .SetShowOnce()
                .SetShowConditions(ConditionsBuilder.New().FlagInRange(Flags.GatewayToInsanityAlarm, 999, 1)//shows if the alarm is raised
                    .CheckPassed(GatewayToInsanityChecks[14]).CheckPassed(GatewayToInsanityChecks[15]).CheckPassed(GatewayToInsanityChecks[16])//shows if they passed any of the checks
                    .CheckFailed(GatewayToInsanityChecks[14]).CheckFailed(GatewayToInsanityChecks[15]).CheckFailed(GatewayToInsanityChecks[16])//or failed any of the checks
                    .UseOr())
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[10]))
                .Configure();

        }

        private static void GatewayLab()
        {
            BookPageConfigurator.New("GatewayToInsanityPage10", GatewayToInsanityPages[10])
               .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
               .SetCues(GatewayToInsanityCues[56], GatewayToInsanityCues[57], GatewayToInsanityCues[58], GatewayToInsanityCues[59], GatewayToInsanityCues[60])
               .SetAnswers(GatewayToInsanityAnswers[28], GatewayToInsanityAnswers[29], GatewayToInsanityAnswers[30], GatewayToInsanityAnswers[31])
               .Configure();

            CueConfigurator.New("GatewayToInsanity56", GatewayToInsanityCues[56])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.56"))
              .Configure();

            CueConfigurator.New("GatewayToInsanity57", GatewayToInsanityCues[57])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.57"))
              .Configure();

            CueConfigurator.New("GatewayToInsanity58", GatewayToInsanityCues[58])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.58"))
              .Configure();

            AnswerConfigurator.New("GatewayToInsanityA28", GatewayToInsanityAnswers[28])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.28"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[10]))
                .Configure();

            CueConfigurator.New("GatewayToInsanity59", GatewayToInsanityCues[59])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.59"))
              .SetConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[28]))
              .Configure();

            AnswerConfigurator.New("GatewayToInsanityA29", GatewayToInsanityAnswers[29])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.29"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[10]))
                .Configure();

            CueConfigurator.New("GatewayToInsanity60", GatewayToInsanityCues[60])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.60"))
              .SetConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[29]))
              .Configure();

            AlignmentShift TreatedFleshwarpsAsMonsters = new AlignmentShift();
            TreatedFleshwarpsAsMonsters.Direction = AlignmentShiftDirection.Evil;
            TreatedFleshwarpsAsMonsters.Value = 2;
            TreatedFleshwarpsAsMonsters.Description = LocalizationTool.GetString("Plot.Alignment.Fleshwarps.TreatedAsMonsters");



            AnswerConfigurator.New("GatewayToInsanityA30", GatewayToInsanityAnswers[30])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.30"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[11]))
                .SetAlignmentShift(TreatedFleshwarpsAsMonsters)
                .SetOnSelect(ActionsBuilder.New().IncrementFlagValue(Flags.CowgirlApproval, true, new EvaluatorInt(-2)))
                .Configure();

            AlignmentShift TriedToHelpFleshwarps = new AlignmentShift();
            TriedToHelpFleshwarps.Direction = AlignmentShiftDirection.Good;
            TriedToHelpFleshwarps.Value = 2;
            TriedToHelpFleshwarps.Description = LocalizationTool.GetString("Plot.Alignment.Fleshwarps.TreatedAsVictims");


            AnswerConfigurator.New("GatewayToInsanityA31", GatewayToInsanityAnswers[31])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.31"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[12]))
                .SetAlignmentShift(TriedToHelpFleshwarps)
                .SetOnSelect(ActionsBuilder.New().IncrementFlagValue(Flags.CowgirlApproval,true, new EvaluatorInt(2)))
                .Configure();
        }

        private static void GatewayFleshwarpsAsMonsters()
        {
            BookPageConfigurator.New("GatewayToInsanityPage11", GatewayToInsanityPages[11])
               .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
               .SetCues(GatewayToInsanityCues[61], GatewayToInsanityCues[62], GatewayToInsanityCues[63], GatewayToInsanityCues[64], GatewayToInsanityCues[65])
               .SetAnswers(GatewayToInsanityAnswers[32], GatewayToInsanityAnswers[33], GatewayToInsanityAnswers[34], GatewayToInsanityAnswers[35])
               .Configure();

            CueConfigurator.New("GatewayToInsanity61", GatewayToInsanityCues[61])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.61"))
              .Configure();

            AnswerConfigurator.New("GatewayToInsanityA32", GatewayToInsanityAnswers[32])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.32"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[11]))
                .SetOnSelect(ActionsBuilder.New().IncrementFlagValue(Flags.CowgirlApproval, true, new EvaluatorInt(-2)).SetObjectiveStatus(HelpTheFleshwarpsQuestGUID, status: Kingmaker.Designers.Quests.Common.SummonPoolCountTrigger.ObjectiveStatus.Fail))
                .Configure();

            CueConfigurator.New("GatewayToInsanity62", GatewayToInsanityCues[62])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.62"))
              .SetConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[32]))
              .Configure();

            AnswerConfigurator.New("GatewayToInsanityA33", GatewayToInsanityAnswers[33])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.33"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[17]))
                .SetOnSelect(ActionsBuilder.New()
                    .IncrementFlagValue(Flags.CowgirlApproval, true, new EvaluatorInt(-1))//lose 1 approval
                    .IncrementFlagValue(Flags.CowgirlRespect, true, new EvaluatorInt(-1))//and one respect
                    .IncrementFlagValue(Flags.GatewayToInsanityLongTime, true, new EvaluatorInt(1))//and you've taken a long tiem
                    .TimeSkip(minutesToSkip: new EvaluatorInt(30)))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC17", GatewayToInsanityChecks[17])
                .SetDC(28)
                .SetExperience(DialogExperience.NormalExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana)
                .SetSuccess(GatewayToInsanityPages[11])
                .SetFail(GatewayToInsanityPages[11])
                .Configure();

            CueConfigurator.New("GatewayToInsanity63", GatewayToInsanityCues[63])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.63"))
              .SetConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[33]))
              .Configure();

            CueConfigurator.New("GatewayToInsanity64", GatewayToInsanityCues[64])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.64"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[17]))
              //add decree Brainweaver notes, increment flag FleshwarpResearch to 1
              .Configure();

            CueConfigurator.New("GatewayToInsanity65", GatewayToInsanityCues[65])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.65"))
              .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[17]))
              .SetOnShow(UpdateMadness.MadnessActionBuilder(1).IncrementFlagValue(Flags.CowgirlRespect, true, new EvaluatorInt(-1)))
              .Configure();

            AnswerConfigurator.New("GatewayToInsanityA34", GatewayToInsanityAnswers[34])
                .SetText(LocalizationTool.GetString("Plot.Continue"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[13]))
                .SetShowConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[33]).AnswerSelected(GatewayToInsanityAnswers[32]))
                .Configure();

            AnswerConfigurator.New("GatewayToInsanityA35", GatewayToInsanityAnswers[35])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.35"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[13]))
                .SetShowConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[33],negate:true))
                //unlock fleshwarps event if not already
                .Configure();
        }

        private static void GatewayFleshwarpVictims()
        {
            BookPageConfigurator.New("GatewayToInsanityPage12", GatewayToInsanityPages[12])
               .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
               .SetCues(GatewayToInsanityCues[66], GatewayToInsanityCues[67], GatewayToInsanityCues[68], GatewayToInsanityCues[69], GatewayToInsanityCues[70], GatewayToInsanityCues[71], GatewayToInsanityCues[72], GatewayToInsanityCues[73], GatewayToInsanityCues[74], GatewayToInsanityCues[75], GatewayToInsanityCues[76], GatewayToInsanityCues[77])
               .SetAnswers(GatewayToInsanityAnswers[36], GatewayToInsanityAnswers[37], GatewayToInsanityAnswers[38], GatewayToInsanityAnswers[39], GatewayToInsanityAnswers[40], GatewayToInsanityAnswers[41], GatewayToInsanityAnswers[42], GatewayToInsanityAnswers[43])
               .Configure();

            CueConfigurator.New("GatewayToInsanity66", GatewayToInsanityCues[66])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.66"))
              .Configure();

            CueConfigurator.New("GatewayToInsanity67", GatewayToInsanityCues[67])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.67"))
              //unlock fleshwarps event
              .Configure();

            #region Answer36Diplomacy
            AnswerConfigurator.New("GatewayToInsanityA36", GatewayToInsanityAnswers[36])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.36"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[18]))
                .SetShowConditions(ConditionsBuilder.New().FlagInRange(Flags.TreatedFleshwarps,0).FlagUnlocked(Flags.TreatedFleshwarps,negate:true).UseOr())//show only if you have not already treated them once
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC18", GatewayToInsanityChecks[18])
                .SetDC(33)
                .SetExperience(DialogExperience.NormalExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.CheckDiplomacy)
                .SetSuccess(GatewayToInsanityPages[12])
                .SetFail(GatewayToInsanityPages[12])
                .Configure();

            CueConfigurator.New("GatewayToInsanity68", GatewayToInsanityCues[68])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.68"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[18]))
              .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.TreatedFleshwarps,true,new EvaluatorInt(1)))
              .Configure();

            CueConfigurator.New("GatewayToInsanity69", GatewayToInsanityCues[69])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.69"))
              .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[18]))
              .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.GatewayToInsanityLongTime, true, new EvaluatorInt(1)).TimeSkip(minutesToSkip: new EvaluatorInt (10)))
              .Configure();
            #endregion


            #region Answer37Arcana
            AnswerConfigurator.New("GatewayToInsanityA37", GatewayToInsanityAnswers[37])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.37"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[19]))
                .SetOnSelect(ActionsBuilder.New().TimeSkip(minutesToSkip: new EvaluatorInt(30))
                .IncrementFlagValue(Flags.GatewayToInsanityLongTime, true, new EvaluatorInt(1)))//and you've taken a long tiem
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC19", GatewayToInsanityChecks[19])
                .SetDC(28)
                .SetExperience(DialogExperience.NormalExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana)
                .SetSuccess(GatewayToInsanityPages[12])
                .SetFail(GatewayToInsanityPages[12])
                .Configure();

            CueConfigurator.New("GatewayToInsanity70", GatewayToInsanityCues[70])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.70"))
              .SetConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[37]))
              .Configure();

            CueConfigurator.New("GatewayToInsanity71", GatewayToInsanityCues[71])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.71"))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[19]))
              .Configure();

            CueConfigurator.New("GatewayToInsanity72", GatewayToInsanityCues[72])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.72"))
              .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[19]))
              .SetOnShow(UpdateMadness.MadnessActionBuilder(1))
              .Configure();
            #endregion

            #region Answer38LoreReligion
            AnswerConfigurator.New("GatewayToInsanityA38", GatewayToInsanityAnswers[38])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.38"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[20]))
                .SetShowConditions(ConditionsBuilder.New().FlagInRange(Flags.TreatedFleshwarps, 0).FlagUnlocked(Flags.TreatedFleshwarps, negate: true).UseOr())//show only if you have not already successfuly treated them
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC20", GatewayToInsanityChecks[20])
                .SetDC(31)
                .SetExperience(DialogExperience.NormalExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillLoreReligion)
                .SetSuccess(GatewayToInsanityPages[12])
                .SetFail(GatewayToInsanityPages[12])
                .Configure();

            CueConfigurator.New("GatewayToInsanity73", GatewayToInsanityCues[73])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.73"))
              .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.TreatedFleshwarps, true, new EvaluatorInt(1)))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[20]))
              .Configure();

            CueConfigurator.New("GatewayToInsanity74", GatewayToInsanityCues[74])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.74"))
              .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[20]))
              .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.GatewayToInsanityLongTime, true, new EvaluatorInt(1)).TimeSkip(minutesToSkip: new EvaluatorInt(10)))
              .Configure();
            #endregion

            #region Answer39Aeon
            AnswerConfigurator.New("GatewayToInsanityA39", GatewayToInsanityAnswers[39])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.39"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[12]))
                .SetMythicRequirement(Mythic.PlayerIsAeon)
                .SetShowConditions(ConditionsBuilder.New().FlagInRange(Flags.TreatedFleshwarps, 0).FlagUnlocked(Flags.TreatedFleshwarps, negate: true).UseOr())//show only if you have not already successfuly treated them and are an aeon
                .Configure();


            CueConfigurator.New("GatewayToInsanity75", GatewayToInsanityCues[75])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.75"))
              .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.TreatedFleshwarps, true, new EvaluatorInt(1)))
              .SetConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[39]))
              .Configure();

            #endregion

            #region Answer40Angel
            AnswerConfigurator.New("GatewayToInsanityA40", GatewayToInsanityAnswers[40])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.40"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[12]))
                .SetMythicRequirement(Mythic.PlayerIsAngel)
                .SetShowConditions(ConditionsBuilder.New().FlagInRange(Flags.TreatedFleshwarps, 0).FlagUnlocked(Flags.TreatedFleshwarps, negate: true).UseOr())//show only if you have not already successfuly treated them and are an angel
                .Configure();


            CueConfigurator.New("GatewayToInsanity76", GatewayToInsanityCues[76])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.76"))
              .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.TreatedFleshwarps, true, new EvaluatorInt(1)))
              .SetConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[40]))
              .Configure();

            #endregion

            AnswerConfigurator.New("GatewayToInsanityA41", GatewayToInsanityAnswers[41])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.41"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[12]))
                .SetShowConditions(ConditionsBuilder.New().FlagInRange(Flags.TreatedFleshwarps, minValue:1, maxValue: 999))//show only if you have successfuly treated them
                .Configure();

            CueConfigurator.New("GatewayToInsanity77", GatewayToInsanityCues[77])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.77"))
              .SetConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[41]))
              .Configure();

            AnswerConfigurator.New("GatewayToInsanityA42", GatewayToInsanityAnswers[42])
               .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.41"))
               .SetShowOnce()
               .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[13]))
               .SetShowConditions(ConditionsBuilder.New()
                .FlagUnlocked(Flags.TreatedFleshwarps, negate: true)
                .AnswerSelected(GatewayToInsanityAnswers[38])
                .AnswerSelected(GatewayToInsanityAnswers[37])
                .AnswerSelected(GatewayToInsanityAnswers[36])
                )//show only if you have not successfuly treated them and have tried everything
               .Configure();

            AnswerConfigurator.New("GatewayToInsanityA43", GatewayToInsanityAnswers[43])
                .SetText(LocalizationTool.GetString("Plot.Continue"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[13]))
                .SetShowConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[41]))//show only if you have tried to leave and gotten the sort of thanks from fleshwarps
                .Configure();

        }

        private static void GatewayWarpedHallway()
        {
            BookPageConfigurator.New("GatewayToInsanityPage13", GatewayToInsanityPages[13])
               .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
               .SetCues(GatewayToInsanityCues[78], GatewayToInsanityCues[79], GatewayToInsanityCues[80], GatewayToInsanityCues[81], GatewayToInsanityCues[82], GatewayToInsanityCues[83], GatewayToInsanityCues[84], GatewayToInsanityCues[85], GatewayToInsanityCues[86], GatewayToInsanityCues[87], GatewayToInsanityCues[88])
               .SetAnswers(GatewayToInsanityAnswers[44], GatewayToInsanityAnswers[45], GatewayToInsanityAnswers[46], GatewayToInsanityAnswers[47], GatewayToInsanityAnswers[48])
               .Configure();

            CueConfigurator.New("GatewayToInsanity78", GatewayToInsanityCues[78])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.78"))
              .Configure();

            CueConfigurator.New("GatewayToInsanity79", GatewayToInsanityCues[79])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.79"))
              .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.GatewayToInsanityAlarm,minValue:1, maxValue: 999).FlagInRange(Flags.GatewayToInsanityLongTime, maxValue: 0))
              .Configure();

            CueConfigurator.New("GatewayToInsanity80", GatewayToInsanityCues[80])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.80"))
              .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.GatewayToInsanityAlarm, minValue: 1, maxValue: 999).FlagInRange(Flags.GatewayToInsanityLongTime, minValue: 1, maxValue: 999))
              .Configure();

            CueConfigurator.New("GatewayToInsanity81", GatewayToInsanityCues[81])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.81"))
              .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.GatewayToInsanityAlarm, maxValue: 0))
              .Configure();

            CueConfigurator.New("GatewayToInsanity82", GatewayToInsanityCues[82])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.82"))
              .Configure();

            #region Answer44Trickster
            AnswerConfigurator.New("GatewayToInsanityA44", GatewayToInsanityAnswers[44])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.44"))
                .SetShowOnce()
                .SetShowConditions(ConditionsBuilder.New()
                                    .AnswerSelected(GatewayToInsanityAnswers[44], negate: true)
                                    .AnswerSelected(GatewayToInsanityAnswers[45], negate: true)
                                    .AnswerSelected(GatewayToInsanityAnswers[46], negate: true)
                                    .AnswerSelected(GatewayToInsanityAnswers[47], negate: true))
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[13]))
                .SetMythicRequirement(Mythic.PlayerIsTrickster)
                .Configure();

            CueConfigurator.New("GatewayToInsanity83", GatewayToInsanityCues[83])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.83"))
              .SetConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[44]))
              .Configure();

            #endregion

            #region Answer45Mythos
            AnswerConfigurator.New("GatewayToInsanityA45", GatewayToInsanityAnswers[45])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.45"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[13]))
                .SetShowConditions(ConditionsBuilder.New().FlagInRange(Flags.Mythos,minValue:1, maxValue: 999)
                                    .AnswerSelected(GatewayToInsanityAnswers[44], negate: true)
                                    .AnswerSelected(GatewayToInsanityAnswers[45], negate: true)
                                    .AnswerSelected(GatewayToInsanityAnswers[46], negate: true)
                                    .AnswerSelected(GatewayToInsanityAnswers[47], negate: true))
                .Configure();

            CueConfigurator.New("GatewayToInsanity84", GatewayToInsanityCues[84])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.84"))
              .SetConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[45]))
              .Configure();

            #endregion

            DamageDescription SuccessDamage = new DamageDescription();
            SuccessDamage.SetDice(new ModifiableDiceFormula(new Kingmaker.RuleSystem.DiceFormula(2, Kingmaker.RuleSystem.DiceType.D6)));

            DamageDescription FailDamage = new DamageDescription();
            FailDamage.SetDice(new ModifiableDiceFormula(new Kingmaker.RuleSystem.DiceFormula(6, Kingmaker.RuleSystem.DiceType.D6)));


            DCModifier AlarmShortTime = new DCModifier();
            AlarmShortTime.Conditions = ConditionsBuilder.New()
                .FlagInRange(Flags.GatewayToInsanityAlarm, minValue: 1, maxValue: 999)//if the alarm is raised
                .AddOrAndLogic(//and either
                ConditionsBuilder.New()
                    .FlagInRange(Flags.GatewayToInsanityLongTime, maxValue: 0)//long time is 0
                    .FlagUnlocked(Flags.GatewayToInsanityAlarm,negate:true).UseOr()//or long time is not defined
                    ).Build();
            AlarmShortTime.Mod = 5;
            DCModifier AlarmLongTime = new DCModifier();
            AlarmLongTime.Conditions = ConditionsBuilder.New()
                .FlagInRange(Flags.GatewayToInsanityAlarm, minValue: 1, maxValue: 999)//if the alarm is raised
                    .FlagInRange(Flags.GatewayToInsanityLongTime, minValue: 1, maxValue: 999).Build();//long time is 1
            AlarmLongTime.Mod = 5;


            #region Answer46Reflex
            AnswerConfigurator.New("GatewayToInsanityA46", GatewayToInsanityAnswers[46]) 
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.46"))
                .SetShowOnce()
                .SetShowConditions(ConditionsBuilder.New()
                                    .AnswerSelected(GatewayToInsanityAnswers[44], negate: true)
                                    .AnswerSelected(GatewayToInsanityAnswers[45], negate: true)
                                    .AnswerSelected(GatewayToInsanityAnswers[46], negate: true)
                                    .AnswerSelected(GatewayToInsanityAnswers[47], negate: true))
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[21]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC21", GatewayToInsanityChecks[21])
                .SetDC(28)
                .SetDCModifiers(AlarmShortTime, AlarmLongTime)
                .SetExperience(DialogExperience.NormalExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SaveReflex)
                .SetSuccess(GatewayToInsanityPages[13])
                .SetFail(GatewayToInsanityPages[13])
                .Configure();

            CueConfigurator.New("GatewayToInsanity85", GatewayToInsanityCues[85])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.85"))
              .SetOnShow(ActionsBuilder.New().DamageParty(SuccessDamage, new PlayerCharacter()))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[21]).CheckPassed(GatewayToInsanityChecks[22]).CheckPassed(GatewayToInsanityChecks[23]).UseOr())
              .Configure();

            CueConfigurator.New("GatewayToInsanity86", GatewayToInsanityCues[86])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.86"))
              .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[21]).CheckFailed(GatewayToInsanityChecks[22]).CheckFailed(GatewayToInsanityChecks[23]).UseOr())
              .SetOnShow(ActionsBuilder.New().DamageParty(FailDamage, new PlayerCharacter()).DealLevelDamage(new PlayerCharacter(),new DiceFormula(),2,EnergyDrainType.Permanent,null))
              .Configure();

            #endregion

            #region Answer47Perception
            AnswerConfigurator.New("GatewayToInsanityA47", GatewayToInsanityAnswers[47])
                .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Answer.47"))
                .SetShowOnce()
                .SetShowConditions(ConditionsBuilder.New()
                                    .AnswerSelected(GatewayToInsanityAnswers[44], negate: true)
                                    .AnswerSelected(GatewayToInsanityAnswers[45], negate: true)
                                    .AnswerSelected(GatewayToInsanityAnswers[46], negate: true)
                                    .AnswerSelected(GatewayToInsanityAnswers[47], negate: true))
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityChecks[24]))
                .Configure();

            CheckConfigurator.New("GatewayToInsanityC24", GatewayToInsanityChecks[24])
                .SetDC(28)
                .SetDCModifiers(AlarmShortTime, AlarmLongTime)
                .SetExperience(DialogExperience.NormalExperience)
                .SetType(Kingmaker.EntitySystem.Stats.StatType.SkillPerception)
                .SetSuccess(GatewayToInsanityPages[13])
                .SetFail(GatewayToInsanityPages[13])
                .Configure();

            CueConfigurator.New("GatewayToInsanity87", GatewayToInsanityCues[87])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.85"))
              .SetOnShow(ActionsBuilder.New().DamageParty(SuccessDamage, new PlayerCharacter()))
              .SetConditions(ConditionsBuilder.New().CheckPassed(GatewayToInsanityChecks[24]))
              .Configure();

            CueConfigurator.New("GatewayToInsanity88", GatewayToInsanityCues[88])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.86"))
              .SetConditions(ConditionsBuilder.New().CheckFailed(GatewayToInsanityChecks[24]))
              .SetOnShow(ActionsBuilder.New().DamageParty(FailDamage, new PlayerCharacter()).DealLevelDamage(new PlayerCharacter(), new DiceFormula(), 2, EnergyDrainType.Permanent, null))
              .Configure();

            #endregion

            AnswerConfigurator.New("GatewayToInsanityA48", GatewayToInsanityAnswers[48])
                .SetText(LocalizationTool.GetString("Plot.Continue"))
                .SetShowOnce()
                .SetShowConditions(ConditionsBuilder.New()
                                    .AnswerSelected(GatewayToInsanityAnswers[44])
                                    .AnswerSelected(GatewayToInsanityAnswers[45])
                                    .AnswerSelected(GatewayToInsanityAnswers[46])
                                    .AnswerSelected(GatewayToInsanityAnswers[47])
                                    .UseOr())
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[14]))
                .Configure();


        }

        private static void GatewayEnd()
        {
            BookPageConfigurator.New("GatewayToInsanityPage14", GatewayToInsanityPages[14])
               .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
               .SetCues(GatewayToInsanityCues[89], GatewayToInsanityCues[90], GatewayToInsanityCues[91], GatewayToInsanityCues[92], GatewayToInsanityCues[93], GatewayToInsanityCues[94])
               .SetAnswers(GatewayToInsanityAnswers[49], GatewayToInsanityAnswers[50], GatewayToInsanityAnswers[51], GatewayToInsanityAnswers[52])
               .Configure();

            CueConfigurator.New("GatewayToInsanity89", GatewayToInsanityCues[89])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.89"))
              .Configure();

            CueConfigurator.New("GatewayToInsanity90", GatewayToInsanityCues[90])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.90"))
              .Configure();

            CueConfigurator.New("GatewayToInsanity91", GatewayToInsanityCues[91])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.91"))
              .Configure();

            CueConfigurator.New("GatewayToInsanity92", GatewayToInsanityCues[92])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.92"))
              .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.TreatedFleshwarps,minValue:1,maxValue:999))
              .Configure();

            CueConfigurator.New("GatewayToInsanity93", GatewayToInsanityCues[93])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.93"))
              .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.TreatedFleshwarps, maxValue: 0).FlagUnlocked(Flags.TreatedFleshwarps,negate:true).UseOr())
              .Configure();

            CueConfigurator.New("GatewayToInsanity94", GatewayToInsanityCues[94])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.94"))
              .SetConditions(ConditionsBuilder.New().AnswerSelected(GatewayToInsanityAnswers[32],true,true))//so long as you have not killed the fleshwarps
              .SetOnShow(ActionsBuilder.New().IncrementFlagValue(Flags.CowgirlInDrezen,true,new EvaluatorInt(1)))//put her back in drezen
              .Configure();

           AnswerConfigurator.New("GatewayToInsanityA52", GatewayToInsanityAnswers[52])
                .SetText(LocalizationTool.GetString("Plot.Continue"))
                .SetShowOnce()
                .SetNextCue(Utilities.MakeCueSelection(GatewayToInsanityPages[15]))
                .Configure();
        }

        private static void GatewayReward()
        {
            BookPageConfigurator.New("GatewayToInsanityPage15", GatewayToInsanityPages[15])
               .SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Title"))
               .SetCues(GatewayToInsanityCues[97], GatewayToInsanityCues[98], GatewayToInsanityCues[99])
               .SetAnswers(GatewayToInsanityAnswers[53])
               .Configure();



            CueConfigurator.New("GatewayToInsanity97", GatewayToInsanityCues[97])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.97"))
              .Configure();

            CueConfigurator.New("GatewayToInsanity98", GatewayToInsanityCues[98])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.98"))
              .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.GatewayToInsanityLongTime, maxValue: 0).FlagUnlocked(Flags.GatewayToInsanityLongTime,negate:true).UseOr())
              .SetOnShow(Utilities.LootBuilder(Rifle.GatewayLootRifleID).GiveItemToPlayer("f2bc0997c24e573448c6c91d2be88afa", quantity: 1386))//give good loot plus unique rifle
              .Configure();

            CueConfigurator.New("GatewayToInsanity99", GatewayToInsanityCues[99])
              .SetText(LocalizationTool.GetString("Plot.GatewayToInsanity.Cue.99"))
              .SetConditions(ConditionsBuilder.New().FlagInRange(Flags.GatewayToInsanityLongTime, minValue: 1, maxValue: 999))
              .SetOnShow(ActionsBuilder.New().GiveItemToPlayer("f2bc0997c24e573448c6c91d2be88afa", quantity: 1000))//give modest gold
              .Configure();

            AnswerConfigurator.New("GatewayToInsanityA53", GatewayToInsanityAnswers[53])
                .SetText(LocalizationTool.GetString("Plot.Leave"))
                .SetOnSelect(ActionsBuilder.New().FinishObjective(ReachTheGatewayToInsanityGUID).IncrementFlagValue(Flags.CompletedGatewayToInsanity,true, new EvaluatorInt(1)))//complete reach the gateway to insanity
                .Configure();
        }


        private static void Fleshwarps()
        {//this one is the parent for all the events pertaining to the fleshwarps
            QuestObjectiveConfigurator HelpTheFleshwarpsQuest = QuestObjectiveConfigurator.New("GatewayToInsanityFleshwarps", HelpTheFleshwarpsQuestGUID);
            HelpTheFleshwarpsQuest.SetTitle(LocalizationTool.GetString("Plot.GatewayToInsanity.Fleshwarps.Title"));
            HelpTheFleshwarpsQuest.SetDescription(LocalizationTool.GetString("Plot.GatewayToInsanity.Fleshwarps.Description"));
            HelpTheFleshwarpsQuest.AddExperience(cR: 10, encounter: Kingmaker.Blueprints.Classes.Experience.EncounterType.QuestMain, dummy: false, modifier: 1.0f);
            HelpTheFleshwarpsQuest.SetQuest(BlueprintTool.Get<BlueprintQuest>(GatewayToInsanityQuestGUID));
            HelpTheFleshwarpsQuest.Configure();

        }
    }
}
