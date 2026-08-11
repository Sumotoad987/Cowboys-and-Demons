using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using gun.Deeds;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.Rendering.DebugUI;

namespace gun.Classes.Gunslinger
{
    internal static class Buccaneer
    {
        public const string SeaDogFeature = "";
        public const string SeaDogAbility = "";
        public const string SeaDogBuff = "";
        public const string JargonFeature = "";
        public const string JargonBuff = "";
        public const string JargonAbility = "";
        public const string GrogBuff = "";
        public const string GrogAbility = "";
        public const string GrogFeature = "";
        public static void Configure()
        {
            SeaDog();
            PirateJargon();

            //cha grit
        }

        private static void SeaDog()
        {
            BuffConfigurator.New("SeaDogBuff", SeaDogBuff)
                .SetDisplayName("Gunslinger.Buccaneer.SeaDog.Name")
                .SetDescription("Gunslinger.Buccaneer.SeaDog.Ability.Description")
                .SetIcon(Utilities.MakeIcon("SeaDog.png"))
                .AddConditionImmunity(UnitCondition.DifficultTerrain)
                .Configure();

            AbilityConfigurator.New("SeaDogAbility", SeaDogAbility)
                .SetDisplayName("Gunslinger.Buccaneer.SeaDog.Name")
                .SetDescription("Gunslinger.Buccaneer.SeaDog.Ability.Description")
                .SetIcon(Utilities.MakeIcon("SeaDog.png"))
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuffWithDurationSeconds(SeaDogBuff, 6))
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: Grit.GritResource)
                .Configure();

            FeatureConfigurator.New("SeaDogFeature", SeaDogFeature)
                .SetDisplayName("Gunslinger.Buccaneer.SeaDog.Name")
                .SetDescription("Gunslinger.Buccaneer.SeaDog.Description")
                .SetIcon(Utilities.MakeIcon("SeaDog.png"))
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, stat: Kingmaker.EntitySystem.Stats.StatType.SkillMobility)
                .AddFacts([SeaDogAbility])
                .Configure();
        }

        private static void PirateJargon()
        {
            BuffConfigurator.New("JargonBuff", JargonBuff)
                .SetDisplayName("Gunslinger.Buccaneer.Jargon.Name")
                .SetDescription("Gunslinger.Buccaneer.Jargon.Ability.Description")
                .SetIcon(BlueprintTool.Get<BlueprintBuff>("886c7407dc629dc499b9f1465ff382df").Icon)
                .AddCondition(UnitCondition.Confusion)
                .Configure();
            //not sure if this is right but we'l go with it for now
            AbilityConfigurator.New("JargonAbility", JargonAbility)
                .SetDisplayName("Gunslinger.Buccaneer.Jargon.Name")
                .SetDescription("Gunslinger.Buccaneer.Jargon.Ability.Description")
                .SetIcon(BlueprintTool.Get<BlueprintBuff>("886c7407dc629dc499b9f1465ff382df").Icon)
                .AddContextCalculateAbilityParamsBasedOnClass(Gunslinger.GunslingerClassGUID, statType: Kingmaker.EntitySystem.Stats.StatType.Charisma)
                .AddAbilityEffectRunAction(ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Will, onResult: ActionsBuilder.New().ConditionalSaved(ActionsBuilder.New().ApplyBuffWithDurationSeconds(JargonBuff, 6))))
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Close)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: Grit.GritResource)
                .SetCanTargetEnemies(true)
                .AddSpellDescriptorComponent(new SpellDescriptorWrapper(SpellDescriptor.MindAffecting))
                .AddSpellDescriptorComponent(new SpellDescriptorWrapper(SpellDescriptor.Confusion))
                .Configure();

            FeatureConfigurator.New("JargonFeature", JargonFeature)
                .SetDisplayName("Gunslinger.Buccaneer.Jargon.Name")
                .SetDescription("Gunslinger.Buccaneer.Jargon.Description")
                .SetIcon(BlueprintTool.Get<BlueprintBuff>("886c7407dc629dc499b9f1465ff382df").Icon)//copy from confused
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, stat: Kingmaker.EntitySystem.Stats.StatType.CheckBluff)
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, stat: Kingmaker.EntitySystem.Stats.StatType.CheckIntimidate)
                .AddFacts([SeaDogAbility])
                .Configure();
        }

        private static void Grog()
        {

            BuffConfigurator.New("GrogBuff", GrogBuff)
                .SetIcon(BlueprintTool.Get<BlueprintFeature>("300e212868bca984687c92bcb66d381b").Icon)
                .SetRanks(20)
                .SetStacking(StackingType.Rank)
                .Configure();
        }
    }

    public class Grog : UnitFactComponentDelegate, IUnitAbilityResourceHandler, IResourceAmountBonusHandler, IUnitSubscriber, ISubscriber
    {

        private BlueprintAbilityResource Resource => Grit.GritResource;


        public void CalculateMaxResourceAmount(BlueprintAbilityResource resource, ref int bonus)
        {
            if (base.Fact.Active && resource == Resource)
            {
                bonus += base.Fact.GetRank();
            }
        }
        public override void OnActivate()
        {
            this.Owner.Resources.Restore(Grit.GritResource, 1);
        }

        public override void OnDeactivate()
        {
        }
        public void HandleAbilityResourceChange(UnitEntityData unit, UnitAbilityResource resource, int oldAmount)
        {
            if (unit == this.Owner && resource.Blueprint == Resource)
            {
                int spent = oldAmount - resource.Amount;
                while (spent > 0)
                {
                    this.Owner.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(Buccaneer.GrogBuff)).RemoveRank();
                }
            }
        }
    }
}
