using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace gun.Cowgirl
{
    internal static class Secrets
    {
        public static string MonsterTemplateGUID = "a466216a77014e63ab6506dd98a15d27";

        public static void Configure()
        {
            
            FeatureConfigurator TemplateConfig = FeatureConfigurator.New("CowgirlTemplate", MonsterTemplateGUID);
            AddStatBonusPerLevel ACBoost = new AddStatBonusPerLevel();
            ACBoost.Value = 1;
            ACBoost.divider = 2;
            ACBoost.Stat = StatType.AC;
            ACBoost.Descriptor = ModifierDescriptor.NaturalArmor;

            TemplateConfig.AddComponent(ACBoost);

            //BlueprintUnitFactReference aberationref = BlueprintTool.GetRef<BlueprintUnitFactReference>("3bec99efd9a363242a6c8d9957b75e91");

            TemplateConfig.AddSavingThrowBonusAgainstDescriptor(spellDescriptor: SpellDescriptor.MindAffecting, bonus: 4, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Racial);
            TemplateConfig.AddFacts([
                BlueprintTool.GetRef<BlueprintUnitFactReference>("3bec99efd9a363242a6c8d9957b75e91"), //abberation type
                BlueprintTool.GetRef<BlueprintUnitFactReference>("24700a71dd3dc844ea585345f6dd18f6"),//fire resistance 10
                BlueprintTool.GetRef<BlueprintUnitFactReference>("daf27e1f12e736d4294b525489e99de4"),//cold resistance 10
                BlueprintTool.GetRef<BlueprintUnitFactReference>("205205053a2915d4782cf48dc0cc3c09"),//spell res 11+CR
                BlueprintTool.GetRef<BlueprintUnitFactReference>("202af59b918143a4ab7c33d72c8eb6d5"),//poison immunity
                BlueprintTool.GetRef<BlueprintUnitFactReference>("9e0051f85452c0f4689eee2eda041bb3"),//disease immunity
                BlueprintTool.GetRef<BlueprintUnitFactReference>("d09b20029e9abfe4480b356c92095623")//toughness feat
            ]);
            TemplateConfig.AddStatBonus(stat: StatType.Strength, value: 2);
            TemplateConfig.AddStatBonus(stat: StatType.Constitution, value: 4);
            TemplateConfig.AddStatBonus(stat: StatType.Intelligence, value: 4);
            TemplateConfig.AddStatBonus(stat: StatType.Charisma, value: -2);
            TemplateConfig.AddStatBonus(stat: StatType.SkillKnowledgeArcana, value: 4, descriptor: ModifierDescriptor.Racial);
            TemplateConfig.AddClassSkill(StatType.SkillKnowledgeArcana);
            TemplateConfig.AddClassSkill(StatType.SkillKnowledgeWorld);

            TemplateConfig.SetDisplayName(LocalizationTool.GetString("Cowgirl.Secret.Template.Name"));
            TemplateConfig.SetDescription(LocalizationTool.GetString("Cowgirl.Secret.Template.Description"));
            TemplateConfig.SetDescriptionShort(LocalizationTool.GetString("Cowgirl.Secret.Template.Description.Short"));
            TemplateConfig.SetHideInCharacterSheetAndLevelUp(true);
            TemplateConfig.SetHideInUI(true);
            TemplateConfig.Configure();
            //could add some spells but for now will not bother


        }
    }

    public class AddStatBonusPerLevel : UnitFactComponentDelegate<AddStatBonus.ComponentData>
    {
        public class ComponentData
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public PowerfulChangeType PowerfulChange;

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public int PowerfulChangeBonus;
        }

        public ModifierDescriptor Descriptor;

        public StatType Stat;

        public int Value;

        public int divider;//what to divide the level by to calculate the bonus

        public static int TryApplyArcanistPowerfulChange(int value, int? bonus)
        {
            if (value < 0 || !bonus.HasValue)
            {
                return value;
            }

            return value + bonus.Value;
        }

        public static int CalculatePowerfulChangeBonus(EntityFact fact, StatType stat)
        {
            MechanicsContext mechanicsContext = fact?.MaybeContext;
            if (mechanicsContext == null)
            {
                return 0;
            }

            UnitEntityData maybeCaster = mechanicsContext.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing");
                return 0;
            }

            if (!stat.IsAttribute())
            {
                return 0;
            }

            int num = 0;
            AbilityExecutionContext sourceAbilityContext = mechanicsContext.SourceAbilityContext;
            AbilityData abilityData = sourceAbilityContext?.Ability;
            if (abilityData?.Spellbook != null && abilityData.Blueprint.IsSpell && abilityData.Spellbook.Blueprint.IsArcanist && sourceAbilityContext.SpellSchool == SpellSchool.Transmutation)
            {
                if ((bool)maybeCaster.State.Features.PowerfulChange)
                {
                    num += 2;
                }

                if ((bool)maybeCaster.State.Features.ImprovedPowerfulChange)
                {
                    num += 2;
                }
            }

            return num;
        }

        public static int GetLegacySaveBonusValue(PowerfulChangeType? powerfulChangeType)
        {
            if (!powerfulChangeType.HasValue || powerfulChangeType == PowerfulChangeType.None)
            {
                return 0;
            }

            if (powerfulChangeType == PowerfulChangeType.Simple)
            {
                return 2;
            }

            return 4;
        }

        public override void OnActivate()
        {
            int num = CalculatePowerfulChangeBonus(base.Fact, Stat);
            if (num > 0)
            {
                base.Data.PowerfulChangeBonus = num;
            }
        }

        public override void OnTurnOn()
        {
            ModifiableValue stat = base.Owner.Stats.GetStat(Stat);
            int increments = base.Owner.Progression.CharacterLevel / divider;//for a character using class levesl hit die is equal to character level 
            
            int num = Value * increments;
            FixLegacySaveData();
            num = TryApplyArcanistPowerfulChange(num, base.MaybeData?.PowerfulChangeBonus);
            stat?.AddModifierUnique(num, base.Runtime, Descriptor);
        }

        public override void OnTurnOff()
        {
            base.Owner.Stats.GetStat(Stat).RemoveModifiersFrom(base.Runtime);
        }

        public void FixLegacySaveData()
        {
            int legacySaveBonusValue = GetLegacySaveBonusValue(base.MaybeData?.PowerfulChange);
            if (legacySaveBonusValue != 0)
            {
                base.Data.PowerfulChangeBonus = legacySaveBonusValue;
                base.Data.PowerfulChange = PowerfulChangeType.None;
            }
        }
    }
}
