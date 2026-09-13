using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using gun.Deeds;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gun.Classes.Gunslinger
{
    internal static class MusketMaster
    {
        public const string GUID = "9eb30ecae1e64a05a6de4d1482a184f6";
        public const string FastMusketUnlock = "daf840ff539846499c1181d49bdf4b66";
        public const string FastMusketEffect = "93589786ba8e4ae2b25a2f6f55f24f31";
        public static void Configure()
        {
            FastMusket();
            ArchetypeConfigurator.New("GunslingerMusketMaster", GUID, Gunslinger.GunslingerClassGUID)
                .SetLocalizedName("MusketMaster.Name")
                .SetLocalizedDescription("MusketMaster.Description")
                .AddToRemoveFeatures(DefineRemoveFeatures())
                .AddToAddFeatures(DefineAddFeatures())
                .Configure();
        }

        private static LevelEntry[] DefineRemoveFeatures()
        {
            LevelEntry level3 = new LevelEntry();
            level3.Level = 3;
            level3.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(DeedConfigurator.SlingerInitUnlockGUID),
            ];

            LevelEntry level5 = new LevelEntry();
            level5.Level = 5;
            level5.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(GunTraining.GunTrainingGUID),//get reduced version for only two handed firearms instead
            ];

            return new LevelEntry[] { level3, level5 };
        }

        private static LevelEntry[] DefineAddFeatures()
        {

            LevelEntry level3 = new LevelEntry();
            level3.Level = 3;
            level3.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(FastMusketUnlock),
            ];

            LevelEntry level5 = new LevelEntry();
            level5.Level = 5;
            level5.m_Features = [
                BlueprintTool.GetRef<BlueprintFeatureBaseReference>(GunTraining.MusketTrainingGUID)
                
            ];


            return new LevelEntry[] { level3, level5 };
        }

        private static void FastMusket()
        {
            FeatureConfigurator.New("FastMusketEffect", FastMusketEffect)
                .SetDescription("MusketMaster.FastMusket.Description")
                .SetDisplayName("MusketMaster.FastMusket.Name")
                .SetHideInCharacterSheetAndLevelUp()
                .SetHideInUI()
                .Configure();
            GritUnlock FastMusketGrit = new GritUnlock();
            FastMusketGrit.m_NewFact = BlueprintTool.GetRef<BlueprintUnitFactReference>(FastMusketEffect);

            FeatureConfigurator.New("FastMusket", FastMusketUnlock)
                .SetIcon(BlueprintTool.Get<BlueprintFeature>("9c928dc570bb9e54a9649b3ebfe47a41").Icon)
                .SetIsClassFeature(true)
                .SetDescription("MusketMaster.FastMusket.Description")
                .SetDisplayName("MusketMaster.FastMusket.Name")
                .AddComponent(FastMusketGrit)
                .Configure();
        }
    }
}
