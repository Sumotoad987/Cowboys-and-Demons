using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using gun.Plot;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Controllers.Units;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace gun
{
    internal static class Madness
    {
        public const int MadnessTheshold = 4;
        public const string BuffLight = "93811943c29b44448d2fe4eba8488730";
        public const string Buff = "b44be74240d54f2692622586af0c84d6";
        public static void Configure()
        {
            BuffConfigurator.New("MadnessLightbuff", BuffLight)
                .SetDisplayName("MadnessLight.Name")
                .SetDescription("MadnessLight.Description")
                .SetRanks(4)
                .AddSavingThrowBonusAgainstSchool(modifierDescriptor: ModifierDescriptor.Penalty, school: SpellSchool.Illusion, value:-1)
                .AddConcentrationBonus(value:-1)
                .Configure();

            BuffConfigurator.New("Madnessbuff", Buff)
                .SetDisplayName("Madness.Name")
                .SetDescription("Madness.Description")
                .SetRanks(4)
                .AddComponent(new MadnessSaveEachRound(actions:ActionsBuilder.New().ApplyBuffWithDurationSeconds("886c7407dc629dc499b9f1465ff382df",6),10,2,saveType: SavingThrowType.Will))
                .Configure();

        }


    }

    public class UpdateMadness : GameAction
    {
        public int Increment = 1;
        private BlueprintUnlockableFlag MythosFlag = BlueprintTool.Get<BlueprintUnlockableFlag>(Plot.Flags.Mythos);

        public UpdateMadness(int increment)
        {
            Increment = increment;
        }

        public static ActionsBuilder MadnessActionBuilder(int incrment)
        {
            ActionsBuilder builder = ActionsBuilder.New();
            builder.Add(new UpdateMadness(incrment));
            return builder;
        }
        public override string GetCaption()
        {
            return "Running Madness Check";
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

            int value = Game.Instance.Player.UnlockableFlags.GetFlagValue(BlueprintTool.Get<BlueprintUnlockableFlag>(Flags.Madness));
            if (value > Madness.MadnessTheshold)
            {
                //give the player a number of stacks of the madness buf equal to the value of the flag (possibly minus the theshold)
            }
        }
    }

    public class MadnessSaveEachRound : UnitBuffComponentDelegate<BuffSaveEachRoundData>, ITickEachRound
    {
        public SavingThrowType SaveType;

        public int BaseDC;

        public int IncreaseDC;

        public ActionList ActionsOnFail;

        public MadnessSaveEachRound(ActionsBuilder actions, int baseDC, int increaseDC, SavingThrowType saveType)
        {
            this.SaveType = saveType;
            this.BaseDC = baseDC;
            this.IncreaseDC = increaseDC;
            this.ActionsOnFail = actions.Build();
        }

        void ITickEachRound.OnNewRound()
        {
            int DC = BaseDC;
            DC += IncreaseDC * base.Buff.Rank;
            if (SaveType != 0)
            {
                RuleSavingThrow ruleSavingThrow = new RuleSavingThrow(base.Owner, SaveType, BaseDC)
                {
                    Reason = base.Fact
                };
                Game.Instance.Rulebook.TriggerEvent(ruleSavingThrow);
                if (!ruleSavingThrow.IsPassed)
                {
                    base.Buff.RunActionInContext(ActionsOnFail, base.Owner);
                }
            }
        }

        public override void OnActivate()
        {
            base.Data.StackDC = BaseDC;
        }
    }

}
