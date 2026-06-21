using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Controllers.Projectiles;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.EntitySystem.EntityDataBase;
using static Kingmaker.EntitySystem.Properties.BaseGetter.PropertyContextAccessor;

namespace gun.Firearms
{
    internal static class FirearmFix
    {
        public static void Attack(RulebookEventContext context, RuleAttackWithWeapon Rule)
        {
            Rulebook.Trigger(Rule.WeaponStats);
            RuleAttackRoll AttackRoll = new RuleAttackRoll(Rule.Initiator, Rule.Target, Rule.WeaponStats, Rule.AttackBonusPenalty)
            {
                AutoHit = Rule.AutoHit,
                AutoCriticalThreat = Rule.AutoCriticalThreat,
                AutoCriticalConfirmation = (TacticalCombatHelper.IsActive || Rule.AutoCriticalConfirmation),
                SuspendCombatLog = Rule.Weapon.Blueprint.IsRanged,
                RuleAttackWithWeapon = Rule,
                DoNotProvokeAttacksOfOpportunity = Rule.IsAttackOfOpportunity,
                ForceFlatFooted = Rule.ForceFlatFooted
            };
            if (Rule.ReplaceAttackType)
            {
                AttackRoll.AttackType = Rule.AttackTypeReplacement;
            }
            Rulebook.Trigger(AttackRoll);
            if (AttackRoll.IsHit)
            {
                RuleDealDamage ruleDealDamage = Rule.CreateRuleDealDamage(TacticalCombatHelper.IsActive);
                    if (ruleDealDamage.DamageBundle != null)
                    {
                        context.Trigger(ruleDealDamage);
                    }
            }
            RuleAttackWithWeaponResolve ruleAttackWithWeaponResolve = new RuleAttackWithWeaponResolve(Rule, null);
            Rule.ResolveRules.Add(ruleAttackWithWeaponResolve);
            context.Trigger(ruleAttackWithWeaponResolve);
        }
    }
}
