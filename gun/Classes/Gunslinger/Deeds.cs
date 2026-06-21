using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Properties;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using gun.Firearms;
using HarmonyLib;
using Kingmaker.Assets.UnitLogic.Mechanics.Properties;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace gun.Classes.Gunslinger
{
    internal static class Deeds
    {
        public const string QuickClearFeatureGUID = "dc6e1af7153f4559a8f079042c11edbd";
        public const string QuickClearMoveGUID = "f860c6cf75954be5a16f43079a0bae3c";
        public const string QuickClearStandardGUID = "ee9c247c927d4e34adf27531c38e6223";

        public const string SlingerInitFeatureGUID = "b0f506f8f15c47beafd147d160cce7a0";
        public const string SlingerInitUnlockGUID = "bc993c8b363249a2ab29b8e321313bf6";

        public const string TargetingFeatureGUID = "2aa215c79a5a43939bccf31b92dfd8ec";
        public const string TargetingBaseGUID = "e046a532e81c4e6f8cd794ec78ed36f0";
        public const string TargetingHeadGUID = "c324929600bc493bae9564c11c334bf5";
        public const string TargetingArmsGUID = "170e5024c6864e9696f581210a663067";
        public const string TargetingLegsGUID = "8ae6b078d33a4b51808108a13d36e2e2";

        public const string EvasiveDeedGUID = "bcb8f165ce464ba3bd86167831b8d0c2";
        public const string EvasiveDeedBuffGUID = "c31a76e3e8144b86ac69c1c59cf46495";

        public const string BleedingShotFeatureGUID = "8f56d6501f2d46d88ac333e5eedfbbea";
        public const string BleedingShotGUID = "2c3803357b454571900fefafb817906f";
        public const string BleedingShotBuffGUID = "98f3f2d1187a40feb503b9f87dfd5fca";
        public const string BasicBleedBuffGUID = "3b9efbe756934564909aeaee3cdc6ae8";
        public const string BleedingShotSTRGUID = "6ff0f2054f6c480d8204b485ec5d82d1";
        public const string BleedingShotSTRBuffGUID = "d9ed98d245a54d63860ca577ca2fced8";
        public const string STRBleedBuffGUID = "8edd553b50f749a88f7de26e27061d9e";
        public const string BleedingShotCONGUID = "67200283eb474d9b9f403265e58c2e6c";
        public const string BleedingShotCONBuffGUID = "a1152c0c035a425fa691addd69196e5c";
        public const string CONBleedBuffGUID = "0f89f41de3954fcfb491265ca7cc728d";
        public const string BleedingShotDEXGUID = "d17d422140124c819888f8c49bfe70a2";
        public const string BleedingShotDEXBuffGUID = "0ead67c8585f40ecb8daffe115f19215";
        public const string DEXBleedBuffGUID = "1f90636a96394c8c915f5c273b29357a";

        public const string StunningShotFeatureGUID = "a3b2904ecc2747b8abd53a600d8bb93e"; 
        public const string StunningShotGUID = "5acdd0898bad480e98d43fbd8ebf86eb";
        public const string StunningShotBuffGUID = "db4e36a01ddb4fb68e31007db64a8246";
        public const string StunningShotDCGUID = "3dc1b5ff843f41bbbef92ba0f96b8210";

        public static void Configure()
        {
            FirstLevelDeeds();
            ThirdLevelDeeds();
            SeventhLevelDeeds();
            EleventhLevelDeeds();
            FifteenthLevelDeeds();
            NineteenthLevelDeeds();
        }
        public static void FirstLevelDeeds()
        {
            //Deadeye cares about mechanics not in the game

            //Gunslinger's dodge not really manageable in the game

            //quick clear is you have one grit can clear broken firearm as standard action or spend 1 grit to do it as move acion
            QuickClear();
        }

        public static void QuickClear()
        {
            //quick clear is you have one grit can clear broken firearm as standard action or spend 1 grit to do it as move acion
            byte[] data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/Reload.png");
            Texture2D texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite icon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));
            ContextActionRemoveBuff QuickClear = new ContextActionRemoveBuff();//will need to tweak this later to account for capacity for now will simply forbid stacking on reload and have no weapons with capacity
            QuickClear.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(DamagedFirearm.DamagedFirearmGUID);

            AbilityEffectRunAction QuickClearAction = new AbilityEffectRunAction();
            QuickClearAction.SavingThrowType = Kingmaker.EntitySystem.Stats.SavingThrowType.Unknown;
            QuickClearAction.IgnoreCaster = false;
            QuickClearAction.Actions = new Kingmaker.ElementsSystem.ActionList();
            QuickClearAction.Actions.Actions = new Kingmaker.ElementsSystem.GameAction[1] { QuickClear };


            AbilityConfigurator.New("Quick Clear Move", QuickClearMoveGUID)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.CombatManeuver)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetDisplayName(LocalizationTool.GetString("Deeds.QuickClear.Move.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.QuickClear.Move.Description"))
                .SetIcon(icon)
                .AddComponent(QuickClearAction)
                .AddComponent(new HasDamagedFirearm())
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: Grit.GritResource)
                .Configure();

            AbilityConfigurator.New("Quick Clear Standard", QuickClearStandardGUID)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.CombatManeuver)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetDisplayName(LocalizationTool.GetString("Deeds.QuickClear.Standard.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.QuickClear.Standard.Description"))
                .SetIcon(icon)
                .AddComponent(QuickClearAction)
                .AddComponent(new HasDamagedFirearm())
                .AddAbilityResourceLogic(1, isSpendResource: false, requiredResource: Grit.GritResource)
                .Configure();

            BlueprintUnitFactReference QuickClearStandardFact = new BlueprintUnitFactReference();
            QuickClearStandardFact.deserializedGuid = BlueprintTool.Get<BlueprintAbility>(QuickClearStandardGUID).AssetGuid;
            BlueprintUnitFactReference QuickClearMoveFact = new BlueprintUnitFactReference();
            QuickClearMoveFact.deserializedGuid = BlueprintTool.Get<BlueprintAbility>(QuickClearMoveGUID).AssetGuid;

            AddFacts QuickClearFacts = new AddFacts();
            QuickClearFacts.m_Facts = new BlueprintUnitFactReference[] { QuickClearStandardFact, QuickClearMoveFact };


            FeatureConfigurator.New("QuickClear", QuickClearFeatureGUID)
                .SetDisplayName(LocalizationTool.GetString("Deeds.QuickClear.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.QuickClear.Description"))
                .AddComponent(QuickClearFacts)
                .Configure()               
                ;
        }

        public static void ThirdLevelDeeds() 
        {
            //Gunslinger Initiative: gain +2 initative as long as you have 1 grit point
            GunslingersInitiative();

            //Pistol Whip:This one is a bit odd standard action make an attack with the firearms enhancement bonus to attack and damage
            //damge die based on weapon size so d6 for one handed d10 for two handed
            //crit multiplier 20/x2
            //if it hits make a free combat manuever check to trip
            //costs 1 grit


            //Utility Shot
            //none of this is implementable with the games mechanics
        }

        public static void GunslingersInitiative()
        {
            FeatureConfigurator.New("SlingerInitFeature", SlingerInitFeatureGUID)
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, false, Kingmaker.EntitySystem.Stats.StatType.Initiative, 2)
                .SetDisplayName(LocalizationTool.GetString("Deeds.SlingerInitiative.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.SlingerInitiative.Description"))
                .Configure();

            GritUnlock slingerUnlock = new GritUnlock();
            slingerUnlock.m_NewFact = BlueprintTool.GetRef<BlueprintUnitFactReference>(SlingerInitFeatureGUID);


            FeatureConfigurator.New("SlingerInitUnlock", SlingerInitUnlockGUID)
                .AddComponent(slingerUnlock)//It has the above conditional system to add everything else
                .SetDisplayName(LocalizationTool.GetString("Deeds.SlingerInitiative.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.SlingerInitiative.Description"))
                .SetRanks(1)//again not sure but copied from monk
                .SetIsClassFeature(true)//this is a class feature
                .Configure();
        }

        public static void SeventhLevelDeeds() 
        {
            //Startling Shot
            //if you have at least 1 grit can make an auto miss to give a target flat-footed
            //simplify to spend 1 round of ammo as standard action to give target flat-footed

            //Targeting
            //This is the coolest deed
            //limit target to head, arms, legs, and Torso as flight works differently in game
            //costs 1 grit
            //full round action
            //make one firearm strike
            //probably works as ability with varients 
            TargetedShot();

        }

        public static void TargetedShot()
        {

            #region Base
            //creates an Icon
            byte[] data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/Targeting.png");
            Texture2D texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite DefaultIcon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));

            AbilityConfigurator TargetingBase = AbilityConfigurator.New("TargetingBase", TargetingBaseGUID)//create the base ability
                .SetDisplayName(LocalizationTool.GetString("Deeds.Targeting.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.Targeting.Description"))
                .AddAbilityResourceLogic(amount: 1, isSpendResource: true, requiredResource: Grit.GritResource)//costs 1 grit
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(true)//full round action
                .AddComponent(new HasLoadedGun())//can only use the ability if they have a loaded gun
                .SetCanTargetEnemies(true)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Weapon)//uses weapon range
                .SetShouldTurnToTarget(true)
                .SetUseCurrentWeaponAsReasonItem(true)
                .SetIcon(DefaultIcon)
                ;//note need to add a limit that the ability does not work on creatures immune to sneak attack.
            #endregion

            #region Head
            //headshot works as normal but also gives target confused for 1 round on a hit (really nice ability)
            //creates an Icon
            data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/TargetingHead.png");
            texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite HeadShotIcon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));

            AbilityEffectRunAction Headshot = new AbilityEffectRunAction();//create the headshot action
            ContextActionApplyBuff AddConfused = new ContextActionApplyBuff();//create the action which adds the condition
            AddConfused.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>("886c7407dc629dc499b9f1465ff382df");//the confusion buff
            AddConfused.Permanent = false;//not permanent
            AddConfused.DurationSeconds = 6;//lasts 6 seconds or 1 round
            AddConfused.UseDurationSeconds = true;//use the seconds
            AddConfused.IsFromSpell = false;//it's not a spell
            Headshot.Actions = new ActionList();//make an action list 
            Headshot.Actions.Actions = new GameAction[] { AddConfused };//put the action to add confused in there

            
            SpellDescriptorComponent MindEffectingDescriptor = new SpellDescriptorComponent();
            MindEffectingDescriptor.Descriptor = SpellDescriptor.MindAffecting;

            AbilityConfigurator.New("TargetingHead", TargetingHeadGUID)
                .SetDisplayName(LocalizationTool.GetString("Deeds.Targeting.Head.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.Targeting.Head.Description"))
                .SetParent(BlueprintTool.GetRef<BlueprintAbilityReference>(TargetingBaseGUID))
                .AddComponent(Headshot)
                .AddComponent(MindEffectingDescriptor)
                .AddAbilityDeliveredByWeapon()//this one includes an attack
                .SetIcon(HeadShotIcon)
                .Configure();

            #endregion

            #region Arms
            //Arms does no damage but gives the disarmed condition same as the disarm manuever will need to think about duration

            data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/TargetingArms.png");
            texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite ArmShotIcon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));

            AbilityEffectRunAction ArmShot = new AbilityEffectRunAction();

            ContextActionCombatManeuver disarmingShot = new ContextActionCombatManeuver();
            disarmingShot.Type = Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm;
            disarmingShot.NewStat = Kingmaker.EntitySystem.Stats.StatType.Dexterity;//disarm manuever but at range and with dex rather than strength
            disarmingShot.ReplaceStat = true;
            ArmShot.Actions = new ActionList();
            ArmShot.Actions.Actions = new GameAction[1] { disarmingShot };

            AbilityConfigurator.New("TargetingArms", TargetingArmsGUID)
                .SetDisplayName(LocalizationTool.GetString("Deeds.Targeting.Arms.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.Targeting.Arms.Description"))
                .SetParent(BlueprintTool.GetRef<BlueprintAbilityReference>(TargetingBaseGUID))
                .AddComponent(ArmShot)
                .SetIcon(ArmShotIcon)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.CoupDeGrace)//not at all confident this will work
                .Configure();

            #endregion

            #region Legs
            //Legs does damage and target knocked prone

            data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/TargetingLegs.png");
            texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite LegShotIcon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));

            AbilityEffectRunAction LegShot = new AbilityEffectRunAction();//create the Legshot action
            ContextActionApplyBuff AddProne = new ContextActionApplyBuff();//create the action which adds the condition
            AddProne.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>("24cf3deb078d3df4d92ba24b176bda97");//the prone buff
            LegShot.Actions = new ActionList();
            LegShot.Actions.Actions = new GameAction[1] { AddProne };
            AbilityConfigurator.New("TargetingLegs", TargetingLegsGUID)
                .SetDisplayName(LocalizationTool.GetString("Deeds.Targeting.Legs.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.Targeting.Legs.Description"))
               .SetParent(BlueprintTool.GetRef<BlueprintAbilityReference>(TargetingBaseGUID))
               .AddComponent(LegShot)
               .AddAbilityDeliveredByWeapon()//this one includes an attack
               .SetIcon(LegShotIcon)
               .Configure();
            #endregion

            #region Feature
            List<Blueprint<BlueprintAbilityReference>> TargetingVarients = new() { BlueprintTool.GetRef<BlueprintAbilityReference>(TargetingHeadGUID), BlueprintTool.GetRef<BlueprintAbilityReference>(TargetingLegsGUID), BlueprintTool.GetRef<BlueprintAbilityReference>(TargetingArmsGUID) };
            TargetingBase.AddAbilityVariants(TargetingVarients);
            TargetingBase.Configure();

            BlueprintUnitFactReference TargetingBaseFact = new BlueprintUnitFactReference();
            TargetingBaseFact.deserializedGuid = BlueprintTool.Get<BlueprintAbility>(TargetingBaseGUID).AssetGuid;
            BlueprintUnitFactReference TargetingHeadFact = new BlueprintUnitFactReference();
            TargetingHeadFact.deserializedGuid = BlueprintTool.Get<BlueprintAbility>(TargetingHeadGUID).AssetGuid;
            BlueprintUnitFactReference TargetingArmsFact = new BlueprintUnitFactReference();
            TargetingArmsFact.deserializedGuid = BlueprintTool.Get<BlueprintAbility>(TargetingArmsGUID).AssetGuid;
            BlueprintUnitFactReference TargetingLegsFact = new BlueprintUnitFactReference();
            TargetingLegsFact.deserializedGuid = BlueprintTool.Get<BlueprintAbility>(TargetingLegsGUID).AssetGuid;

            AddFacts TargetingFacts = new AddFacts();
            TargetingFacts.m_Facts = new BlueprintUnitFactReference[] { TargetingBaseFact, TargetingHeadFact, TargetingArmsFact, TargetingLegsFact };


            FeatureConfigurator.New("TargetingFeature", TargetingFeatureGUID)
                .SetDisplayName(LocalizationTool.GetString("Deeds.Targeting.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.Targeting.Description"))
                .AddComponent(TargetingFacts)
                .SetIcon(DefaultIcon)
                .Configure()
                ;
            #endregion
        }

        public static void EleventhLevelDeeds()
        {
            //Bleeding Wound
            //four activatable abilities:
            //first costs 1 grit and gives the target bleed equal to get on hit (I think I'll let this not consume if it has already been applied since bleed doesn't stack)
            //second costs 2 grit ad gives strength bleed
            //thrid costs 2 gives dex bleed
            //foruth costs 2 gives con bleed
            //the activatable abilities will trigger on a hit to spend the grit and inflict bleed then deactive themselves
            //hopefully by being varients they will be mutually exclusion
            BleedingShot();


            //Expert loading
            //one activatable ability
            //gives a buff that means when a gun would explod it charges user one grit instead until grit is 0 then explodes

            //Lightning reload
            //grants a swift action reload ability that can be used once per round (if you have at least 1 grit)
            //also grants activable that gives a buff which causes the first time you attempt to fire with no ammo you lose one grit fire anyway and lose the buff)

        }

        public static void BleedingShot()
        {
            #region StandardBleedBuff
            BlueprintBuff BasicD4Bleed = BlueprintTool.Get<BlueprintBuff>("5eb68bfe186d71a438d4f85579ce40c1");//gets the basic d4 bleed from the game
            AddFactContextActions BleedContextAction = (AddFactContextActions)BasicD4Bleed.Components[0];//find the bit that calls the action to inflict damage
            ContextActionDealDamage BleedDamage = (ContextActionDealDamage)BleedContextAction.NewRound.Actions[0];//take out the part about doing damage
            BleedDamage.Value.DiceType = Kingmaker.RuleSystem.DiceType.One;//change it to be a multiple of 1 not d4s
            BleedDamage.Value.DiceCountValue.ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.CasterProperty;//then with a value based on the caster property rather than a constant
            BleedDamage.Value.DiceCountValue.Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatDexterity;//and that value is the casters dex
            BleedContextAction.NewRound.Actions[0] = BleedDamage;

            BuffConfigurator.New("BasicBleedBuff", BasicBleedBuffGUID)
                .SetDisplayName(LocalizationTool.GetString("Bleed.Standard.Name"))
                .SetDescription(LocalizationTool.GetString("Bleed.Standard.Description"))
                .SetIcon(BasicD4Bleed.Icon)//copy the icon from standard bleed
                .AddComponent(BleedContextAction)//use the newly adjusted bleed context action
                .AddComponent(BasicD4Bleed.Components[1])//and the other components can be copyied from the basic bleed
                .AddComponent(BasicD4Bleed.Components[2])
                .AddComponent(BasicD4Bleed.Components[3])
                .Configure();

            ContextActionApplyBuff BleedAction = new ContextActionApplyBuff();
            BleedAction.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(BasicBleedBuffGUID);
            #endregion

            #region Conditions
            ContextSpendResource SpendGrit = new ContextSpendResource();
            SpendGrit.m_Resource = Grit.GritResource;
            SpendGrit.Value = new Kingmaker.UnitLogic.Mechanics.ContextValue();
            SpendGrit.Value.ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple;
            SpendGrit.Value.Value = 1;

            Conditional BleedTriggerCondition = new Conditional();

            HasBuff AlreadyBleeding = new HasBuff();
            AlreadyBleeding.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(BasicBleedBuffGUID);//checks if they have the bleed condition
            BleedTriggerCondition.ConditionsChecker = new ConditionsChecker();
            BleedTriggerCondition.ConditionsChecker.Conditions = new Condition[] { AlreadyBleeding };
            BleedTriggerCondition.IfFalse = new ActionList();
            BleedTriggerCondition.IfFalse.Actions = new GameAction[] { BleedAction, SpendGrit };

            Conditional GritTrigger = new Conditional();//makes sure the user has enough grit 
            GritTrigger.ConditionsChecker = new ConditionsChecker();
            GritTrigger.ConditionsChecker.Conditions = new Condition[] { new HasGrit(1) };
            GritTrigger.IfTrue = new ActionList();
            GritTrigger.IfTrue.Actions = new GameAction[] { BleedTriggerCondition };

            AddInitiatorAttackWithWeaponTrigger BleedTrigger = new AddInitiatorAttackWithWeaponTrigger();//this is the bit that is triggered by making an attack
            BleedTrigger.OnlyHit = true;//only works on a hit
            BleedTrigger.CheckWeaponCategory = true;//only work if the weapon is of type
            BleedTrigger.Category = BaseFirearm.FirearmCategory;//firearm
            BleedTrigger.Action = new ActionList();
            BleedTrigger.Action.Actions = new GameAction[] { GritTrigger };//then performs the bleedcheck and etc.
            #endregion

            #region StandardShot
            BuffConfigurator.New("BleedingShotBuff", BleedingShotBuffGUID)
                .AddComponent(BleedTrigger)
                .SetDisplayName(LocalizationTool.GetString("Deeds.BleedingShot.Standard.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.BleedingShot.Standard.Description"))
                .SetIcon(BasicD4Bleed.Icon)//copy the icon from standard bleed
                .Configure();

            ActivatableAbilityConfigurator.New("BleedingShot", BleedingShotGUID)
                .SetBuff(BlueprintTool.GetRef<BlueprintBuffReference>(BleedingShotBuffGUID))
                .SetDisplayName(LocalizationTool.GetString("Deeds.BleedingShot.Standard.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.BleedingShot.Standard.Description"))
                .SetIcon(BasicD4Bleed.Icon)//copy the icon from standard bleed
                .Configure();
            #endregion

            #region AttributeBleedSetup
            SpendGrit.Value.Value = 2;//for all the remaining bleeds the cost is 2 grit not 1
            BleedTriggerCondition.IfFalse.Actions[1] = SpendGrit;//update the bleed trigger acordingly
            GritTrigger.ConditionsChecker.Conditions[0] = (new HasGrit(2));

            //adjust bleed damage to be strength bleed
            BleedDamage.AbilityType = Kingmaker.EntitySystem.Stats.StatType.Strength;
            BleedDamage.m_Type = ContextActionDealDamage.Type.AbilityDamage;
            BleedDamage.Value.DiceCountValue.ValueType = ContextValueType.Simple;//the attribute bleed just does a simple
            BleedDamage.Value.DiceCountValue.Value = 1;//1 damage
            BleedContextAction.NewRound.Actions[0] = BleedDamage;//update the context action
            #endregion

            #region StrengthBleed
            BuffConfigurator.New("STRBleedBuff", STRBleedBuffGUID)
                .SetDisplayName(LocalizationTool.GetString("Bleed.Strength.Name"))
                .SetDescription(LocalizationTool.GetString("Bleed.Strength.Description"))
                .SetIcon(BasicD4Bleed.Icon)//copy the icon from standard bleed
                .AddComponent(BleedContextAction)//use the newly adjusted bleed context action
                .AddComponent(BasicD4Bleed.Components[1])//and the other components can be copyied from the basic bleed
                .AddComponent(BasicD4Bleed.Components[2])
                .AddComponent(BasicD4Bleed.Components[3])
                .Configure();

            BleedAction.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(STRBleedBuffGUID);//make the action to apply the buff

            BleedTriggerCondition.IfFalse.Actions[0] = BleedAction;//change the main action for the bleed trigger to apply strength bleed instead

            GritTrigger.IfTrue.Actions[0] = BleedTriggerCondition;

            BleedTrigger.Action.Actions[0] = GritTrigger;//update tghe bleed trigger acordingly

            BuffConfigurator.New("BleedingShotSTRBuff", BleedingShotSTRBuffGUID)
                .AddComponent(BleedTrigger)
                .SetDisplayName(LocalizationTool.GetString("Deeds.BleedingShot.Strength.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.BleedingShot.Strength.Description"))
                .SetIcon(BasicD4Bleed.Icon)//copy the icon from standard bleed
                .Configure();

            ActivatableAbilityConfigurator.New("BleedingShotSTR", BleedingShotSTRGUID)
                .SetBuff(BlueprintTool.GetRef<BlueprintBuffReference>(BleedingShotSTRBuffGUID))
                .SetDisplayName(LocalizationTool.GetString("Deeds.BleedingShot.Strength.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.BleedingShot.Strength.Description"))
                .SetIcon(BasicD4Bleed.Icon)//copy the icon from standard bleed
                .Configure();
            #endregion

            #region ConstitutionBleed
            //adjust bleed damage to be CON bleed
            BleedDamage.AbilityType = Kingmaker.EntitySystem.Stats.StatType.Constitution;
            BleedDamage.m_Type = ContextActionDealDamage.Type.AbilityDamage;
            BleedContextAction.NewRound.Actions[0] = BleedDamage;//update the context action

            BuffConfigurator.New("CONBleedBuff", CONBleedBuffGUID)
                .SetDisplayName(LocalizationTool.GetString("Bleed.Constitution.Name"))
                .SetDescription(LocalizationTool.GetString("Bleed.Constitution.Description"))
                .SetIcon(BasicD4Bleed.Icon)//copy the icon from standard bleed
                .AddComponent(BleedContextAction)//use the newly adjusted bleed context action
                .AddComponent(BasicD4Bleed.Components[1])//and the other components can be copyied from the basic bleed
                .AddComponent(BasicD4Bleed.Components[2])
                .AddComponent(BasicD4Bleed.Components[3])
                .Configure();

            BleedAction.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(CONBleedBuffGUID);//make the action to apply the buff

            BleedTriggerCondition.IfFalse.Actions[0] = BleedAction;//change the main action for the bleed trigger to apply strength bleed instead

            GritTrigger.IfTrue.Actions[0] = BleedTriggerCondition;

            BleedTrigger.Action.Actions[0] = GritTrigger;//update tghe bleed trigger acordingly

            BuffConfigurator.New("BleedingShotCONBuff", BleedingShotCONBuffGUID)
                .AddComponent(BleedTrigger)
                .SetDisplayName(LocalizationTool.GetString("Deeds.BleedingShot.Constitution.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.BleedingShot.Constitution.Description"))
                .SetIcon(BasicD4Bleed.Icon)//copy the icon from standard bleed
                .Configure();

            ActivatableAbilityConfigurator.New("BleedingShotCON", BleedingShotCONGUID)
                .SetBuff(BlueprintTool.GetRef<BlueprintBuffReference>(BleedingShotCONBuffGUID))
                .SetDisplayName(LocalizationTool.GetString("Deeds.BleedingShot.Constitution.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.BleedingShot.Constitution.Description"))
                .SetIcon(BasicD4Bleed.Icon)//copy the icon from standard bleed
                .Configure();
            #endregion

            #region DexterityBleed
            //adjust bleed damage to be DEX bleed
            BleedDamage.AbilityType = Kingmaker.EntitySystem.Stats.StatType.Dexterity;
            BleedDamage.m_Type = ContextActionDealDamage.Type.AbilityDamage;
            BleedContextAction.NewRound.Actions[0] = BleedDamage;//update the context action

            BuffConfigurator.New("DEXBleedBuff", DEXBleedBuffGUID)
                .SetDisplayName(LocalizationTool.GetString("Bleed.Dexterity.Name"))
                .SetDescription(LocalizationTool.GetString("Bleed.Dexterity.Description"))
                .SetIcon(BasicD4Bleed.Icon)//copy the icon from standard bleed
                .AddComponent(BleedContextAction)//use the newly adjusted bleed context action
                .AddComponent(BasicD4Bleed.Components[1])//and the other components can be copyied from the basic bleed
                .AddComponent(BasicD4Bleed.Components[2])
                .AddComponent(BasicD4Bleed.Components[3])
                .Configure();

            BleedAction.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(DEXBleedBuffGUID);//make the action to apply the buff

            BleedTriggerCondition.IfFalse.Actions[0] = BleedAction;//change the main action for the bleed trigger to apply strength bleed instead

            GritTrigger.IfTrue.Actions[0] = BleedTriggerCondition;

            BleedTrigger.Action.Actions[0] = GritTrigger;//update tghe bleed trigger acordingly

            

            BuffConfigurator.New("BleedingShotDEXBuff", BleedingShotDEXBuffGUID)
                .AddComponent(BleedTrigger)
                .SetDisplayName(LocalizationTool.GetString("Deeds.BleedingShot.Dexterity.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.BleedingShot.Dexterity.Description"))
                .SetIcon(BasicD4Bleed.Icon)//copy the icon from standard bleed
                .Configure();

            ActivatableAbilityConfigurator.New("BleedingShotDEX", BleedingShotDEXGUID)
                .SetBuff(BlueprintTool.GetRef<BlueprintBuffReference>(BleedingShotDEXBuffGUID))
                .SetDisplayName(LocalizationTool.GetString("Deeds.BleedingShot.Dexterity.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.BleedingShot.Dexterity.Description"))
                .SetIcon(BasicD4Bleed.Icon)//copy the icon from standard bleed
                .Configure();
            #endregion

            #region Feature
            BlueprintUnitFactReference StandardRef = new BlueprintUnitFactReference();
            StandardRef.deserializedGuid = BlueprintTool.Get<BlueprintActivatableAbility>(BleedingShotGUID).AssetGuid;
            BlueprintUnitFactReference StrengthRef = new BlueprintUnitFactReference();
            StrengthRef.deserializedGuid = BlueprintTool.Get<BlueprintActivatableAbility>(BleedingShotSTRGUID).AssetGuid;
            BlueprintUnitFactReference ConRef = new BlueprintUnitFactReference();
            ConRef.deserializedGuid = BlueprintTool.Get<BlueprintActivatableAbility>(BleedingShotCONGUID).AssetGuid;
            BlueprintUnitFactReference DexRef = new BlueprintUnitFactReference();
            DexRef.deserializedGuid = BlueprintTool.Get<BlueprintActivatableAbility>(BleedingShotDEXGUID).AssetGuid;

            AddFacts BleedFacts = new AddFacts();
            BleedFacts.m_Facts = new BlueprintUnitFactReference[] { StandardRef, StrengthRef, ConRef, DexRef };

            FeatureConfigurator.New("BleedingShotFeature", BleedingShotFeatureGUID)
                .SetDisplayName(LocalizationTool.GetString("Deeds.BleedingShot.Standard.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.BleedingShot.Description"))
                .AddComponent(BleedFacts)
                .SetIcon(BasicD4Bleed.Icon)
                .Configure();
            #endregion
        }

        public static void FifteenthLevelDeeds()
        {
            //Evasive
            //if you have 1 grit grants the evasion, improved evasion, uncanny dodge, and improved uncanny dodge
            GunslingersEvasion();

            //Menacing Shot
            //spend 1 grit and 1 ammo
            //I assume standard action
            //then effects all creature in range with something like the fear spell


            //Slinger's Luck
            //honestly not sure how to do this one since you'd have to cast it in advance which is a bit rough so might ignore for now
        }

        public static void GunslingersEvasion()
        {
            Sprite icon = BlueprintTool.Get<BlueprintFeature>("576933720c440aa4d8d42b0c54b77e80").Icon;//copy te icon from evasion

            FeatureConfigurator.New("EvasiveDeedBuff", EvasiveDeedBuffGUID)//create a feature that grants evasion and improved evasion
                .AddComponent(new Evasion())
                .Configure();


            GritUnlock EvasiveUnlock = new GritUnlock();//grants said feature if has grit
            EvasiveUnlock.m_NewFact = BlueprintTool.GetRef<BlueprintUnitFactReference>(EvasiveDeedBuffGUID);

            GritUnlock DodgeUnlock = new GritUnlock();//grants uncanny dodge
            DodgeUnlock.m_NewFact = BlueprintTool.GetRef<BlueprintUnitFactReference>("3c08d842e802c3e4eb19d15496145709");//the uncanny dodge reference

            GritUnlock ImprovedDodgeUnlock = new GritUnlock();//grants improved uncanny dodge
            ImprovedDodgeUnlock.m_NewFact = BlueprintTool.GetRef<BlueprintUnitFactReference>("485a18c05792521459c7d06c63128c79");//the improved uncanny dodge reference



            FeatureConfigurator.New("EvasiveDeed", EvasiveDeedGUID)
                .SetDisplayName(LocalizationTool.GetString("Deeds.Evasive.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.Evasive.Description"))
                .AddComponent(EvasiveUnlock)
                .AddComponent(DodgeUnlock)
                .AddComponent(ImprovedDodgeUnlock)
                .SetIcon(icon)
                .Configure();
        }

        public static void NineteenthLevelDeeds()
        {
            //Cheat Death
            //This seems bad in this game as it just means the lose all grit and then the enemy keep attacking you with you on 0 so is more likely to result in instant death rather than unconsious
            //I could make it trigger on death instead and keep you on 0 rather than dying at the cost of all grit perhaps

            //Death Shot
            //Actiatable ability that reduced critical multiplier to x1 and on a crit spends 1 grit gives the target a save or kills them
            //the ability can only be active when the user has at least 1 grit so it doesn't keep negating crits after

            //Stunning Shot
            //activatable ability as above but needs you to have 2 grit in the bank
            //when a target is hit spends 2 grit target gets a save or become stunned for 1 round
            //if the target is stunned when hit it won't spend the grit or give them the save
            StunningShot();

        }

        public static void StunningShot()
        {
            UnitPropertyConfigurator StunningShotDC = UnitPropertyConfigurator.New("StunningShotDC", StunningShotDCGUID);
            SummClassLevelGetter GunslingerLevel = new SummClassLevelGetter();
            Kingmaker.UnitLogic.Mechanics.Properties.StatValueGetter Wis = new StatValueGetter();
            Wis.Stat = Kingmaker.EntitySystem.Stats.StatType.Wisdom;
            GunslingerLevel.Settings = new PropertySettings();
            GunslingerLevel.Settings.m_Progression = PropertySettings.Progression.Div2;
            GunslingerLevel.m_Class.AddItem(BlueprintTool.GetRef<BlueprintCharacterClassReference>(Gunslinger.GunslingerClassGUID));
            StunningShotDC.AddComponent(GunslingerLevel);
            StunningShotDC.AddComponent(Wis);
            StunningShotDC.SetBaseValue(10);
            StunningShotDC.Configure();

            ContextActionApplyBuff StunAction = new ContextActionApplyBuff();
            StunAction.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>("09d39b38bb7c6014394b6daced9bacd3");//stunned condition
            StunAction.IsFromSpell = false;
            StunAction.DurationSeconds = 6;
            StunAction.UseDurationSeconds = true;


            ContextActionSavingThrow StunSave = new ContextActionSavingThrow();
            StunSave.Type = Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude;
            ContextValue SaveDC = new ContextValue();
            SaveDC.m_CustomProperty = BlueprintTool.GetRef<BlueprintUnitPropertyReference>(StunningShotDCGUID);
            SaveDC.ValueType = ContextValueType.CasterCustomProperty;
            StunSave.CustomDC = SaveDC;
            StunSave.HasCustomDC = true;
            ContextActionConditionalSaved StunSaveCondition = new ContextActionConditionalSaved();
            StunSaveCondition.Failed = new ActionList();
            StunSaveCondition.Failed.Actions = new GameAction[] { StunAction };
            StunSave.Actions = new ActionList();
            StunSave.Actions.Actions = new GameAction[] {StunSaveCondition};





            ContextSpendResource SpendGrit = new ContextSpendResource();
            SpendGrit.m_Resource = Grit.GritResource;
            SpendGrit.Value = new Kingmaker.UnitLogic.Mechanics.ContextValue();
            SpendGrit.Value.ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Simple;
            SpendGrit.Value.Value = 1;

            Conditional StunConditionTrigger = new Conditional();

            HasBuff AlreadyBleeding = new HasBuff();
            AlreadyBleeding.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(BasicBleedBuffGUID);//checks if they have the bleed condition
            StunConditionTrigger.ConditionsChecker = new ConditionsChecker();
            StunConditionTrigger.ConditionsChecker.Conditions = new Condition[] { AlreadyBleeding };
            StunConditionTrigger.IfFalse = new ActionList();
            StunConditionTrigger.IfFalse.Actions = new GameAction[] { StunSave };//if they are not bleeding make them bleed then spend the grit
            StunConditionTrigger.IfFalse.Actions.AddItem(SpendGrit);//not certain if this is going to try and consume grit from the target rather than the user

            Conditional GritTrigger = new Conditional();//makes sure the user has enough grit 
            GritTrigger.ConditionsChecker = new ConditionsChecker();
            GritTrigger.ConditionsChecker.Conditions = new Condition[] { new HasGrit(2) };
            GritTrigger.IfTrue = new ActionList();
            GritTrigger.IfTrue.Actions = new GameAction[] { StunConditionTrigger };
            //I suspect one or both of the two conditions might struggle in terms of whether they are checking the right target


            AddInitiatorAttackWithWeaponTrigger StunTrigger = new AddInitiatorAttackWithWeaponTrigger();//this is the bit that is triggered by making an attack
            StunTrigger.OnlyHit = true;//only works on a hit
            StunTrigger.CheckWeaponCategory = true;//only work if the weapon is of type
            StunTrigger.Category = BaseFirearm.FirearmCategory;//firearm/heavy crossbow
            StunTrigger.Action = new ActionList();
            StunTrigger.Action.Actions = new GameAction[] { GritTrigger };//then performs the gritcheck and etc.


            Sprite icon = BlueprintTool.Get<BlueprintBuff>("09d39b38bb7c6014394b6daced9bacd3").Icon;

            BuffConfigurator.New("StunningShotBuff", StunningShotBuffGUID)
                .AddComponent(StunTrigger)
                .SetDisplayName(LocalizationTool.GetString("Deeds.StunningShot.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.StunningShot.Description"))
                .SetIcon(icon)
                .Configure();

            ActivatableAbilityConfigurator.New("StunningShot", StunningShotGUID)
                .SetBuff(BlueprintTool.GetRef<BlueprintBuffReference>(StunningShotBuffGUID))
                .AddActivatableAbilityResourceLogic(requiredResource:Grit.GritResource, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetDisplayName(LocalizationTool.GetString("Deeds.StunningShot.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.StunningShot.Description"))
                .SetIcon(icon)
                .Configure();


            BlueprintUnitFactReference StunRef = new BlueprintUnitFactReference();
            StunRef.deserializedGuid = BlueprintTool.Get<BlueprintActivatableAbility>(StunningShotGUID).AssetGuid;
            AddFacts StunFacts = new AddFacts();
            StunFacts.m_Facts = new BlueprintUnitFactReference[] { StunRef };
            FeatureConfigurator.New("StunningShotFeature", StunningShotFeatureGUID)
                .AddComponent(StunFacts)
                .SetDisplayName(LocalizationTool.GetString("Deeds.StunningShot.Name"))
                .SetDescription(LocalizationTool.GetString("Deeds.StunningShot.Description"))
                .SetIcon(icon)
                .Configure();
        }
    }
}
