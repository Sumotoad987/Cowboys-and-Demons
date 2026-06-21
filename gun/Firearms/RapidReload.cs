using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gun.Firearms
{
    public static class RapidReload
    {
        public const string RapidReloadGUID = "737cc264baee4d02a33921f099dd82da";
        public static void Configure()
        {
            //creates an Icon
            byte[] data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/Rapid Reload.png");
            Texture2D texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite icon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));

            FeatureConfigurator.New("RapidReload", RapidReloadGUID, new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat })
                .SetDescription(LocalizationTool.GetString("Feats.RapidReload.Description"))
                .SetDisplayName(LocalizationTool.GetString("Feats.RapidReload.Name"))
                .SetIcon(icon)
                .Configure()
                ;
        }
    }
    public class HasRapidReload : BlueprintComponent, IAbilityCasterRestriction
    {
        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            return caster.GetFeature(BlueprintTool.Get<BlueprintFeature>(RapidReload.RapidReloadGUID)) != null;
        }

        public string GetAbilityCasterRestrictionUIText()
        {
            return "Requires Rapid Reload";
        }

    }
}
