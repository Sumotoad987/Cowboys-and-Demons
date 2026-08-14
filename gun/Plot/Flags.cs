using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.StoryEx;
using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.Configurators.AreaLogic.Etudes;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.ElementsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gun.Plot
{
    internal static class Flags
    {
        public const string CowgirlRespect = "4f5d55d2fff1491b9d2c3493d07af5ca";
        public const string CowgirlApproval = "2bc8e0a161c44dcbbc7d90e91360c50a";
        public const string SuspectsCowgirl = "a3c8a07d04514d2eb6468d9d0608af15";
        public const string CowgirlInParty = "f2fdcbece99f4c20aaee00a2af8a4c2b";
        public const string OfferedToHelpCowgirl = "ef183c806412468aa5d7a2cb66907108";
        public const string AskedCowgirlToJoinCrusade = "80a691254e5344098f30ce71bfd2a662";
        public const string Mythos = "8ec57227d1fc441194089220d4f0133c";
        public const string Madness = "36d7803faad945d6a1f4da3d6ebf6bc3";
        public const string MetCowgirl = "a5ceb576f2d948bebb4cab2375ae93dd";
        public const string CowgirlInDrezen = "15d22e9571a345aa952ec379558cbabd";
        public const string RejectedCowgirl = "c55b78884af1466193b03b3bf4702c3b";
        public const string BeganGatewayToInsanity = "c1df1b763039427fa7b7ec3abf867608";
        public const string CompletedGatewayToInsanity = "a047e26c5ead4e7ba44039823e5446fb";
        public const string GatewayToInsanityAlarm = "4deb9c6fbdb74d1a9a18a9270ea61b2a";
        public const string GatewayToInsanityLongTime = "df646e65f9e94ad586f17e3b5759f4f8";
        public const string TreatedFleshwarps = "5ac1e7a2e0a64b109dc635e6d963cf6b";
        public const string CowgirlSpawnedInDrezen = "f3ed25ebefb743f39791e8cdf8c83183";
        public static void Configure()
        {
            UnlockableFlagConfigurator.New("CowgirlRespect", CowgirlRespect).Configure();
            UnlockableFlagConfigurator.New("CowgirlApproval", CowgirlApproval).Configure();
            UnlockableFlagConfigurator.New("SuspectsCowgirl", SuspectsCowgirl).Configure();
            UnlockableFlagConfigurator.New("OfferedToHelpCowgirl", OfferedToHelpCowgirl).Configure();
            UnlockableFlagConfigurator.New("AskedCowgirlToJoinCrusade", AskedCowgirlToJoinCrusade).Configure();
            UnlockableFlagConfigurator.New("MetCowgirl", MetCowgirl).Configure();
            UnlockableFlagConfigurator.New("Mythos", Mythos).Configure();
            UnlockableFlagConfigurator.New("Madness", Madness).Configure();
            UnlockableFlagConfigurator.New("CowgirlInDrezen", CowgirlInDrezen).Configure();
            UnlockableFlagConfigurator.New("RejectedCowgirl", RejectedCowgirl).Configure();
            UnlockableFlagConfigurator.New("BeganGatewayToInsanity", BeganGatewayToInsanity).Configure();
            UnlockableFlagConfigurator.New("CompletedGatewayToInsanity", CompletedGatewayToInsanity).Configure();
            UnlockableFlagConfigurator.New("GatewayToInsanityAlarm", GatewayToInsanityAlarm).Configure();
            UnlockableFlagConfigurator.New("GatewayToInsanityLongTime", GatewayToInsanityLongTime).Configure();
            UnlockableFlagConfigurator.New("TreatedFleshwarps", TreatedFleshwarps).Configure();
            UnlockableFlagConfigurator.New("CowgirlInParty", CowgirlInParty).Configure();
            UnlockableFlagConfigurator.New("CowgirlSpawnedInDrezen", CowgirlSpawnedInDrezen).Configure();
        }

        public static ActionsBuilder IncrementFlag(int value,string GUID)
        {
            ActionsBuilder actions = ActionsBuilder.New();
            actions.IncrementFlagValue(GUID, true, Utilities.MakeIntConstant(value));
            return actions;

        }
    }
}

public class EvaluatorInt : IntEvaluator
{
    public int value;
    public override string GetCaption()
    {
        return "" + value;
    }

    public override int GetValueInternal()
    {
        return value;
    }

    public EvaluatorInt(int value)
    {
        this.value = value;
    }
}
