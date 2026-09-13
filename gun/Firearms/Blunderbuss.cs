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
    internal static class Blunderbuss
    {
        public const string WeaponID = "8744510d005347789e8fde2d43812152";
        public static string[] BasicItemIDs = {
            "d9a9d5f654324e0d96aca0072fcd7334",
            "6cc7a58e3b4c4d1cb9e941f98175f4d3",
            "4ff80bd4c04946198dba93588d9767ae",
            "ecc11946ef7f4dbcaa8d66ff430042c4",
            "51d6f36b4a21461b99773d76285b37f6",
            "f0471e44a43842aca8dbe75e3bf9b3d3",
        };
        public static string[] FocusIDs = {
            "0991e2c4c6dc48bfacd9ca10df7b4e24",
            "b8c39883c4ed40599f0de7e7b921eebb",
            "996e7f6f88bd4b8cb03b60427b408600",
            "e27902118bbc47b6bc1a80f0ed555115",
            "6770426b02ad4a28b0aedd38dab42738",
            "eadd37cc10674c529da7843dd8dc8f72",
            "4093070d723846f8bf53e12e586d2d76",
            "c704e041fa594fa68999968ec4563288",
            "e4c41542104d4002b1a3c3344dccdd5b",
            "12c54d2c15a042f886a504ac4f861ae1",
            "34bd26085e484f4d94d6ec984adbe9c8"
        };
        public static string[] FinneanIDs =
        {
            "a8d1e5c58bac4968a58c7368b87f4f05",
            "8c7ce6d50bfb445c80fb5df15d5574a8",
            "6c9c2fdcd46a4dc983b7b0152dab5a71"
        };
        public const string ShockwaveBlunderbuss = "8390d362c7ab4965bbe463a7e6c896c0";
        public const string ShockwaveEnchantment = "51461a5a443c40e9937b148fbfc4e6c8";
        public static void Configure()
        {
            //WeaponVisualParameters Uses crossbow animation style
            WeaponVisualParameters visuals = DefineVisualParameters("bae4ce91bc9d4714a9acc6d549c3604a");
            //defines the damage dice stuff
            DiceFormula Dice = new DiceFormula();
            Dice.m_Rolls = 1;
            Dice.m_Dice = DiceType.D8;

            //creates an Icon
            byte[] data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/Blunderbuss.png");
            Texture2D texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite icon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));

            //creates the rifle weapon type by calling from base firearm
            CreateWeapon("Blunderbuss", WeaponID, false, Kingmaker.Utility.FeetExtension.Feet(20), Dice, DamageCriticalModifierType.X2, 20, DefaultFirearmDamageType(), icon, 9, visuals, MisfireEnhancement.Misfire12_10, false, 1, 15);


            //create a basic rifle and all the normal variants
            CreateBasicWeapons("Blunderbuss", BasicItemIDs, WeaponID, 2000);


            WeaponEnchantmentConfigurator.New("ShockwaveEnhancement", ShockwaveEnchantment)
                .SetDescription(LocalizationTool.GetString("Firearms.Shockwave.Enchant.Description"))
                .SetEnchantName(LocalizationTool.GetString("Firearms.Shockwave.Enchant.Name"))
                .AddComponent(new Shockwave())
                .Configure();

            CreateWeaponItem("ShockwaveBlunderbuss", ShockwaveBlunderbuss, WeaponID, 87300)
                .AddToEnchantments(BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("eb2faccc4c9487d43b3575d7e77ff3f5"),//+2
                                   BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>("83bd616525288b34a8f34976b2759ea1"),//thundering burst
                                   ShockwaveEnchantment)//shockwave
                .SetDisplayNameText("Firearms.ShockwaveBlunderbuss.Name")
                .SetDescriptionText("Firearms.ShockwaveBlunderbuss.Description")
                .Configure();

            //setup any special enchanted variants we want to be in game
            //put all relevant versions into the shops
            AddWeapontoShop(BasicItemIDs, 3);//put the basic +1,+2 etc. in the chapter 3 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 5);//and again in the chapter 5 exotic weapons vendor
            AddWeapontoShop(BasicItemIDs, 6);//and in the roguelike

            //add to finnean polymorph
            AddWeapontoFinnean("Blunderbuss", FinneanIDs, WeaponID);//does not allow stage 0 finnean

            AddWeaponFeats("Blunderbuss", WeaponID, FocusIDs);

        }
    }
}
