using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.Configurators.Items;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Assets;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.JsonSystem.EditorDatabase;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.EquipmentEnchants;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.ResourceManagement;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking.Types;
using static Kingmaker.Armies.TacticalCombat.Grid.TacticalMapObstacle;
using static Kingmaker.EntitySystem.Properties.BaseGetter.ListPropertyGetter;


namespace gun.Firearms
{
    internal static class BaseFirearm
    {
        //define all the GUIDs at the top of the file
        public const WeaponCategory FirearmCategory = WeaponCategory.HandCrossbow;//hand crossbow apears to be unimplemented so I will use that for now
        public const string ProjectileRef = "0f083f2598b3e6441992ebadbc0325aa";
        const string ArmorPiercingGUID = "027bf51a88d94dbc86bf8848d2f2cff0";
        public const string RoundsGUID = "72a83c73e7ce42e0adb54339c6098f21";
        public const string ReloadGUID = "6f90360117aa4198b1948a5ff979457c";
        public const string ReloadStandardGUID = "84a5658968e04f6f91a9b32a1b4462f5";
        const string ReloadOneHandFeatureGUID = "5d503d48e12b4fb38aea53fca2de8768";
        public const string ReloadFullRoundGUID = "45a3e148603f4960bfdd820cc95c8cd3";
        const string ReloadTwoHandFeatureGUID = "5dba0835a9cb451d85551c2073dcda05";
        public const string ReloadMoveGUID = "4010b6d277654b00a263a1c53df1b224";
        const string RapidReloadTwoHandGUID = "c9caffb7a65a49a5888039881bc6070e";
        public const string ReloadAdvancedGUID = "1b6361094bca408593cfa0ead4b4ea82";
        const string ReloadAdvancedFeatureGUID = "c7e33be72b0b48658465341ce4590695";
        public const string ClipGUID = "5cfc737d3bf84ed3be57c765d1d9957b";
        const string ClipOneHandGUID = "50a4c21bb11e43c7ba23891a7cdf66c5";
        const string ClipTwoHandGUID = "f49df5707fa04eb6ae435723bab1cbb4";
        public const string AdvancedClipGUID = "78f6979ff32e425291709489c652a90b";
        public static string[] Vendors = { "5f17d3b47752fb94abe8c98534af8920", "7aaf7d11ce8541b69b3ce0064dd45d2a", "9c597a1f92dde2f4f8adb27ee5730188", "", "73895d43f46b45079e19d1afcb96efdd", "195579adaa20483ca3aad66bb2b06f8f" };//one on the end is for Roguelike DLC
        public const string FinneanItem = "95c126deb99ba054aa5b84710520c035";
        public const string BulletGUID = "608d2e99ebf14967a023672b0764aa5c";
        public static string[] Capacity = {"", "36423d2718b14ee6b8bfd6e5b556a834","","","","", "6101cd89ec854ce0ad87cc82cd8b838b" };

        public static void Configure()
        {
            FirearmProficiency.Configure();//create the firearm proficiency feat
            Scatter.Configure();

            MakeProjectile();//create the bullet
            //WeaponCategoryExtension.Data[59].SubCategories.Remove((WeaponSubCategory cat) => cat == WeaponSubCategory.Disabled);
            //may need to alter the order depending on how each bit interacts with the others
            //define Armour Piercing enhancement
            ArmorPiercingEnhancement();

            //create the rounds resource (visible)
            BuffConfigurator.New("Rounds", RoundsGUID)
                .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Rank)//stacks are how many rounds are available
                .SetRanks(10)
                .Configure();
            ;

            //rename weapon training for crossbows to include firearms
            FeatureConfigurator.For("9cdfc2a236ee6d349ad6d8a2170477d5")
                .SetDisplayName(LocalizationTool.GetString("Feats.WeaponTraining.CrossbowsAndFirearms.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.WeaponTraining.CrossbowsAndFirearms.Description"))
                .Configure();


            //define reload action and rapid reload feat
            RapidReload.Configure();
            SetupReload();

            //define clip including the weapon feature that grants the reload action and requires ammunition
            SetupClip();

            //define misfire
            MisfireEnhancement.Configure();
            //define scatter
            
        }

