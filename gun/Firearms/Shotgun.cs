using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static gun.Firearms.BaseFirearm;
using BlueprintCore.Utils;
using BlueprintCore.Blueprints;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.BasicEx;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;

namespace gun.Firearms
{
    internal static class Shotgun
    {
        public const string WeaponID = "5cca627fdec04c0485e7f893833fa0fe";
        public static string[] BasicItemIDs = {
            "17cf62cf8252451db6e4a9fcc5217998",
            "098a412bfb164f419f10715e7c1bc611",
            "8dc3b221fe4f4351b0666dbeb33d70d9",
            "2bd7ae668e1f405595abc714191fad81",
            "a4d45b5aecb84155a43d8004aa3d0900",
            "73bf9fb2d25f4e2fba46c1c7e6476772",
        };
        public static string[] FocusIDs = {
            "14dbf1d0135e4ae0b72cdff7b52929e5",
            "32ecc0e93a0f4fee82b7be1cbb5f3411",
            "81acdc225fce444fac322071df5bbf59",
            "fdc9d2db80a042e0b3e9b82f1486c932",
            "487390c03580494da10467a68337829b",
            "430b880312114f0f82732d1da6a356c3",
            "6e6a6a4085ee4c05bbceb1c969d3bbe1",
            "cc0d12c728b84cdb82f77947c6415e09",
            "9aa4c0f4fd0f47a6ad7abf3b88a0d38f",
            "19c6b2881e0345eb9dce18202385af3a",
            "d3d911e68e5f4753b705a21695a12b98"
        };
        public static string[] FinneanIDs =
        {
            "55e8da8666624d75b19d02a21d0b3bd9",
            "5019e2b6c0c14c4b925b5c8849b82626",
            "5829fbbcdf0c481e8174b08766cf091e"
        };
        public static void Configure()
        {
            //WeaponVisualParameters Uses crossbow animation style
            WeaponVisualParameters visuals = DefineVisualParameters("44a27185a1f8d7e45b12166585953e04");
            //defines the damage dice stuff
            DiceFormula Dice = new DiceFormula();
            Dice.m_Rolls = 1;
            Dice.m_Dice = DiceType.D10;

            //creates an Icon
            byte[] data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/Rifle.png");
            Texture2D texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite icon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));

            //creates the rifle weapon type by calling from base firearm
            CreateWeapon("Shotgun", WeaponID, false, Kingmaker.Utility.FeetExtension.Feet(20), Dice, DamageCriticalModifierType.X4, 20, DefaultFirearmDamageType(), icon, 9, visuals, MisfireEnhancement.Misfire12_A, true,30);


            //create a basic rifle and all the normal variants
            CreateBasicWeapons("Shotgun", BasicItemIDs, WeaponID, 5000);


            //setup any special enchanted variants we want to be in game
            //put all relevant versions into the shops
            AddWeapontoShop(BasicItemIDs, 3);//put the basic +1,+2 etc. in the chapter 3 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 5);//and again in the chapter 5 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 6);//and in the roguelike

            //add to finnean polymorph
            AddWeapontoFinnean("Shotgun", FinneanIDs, WeaponID);//does not allow stage 0 finnean

            AddWeaponFeats("Shotgun", WeaponID, FocusIDs);

        }
    }
}
