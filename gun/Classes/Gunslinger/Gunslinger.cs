using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using gun.Firearms;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.Rendering.DebugUI;

namespace gun.Classes.Gunslinger
{
    internal static class Gunslinger
    {
        public const string GunslingerClassGUID = "5af5a87697264a96a9d7ce834bd7b0ba";
        
        
        public static void Configure()
        {
            Nimble.Configure();
            Grit.Configure();
            Deeds.Configure();
            GunTraining.Configure();
            Progression.Configure();

           // LocalString name = new LocalString();

            CharacterClassConfigurator GunslingerConfig = CharacterClassConfigurator.New("Gunslinger", GunslingerClassGUID)
                .SetLocalizedName(LocalizationTool.GetString("Gunslinger.Class.Name"))
                .SetLocalizedDescriptionShort(LocalizationTool.GetString("Gunslinger.Class.DescriptionShort"))
                .SetLocalizedDescription(LocalizationTool.GetString("Gunslinger.Class.Description"))
                .SetBaseAttackBonus(BlueprintTool.GetRef<BlueprintStatProgressionReference>("b3057560ffff3514299e8b93e7648a9d"))//full BAB
                .SetSkillPoints(4)//4+int skill points
                .SetHitDie(Kingmaker.RuleSystem.DiceType.D10)//d10 hit die
                .SetProgression(BlueprintTool.GetRef<BlueprintProgressionReference>(Progression.GunslingerProgressionGUID))//uses gunslinger progression
                .SetFortitudeSave(BlueprintTool.GetRef<BlueprintStatProgressionReference>("ff4662bde9e75f145853417313842751"))//high fort
                .SetReflexSave(BlueprintTool.GetRef<BlueprintStatProgressionReference>("ff4662bde9e75f145853417313842751"))//high reflex
                .SetWillSave(BlueprintTool.GetRef<BlueprintStatProgressionReference>("dc0c7c1aba755c54f96c089cdf7d14a3"))//low will
                .SetClassSkills(StatType.SkillMobility, StatType.SkillAthletics, StatType.SkillPerception,StatType.SkillThievery)//gives class skills mobility athletics perception and trickery (maybe add knowledge world but not for now)
                .SetStartingGold(411)
                .SetStartingItems(BlueprintTool.GetRef<BlueprintItemReference>("afbe88d27a0eb544583e00fa78ffb2c7"),//studded leather
                    BlueprintTool.GetRef<BlueprintItemReference>("08c7987d7320d1645a4a1f005ab2dfcb"),//cold iron short sword
                    BlueprintTool.GetRef<BlueprintItemReference>("0a6a7bbf0ddcebb4f8c4c7bd5d20c003"),//potion of reduce person
                    BlueprintTool.GetRef<BlueprintItemReference>("d52566ae8cbe8dc4dae977ef51c27d91"),//potion of cure light wounds
                    BlueprintTool.GetRef<BlueprintItemReference>(Musket.BasicItemIDs[0]))//musket
                .AddToEquipmentEntities(BlueprintTool.GetRef<KingmakerEquipmentEntityReference>("06557cc7164741f99a4075922bce4e5a"))//add the expert hat (looks like a cowboy hat)
                .SetMaleEquipmentEntities("1f538abc2802c5649b7ce177183f88c8", "54de61e669f916543b96da841357d2ff")
                .SetFemaleEquipmentEntities("dc822f0446c675a45809202953fa52a7", "67d82fc7662a522449d5dc8ed622e33a")
                .SetRecommendedAttributes(StatType.Dexterity,StatType.Wisdom)
                .SetNotRecommendedAttributes(StatType.Strength)
                .SetDifficulty(3)//setting this arbitrarily cause I'm not really sure
                .SetPrimaryColor(54)
                .SetSecondaryColor(69)
                .SetHideIfRestricted(false)
                .SetIsMythic(false)
                .SetIsDivineCaster(false)
                .SetIsArcaneCaster(false)
                .SetPrestigeClass(false)
                ;
            GunslingerConfig.Configure();


            //I think this will add the class to the character creator but not sure
            BlueprintRoot root = BlueprintRoot.Instance;
            root.Progression.m_CharacterClasses = [.. root.Progression.m_CharacterClasses, BlueprintTool.GetRef<BlueprintCharacterClassReference>(GunslingerClassGUID)];

        }

    }
}
