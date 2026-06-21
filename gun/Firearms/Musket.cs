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

namespace gun.Firearms
{
    internal static class Musket
    {
        const string WeaponID =    "91dab8ca9ac84a9db579e39f16aed207";
        public static string[] BasicItemIDs = {
            "8b8b35697df54364bfaa2512dff806af",//standard
            "20bd807392ea4504af92874a78bc5bbc",//plus 1
            "b29ec74329de4d0680a8fbd15b0cf52c",//plus 2
            "7bae979ec7934623a6d0066613ce16dc",//plus 3
            "1549de2550064179ae04c6191f7ff61c",//plus 4
            "6ad5ab280eb04ff5901a9dd757b52e1f"//plus 5
        };
        public static string[] FocusIDs = {
            "9ef58f5c4dd04c7cb20ad7f33c050987",//focus
            "818b990e603b4532857da17253ce5d2f",//greater focus
            "813b88a010e24716937f1126b5bc3176",//specialization
            "0b6be05035924ad7b0e863f4f3b760e9",//greater specialization
        };
        public static void Configure()
        {
            //WeaponVisualParameters Uses crossbow animation style
            WeaponVisualParameters visuals = DefineVisualParameters("3e7e4b0088a919a40ba2ee9614a844ee");
            //defines the damage dice stuff
            DiceFormula Dice = new DiceFormula();
            Dice.m_Rolls = 1;
            Dice.m_Dice = DiceType.D12;

            //creates an Icon
            byte[] data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/Musket.png");
            Texture2D texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite icon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));

            //creates the musket weapon type by calling from base firearm
            CreateWeapon("Musket", WeaponID, false, Kingmaker.Utility.FeetExtension.Feet(40), Dice, DamageCriticalModifierType.X4, 20, DefaultFirearmDamageType(), icon, 9, visuals,MisfireEnhancement.Misfire12_5);

            //create a basic Musket and all the normal variants
            CreateBasicWeapons("Musket", BasicItemIDs, WeaponID, 1500);

            //setup any special enchanted variants we want to be in game
            //put all relevant versions into the shops
            AddWeapontoShop(BasicItemIDs, 3);//put the basic +1,+2 etc. in the chapter 3 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 5);//and again in the chapter 5 exotic weapons vendor

            AddWeaponFocus("Musket", WeaponID, FocusIDs);

        }
    }
}
