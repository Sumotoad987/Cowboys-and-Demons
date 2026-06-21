using BepInEx.AssemblyPublicizer;
using BepInEx.AssemblyPublicizer;
using BlueprintCore.Utils;
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
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace gun.Firearms
{
    //This is used to make sure the user cannot fire the weapon if they have no rounds available and subtracts one round after fireing
    [ComponentName("Empty Clip Check")]
    [TypeId("d007f71f4a7040c3ab5bbadc601d51c8")]
    public class EmptyClipCheck : WeaponEnchantmentLogic, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if ((evt.Reason.Ability != null && evt.Reason.Ability.Blueprint.UseCurrentWeaponAsReasonItem && evt.Reason.Caster?.GetFirstWeapon() == base.Owner) || evt.Reason.Item == base.Owner)
            {
                if (evt.Initiator.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(BaseFirearm.RoundsGUID)) == null)//if the firer has no stacks of the rounds buff (used to track ammo)
                {
                    evt.SkipMainDamage = true;  //the weapon will do no damage seems to be the only solution I have so far 
                }
                else
                {//if there are rounds 
                    evt.Initiator.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(BaseFirearm.RoundsGUID)).Remove();//then remove one round
                    Main.Log.Log("Removed Ammo");
                    FirearmFix.Attack(new RulebookEventContext(), evt);
                    return;
                }
            }
        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
        }

    }

    public class AdvancedClipCheck : WeaponEnchantmentLogic, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if ((evt.Reason.Ability != null && evt.Reason.Ability.Blueprint.UseCurrentWeaponAsReasonItem && evt.Reason.Caster?.GetFirstWeapon() == base.Owner) || evt.Reason.Item == base.Owner)
            {
                Buff Rounds = evt.Initiator.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(BaseFirearm.RoundsGUID));
                if (evt.Initiator.GetFeature(BlueprintTool.Get<BlueprintFeature>(RapidReload.RapidReloadGUID)) == null)//if the user doesn't have rapid reload we check for ammo otherwise we just fire away
                {
                    if (Rounds == null)//if the firer has no stacks of the rounds buff (used to track ammo)
                    {
                        evt.AttackRoll.AutoMiss = true;  //the weapon will automatically miss if there is no ammo
                                                 
                    }
                    else
                    {//if there are rounds 
                        Rounds.Remove();//then remove one round
                    };
                }
                
            }
            FirearmFix.Attack(new RulebookEventContext(), evt);
        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
        }
    }

    public class HasLoadedGun: BlueprintComponent, IAbilityCasterRestriction
    {
        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            bool hasAmmo = caster.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(BaseFirearm.RoundsGUID)) != null;
            bool hasGun = caster.GetFirstWeapon().Blueprint.Category == BaseFirearm.FirearmCategory;
            hasGun = hasGun || caster.GetFirstWeapon().Blueprint.Category == BaseFirearm.FirearmCategory;
            if (hasGun) 
            {//if the caster has a gun
                if (hasAmmo)
                {//and ammo 
                    return true;//return true
                }
                else
                {//if they don't have ammo
                    bool hasRapidReload = caster.GetFeature(BlueprintTool.Get<BlueprintFeature>(RapidReload.RapidReloadGUID)) != null;
                    bool isAdvanced = caster.GetFirstWeapon().Blueprint.Enchantments.Any(predicate: Advancedcheck);//true if the weapon has the advanced clip enhancement
                    if (isAdvanced && hasRapidReload)//if the gun is an advanced weapon and they have rapid reload then they don't need ammo
                    {//so they are treated as having a loaded gun
                        return true;
                    }
                    else 
                    {//if that's not true the gun isn't loaded so action can't be taken
                        return false;
                    }
                }
            }//if they don't have a gun      
            return false;
        }

        private bool Advancedcheck (BlueprintItemEnchantment enchantment)
        {
            return enchantment.AssetGuid == BaseFirearm.AdvancedClipGUID;//true if the enhancement is a advanced clip
        }

        public string GetAbilityCasterRestrictionUIText()
        {
            return "Requires a loaded gun";
        }
    }
}
