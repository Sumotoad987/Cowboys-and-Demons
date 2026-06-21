using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using gun.Firearms;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace gun.Classes.Gunslinger
{
    public static class GunTraining
    {
        public const string GunTrainingGUID = "82a28cc03df64b48900bbe487a5ad8dc";
        public static void Configure()
        {
            Kingmaker.UnitLogic.Mechanics.ContextValue TrainingValue = new Kingmaker.UnitLogic.Mechanics.ContextValue();
            FeatureConfigurator.New("GunTraining", GunTrainingGUID)
                .SetDisplayName(LocalizationTool.GetString("GunTraining.Name"))
                .SetDescription(LocalizationTool.GetString("GunTraining.Description"))
                .AddComponent(new GunTrainingDamageBonus())
                .Configure();
        }
    }

    [ComponentName("Gun Training damage bonus")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("43f87721b5e44632a3c42acee172df33")]
    public class GunTrainingDamageBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon.Blueprint.Category == BaseFirearm.FirearmCategory)
            {
                evt.AddDamageModifier(evt.Initiator.Stats.Dexterity.Bonus, base.Fact);
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }

}
