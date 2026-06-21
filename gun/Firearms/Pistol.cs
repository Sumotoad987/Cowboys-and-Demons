using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static gun.Firearms.BaseFirearm;

namespace gun.Firearms
{
    internal static class Pistol
    {
        const string WeaponID = "dc569aa7-030e-4aa0-9c8b-6f7ca45ead11";
        public static string[] BasicItemIDs = {
            "5f7be1d1-41e8-43c0-aec9-41ffe8f5c2ee",//standard
            "afc13a04-9f93-42ef-a1cf-0a24bfd2e485",//plus 1
            "0950a052-fa9f-4de5-85c9-1d20f4fb3362",//plus 2
            "b8757133-f12a-4643-8886-217cdceb347c",//plus 3
            "1675108a-a3f2-431a-96ff-227c6b011c97",//plus 4
            "c41ac8a9-1c92-4cb5-b6db-0bb55dcb3eb4"//plus 5
        };
        public static string[] FocusIDs = {
            "bff81a636c864f848eef502b6f5f07ad",//weapon focus
            "4b6d0760901645fb839db2c25f010dab",//weapon focus greater
            "789033f251e04a3484c35e86dc7f0eca",//weapon specialisation
            "c5b49993d16d4d4e9529e98ee684aeaa"//weapon specialisation greater
        };
        public static void Configure()
        {
            //WeaponVisualParameters Uses crossbow animation style
            WeaponVisualParameters visuals = DefineVisualParameters("79efc418df928f34a9ceca38961f91cc");
            //defines the damage dice stuff
            DiceFormula Dice = new DiceFormula();
            Dice.m_Rolls = 1;
            Dice.m_Dice = DiceType.D8;

            //creates an Icon
            byte[] data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/Pistol.png");
            Texture2D texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite icon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));

            //creates the pistol weapon type by calling from base firearm
            CreateWeapon("Pistol", WeaponID, true, Kingmaker.Utility.FeetExtension.Feet(20), Dice, DamageCriticalModifierType.X4, 20, DefaultFirearmDamageType(), icon, 9, visuals, MisfireEnhancement.Misfire1_5);

            //create a basic pistol and all the normal variants
            CreateBasicWeapons("Pistol", BasicItemIDs, WeaponID, 1000);


            //setup any special enchanted variants we want to be in game
            //put all relevant versions into the shops
            AddWeapontoShop(BasicItemIDs, 3);//put the basic +1,+2 etc. in the chapter 3 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 5);//and again in the chapter 5 exotic weapons vendor

            AddWeaponFocus("Pistol", WeaponID, FocusIDs);

        }
    }
}
