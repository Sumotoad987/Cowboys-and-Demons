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
    internal static class Rifle
    {
        const string WeaponID = "96b4d5e194e74bf3bc2b7dbf16bbffb4";
        public static string[] BasicItemIDs = {
            "7c6a55c6da594dcfba98973336c65083",//standard
            "09bf4f8e9e1d40ca936812eb85956f37",//plus1
            "b7f1ea279cec4a8dab89b1962b234df8",//+2
            "ba896a2b58f74b8394d5905bafe1bc59",//+3
            "98a47668c4a74e93a04ed0c5e2c5e96c",//+4
            "b5ce89451a104dedbc66169b292b9926"//plus 5
        };
        public static string[] FocusIDs = {
            "2c00239bd7244e1fb59cf2319a9cd4a2",//focus
            "2de7f4b42c9e449c98de3f7ce20ca41c",//greater focus
            "2a34e2828f4b4bed932db7edf8eab49f",//specialisation
            "3ee46d18b3bd48e68bb6e2b5fda82ed8"//greater spec
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

            //creates the musket weapon type by calling from base firearm
            CreateWeapon("Rifle", WeaponID, false, Kingmaker.Utility.FeetExtension.Feet(80), Dice, DamageCriticalModifierType.X4, 20, DefaultFirearmDamageType(), icon, 9, visuals, MisfireEnhancement.Misfire1_A,true);

            //create a basic Musket and all the normal variants
            CreateBasicWeapons("Rifle", BasicItemIDs, WeaponID, 5000);

            //setup any special enchanted variants we want to be in game
            //put all relevant versions into the shops
            AddWeapontoShop(BasicItemIDs, 3);//put the basic +1,+2 etc. in the chapter 3 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 5);//and again in the chapter 5 exotic weapons vendor

            AddWeaponFocus("Rifle", WeaponID, FocusIDs);

        }
    }
}
