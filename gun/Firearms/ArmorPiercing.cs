using BepInEx.AssemblyPublicizer;
using BepInEx.AssemblyPublicizer;
using global::Kingmaker.Blueprints;
using global::Kingmaker.Blueprints.Facts;
using global::Kingmaker.Blueprints.JsonSystem;
using global::Kingmaker.Designers.Mechanics.Facts;
using global::Kingmaker.Enums;
using global::Kingmaker.PubSubSystem;
using global::Kingmaker.RuleSystem.Rules;
using global::Kingmaker.UnitLogic;
using global::Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace gun.Firearms
{
    //This is used to bypass armor bonus shield bonus and natural armor bonus on the attack since fire arms target touch AC but are otherwise treated as standard ranged attacks for stuff like deadly aim
    [ComponentName("Armor Piercing")]
    [TypeId("cb00ec03dcf641978e4ada52b9805838")]
    public class ArmorPiercing : WeaponEnchantmentLogic, IInitiatorRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            if ((evt.Reason.Ability != null && evt.Reason.Ability.Blueprint.UseCurrentWeaponAsReasonItem && evt.Reason.Caster?.GetFirstWeapon() == base.Owner) || evt.Reason.Item == base.Owner)
            {
                evt.AddModifier(evt.Target.Stats.AC.Touch - evt.Target.Stats.AC.ModifiedValue, base.Fact);//This might work but not sure
            }
        }

        public void OnEventDidTrigger(RuleCalculateAC evt)
        {
        }
    }
}
