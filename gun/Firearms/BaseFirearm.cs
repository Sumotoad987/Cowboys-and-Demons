using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.JsonSystem.EditorDatabase;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.EquipmentEnchants;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.Configurators.Items;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Loot;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Blueprints.Classes.Selection;


namespace gun.Firearms
{
    internal static class BaseFirearm
    {
        //define all the GUIDs at the top of the file
        public const WeaponCategory FirearmCategory = WeaponCategory.HandCrossbow;//hand crossbow apears to be unimplemented so I will use that for now
        public const string ProjectileRef = "0f083f2598b3e6441992ebadbc0325aa";
        const string ArmorPiercingGUID = "027bf51a88d94dbc86bf8848d2f2cff0";
        const string CapacityGUID = "e712d0661d0f4507af2f18addf53cab3";
        public const string RoundsGUID = "72a83c73e7ce42e0adb54339c6098f21";
        const string ReloadOneHandGUID = "84a5658968e04f6f91a9b32a1b4462f5";
        const string ReloadOneHandFeatureGUID = "5d503d48e12b4fb38aea53fca2de8768";
        const string ReloadTwoHandGUID = "45a3e148603f4960bfdd820cc95c8cd3";
        const string ReloadTwoHandFeatureGUID = "5dba0835a9cb451d85551c2073dcda05";
        const string RapidReloadOneHandGUID = "4010b6d277654b00a263a1c53df1b224";
        const string RapidReloadTwoHandGUID = "c9caffb7a65a49a5888039881bc6070e";
        const string ReloadAdvancedGUID = "1b6361094bca408593cfa0ead4b4ea82";
        const string ReloadAdvancedFeatureGUID = "c7e33be72b0b48658465341ce4590695";
        const string ClipOneHandGUID = "50a4c21bb11e43c7ba23891a7cdf66c5";
        const string ClipTwoHandGUID = "f49df5707fa04eb6ae435723bab1cbb4";
        public const string AdvancedClipGUID = "78f6979ff32e425291709489c652a90b";
        public static string[] Vendors = { "5f17d3b47752fb94abe8c98534af8920", "7aaf7d11ce8541b69b3ce0064dd45d2a", "9c597a1f92dde2f4f8adb27ee5730188", "", "73895d43f46b45079e19d1afcb96efdd" };
        public static void Configure()
        {
            FirearmProficiency.Configure();//create the firearm proficiency feat


            //may need to alter the order depending on how each bit interacts with the others
            //define Armour Piercing enhancement
            ArmorPiercingEnhancement();

            //create the rounds resource (visible)
            BuffConfigurator.New("Rounds", RoundsGUID)
                .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Replace)//stacks are how many rounds are available
                .Configure();
            ;

            //define reload action and rapid reload feat
            RapidReload.Configure();
            SetupReload();

            //define clip including the weapon feature that grants the reload action and requires ammunition
            SetupClip();

            //define misfire
            MisfireEnhancement.Configure();
            //define scatter
        }
        //creates the capacity "enhancement" on a weapon alongside the associated resource and ability
        private static void SetupClip()
        {
            //create the capacity condition (hidden in UI perhaps)
            BuffConfigurator.New("Capacity", CapacityGUID)
                .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Stack)//it stacks and the number of stacks is the max number of rounds
                .Configure();
                ;

            FeatureConfigurator.New("OneHandReloadingFeature", ReloadOneHandFeatureGUID)
                .AddFacts(new List<Blueprint<BlueprintUnitFactReference>> {BlueprintTool.GetRef<BlueprintUnitFactReference>(ReloadOneHandGUID), BlueprintTool.GetRef<BlueprintUnitFactReference>(RapidReloadOneHandGUID) })
                .Configure();

            AddUnitFeatureEquipment AddOneHandReload = new AddUnitFeatureEquipment();
            AddOneHandReload.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(ReloadOneHandFeatureGUID);


            AddUnitFeatureEquipment AddOneHandRapidReload = new AddUnitFeatureEquipment();
            AddOneHandRapidReload.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(RapidReloadOneHandGUID);

            WeaponEnchantmentConfigurator.New("LoadingOneHand", ClipOneHandGUID)
                .SetDescription(LocalizationTool.GetString("Firearms.Early.Reload.Description"))
                .SetEnchantName(LocalizationTool.GetString("Firearms.Early.Reload.Name"))
                .AddComponent(new EmptyClipCheck())
                .AddComponent(AddOneHandReload)
                .AddComponent(AddOneHandRapidReload)
                .Configure();


            FeatureConfigurator.New("TwoHandReloadingFeature", ReloadTwoHandFeatureGUID)
               .AddFacts(new List<Blueprint<BlueprintUnitFactReference>> { BlueprintTool.GetRef<BlueprintUnitFactReference>(ReloadTwoHandGUID), BlueprintTool.GetRef<BlueprintUnitFactReference>(RapidReloadTwoHandGUID) })
               .Configure();

            AddUnitFeatureEquipment AddTwoHandReload = new AddUnitFeatureEquipment();
            AddTwoHandReload.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(ReloadTwoHandFeatureGUID);


            AddUnitFeatureEquipment AddTwoHandRapidReload = new AddUnitFeatureEquipment();
            AddTwoHandRapidReload.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(RapidReloadTwoHandGUID);


            WeaponEnchantmentConfigurator.New("LoadingTwoHand", ClipTwoHandGUID)
                .SetDescription(LocalizationTool.GetString("Firearms.Early.Reload.Description"))
                .SetEnchantName(LocalizationTool.GetString("Firearms.Early.Reload.Name"))
                .AddComponent(new EmptyClipCheck())
                .AddComponent(AddTwoHandReload)
                .AddComponent(AddTwoHandRapidReload)
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
                .AddComponent(AddAdvancedReload)
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
            reloadSingleEffect.Actions.Actions = new Kingmaker.ElementsSystem.GameAction[1] { reloadSingle };
            //reloadSingleEffect.Actions.Actions.AddItem(reloadSingle);


