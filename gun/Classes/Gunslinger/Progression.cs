using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using gun.Firearms;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gun.Classes.Gunslinger
{
    internal static class Progression
    {
        public const string GunslingerProgressionGUID = "1a961e6c4d694ac08e6e75a2599f4f4b";
        public const string GunslingerProficienciesGUID = "8d2aa7ba992a41c5abb8741c6652180c";
        public static void Configure()
        {

            BlueprintUnitFactReference LightArmor = new BlueprintUnitFactReference();
            LightArmor.deserializedGuid = BlueprintTool.Get<BlueprintFeature>("6d3728d4e9c9898458fe5e9532951132").AssetGuid;
            BlueprintUnitFactReference SimpleWeapons = new BlueprintUnitFactReference();
            SimpleWeapons.deserializedGuid = BlueprintTool.Get<BlueprintFeature>("e70ecf1ed95ca2f40b754f1adb22bbdd").AssetGuid;
            BlueprintUnitFactReference MartialWeapons = new BlueprintUnitFactReference();
            MartialWeapons.deserializedGuid = BlueprintTool.Get<BlueprintFeature>("203992ef5b35c864390b4e4a1e200629").AssetGuid;
            BlueprintUnitFactReference Firearms = new BlueprintUnitFactReference();
            Firearms.deserializedGuid = BlueprintTool.Get<BlueprintFeature>(FirearmProficiency.FirearmProficiencyGUID).AssetGuid;

            AddFacts Proficiencies = new AddFacts();
            Proficiencies.m_Facts = new BlueprintUnitFactReference[] { LightArmor, SimpleWeapons, MartialWeapons, Firearms };
            FeatureConfigurator.New("GunslingerProficiencies", GunslingerProficienciesGUID)
                .AddComponent(Proficiencies)
                .SetDescription(LocalizationTool.GetString("Gunslinger.Proficiencies.Description"))
                .SetDisplayName(LocalizationTool.GetString("Gunslinger.Proficiencies.Name"))
                .Configure();

            ProgressionConfigurator GunslingProgression = ProgressionConfigurator.New("GunslingerProgression", GunslingerProgressionGUID)
                .SetClasses(Gunslinger.GunslingerClassGUID);
            GunslingProgression.SetLevelEntry(1, //Level 1
                                GunslingerProficienciesGUID,              //Weapon and Armour Proficiencies
                                Grit.GritFeatureGUID,                     //Grit
                                Deeds.QuickClearFeatureGUID               //Deeds (only quick clear seems likely to work)
                                )
                .SetLevelEntry(2,//Level 2
                                Nimble.NimbleUnlockGUID//Nimble gain a +1 dodge bonus to AC when wearing light or no Armor
                )
                .SetLevelEntry(3,//Level 3
                                Deeds.SlingerInitUnlockGUID//Gunslinger Initiative:if has any grit gain +2 initiative
                                                           //the other two seem either no useful in game or tricky to implement
                                                           //though I might be able to do pistol whip, which seems to be make a melee attack with the weapon then attempt to trip oponent as standard action costing 1 grit
                )
                .SetLevelEntry(4,//Level 4
                                "41c8486641f7d6d4283ca9dae4147a9f"//Bonus Feat Selection (I will make Grit feats combat feats for simplicity
                )
                .SetLevelEntry(5,//Level 5
                                GunTraining.GunTrainingGUID //Gun Training: add dex to damage with firearms
                )
                .SetLevelEntry(6//Level 6
                                //Nimble +2 (this is part of the base nimble feature)
                )
                .SetLevelEntry(7,//Level 7
                                 //Deeds
                                 //Deadshot looks really complicated for not much benefit
                                 //startling shot also looks kind of bad but I can probably make it work
                                Deeds.TargetingFeatureGUID//Targeting is cool and should be workable limit to arms legs and head
                )
                .SetLevelEntry(8,
                                 "41c8486641f7d6d4283ca9dae4147a9f"//Bonus Feat Selection
                )
                .SetLevelEntry(9)//Level 9 actually should just be blank
                .SetLevelEntry(10//Level 10
                                 //Nimble +3 (this is part of the base nimble feature)
                )
                .SetLevelEntry(11,//Level 11
                                  //Deeds
                                  //Bleeding wound should be possible
                                 Deeds.BleedingShotFeatureGUID
                //Expert loading seems annoying but possible
                //not sure lightning reload does much so might ignore for now
                )
                .SetLevelEntry(12,//Level 12
                                  "41c8486641f7d6d4283ca9dae4147a9f"//Bonus Feat Selection
                )
                .SetLevelEntry(13)//Level 13 is also blank
                .SetLevelEntry(14//Level 14
                                 //Nimble +4 (this is part of the base nimble feature)
                )
                .SetLevelEntry(15,//Level 15 
                                 Deeds.EvasiveDeedGUID//Evasive: so long as she has 1 grit she gains the benefit of the evasion, uncanny dodge, and improved uncanny dodge rogue class features. She uses her gunslinger level as her rogue level for improved uncanny dodge. 
                                                      //Menacing Shot (Ex) (Ultimate Combat pg. 12): At 15th level, the gunslinger can spend 1 grit point, shoot a firearm into the air, and affect all living creatures within a 30-foot-radius burst as if they were subject to the fear spell. The DC of this effect is equal to 10 + 1/2 the gunslinger's level + the gunslinger's Wisdom modifier.
                )
                .SetLevelEntry(16,//Level 16
                                  "41c8486641f7d6d4283ca9dae4147a9f"//Bonus Feat Selection
                )
                .SetLevelEntry(17//Level 17 is also blank
                )
                .SetLevelEntry(18//Level 18
                                 //Nimble +5 (this is part of the base nimble feature)
                )
                .SetLevelEntry(19,//Level 19
                                 Deeds.StunningShotFeatureGUID//Stunning Shot (Ex) (Ultimate Combat pg. 12): At 19th level, when a gunslinger hits a creature, she can spend 2 grit points to stun the creature for 1 round. The creature must make a Fortitude saving throw (the DC = 10 + 1/2 the gunslinger's level + the gunslinger's Wisdom modifier). If the creature fails, it is stunned for 1 round. Creatures that are immune to critical hits are also immune to this effect. 
                )
                .SetLevelEntry(20,//Level 20
                                  "41c8486641f7d6d4283ca9dae4147a9f",//Bonus Feat Selection
                                  Grit.TrueGritFeatureGUID           //True Grit altered to refund 1 grit per grit spend
                ).Configure();
                ;
        }
    }
}
