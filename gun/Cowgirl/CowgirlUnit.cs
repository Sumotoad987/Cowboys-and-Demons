using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.StoryEx;
using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Customization;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Assets;
using gun.Classes.Gunslinger;
using gun.Classes.Spellscar_Drifter;
using gun.Firearms;
using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.CharGen;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.DialogSystem;
using Kingmaker.Dungeon;
using Kingmaker.Dungeon.Units;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Localization;
using Kingmaker.PubSubSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.ResourceManagement;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Components;
using Kingmaker.UnitLogic.Customization;
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
        public static string GUID = "16bd150344a748a6b41dddcb808b27b3";
        public static string FeaturesGUID = "3239ec314a7148f082f8fd187635c54f";


        public static void Configure()
        {
            Secrets.Configure();
            UnitConfigurator Cowgirl = UnitConfigurator.New("CowgirlUnit", GUID).CopyFrom(UnitHelper.CustomCompanion());
            Cowgirl.AddUnitIsStoryCompanion();
            Cowgirl.SetDisplayName(LocalizationTool.GetString("Cowgirl.Name"));
            Cowgirl.SetGender(Gender.Female);
            //Cowgirl.SetColor(); Not sure what to do with this one
            Cowgirl.SetRace(BlueprintTool.GetRef<BlueprintRaceReference>("0a5d473ead98b0646b94495af250fdc4"));//human (sort of)
            //will need to make a portrait
            Portrait.Configure();
            Cowgirl.SetPortrait(Portrait.GUID);

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


          
            Cowgirl.AddFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(FeaturesGUID) ]);
            Cowgirl.Configure();
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

            SelectionEntry[] CowgirlSelections = { OrderSelection, BasicFeats, BonusFeats, Background, Deity, Horse };
            return CowgirlSelections;
        }

        public static DialogSpeaker GetSpeaker()
        {
            DialogSpeaker CowgirlSpeaker = new DialogSpeaker();
            CowgirlSpeaker.m_Blueprint = BlueprintTool.GetRef<BlueprintUnitReference>(GUID);
            CowgirlSpeaker.m_SpeakerPortrait = BlueprintTool.GetRef<BlueprintUnitReference>(GUID);
            return CowgirlSpeaker;
        }
        
    }

    public class RecruitCowgirl : GameAction
    {
        public override string GetCaption()
        {
            return "RecruitingCowgirl";
        }

        public override void RunAction()
        {
            //iset the flags for her being in drezen and in the party
            ActionsBuilder.New().IncrementFlagValue(gun.Plot.Flags.CowgirlInParty, true,new EvaluatorInt(1)).IncrementFlagValue(gun.Plot.Flags.CowgirlInDrezen, true, new EvaluatorInt(1)).Build().Run();

            //spawn a CowgirlUnit and add it as a companion (I sure hope this works)
            Game.Instance.Player.AddCompanion(Game.Instance.EntityCreator.SpawnUnit(BlueprintTool.Get<BlueprintUnit>(Cowgirl.CowgirlUnit.GUID), new Vector3(), Quaternion.identity, Game.Instance.State.LoadedAreaState.MainState));
        }

        public static ActionsBuilder Builder()
        {
            return ActionsBuilder.New().Add(new RecruitCowgirl());
        }
    }
}
