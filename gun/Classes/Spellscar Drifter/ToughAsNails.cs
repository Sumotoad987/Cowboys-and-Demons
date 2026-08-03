using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using gun.Deeds;
using gun.Firearms;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.RuleSystem.RulebookEvent;
using static UnityEngine.Rendering.DebugUI;

namespace gun.Classes.Spellscar_Drifter
{
    internal static class ToughAsNails
    {
        public static string GUID = "7fbeb15b7064412a988035b6ae8f8711";

        public static void Configure()
        {
            FeatureConfigurator.New("ToughAsNails", GUID)
                .SetDisplayName(LocalizationTool.GetString("ToughAsNails.Name"))
                .SetDescription(LocalizationTool.GetString("ToughAsNails.Description"))
                .SetDescriptionShort(LocalizationTool.GetString("ToughAsNails.Description.Short"))
                .AddComponent(new ToughAsNailsShot())
                .Configure();
        }
    }

    internal class ToughAsNailsShot : AbstractWeaponTrigger, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {

        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.AttackRoll.IsCriticalConfirmed &&//if we crit the target
                evt.Weapon.Blueprint.Category == BaseFirearm.FirearmCategory//and the weapon is a firearm
                )
            {
                int DC = 10;
                DC += evt.Initiator.Stats.BaseAttackBonus;//DC is 10 + BAB
                RuleSavingThrow StunSave = new RuleSavingThrow(evt.Target, SavingThrowType.Fortitude, DC);
                Context.TriggerRule(StunSave);

                evt.Initiator.Resources.Spend(Grit.GritResource, 2);
                int rounds = Dice.D(1, DiceType.D4);
                if (StunSave.IsPassed)//if the target passes the save
                {
                    evt.Target.AddBuff(BlueprintTool.Get<BlueprintBuff>("df3950af5a783bd4d91ab73eb8fa0fd3"), Context, TimeSpan.FromSeconds(rounds * 6));//staggered for d4 round  or d4 * 6 seconds)
                }
                else
                {
                    
                    evt.Target.AddBuff(BlueprintTool.Get<BlueprintBuff>("09d39b38bb7c6014394b6daced9bacd3"), Context, TimeSpan.FromSeconds(rounds * 6));//stunned for d4 round  or d4 * 6 seconds)
                }

            }
        }
    }
}
