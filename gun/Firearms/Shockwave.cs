using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Kingmaker.RuleSystem.Rules.RuleCombatManeuver;

namespace gun.Firearms
{
    public class Shockwave : WeaponEnchantmentLogic, IInitiatorRulebookHandler<Kingmaker.RuleSystem.Rules.RuleAttackWithWeapon>, IRulebookHandler<Kingmaker.RuleSystem.Rules.RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {

        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {

            if (((evt.Reason.Ability.Blueprint != BlueprintTool.Get<BlueprintAbility>(Scatter.Ability30ft) || evt.Reason.Ability.Blueprint != BlueprintTool.Get<BlueprintAbility>(Scatter.Ability15ft)) && evt.Reason.Ability.Blueprint.UseCurrentWeaponAsReasonItem && evt.Reason.Caster?.GetFirstWeapon() == base.Owner) || evt.Reason.Item == base.Owner)
            {//if the attack was made with this weapon and a scatter ability
               if (evt.AttackRoll.Result == AttackResult.Hit ||  evt.AttackRoll.Result == AttackResult.CriticalHit)
                {//if we hit or crit


                    RuleCombatManeuver trip = new RuleCombatManeuver(evt.Initiator, evt.Target, CombatManeuver.Trip);//make a trip rule from user to target
                    RuleCalculateCMB CMBRule = Rulebook.Trigger(new RuleCalculateCMB(evt.Initiator, evt.Target, CombatManeuver.Trip));//calculate CMB
                    trip.AttackRule = evt.AttackRoll.AttackBonusRule;

                    if (CMBRule.Result < 20)//if CMB is less than 20
                    {
                        trip.OverrideBonus = 20;//set it to 20
                        
                    }
                    Rulebook.Trigger(trip);

                }
            }
        }
    }

}
