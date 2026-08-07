using BlueprintCore.Actions.Builder;
using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.Log;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Owlcat.QA.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gun.Plot
{
    internal class StoryBookActions
    {
    }

    public class DealDamageAbility : GameAction, IValidated
    {

        [ValidateNotNull]
        [SerializeReference]
        public UnitEvaluator Target;

        public DiceFormula dice;
        public int bonus;
        public Kingmaker.EntitySystem.Stats.StatType Stat;

        public bool DisableBattleLog;


        public override string GetDescription()
        {
            return $"Deal damage to {Target} with source {Target}\n" + (DisableBattleLog ? "Log disabled" : "Log enabled") + "\n";
        }

        public override void RunAction()
        {
            using (new DisableBattleLog(DisableBattleLog))
            {
                Rulebook.Trigger(new RuleDealStatDamage(Target.GetValue(), Target.GetValue(), Stat, dice, bonus)
                {
                    DisableBattleLogSelf = DisableBattleLog,
                });
            }
        }

        public override string GetCaption()
        {
            return "Deal Ability Damage";
        }

        public void Validate(ValidationContext context, int parentIndex)
        {
        }

        
    }

    public class DealLevelDamage : GameAction, IValidated
    {

        [ValidateNotNull]
        [SerializeReference]
        public UnitEvaluator Target;

        public DiceFormula dice;
        public int bonus;

        public EnergyDrainType DrainType;

        public TimeSpan? duration;

        public bool DisableBattleLog;


        public override string GetDescription()
        {
            return $"Deal damage to {Target} with source {Target}\n" + (DisableBattleLog ? "Log disabled" : "Log enabled") + "\n";
        }

        public override void RunAction()
        {
            using (new DisableBattleLog(DisableBattleLog))
            {
                Rulebook.Trigger(new RuleDrainEnergy(Target.GetValue(), Target.GetValue(), DrainType, duration, dice, bonus)
                {
                    DisableBattleLogSelf = DisableBattleLog,
                });
            }
        }

        public override string GetCaption()
        {
            return "Deal Permanent Level Damage";
        }

        public void Validate(ValidationContext context, int parentIndex)
        {
        }


    }
    public static class ActionsBuilderStorybookEX
    {
        public static ActionsBuilder DealAbilityDamage(this ActionsBuilder builder, UnitEvaluator Target, DiceFormula dice, int bonus, Kingmaker.EntitySystem.Stats.StatType Stat)
        {
            DealDamageAbility dealDamageAbility = ElementTool.Create<DealDamageAbility>();
            dealDamageAbility.Target = Target;
            dealDamageAbility.dice = dice;
            dealDamageAbility.bonus = bonus;
            dealDamageAbility.Stat = Stat;
            return builder.Add(dealDamageAbility);

        }
        public static ActionsBuilder DealLevelDamage(this ActionsBuilder builder, UnitEvaluator Target, DiceFormula dice, int bonus, EnergyDrainType drainType, TimeSpan? duration)
        {
            DealLevelDamage dealLevelDamage = ElementTool.Create<DealLevelDamage>();
            dealLevelDamage.Target = Target;
            dealLevelDamage.dice = dice;
            dealLevelDamage.bonus = bonus;
            dealLevelDamage.DrainType = drainType;
            dealLevelDamage.duration = duration;
            return builder.Add(dealLevelDamage);

        }
    }
}