            AbilityConfigurator.New("Reload One-Handed", ReloadOneHandGUID)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.CombatManeuver)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetDescription(LocalizationTool.GetString("Firearms.Early.Reload.Description"))
                .SetDisplayName(LocalizationTool.GetString("Firearms.Reload.Name"))
                .SetIcon(icon)
                .AddComponent(reloadSingleEffect)
                .Configure();
            AbilityConfigurator.New("Reload Two-Handed", ReloadTwoHandGUID)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.CombatManeuver)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(true)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetDescription(LocalizationTool.GetString("Firearms.Early.Reload.Description"))
                .SetDisplayName(LocalizationTool.GetString("Firearms.Reload.Name"))
                .SetIcon(icon)
                .AddComponent(reloadSingleEffect)
                .Configure();
            AbilityConfigurator.New("Rapid Reload One-Handed", RapidReloadOneHandGUID)
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

        //adds the scatter enhancement and action
        private static void SetupScatter ()
        {
            //create the scatter shot action
            //configure the action to make an attack roll against all enemies in a cone
            //add the scatter enhancement which grants the scatter shot action
        }

        //sets up the visual parameters of the weapon
        public static WeaponVisualParameters DefineVisualParameters(string ModelID = "f4ef679dee9518b40806cea527b62958")//default to crossbow
        {
            PrefabLink model = new PrefabLink();
            model.AssetId = ModelID;
            WeaponVisualParameters visuals = new WeaponVisualParameters();
            visuals.m_WeaponAnimationStyle = Kingmaker.View.Animation.WeaponAnimationStyle.Crossbow;
            visuals.m_Projectiles = new BlueprintProjectileReference[1] { BlueprintTool.GetRef<BlueprintProjectileReference>(ProjectileRef) };
            visuals.m_SpecialAnimation = Kingmaker.Visual.Animation.Kingmaker.UnitAnimationSpecialAttackType.None;
            visuals.m_WeaponModel = model;//not sure this is right but will run with it for now
            visuals.m_OverrideAttachSlots = false;
            visuals.m_ReachFXThresholdBonus = 0;
            visuals.m_SoundSize = Kingmaker.Visual.Sound.WeaponSoundSizeType.Medium;//not sure what this is for
            visuals.m_SoundType = Kingmaker.Visual.Sound.WeaponSoundType.PierceMetal;//not sure what this does yet either will need to give some through
            //visuals.m_WhooshSound = CrossbowShot; //Not sure what this is or how to set it
            visuals.m_MissSoundType = Kingmaker.Visual.Sound.WeaponMissSoundType.MediumMetal;//not sure if this is the right one but will go with it for now
            //visuals.m_EquipSound = Weapon_Bow_Equip; don't know how to set these
            //visuals.m_UnequipSound = Weapon_Bow_Remove;
            //visuals.m_InventoryUnequipSound = BowEquip;
            //visuals.m_InventoryPutSound = CrossbowPut;
            //visuals.m_InventoryTakeSound = CrossbowTake;
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
        public static void CreateWeapon(string name, string ID, bool OneHanded, Kingmaker.Utility.Feet range, DiceFormula damage, DamageCriticalModifierType CritMod, int CritRange, DamageTypeDescription DamageType, Sprite icon, float weight, WeaponVisualParameters visuals, string missfireType, bool isAdvanced = false ,bool isLight = false, bool isMonk = false)
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
            //current issues: no progiciency of its own might be able to set some other kind of restriction of it like the way some require one to be a barbarian to wear make them require the firearm prof feat or something
            
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
            weapon.Configure();
        }

        public static void AddWeaponFocus(string name, string WeaponID, string[] FocusIDs)
        {
            //Weapon Focus
           Sprite Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e").Icon;//weapon focus icon
           FeatureConfigurator.New("WeaponFocus" + name, FocusIDs[0])
                .AddWeaponFocus(1,ModifierDescriptor.UntypedStackable,WeaponID)
                .SetDisplayName(LocalizationTool.GetString("Feats.WeaponFocus." + name + ".Name"))
                .SetDescription(LocalizationTool.GetString("Feats.WeaponFocus." + name + ".Description"))
                .SetIcon(Icon)
                .AddToIsPrerequisiteFor(BlueprintTool.GetRef<BlueprintFeatureReference>(FocusIDs[1]))
                .Configure()
                ; 
            //Greater Weapon Focus
            Icon = BlueprintTool.Get<BlueprintParametrizedFeature>("09c9e82965fb4334b984a1e9df3bd088").Icon;//greater weapon focus icon
            FeatureConfigurator.New("WeaponFocusGreater" + name, FocusIDs[1])
                .AddWeaponFocus(1, ModifierDescriptor.UntypedStackable, WeaponID)
                .SetDisplayName(LocalizationTool.GetString("Feats.WeaponFocus.Greater." + name + ".Name"))
                .SetDescription(LocalizationTool.GetString("Feats.WeaponFocus.Greater." + name + ".Description"))
                .SetIcon(Icon)
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
                .AddPrerequisiteFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(FocusIDs[2]))
                .AddPrerequisiteClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>("48ac8db94d5de7645906c7d0ad3bcfbd"), 12, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
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
            BasicWeapon.ModifyVisualParameters((WeaponVisualParameters vis) => vis.m_Projectiles = new BlueprintProjectileReference[1] { BlueprintTool.GetRef<BlueprintProjectileReference>(ProjectileRef) });
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
