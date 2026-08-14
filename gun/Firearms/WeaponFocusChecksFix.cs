using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace gun.Firearms
{
    public class PenetratingStrikeFirearm : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public bool UseContextValue;

        public int ReductionReduction;

        public ContextValue ReductionPenaltyModifier;
        public PenetratingStrikeFirearm(bool useContext, int Reduction, ContextValue ReductionMod)
        {
            UseContextValue = useContext;
            ReductionReduction = Reduction;
            ReductionPenaltyModifier = ReductionMod;
        }

        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            if (evt.DamageBundle.Weapon == null || evt.DamageBundle.WeaponDamage == null)
            {
                return;
            }
            

            bool flag = false;
            foreach (Feature feature in base.Owner.Progression.Features)
            {
                WeaponFocus focus = feature.GetComponent<WeaponFocus>();
                if (focus != null)
                {
                    flag = focus.WeaponType == evt.DamageBundle.Weapon.Blueprint.Type;
                }
            }

            if (flag)
            {
                int value = (UseContextValue ? ReductionPenaltyModifier.Calculate(base.Context) : ReductionReduction);
                evt.DamageBundle.WeaponDamage.ReductionPenalty.Add(new Modifier(value, base.Fact, ModifierDescriptor.Competence));
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt)
        {
        }
    }

    public class AbilityCasterHasChosenWeaponFix : BlueprintComponent, IAbilityCasterRestriction
    {

        public BlueprintParametrizedFeatureReference m_ChosenWeaponFeature;

        public BlueprintUnitFactReference m_IgnoreWeaponFact;

        public BlueprintParametrizedFeature ChosenWeaponFeature => m_ChosenWeaponFeature?.Get();

        public BlueprintUnitFact IgnoreWeaponFact => m_IgnoreWeaponFact?.Get();

        public int firearmfeatID;

        public AbilityCasterHasChosenWeaponFix(BlueprintParametrizedFeatureReference chosenfeature, int featID, BlueprintUnitFactReference ignorefact)
        {
            m_ChosenWeaponFeature = chosenfeature;
            firearmfeatID = featID;
            m_IgnoreWeaponFact = ignorefact;
        }

        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            if (HasSuitableWeapon(caster.Body.PrimaryHand) || HasSuitableWeapon(caster.Body.SecondaryHand) || caster.HasFact(IgnoreWeaponFact))
            {
                return true;
            }

            foreach (WeaponSlot additionalLimb in caster.Body.AdditionalLimbs)
            {
                if (HasSuitableWeapon(additionalLimb))
                {
                    return true;
                }
            }

            return false;
        }
        public bool HasSuitableWeapon(WeaponSlot slot)
        {
            if (slot.MaybeWeapon == null)
            {
                return false;
            }
            if (slot.Owner.GetFeature(ChosenWeaponFeature, slot.MaybeWeapon.Blueprint.Category) != null)
            {
                return true;
            }
            else
            {//this should add the logic to check for weapon focus on firearms
                if (slot.MaybeWeapon.Blueprint.Category == WeaponCategory.HandCrossbow)
                {
                    switch (slot.MaybeWeapon.Blueprint.Type.AssetGuid.ToString())
                    {
                        case (Musket.WeaponID):
                            return (slot.Owner.GetFeature(BlueprintTool.Get<BlueprintFeature>(Musket.FocusIDs[firearmfeatID])) != null);
                        case (Pistol.WeaponID):
                            return (slot.Owner.GetFeature(BlueprintTool.Get<BlueprintFeature>(Pistol.FocusIDs[firearmfeatID])) != null);
                        case (Revolver.WeaponID):
                            return (slot.Owner.GetFeature(BlueprintTool.Get<BlueprintFeature>(Revolver.FocusIDs[firearmfeatID])) != null);
                        case (Rifle.WeaponID):
                            return (slot.Owner.GetFeature(BlueprintTool.Get<BlueprintFeature>(Rifle.FocusIDs[firearmfeatID])) != null);
                        default:
                            return false;
                    }
                }
            }
            return false;

            
        }

        public string GetAbilityCasterRestrictionUIText()
        {
            return LocalizedTexts.Instance.Reasons.ChosenWeaponRequired;
        }
    }
}
