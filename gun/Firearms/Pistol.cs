using BlueprintCore.Blueprints.Configurators.Items.Weapons;
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
        public const string WeaponID = "dc569aa7-030e-4aa0-9c8b-6f7ca45ead11";
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
            "c5b49993d16d4d4e9529e98ee684aeaa",//weapon specialisation greater
            "6309ae3a71b142138e25123c159b1f8c",//imrpovedcrit 
            "a7d02c0a8c374510aefcd72fa356c06c",//weapon focus mythic
            "f05cb7eaefc048a9ac89d38240e9a030",//weapon specialisation mythic
            "8eafafcef07e47948d4cf4f096237892",//imroved critical mythic
            "95842d9bce4747ad82469b0e5fa93b16",//improved improved crit 
            "8269e4128fab4e7488e0160bbee52597",//improved improved improved crit 
            "bdeb627f6f9a4d9682fadd37825f6bf0"//improved improved improved crit improved
        };
        public static string[] FinneanIDs =
        {
            "d4436f16a42f49f09d1545634ad1a6ca",
            "b7fe4a77cec6421483c70fc3c79e5bf4",
            "b910e55bc8f94c92b9afc3fc860d5f72",
            "019079a6e77f4a8d8303e0dd51b90e46"
        };

        public const string GatewayLootPistolID = "5f1372bcb8f3461ab3927131669c139b";

        public const string PistolofInfiniteSky = "561192dcaacf4fe79d796deacb9a2596";
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
            CreateWeapon("Pistol", WeaponID, true, Kingmaker.Utility.FeetExtension.Feet(30), Dice, DamageCriticalModifierType.X4, 20, DefaultFirearmDamageType(), icon, 9, visuals, MisfireEnhancement.Misfire1_5);

            //create a basic pistol and all the normal variants
            CreateBasicWeapons("Pistol", BasicItemIDs, WeaponID, 1000);

            CreateWeaponItem("Pistol" + "Gateway", GatewayLootPistolID, WeaponID, 5000)
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("80bb8a737579e35498177e1e3c75899b"),
                                   BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("629c383ffb407224398bb71d1bd95d14"))
                .Configure();

            CreateWeaponItem("PistolInfinateSky", PistolofInfiniteSky, WeaponID, 73300)
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("bdba267e951851449af552aa9f9e3992"))
                .RemoveFromEnchantments(BaseFirearm.ClipGUID, MisfireEnhancement.Misfire1_5)//does not have a clip and can't misfire

                .SetDisplayNameText(LocalizationTool.GetString("Firearms.PistolInfinateSky.Name"))
                .SetDescriptionText(LocalizationTool.GetString("Firearms.PistolInfinateSky.Description"))
                .Configure();


            //setup any special enchanted variants we want to be in game
            //put all relevant versions into the shops
            AddWeapontoShop(new string[] { BasicItemIDs[0] }, 1);//put the unchanted version in the chapter 1 vendor
            AddWeapontoShop(new string[] { BasicItemIDs[0], BasicItemIDs[1], BasicItemIDs[2] }, 2);//put the basic +1,+2 in the chapter 2 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 3);//put the basic +1,+2 etc. in the chapter 3 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 5);//and again in the chapter 5 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 6);//and in the roguelike

            //add to finnean polymorph
            AddWeapontoFinnean("Pistol", FinneanIDs, WeaponID, true);

            AddWeaponFeats("Pistol", WeaponID, FocusIDs);

        }
    }
}
