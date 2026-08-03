using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using static Kingmaker.RuleSystem.RulebookEvent;
using static UnityEngine.Networking.UnityWebRequest;

namespace gun.Classes.Spellscar_Drifter
{
    internal static class OldReliable
    {
        public static string GUID = "2cce39e0f77540b79eacd76abf759349";

        public static void Configure()
        {
            CritEdgeMultiplierAgainstFactOwner Effect = new CritEdgeMultiplierAgainstFactOwner();
            FeatureConfigurator.New("OldReliable", GUID)
                .SetDisplayName(LocalizationTool.GetString("OldReliable.Name"))
                .SetDescription(LocalizationTool.GetString("OldReliable.Description"))
                .SetDescriptionShort(LocalizationTool.GetString("OldReliable.Description.Short"))
                .AddComponent(Effect)
                .Configure();
        }
    }

    public class CritEdgeMultiplierAgainstFactOwner : UnitFactComponentDelegate<AttackBonusConditional.RuntimeData>, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>
    {
        public BlueprintUnitFactReference m_CheckedFact = BlueprintTool.GetRef<BlueprintUnitFactReference>("4f0218323ad379248b69de8a9501159f");


        public BlueprintUnitFact CheckedFact => m_CheckedFact?.Get();

        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.Weapon != null && evt.Target.Descriptor.HasFact(CheckedFact))//if the target has the fact
            {
                if (evt.Result == AttackResult.Hit//and the attack was a hit but not a crit
                    && !evt.WeaponStats.DoubleCriticalEdge)//and the critical edge was not already doubled
                {
                    int BoostedEdge = 20 - evt.WeaponStats.CriticalEdge;//get the distance from 20 so a critical edge of 18 becomes 2
                    BoostedEdge *= 2;//then double that range
                    BoostedEdge = 20 - BoostedEdge;//then subtract that from 20 to get the new edge
                    
                    if (evt.D20.Result <= evt.WeaponStats.CriticalEdge//if it was below the normal critical edge (if it was higher it was a crit that was negated
                        && !evt.AutoCriticalThreat &&//and wasn't an auto crit threat (again must therefore be a crit that failed)
                        evt.D20.Result >= BoostedEdge)//and would be a crit with the boosted edge
                    {
                        //then we do all the normal critical hit tests
                        RuleCalculateAC ruleCalculateAC = Rulebook.Trigger(new RuleCalculateAC(evt.Initiator, evt.Target, evt.AttackType)
                        {
                            IsCritical = true
                        });
                        evt.TargetCriticalAC = ruleCalculateAC.Result;
                        if (evt.AutoCriticalConfirmation)
                        {
                            evt.CriticalConfirmationD20 = new RuleRollD20(evt.Initiator, 20)
                            {
                                IsFake = evt.IsFake
                            };
                        }
                        else
                        {
                            evt.CriticalConfirmationD20 = Dice.GenerateD20(evt.IsFake);
                        }

                        evt.IsCriticalConfirmed = evt.AutoCriticalConfirmation || evt.CriticalConfirmationRoll >= evt.TargetCriticalAC;
                    }

                    if (evt.IsCriticalConfirmed)
                    {
                        evt.FortificationChance = evt.Target.Get<UnitPartFortification>()?.Value ?? 0;
                        if (evt.TargetUseFortification)
                        {
                            evt.FortificationRoll = Dice.D100;
                            if (!evt.FortificationOvercomed)
                            {
                                evt.FortificationNegatesCriticalHit = evt.IsCriticalConfirmed;
                                evt.IsCriticalConfirmed = false;
                            }
                        }
                    }

                    if (evt.IsCriticalConfirmed)
                    {
                        evt.Result = AttackResult.CriticalHit;
                    }

                }
            }
        }

        public void OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {

        }

        public void OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {
        }
    }
}
