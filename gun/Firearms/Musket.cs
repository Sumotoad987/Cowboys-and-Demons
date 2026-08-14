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
        public const string WeaponID =    "91dab8ca9ac84a9db579e39f16aed207";
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
            "3cbecd27964f432398a310b5f78e215c",//improved critical
            "b4e5f4d730f049a296dd6a1ad4048866",//mythic focus
            "d54255f875c943d6bafc3193d272531e",//mythic spec
            "844d2d752346434bba13e23ef2c138ce",//mythic improved crit
            "31c91e99b14146568581d490779d81fe",//improved improved crit
            "0146ddf51d8a4c5c9d847b2863116143",//improved improved improved crit
            "da287f2f2bd74726a4306301718e4fdb"//improved improved improved crit improved
        };
        public static string[] FinneanIDs = {
            "906f2fc5b2dc45218631454fcda56553",//Stage 1
            "57a4291685e64633a82af4f9e343ea50",//Stage 2
            "7a2c224f79ae4386b8d7ae32acf5c2a6",//Stage 3
            "ae5faf162aac42969f3b7e82627031ad",//Stage Lich

        };
        public const string GatewayLootMusketID = "44898aa9d1214792950c112c6927a841";
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

            //create an enhancted musket for the gateway loot
            CreateWeaponItem("Musket" + "Gateway", GatewayLootMusketID, WeaponID, 5000)
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("80bb8a737579e35498177e1e3c75899b"),
                                   BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("633b38ff1d11de64a91d490c683ab1c8"))
                .Configure();

            //setup any special enchanted variants we want to be in game
            //put all relevant versions into the shops
            AddWeapontoShop(new string[] { BasicItemIDs[0]}, 1);//put the unchanted version in the chapter 1 vendor
            AddWeapontoShop(new string[] { BasicItemIDs[0], BasicItemIDs[1] , BasicItemIDs[2] }, 2);//put the basic +1,+2 in the chapter 2 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 3);//put the basic +1,+2 etc. in the chapter 3 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 5);//and again in the chapter 5 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 6);//and in the roguelike

            //add to finnean polymorph
            AddWeapontoFinnean("Musket", FinneanIDs, WeaponID, true);

            //create weapon focus and weapon specialisation and improved critical
            AddWeaponFeats("Musket", WeaponID, FocusIDs);

        }
    }
}
