using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Owlcat.Runtime.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace gun.Feats
{
    public class ChangeWeaponDamageType : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, IInitiatorRulebookSubscriber, RuleDealDamage.IOutgoingDamageHandler, IUnitSubscriber
    {
        public DamageTypeDescription Type;
        public WeaponCategory weapons;
        public bool checkCategory;

        public ChangeWeaponDamageType(DamageTypeDescription type, WeaponCategory category, bool needtocheckcategory)
        {
            Type = type;
            weapons = category;
            checkCategory = needtocheckcategory;
        }
        public void OnEventAboutToTrigger(RuleDealDamage evt)
        {
            if (evt.DamageBundle.Weapon == null)//if the damage is not from a weapon
            {
                return;//do nothing
            }
            if (evt.DamageBundle.Weapon.Blueprint.Category != weapons && checkCategory)//if it is not the right type of weapon and we care about that
            {
                return;//also do nothing
            }
            List<BaseDamage> list = TempList.Get<BaseDamage>();
            foreach (BaseDamage item in evt.DamageBundle)
            {
                list.Add(ChangeType(item));
            }

            evt.Remove((BaseDamage _) => true);
            foreach (BaseDamage item2 in list)
            {
                evt.Add(item2);
            }
        }

        public void OnEventDidTrigger(RuleDealDamage evt)
        {
        }

        public void HandleOutgoingDamageWillBeAdded(BaseDamage damage, out BaseDamage damageToAdd)
        {
            damageToAdd = ChangeType(damage);
        }

        
        public BaseDamage ChangeType(BaseDamage damage)
        {
            if (damage.Type == Type.Type)
            {
                switch (damage.Type)
                {
                    case DamageType.Energy:
                        break;
                    case DamageType.Force:
                    case DamageType.Direct:
                    case DamageType.Untyped:
                        return damage;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if ((damage as EnergyDamage)?.EnergyType == Type.Energy)
                {
                    return damage;
                }
            }

            BaseDamage baseDamage = Type.CreateDamage(new ModifiableDiceFormula(damage.Dice), damage.Bonus);
            baseDamage.CopyFrom(damage);
            return baseDamage;
        }
    }
}
