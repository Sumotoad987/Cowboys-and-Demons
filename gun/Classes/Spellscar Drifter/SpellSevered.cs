using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gun.Classes.Spellscar_Drifter
{
    internal static class SpellSevered
    {
        public static string GUID = "7155ae0ef75442e789a7cc5c4aaa4b41";

        public static void Configure()
        {
            ContextValue Value = new ContextValue();
            Value.ValueType = ContextValueType.Rank;
            Value.ValueRank = Kingmaker.Enums.AbilityRankType.Default;

            ContextRankConfig rankConfig = new ContextRankConfig();
            rankConfig.m_Type = Kingmaker.Enums.AbilityRankType.Default;
            rankConfig.m_Progression = ContextRankProgression.BonusValue;
            rankConfig.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
            rankConfig.m_StepLevel = 10;
            FeatureConfigurator.New("SpellSevered",GUID)
                .SetDisplayName(LocalizationTool.GetString("SpellSevered.Name"))
                .SetDescription(LocalizationTool.GetString("SpellSevered.Description"))
                .SetDescriptionShort(LocalizationTool.GetString("SpellSevered.Description.Short"))
                .AddContextRankConfig(rankConfig)
                .AddSpellResistance(value: Value)
                .Configure();
        }
    }
}
