using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums.Damage;
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
    internal static class Revolver
    {
        const string WeaponID = "3658f7e49f214a20afae7754b0e44e61";
        public static string[] BasicItemIDs = {
            "fdd20dd46492465fba197deda9e84b0d",
            "b394131b25d4447397b253759f2d5222",
            "29b43b41ee914b35afb22aa62f6d9cd0",
            "a758ff74483a43b48d93811892b4db5d",
            "84a6d98a62f344ad95610d3e96bf8354",
            "8c0552722a074822a3fc322f1c83e5a0"
        };
        public static string[] FocusIDs = {
            "71562452a43d4ffaaa238bc0872ba8ba",
            "36888b3f9ae94c97ba6aebcbfef40e56",
            "d6805db4a7924cae944a9426a3d9d254",
            "76968733082246f0b0c3424758303f46",//greater spec
            "418faba044344e1f962c42244bf7e1a2",
            "da327ae8bc0a424bb26e16fb35cf6d62",
            "971f77ee5cfa461ea3078a5ca8daed36",
            "dec55ca4f1a24aee88747f75b7d6927e",
            "4a702408b00c4037b57883a99e1df8cd",
            "f7c2834a9f9d4fbeac2228c4da11bb33",
            "9465bc9ffbaf47ee99b95f7b80c239ec"
        };
        public static string[] FinneanIDs =
        {
            "120915421ded4f8d92ce4719546adb1b",
            "c02d66ff2fef44a69097f54fa3c67e5d",
            "773b5ea06a7a490798e30a32886cc02d",
        };
        public static void Configure()
        {
            //WeaponVisualParameters Uses crossbow animation style
            WeaponVisualParameters visuals = DefineVisualParameters("9c4cf68771ea4394997251a37bb2bb92");
            //defines the damage dice stuff
            DiceFormula Dice = new DiceFormula();
            Dice.m_Rolls = 1;
            Dice.m_Dice = DiceType.D8;

            //creates an Icon
            byte[] data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/Revolver.png");
            Texture2D texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite icon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));

            //creates the musket weapon type by calling from base firearm
            CreateWeapon("Revolver", WeaponID, true, Kingmaker.Utility.FeetExtension.Feet(20), Dice, DamageCriticalModifierType.X4, 20, DefaultFirearmDamageType(), icon, 9, visuals, MisfireEnhancement.Misfire1_A, true,6);

            //create a basic Musket and all the normal variants
            CreateBasicWeapons("Revolver", BasicItemIDs, WeaponID, 4000);

            //setup any special enchanted variants we want to be in game
            //put all relevant versions into the shops
            AddWeapontoShop(BasicItemIDs, 3);//put the basic +1,+2 etc. in the chapter 3 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 5);//and again in the chapter 5 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 6);//and in the roguelike

            //add to finnean polymorph
            AddWeapontoFinnean("Revolver", FinneanIDs, WeaponID);//does not allow stage 0 finnean

            AddWeaponFeats("Revolver", WeaponID, FocusIDs);

        }
    }
}
