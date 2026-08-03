using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using gun.Deeds;
using gun.Firearms;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gun.Classes.Spellscar_Drifter
{
    internal static class SpellscarDrifter
    {
        public const string SpellscarDrifterGUID = "0b3b3e7a3b8d4492b4127b4b6a2a01eb";

        public static void Configure()
        {
            ArchetypeConfigurator DrifterConfig = ArchetypeConfigurator.New("SpellscarDrifter", SpellscarDrifterGUID);
            DrifterConfig.SetLocalizedName(LocalizationTool.GetString("SpellscarDrifter.Name"));
            DrifterConfig.SetLocalizedDescription(LocalizationTool.GetString("SpellscarDrifter.Description"));
            DefineRemovedFeatures(DrifterConfig);
            DefineAddedFeatures(DrifterConfig);
            DrifterConfig.SetParentClass(BlueprintTool.Get<BlueprintCharacterClass>("3adc3439f98cb534ba98df59838f02c7"));//its an archetype of cavalier
            DrifterConfig.SetReplaceStartingEquipment(true);
            DrifterConfig.SetStartingItems(BlueprintTool.GetRef<BlueprintItemReference>("afbe88d27a0eb544583e00fa78ffb2c7"),//Studded Leather
                    BlueprintTool.GetRef<BlueprintItemReference>("533e10c8b4c6a4940a3767d096f4f05d"),//cold iron long sword
                    BlueprintTool.GetRef<BlueprintItemReference>("f4cef3ba1a15b0f4fa7fd66b602ff32b"),//Heavy Sheild
                    BlueprintTool.GetRef<BlueprintItemReference>("d52566ae8cbe8dc4dae977ef51c27d91"),//potion of cure light wounds
                    BlueprintTool.GetRef<BlueprintItemReference>(gun.Firearms.Musket.BasicItemIDs[0]));//musket
            DrifterConfig.Configure();
            
            CharacterClassConfigurator.For(BlueprintTool.GetRef<BlueprintCharacterClassReference>("3adc3439f98cb534ba98df59838f02c7"))
                .AddToArchetypes(BlueprintTool.GetRef<BlueprintArchetypeReference>(SpellscarDrifterGUID))
                .Configure();
            
        }

        public static void DefineRemovedFeatures(ArchetypeConfigurator Config)
        {
            LevelEntry Level1 = new LevelEntry();
            Level1.Level = 1;
            Level1.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>("0a0f032ccfe411d4d86b298da4657e58"),//cavalier proficiencies
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>("404f6e3da48b87f4e9fca21150d47f71"),//tactician
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>("7bc55b5e381358c45b42153b8b2603a6"),//tactician Feat Selection
            ];

            LevelEntry Level3 = new LevelEntry();
            Level3.Level = 3;
            Level3.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>("30da5120b692d0d4f8a6840dff777abf")//cavalier charge
            ];

            LevelEntry Level9 = new LevelEntry();
            Level9.Level = 9;
            Level9.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>("7bc55b5e381358c45b42153b8b2603a6"),//Tactician selection
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>("c7913a6c300cb994b8e800e1096f9280")//Greater Tactician
            ];

            LevelEntry Level11 = new LevelEntry();
            Level11.Level = 11;
            Level11.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>("ded43678aa1fbe241827175b65e9a749")//Mighty charge
            ];

            LevelEntry Level12 = new LevelEntry();
            Level12.Level = 12;
            Level12.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>("3fca19d3f3886ee47be4b4019908d223")//Demanding Challenge
            ];

            LevelEntry Level17 = new LevelEntry();
            Level17.Level = 17;
            Level17.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>("7bc55b5e381358c45b42153b8b2603a6")//Tactician selection
            ];

            LevelEntry Level20 = new LevelEntry();
            Level20.Level = 20;
            Level20.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>("77af3c58e71118d4481c50694bd99e77")//Supreme Charge
            ];


            Config.SetRemoveFeatures(Level1,Level3,Level9,Level11,Level12,Level17,Level20);
        }

        public static void DefineAddedFeatures(ArchetypeConfigurator Config)
        {
            LevelEntry Level1 = new LevelEntry();
            Level1.Level = 1;
            Level1.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(Gunslinger.Progression.GunslingerProficienciesGUID),//gain gunslinger prof
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(AmateurGunslinger.DrifterGUID),//Amateur Gunslinger with charisma
            ];

            LevelEntry Level3 = new LevelEntry();
            Level3.Level = 3;
            Level3.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(RapidReload.RapidReloadGUID)//Gain Rapid Reload as a bonus feat
            ];

            LevelEntry Level9 = new LevelEntry();
            Level9.Level = 9;
            Level9.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(DeedConfigurator.SlingerInitFeatureGUID),//Gunslinger's Initative
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(DeedConfigurator.TargetingFeatureGUID)//Targeted shot
            ];

            LevelEntry Level11 = new LevelEntry();
            Level11.Level = 11;
            Level11.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(OldReliable.GUID)//Old Reliable
            ];//I need to make a new feature here

            LevelEntry Level12 = new LevelEntry();
            Level12.Level = 12;
            Level12.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(SpellSevered.GUID)//Spell Severed
            ];

            LevelEntry Level17 = new LevelEntry();
            Level17.Level = 17;
            Level17.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(DeedConfigurator.BleedingShotGUID)
            ];

            LevelEntry Level20 = new LevelEntry();
            Level20.Level = 20;
            Level20.m_Features =
            [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(DeedConfigurator.EvasiveDeedGUID),
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(DeedConfigurator.StunningShotGUID),
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(ToughAsNails.GUID)
            ];


            Config.SetAddFeatures(Level1, Level3, Level9, Level11, Level12, Level17, Level20);
        }
    }
}
