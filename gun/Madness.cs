using BlueprintCore.Actions.Builder;
using BlueprintCore.Utils;
using gun.Plot;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace gun
{
    internal static class Madness
    {
        public const int MadnessTheshold = 999;
        public const string Buff = "";
        public static void Configure()
        {

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
}