        public static void MakeProjectile()
        {
            ProjectileLink Bulletlink = new ProjectileLink();
            Bulletlink.AssetId = "1f2be13f05afd4a45853dc027dd7c82c";
            Bulletlink.m_Handle = BundledResourceHandle<ProjectileView>.Request("1f2be13f05afd4a45853dc027dd7c82c");
            ProjectileView view = Bulletlink.Load();
            Main.Log.Log(view.name);
            ProjectileConfigurator Bullet = ProjectileConfigurator.New("Bullet", BulletGUID).CopyFrom(BlueprintTool.GetRef<BlueprintProjectileReference>(ProjectileRef))
               .SetSpeed(46);
            Bullet.SetView(Bulletlink);//This is not yet working may try again in the future but for now we'll stick with the crossbow bolt

            Bullet.Configure();
        }

        //creates the capacity "enhancement" on a weapon alongside the associated resource and ability
        private static void SetupClip()
        {

            WeaponEnchantmentConfigurator.New("Capacity1", Capacity[1])
                .SetHiddenInUI()
                .AddComponent(new Capacity(1))
                .Configure();

            WeaponEnchantmentConfigurator.New("Capacity6", Capacity[6])
                .SetDescription(LocalizationTool.GetString("Firearms.Capacity.6.Description"))
                .SetEnchantName(LocalizationTool.GetString("Firearms.Capacity.6.Name"))
                .AddComponent(new Capacity(6))
                .Configure();

            FeatureConfigurator.New("OneHandReloadingFeature", ReloadOneHandFeatureGUID)
                .AddFacts(new List<Blueprint<BlueprintUnitFactReference>> {BlueprintTool.GetRef<BlueprintUnitFactReference>(ReloadStandardGUID), BlueprintTool.GetRef<BlueprintUnitFactReference>(ReloadMoveGUID) })
                .Configure();

            AddUnitFeatureEquipment AddOneHandReload = new AddUnitFeatureEquipment();
            AddOneHandReload.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(ReloadOneHandFeatureGUID);

            AddUnitFeatureEquipment AddReload = new AddUnitFeatureEquipment();
            AddReload.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(ReloadGUID);

            /*AddUnitFeatureEquipment AddOneHandRapidReload = new AddUnitFeatureEquipment();
            AddOneHandRapidReload.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(RapidReloadOneHandGUID);*/

            WeaponEnchantmentConfigurator.New("LoadingOneHand", ClipOneHandGUID)
                .SetDescription(LocalizationTool.GetString("Firearms.Early.Reload.Description"))
                .SetEnchantName(LocalizationTool.GetString("Firearms.Early.Reload.Name"))
                .AddComponent(new EmptyClipCheck())
                .AddComponent(new CheckLoadingEquipment())
                //.AddComponent(AddOneHandReload)
                .Configure();

            WeaponEnchantmentConfigurator.New("Loading", ClipGUID)
                .SetDescription(LocalizationTool.GetString("Firearms.Early.Reload.Description"))
                .SetEnchantName(LocalizationTool.GetString("Firearms.Early.Reload.Name"))
                .AddComponent(new EmptyClipCheck())
                .AddComponent(new CheckLoadingEquipment())
                //.AddComponent(AddReload)
                .Configure();


            FeatureConfigurator.New("TwoHandReloadingFeature", ReloadTwoHandFeatureGUID)
               .AddFacts(new List<Blueprint<BlueprintUnitFactReference>> { BlueprintTool.GetRef<BlueprintUnitFactReference>(ReloadFullRoundGUID), BlueprintTool.GetRef<BlueprintUnitFactReference>(RapidReloadTwoHandGUID) })
               .Configure();

            AddUnitFeatureEquipment AddTwoHandReload = new AddUnitFeatureEquipment();
            AddTwoHandReload.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(ReloadTwoHandFeatureGUID);


            /*AddUnitFeatureEquipment AddTwoHandRapidReload = new AddUnitFeatureEquipment();
            AddTwoHandRapidReload.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(RapidReloadTwoHandGUID);*/
            


            WeaponEnchantmentConfigurator.New("LoadingTwoHand", ClipTwoHandGUID)
                .SetDescription(LocalizationTool.GetString("Firearms.Early.Reload.Description"))
                .SetEnchantName(LocalizationTool.GetString("Firearms.Early.Reload.Name"))
                .AddComponent(new EmptyClipCheck())
                .AddComponent(new CheckLoadingEquipment())
                //.AddComponent(AddTwoHandReload)
                .Configure();


            FeatureConfigurator.New("AdvancedReloadingFeature", ReloadAdvancedFeatureGUID)
               .AddFacts(new List<Blueprint<BlueprintUnitFactReference>> { BlueprintTool.GetRef<BlueprintUnitFactReference>(ReloadAdvancedGUID)})
               .Configure();

            AddUnitFeatureEquipment AddAdvancedReload = new AddUnitFeatureEquipment();
            AddAdvancedReload.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(ReloadAdvancedFeatureGUID);

            WeaponEnchantmentConfigurator.New("LoadingAdvanced", AdvancedClipGUID)
                .SetDescription(LocalizationTool.GetString("Firearms.Advanced.Reload.Description"))
                .SetEnchantName(LocalizationTool.GetString("Firearms.Advanced.Reload.Name"))
                .AddComponent(new AdvancedClipCheck())
                .AddComponent(new CheckLoadingEquipment())
                //.AddComponent(AddAdvancedReload)
                .Configure();
            //configure capacity condition reduce rounds resource by one after each attack and give empty clip when rounds hits 0
            //might be simpler to add a second version of the condition on advanced weapons which only adjusts the rounds resoruce if the user doesn't have rapid reload
        }

