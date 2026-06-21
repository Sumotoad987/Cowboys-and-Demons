using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
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
            DrifterConfig.SetParentClass(BlueprintTool.Get<BlueprintCharacterClass>("3adc3439f98cb534ba98df59838f02c7"));//its an archetype of cavalier
        }
    }
}
