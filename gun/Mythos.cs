using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using gun.Plot;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.Rendering.DebugUI;

namespace gun
{
    internal static class Mythos
    {
        public const string WarpSpaceAbility = "f3a530f67023418fa89c59ac73b5ac29";
        public static void Configure()
        {
            WarpSpace();
        }

        public static void WarpSpace()
        {
            BlueprintAbility DimensionDoor = BlueprintTool.Get<BlueprintAbility>("a9b8be9b87865744382f7c64e599aeb2");
            AbilityCustomDimensionDoor Teleport = DimensionDoor.GetComponent<AbilityCustomDimensionDoor>();
            AbilityConfigurator.New("MythosWarpSpace", WarpSpaceAbility)
                .AddComponent(Teleport)
                .AddLineOfSightIgnorance()
                .SetDisplayName(LocalizationTool.GetString("Mythos.WarpSpace.Name"))
                .SetDescription(LocalizationTool.GetString("Mythos.WarpSpace.Description"))
                .SetDescriptionShort(LocalizationTool.GetString("Mythos.WarpSpace.Description.Short"))
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Long)
                .SetCanTargetPoint(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .AddComponent(new AbilityRequirementHasBuff(BlueprintTool.GetRef<BlueprintBuffReference>("4b0cd08a3cea2844dba9889c1d34d667")))//add requirement for the midnight fane dimsion lock buff
                .SetIcon(DimensionDoor.Icon)
                .Configure();
        }
    }
    public class UpdateMythosCount : GameAction
    {
        public int Increment = 1;
        private BlueprintUnlockableFlag MythosFlag = BlueprintTool.Get<BlueprintUnlockableFlag>(Plot.Flags.Mythos);

        public UpdateMythosCount (int increment)
        {
            Increment = increment;
        }

        public static ActionsBuilder MythosActionBuilder(int incrment) 
        {
            ActionsBuilder builder = ActionsBuilder.New();
            builder.Add(new UpdateMythosCount(incrment));
            return builder;
        }

        private static string[] MythosAbilities =
        {
            Mythos.WarpSpaceAbility
        };
        public override string GetCaption()
        {
            return "Running Mythos Check";
        }

        public override void RunAction()
        {
            if (!MythosFlag.IsUnlocked)
            {
                MythosFlag.Unlock();
            }

            if (MythosFlag.IsUnlocked)
            {
                MythosFlag.Value += Increment;
            }

            int value = Game.Instance.Player.UnlockableFlags.GetFlagValue(MythosFlag);
            if (value > MythosAbilities.Length)
            {
                value = MythosAbilities.Length;
            }
            for (int i = 0; i < value; i++)
            {
                BlueprintAbility ability = BlueprintTool.Get<BlueprintAbility>(MythosAbilities[i]);
                if (Game.Instance.Player.MainCharacter.Value.Abilities.GetAbility(ability) == null)
                {
                    Game.Instance.Player.MainCharacter.Value.AddFact(ability);

                }
            }
            


        }
    }
    public class AbilityRequirementHasBuff : BlueprintComponent, IAbilityRestriction
    {
        public BlueprintBuffReference m_Buff;
        public AbilityRequirementHasBuff(BlueprintBuffReference m_Buff)
        {
            this.m_Buff = m_Buff;
        }
        public bool IsAbilityRestrictionPassed(AbilityData ability)
        {
            return ability.Caster.Buffs.HasFact(m_Buff);

        }

        public string GetAbilityRestrictionUIText()
        {
            return "Requires Planer Tear";
        }
    }
}