        //creates the reload action to be linked to firearms
        private static void SetupReload()
        {
            byte[] data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/Reload.png");
            Texture2D texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite icon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));
            ContextActionApplyBuff reloadSingle = new ContextActionApplyBuff();//will need to tweak this later to account for capacity for now will simply forbid stacking on reload and have no weapons with capacity
            reloadSingle.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(RoundsGUID);
            reloadSingle.Permanent = true;
            reloadSingle.IsFromSpell = false;
            reloadSingle.IsNotDispelable = true;



            AbilityEffectRunAction reloadSingleEffect = new AbilityEffectRunAction();
            reloadSingleEffect.SavingThrowType = Kingmaker.EntitySystem.Stats.SavingThrowType.Unknown;
            reloadSingleEffect.IgnoreCaster = false;
            reloadSingleEffect.Actions = new Kingmaker.ElementsSystem.ActionList();
            reloadSingleEffect.Actions.Actions = new Kingmaker.ElementsSystem.GameAction[1] { new ReloadEffect()};
            //reloadSingleEffect.Actions.Actions.AddItem(reloadSingle);

            AbilityConfigurator.New("Reload", ReloadGUID)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.CombatManeuver)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetDescription(LocalizationTool.GetString("Firearms.Early.Reload.Description"))
                .SetDisplayName(LocalizationTool.GetString("Firearms.Reload.Name"))
                .SetIcon(icon)
                .AddComponent(reloadSingleEffect)
                .Configure();


            AbilityConfigurator.New("Reload One-Handed", ReloadStandardGUID)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.CombatManeuver)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetDescription(LocalizationTool.GetString("Firearms.Early.Reload.Description"))
                .SetDisplayName(LocalizationTool.GetString("Firearms.Reload.Name"))
                .SetIcon(icon)
                .AddComponent(reloadSingleEffect)
                .Configure();
            AbilityConfigurator.New("Reload Two-Handed", ReloadFullRoundGUID)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.CombatManeuver)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(true)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetDescription(LocalizationTool.GetString("Firearms.Early.Reload.Description"))
                .SetDisplayName(LocalizationTool.GetString("Firearms.Reload.Name"))
                .SetIcon(icon)
                .AddComponent(reloadSingleEffect)
                .Configure();
            AbilityConfigurator.New("Rapid Reload One-Handed", ReloadMoveGUID)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.CombatManeuver)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetDescription(LocalizationTool.GetString("Firearms.Early.Reload.Description"))
                .SetDisplayName(LocalizationTool.GetString("Firearms.RapidReload.Name"))
                .SetIcon(icon)
                .AddComponent(reloadSingleEffect)
                .AddComponent(new HasRapidReload())//should prevent using this ability if your don't have rapid reload
                .Configure();
            AbilityConfigurator.New("Rapid Reload Two-Handed", RapidReloadTwoHandGUID)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.CombatManeuver)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetDescription(LocalizationTool.GetString("Firearms.Early.Reload.Description"))
                .SetDisplayName(LocalizationTool.GetString("Firearms.RapidReload.Name"))
                .SetIcon(icon)
                .AddComponent(reloadSingleEffect)
                .AddComponent(new HasRapidReload ())//should prevent using this ability if your don't have rapid reload
                .Configure();

            AbilityConfigurator.New("Reload Advanced", ReloadAdvancedGUID)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.CombatManeuver)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetDescription(LocalizationTool.GetString("Firearms.Advanced.Reload.Description"))
                .SetDisplayName(LocalizationTool.GetString("Firearms.Reload.Name"))
                .SetIcon(icon)
                .AddComponent(reloadSingleEffect)//this will need to be replaced with Reload all but for now shall simply not implement firearms with capacity
                .Configure();
            //action will grant one rounds resource if below current max
            //create variants for how long it takes or some system to adjust that time (if I later add the few advanced weapons with capacity of more than 1 it will need to reset it to full)
            //one handed firearms are standard action, two handed are full round, and advanced are move action reduced to free with rapid reload
        }



        //sets up the visual parameters of the weapon
        public static WeaponVisualParameters DefineVisualParameters(string ModelID = "f4ef679dee9518b40806cea527b62958")//default to crossbow
        {
            PrefabLink model = new PrefabLink();
            model.AssetId = ModelID;
            WeaponVisualParameters visuals = Utilities.Clone(BlueprintTool.Get<BlueprintWeaponType>("36d0551b8a28587438a47fcbbf53c083").m_VisualParameters);//use the heavy crossbow visuals as a base
            visuals.m_WeaponAnimationStyle = Kingmaker.View.Animation.WeaponAnimationStyle.Crossbow;
            visuals.m_SpecialAnimation = Kingmaker.Visual.Animation.Kingmaker.UnitAnimationSpecialAttackType.None;
            visuals.m_WeaponModel = model;//not sure this is right but will run with it for now
            visuals.m_OverrideAttachSlots = false;
            visuals.m_ReachFXThresholdBonus = 0;
            visuals.m_SoundSize = Kingmaker.Visual.Sound.WeaponSoundSizeType.Medium;//not sure what this is for
            visuals.m_SoundType = Kingmaker.Visual.Sound.WeaponSoundType.PierceMetal;//not sure what this does yet either will need to give some through
            visuals.m_MissSoundType = Kingmaker.Visual.Sound.WeaponMissSoundType.MediumMetal;//not sure if this is the right one but will go with it for now


            //Hopefully this will add a new projectile that travels twice as fast as the crossbow bolt and thus remove the fireing again before it hits bug
           
            visuals.m_Projectiles = new BlueprintProjectileReference[1] { BlueprintTool.GetRef<BlueprintProjectileReference>(BulletGUID) };
            if (visuals.Projectiles.Length != 0)
            {
                Main.Log.Log("has projectiles");
            }
            return visuals;

        }
        public static void ArmorPiercingEnhancement()
        {
            //create the weapon enhancement
            WeaponEnchantmentConfigurator.New("Armor Piercing", ArmorPiercingGUID)
                .SetDescription(LocalizationTool.GetString("Firearms.AP.Description"))
                .SetEnchantName(LocalizationTool.GetString("Firearms.AP.Name"))
                .AddComponent(new ArmorPiercing ())
                .Configure();
        }


        //this is called by the subtypes of weapon to simplify the definition of the default version of each
        public static void CreateWeapon(string name, string ID, bool OneHanded, Kingmaker.Utility.Feet range, DiceFormula damage, DamageCriticalModifierType CritMod, int CritRange, DamageTypeDescription DamageType, Sprite icon, float weight, WeaponVisualParameters visuals, string missfireType, bool isAdvanced = false ,int Cap = 1, int ScatterType = 0, bool isLight = false, bool isMonk = false)
        {
            WeaponTypeConfigurator weapon = WeaponTypeConfigurator.New(name, ID)
                .SetIsTwoHanded(!OneHanded)
                .SetIsOneHanded(OneHanded)
                .SetAttackRange(range)
                .SetAttackType(Kingmaker.RuleSystem.AttackType.Ranged)
                .SetBaseDamage(damage)
                .SetCategory(FirearmCategory)
                .SetCriticalModifier(CritMod)
                .SetCriticalRollEdge(CritRange)
                .SetDamageType(DamageType)
                .SetDefaultNameText(LocalizationTool.GetString("Firearms." + name + ".Name"))
                .SetDescriptionText(LocalizationTool.GetString("Firearms." + name + ".Description"))
                .SetIcon(icon)
                .SetIsLight(isLight)
                .SetIsMonk(isMonk)
                .SetWeight(weight)
                .SetVisualParameters(visuals)
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>(ArmorPiercingGUID))
                .AddComponent(new EquipmentRestrictionFirearm())
                .SetFighterGroupFlags(WeaponFighterGroupFlags.Crossbows) //sets it up for fighter weapon group stuff
                ;
            
            weapon.AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>(Capacity[Cap]));
            if (isAdvanced)
            {
                weapon.AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>(AdvancedClipGUID));
            }
            else
            {
                if (OneHanded)
                {
                    weapon.AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>(ClipOneHandGUID));
                }
                else
                {
                    weapon.AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>(ClipTwoHandGUID));
                }
            }
            weapon.AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>(missfireType));

            switch (ScatterType)
            {
                case (30):
                    weapon.AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>(Scatter.Enchant30ft));
                    break;
                case (15):
                    weapon.AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>(Scatter.Enchant15ft));
                    break;
                default:
                    break;
            }
                
                
            weapon.Configure();
        }

        public static void AddWeaponFeats(string name, string WeaponID, string[] FocusIDs)
        {
            //Weapon Focus
           Sprite Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e").Icon;//weapon focus icon
           FeatureConfigurator.New("WeaponFocus" + name, FocusIDs[0])
                .AddWeaponFocus(1,ModifierDescriptor.UntypedStackable,WeaponID)
                .SetDisplayName(LocalizationTool.GetString("Feats.WeaponFocus." + name + ".Name"))
                .SetDescription(LocalizationTool.GetString("Feats.WeaponFocus." + name + ".Description"))
                .SetIcon(Icon)
                .AddToIsPrerequisiteFor(BlueprintTool.GetRef<BlueprintFeatureReference>(FocusIDs[1]))
                .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                .Configure()
                ; 
            
            //Greater Weapon Focus
            Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("09c9e82965fb4334b984a1e9df3bd088").Icon;//greater weapon focus icon
            FeatureConfigurator.New("WeaponFocusGreater" + name, FocusIDs[1])
                .AddWeaponFocus(1, ModifierDescriptor.UntypedStackable, WeaponID)
                .SetDisplayName(LocalizationTool.GetString("Feats.WeaponFocus.Greater." + name + ".Name"))
                .SetDescription(LocalizationTool.GetString("Feats.WeaponFocus.Greater." + name + ".Description"))
                .SetIcon(Icon)
                .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                .AddPrerequisiteFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(FocusIDs[0]))
                .AddPrerequisiteClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd"),8,group:Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .Configure()
                ;
            //Weapon Specialization
            Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("31470b17e8446ae4ea0dacd6c5817d86").Icon;//Weapon Spec icon
            FeatureConfigurator.New("WeaponSpecialization" + name, FocusIDs[2])
                .AddWeaponTypeDamageBonus(2, WeaponID)
                .SetDisplayName(LocalizationTool.GetString("Feats.WeaponSpecialization." + name + ".Name"))
                .SetDescription(LocalizationTool.GetString("Feats.WeaponSpecialization." + name + ".Description"))
                .SetIcon(Icon)
                .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                .AddToIsPrerequisiteFor(BlueprintTool.GetRef<BlueprintFeatureReference>(FocusIDs[3]))
                .AddPrerequisiteClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd"), 4, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .Configure()
                ;
            //Weapon Specialization Greater
            Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("31470b17e8446ae4ea0dacd6c5817d86").Icon;//Weapon Spec icon
            FeatureConfigurator.New("WeaponSpecializationGreater" + name, FocusIDs[3])
                .AddWeaponTypeDamageBonus(2, WeaponID)
                .SetDisplayName(LocalizationTool.GetString("Feats.WeaponSpecialization.Greater." + name + ".Name"))
                .SetDescription(LocalizationTool.GetString("Feats.WeaponSpecialization.Greater." + name + ".Description"))
                .SetIcon(Icon)
                .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                .AddPrerequisiteFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(FocusIDs[2]))
                .AddPrerequisiteClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd"), 12, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .Configure()
                ;

            Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("f4201c85a991369408740c6888362e20").Icon;//improved critical icon
            FeatureConfigurator.New("ImprovedCritical" + name, FocusIDs[4])
                .AddWeaponTypeCriticalEdgeIncrease(WeaponID)
                .SetDisplayName(LocalizationTool.GetString("Feats.ImprovedCritical." + name + ".Name"))
                .SetDescription(LocalizationTool.GetString("Feats.ImprovedCritical." + name + ".Description"))
                .SetIcon(Icon)
                .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.BaseAttackBonus, 8)
                .Configure()
                ;

            Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("74eb201774bccb9428ba5ac8440bf990").Icon;//weapon focus mythic icon
            FeatureConfigurator.New("WeaponFocusMythic" + name, FocusIDs[5])
                 .AddComponent(new WeaponFocusMythic(BlueprintTool.GetRef<BlueprintWeaponTypeReference>(WeaponID), BlueprintTool.GetRef < BlueprintFeatureReference> (FocusIDs[1])))
                 .SetDisplayName(LocalizationTool.GetString("Feats.WeaponFocus.Mythic." + name + ".Name"))
                 .SetDescription(LocalizationTool.GetString("Feats.WeaponFocus.Mythic." + name + ".Description"))
                 .SetIcon(Icon)
                 .AddPrerequisiteFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(FocusIDs[0]))//requires normal wepon focus
                 .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                 .Configure()
                 ;

            Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("d84ac5b1931bc504a98bfefaa419e34f").Icon;//weapon spec mythic icon
            FeatureConfigurator.New("WeaponSpecializationMythic" + name, FocusIDs[6])
                 .AddComponent(new WeaponTypeDamageBonusMythic(BlueprintTool.GetRef<BlueprintWeaponTypeReference>(WeaponID)))
                 .SetDisplayName(LocalizationTool.GetString("Feats.WeaponSpecialization.Mythic." + name + ".Name"))
                 .SetDescription(LocalizationTool.GetString("Feats.WeaponSpecialization.Mythic." + name + ".Description"))
                 .SetIcon(Icon)
                 .AddPrerequisiteFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(FocusIDs[2]))//requires normal wepon spec
                 .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                 .Configure()
                 ;
            Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("8bc0190a4ec04bd489eec290aeaa6d07").Icon;//improved crit mythic icon
            FeatureConfigurator.New("ImprovedCriticalMythic" + name, FocusIDs[7])
                 .AddComponent(new ImprovedCriticalMythic(BlueprintTool.GetRef<BlueprintWeaponTypeReference>(WeaponID)))
                 .SetDisplayName(LocalizationTool.GetString("Feats.ImprovedCritical.Mythic." + name + ".Name"))
                 .SetDescription(LocalizationTool.GetString("Feats.ImprovedCritical.Mythic." + name + ".Description"))
                 .SetIcon(Icon)
                 .AddPrerequisiteFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(FocusIDs[4]))//requires normal improved critical 
                 .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                 .Configure()
                 ;
            Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("56f94badbba018b4b8277ce6e2e79e72").Icon;//improved improved crit icon
            FeatureConfigurator.New("ImprovedImprovedCritical" + name, FocusIDs[8])
                 .AddComponent(new ImprovedImprovedCritical(BlueprintTool.GetRef<BlueprintWeaponTypeReference>(WeaponID)))
                 .AddPrerequisitePlayerHasFeature(BlueprintTool.GetRef<BlueprintFeatureReference>("e9298851786c5334dba1398e9635a83d"))
                 .SetDisplayName(LocalizationTool.GetString("Feats.ImprovedImprovedCritical." + name + ".Name"))
                 .SetDescription(LocalizationTool.GetString("Feats.ImprovedImprovedCritical." + name + ".Description"))
                 .SetIcon(Icon)
                 .AddPrerequisiteFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(FocusIDs[4]))//requires normal improved critical 
                 .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.TricksterFeat)
                 .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                 .Configure()
                 ;
            
            Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("006a966007802a0478c9e21007207aac").Icon;//improved improved improved crit icon
            FeatureConfigurator.New("ImprovedImprovedImprovedCritical" + name, FocusIDs[9])
                 .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.BaseAttackBonus,8)
                 .AddPrerequisiteFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(FocusIDs[8]))//requires improved improved crit
                 .AddComponent(new ImprovedImprovedCritical(BlueprintTool.GetRef<BlueprintWeaponTypeReference>(WeaponID)))
                 .AddPrerequisitePlayerHasFeature(BlueprintTool.GetRef<BlueprintFeatureReference>("e9298851786c5334dba1398e9635a83d"))
                 .SetDisplayName(LocalizationTool.GetString("Feats.ImprovedImprovedImprovedCritical." + name + ".Name"))
                 .SetDescription(LocalizationTool.GetString("Feats.ImprovedImprovedImprovedCritical." + name + ".Description"))
                 .SetIcon(Icon)
                 .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.TricksterFeat)
                 .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                 .Configure()
                 ;
            Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("319c882ab3cc51544ad2f3f43633d5b1").Icon;//improved improved improved crit icon
            FeatureConfigurator.New("ImprovedImprovedImprovedCriticalImproved" + name, FocusIDs[10])
                 .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.BaseAttackBonus, 8)
                 .AddPrerequisiteFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(FocusIDs[9]))//requires improved improved crit
                 .AddComponent(new ImprovedCriticalMythic(BlueprintTool.GetRef<BlueprintWeaponTypeReference>(WeaponID)))
                 .AddPrerequisitePlayerHasFeature(BlueprintTool.GetRef<BlueprintFeatureReference>("e9298851786c5334dba1398e9635a83d"))
                 .SetDisplayName(LocalizationTool.GetString("Feats.ImprovedImprovedImprovedCriticalImproved." + name + ".Name"))
                 .SetDescription(LocalizationTool.GetString("Feats.ImprovedImprovedImprovedCriticalImproved." + name + ".Description"))
                 .SetIcon(Icon)
                 .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.TricksterFeat)
                 .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                 .Configure()
                 ;
        }

        //makes the standard +1,+2,+3,+4,+5 weapons
        public static void CreateBasicWeapons(string name,string[] IDs, string TypeID, int BaseCost = 0)
        {
            CreateWeaponItem("Standard" + name, IDs[0], TypeID, BaseCost).Configure();
            CreateWeaponItem(name + "Plus1", IDs[1], TypeID, BaseCost + 2000)
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("d42fc23b92c640846ac137dc26e000d4"))
                .Configure();
            CreateWeaponItem(name + "Plus2", IDs[2], TypeID, BaseCost + 8300)
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("eb2faccc4c9487d43b3575d7e77ff3f5"))
                .Configure();
            CreateWeaponItem(name + "Plus3", IDs[3], TypeID, BaseCost + 18500)
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("80bb8a737579e35498177e1e3c75899b"))
                .Configure();
            CreateWeaponItem(name + "Plus4", IDs[4], TypeID, BaseCost + 32000)
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("783d7d496da6ac44f9511011fc5f1979"))
                .Configure();
            CreateWeaponItem(name + "Plus5", IDs[5], TypeID, BaseCost + 50000)
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("bdba267e951851449af552aa9f9e3992"))
                .Configure();
        }

        //creates a standard version of the weapon as an item
        public static ItemWeaponConfigurator CreateWeaponItem (string name, string ID, string TypeID, int cost = 0)
        {
            ItemWeaponConfigurator BasicWeapon = ItemWeaponConfigurator.New(name, ID);
            BasicWeapon.SetCost(cost);
            BasicWeapon.SetType(BlueprintTool.GetRef<BlueprintWeaponTypeReference>(TypeID));
            WeaponVisualParameters visuals = Utilities.Clone(BlueprintTool.Get<BlueprintItemWeapon>("19a5092244dcf99478dcd73c974828b1").m_VisualParameters);//copy the visual parameters off the standard heavy crossbow
            BasicWeapon.SetVisualParameters(visuals);
            //BasicWeapon.ModifyVisualParameters((WeaponVisualParameters vis) => vis.m_Projectiles = new BlueprintProjectileReference[] {});
            //BasicWeapon.Configure();

            return BasicWeapon;
            //this function returns the configurator so the configure function can be called outside in case anyting special needs to be added
        }
        //this is used to add a weapon blueprint to the merchant
        public static void AddWeapontoShop(string[] IDs, int chapter)//currintly we don't have a vendor for chapter 4 so just don't try and assign any to that chapter
        {
            foreach (string ID in IDs) 
            {
                LootItemsPackFixed inventoryweapon = new LootItemsPackFixed ();
                inventoryweapon.m_Item = new LootItem();
                inventoryweapon.m_Item.m_Item = BlueprintTool.GetRef<BlueprintItemReference>(ID);
                inventoryweapon.m_Item.m_Type = LootItemType.Item;
                inventoryweapon.m_Count = 1;
                SharedVendorTableConfigurator.For(BlueprintTool.Get<BlueprintSharedVendorTable>(Vendors[chapter - 1]))
                    .AddComponent(inventoryweapon)
                    .Configure();
            }
        }
        public static void AddWeapontoFinnean (string name, string[] IDs, string TypeID, bool HasLevel0 = false)
        {
            BlueprintHiddenItem Finnean = BlueprintTool.Get<BlueprintHiddenItem>("95c126deb99ba054aa5b84710520c035");
            int i = 0;
            if (HasLevel0) {
                CreateWeaponItem("Finnean" + name + "Stage1", IDs[i], TypeID)
                    .SetDisplayNameText(LocalizationTool.GetString("Finnean"))
                    //+1 cold iron ghost touch
                    .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("d42fc23b92c640846ac137dc26e000d4"), BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("e5990dc76d2a613409916071c898eee8"), BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("47857e1a5a3ec1a46adf6491b1423b4f"))
                    .Configure();
                AddToPolymorphList(Finnean, "e50ce85131b64acbb14dc1cade2434d0", IDs[i],1);///Add to the stage 1 finnean list
                i++;
            }
            
            CreateWeaponItem("Finnean" + name + "Stage2", IDs[i], TypeID)
                .SetDisplayNameText(LocalizationTool.GetString("Finnean"))
                //+3 Ghost Touch Heartseeker (Finean Chapter 3 Enhancement)
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("80bb8a737579e35498177e1e3c75899b"), BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("47857e1a5a3ec1a46adf6491b1423b4f"), BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("e252b26686ab66241afdf33f2adaead6"), BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("b183bd491793d194c9e4c96cd11769b1"))
                .Configure();
            AddToPolymorphList(Finnean, "10f9d67c98bf4796831ea5aa99e580b3", IDs[i],2);///Add to the stage 2 finnean list
            i++;

            CreateWeaponItem("Finnean" + name + "Stage3Base", IDs[i], TypeID)
                .SetDisplayNameText(LocalizationTool.GetString("Finnean"))
                 //+5 Ghost Touch Heartseeker (Finean Chapter 5 Enhancement)  (Keeping the ghost touch rather than brilliant energy since that would actually be a debuff on a firearm))
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("bdba267e951851449af552aa9f9e3992"), BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("47857e1a5a3ec1a46adf6491b1423b4f"), BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("e252b26686ab66241afdf33f2adaead6"), BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("6b66e949f348ccd4989a5fd9254f8958"))
                .Configure();
            AddToPolymorphList(Finnean, "dce45f9c5e23496284a6b32a5b3f8a7f", IDs[i],3);///Add to the stage 3 finnean list
            i++;

            CreateWeaponItem("Finnean" + name + "Stage3Lich", IDs[i], TypeID)
                .SetDisplayNameText(LocalizationTool.GetString("Finnean.Lich"))
                .SetFlavorText(LocalizationTool.GetString("Finnean.Lich.Flavor"))
                //+5 Ghost Touch (Finean Chapter 5 Lich Enhancement)  (Keeping the ghost touch rather than brilliant energy since that would actually be a debuff on a firearm))
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("bdba267e951851449af552aa9f9e3992"), BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("47857e1a5a3ec1a46adf6491b1423b4f"), BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("9aa9af4b654662945a410644d3db8d99"))
                .Configure();
            AddToPolymorphList(Finnean, "58dd746a84c54571998753d065684723", IDs[i],4);///Add to the stage 3 lich finnean list
        }

        static void AddToPolymorphList(BlueprintHiddenItem Finnean, string FlagToCheck, string ID, int level)
        {
            HiddenItemConfigurator FineanConfig = HiddenItemConfigurator.For(Finnean.ToReference<BlueprintReference<BlueprintHiddenItem>>());
            FineanConfig.EditComponents<ItemPolymorph>((ItemPolymorph p) => {
                BlueprintItemReference weapon = BlueprintTool.GetRef<BlueprintItemReference>(ID);
                p.m_PolymorphItems.Add(weapon);
            }, (ItemPolymorph p) => {
                return p.m_FlagToCheck == ((ItemPolymorph)Finnean.Components[level]).m_FlagToCheck; 
            });
            FineanConfig.Configure();
            
            
        }
        //most firearms do unaligned physical blugeoining and piercing damage
        public static DamageTypeDescription DefaultFirearmDamageType()
        {
            DamageTypeDescription DamageDescription = new DamageTypeDescription();
            DamageDescription.Type = DamageType.Physical;
            DamageDescription.Common.Reality = 0;
            DamageDescription.Common.Alignment = 0;
            DamageDescription.Common.Precision = false;
            DamageDescription.Physical.Material = 0;
            DamageDescription.Physical.Form = PhysicalDamageForm.Piercing;
            return DamageDescription;
        }
    }
}
