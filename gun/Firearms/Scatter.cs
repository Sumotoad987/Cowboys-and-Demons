using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Crusade.GlobalMagic.Executors;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Kingmaker.EntitySystem.EntityDataBase;

namespace gun.Firearms
{
    internal static class Scatter
    {
        public const string Ability30ft = "4d808e4576da43f8996135b074a11d4d";
        public const string Enchant30ft = "3ca7e18b3cea44e98e59815cdd3967ce";
        public const string Ability15ft = "fffa7bdb4b804d6ab98f6bed2aa2e81f";
        public const string Enchant15ft = "d2d8aa1a795a46aba64043916fa424cd";

        public static void Configure()
        {
            List<BlueprintCore.Utils.Blueprint<Kingmaker.Blueprints.BlueprintProjectileReference>> Bullet = new List<BlueprintCore.Utils.Blueprint<Kingmaker.Blueprints.BlueprintProjectileReference>>();
            Bullet.Add(BlueprintTool.GetRef<BlueprintProjectileReference>(BaseFirearm.BulletGUID));

            AbilityConfigurator.New("ScatterAbility30ft", Ability30ft)
                .AddAbilityDeliverProjectile(length: new Feet(30), lineWidth: new Feet(5), projectiles: Bullet,needAttackRoll:false)
                .AddAbilityEffectRunAction(ActionsBuilder.New().Add(new ScatterAction()))
                .SetDisplayName(LocalizationTool.GetString("Firearms.Scatter.30.Name"))
                .SetDescription(LocalizationTool.GetString("Firearms.Scatter.Description"))
                .SetEffectOnEnemy(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityEffectOnUnit.Harmful)
                .SetEffectOnAlly(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityEffectOnUnit.Harmful)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.Physical)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Projectile)
                .SetCanTargetEnemies()
                .SetCanTargetPoint()
                .SetCanTargetFriends()
                .SetShouldTurnToTarget()
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction()
                .Configure()
                ;
            AbilityConfigurator.New("ScatterAbility15ft", Ability15ft)
                .AddAbilityDeliverProjectile(length: new Feet(15), lineWidth: new Feet(5), projectiles: Bullet, needAttackRoll: false)
                .AddAbilityEffectRunAction(ActionsBuilder.New().Add(new ScatterAction()))
                .SetDisplayName(LocalizationTool.GetString("Firearms.Scatter.15.Name"))
                .SetDescription(LocalizationTool.GetString("Firearms.Scatter.Description"))
                .SetEffectOnEnemy(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityEffectOnUnit.Harmful)
                .SetEffectOnAlly(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityEffectOnUnit.Harmful)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.Physical)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Projectile)
                .SetCanTargetEnemies()
                .SetCanTargetPoint()
                .SetCanTargetFriends()
                .SetShouldTurnToTarget()
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction()
                .Configure()
                ;

            WeaponEnchantmentConfigurator.New("ScatterEnchant30", Enchant30ft)
                .SetEnchantName(LocalizationTool.GetString("Firearms.Scatter.30.Name"))
                .SetDescription(LocalizationTool.GetString("Firearms.Scatter.Description"))
                .AddUnitFactEquipment(BlueprintTool.GetRef<BlueprintUnitFactReference>(Ability30ft))
                .Configure();

            WeaponEnchantmentConfigurator.New("ScatterEnchant15", Enchant15ft)
                .SetEnchantName(LocalizationTool.GetString("Firearms.Scatter.15.Name"))
                .SetDescription(LocalizationTool.GetString("Firearms.Scatter.Description"))
                .AddUnitFactEquipment(BlueprintTool.GetRef<BlueprintUnitFactReference>(Ability15ft))
                .Configure();

        }
    }
    class ScatterAction : ContextAction
    {
        RuleCalculateWeaponStats WeaponStats;
        public override string GetCaption()
        {
            return "Scatter Shot";
        }

        public override void RunAction()
        {
            if (base.Target.Unit == null)
            {
                PFLog.Default.Error("Target unit is missing");
                return;
            }

            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing");
                return;
            }
            ItemEntityWeapon weapon = maybeCaster.GetFirstWeapon();
            int BAB = maybeCaster.Stats.BaseAttackBonus;
            int penalty = 0;
            if (weapon.Blueprint.Enchantments.Any((BlueprintItemEnchantment e) => {
                return e == BlueprintTool.Get<BlueprintItemEnchantment>(BaseFirearm.ClipGUID);
            }))
            {
                MakeAttack(maybeCaster, weapon, base.Target.Unit, 0);
            }
            else
            {
                while (BAB - penalty > 0 || penalty == 0)
                {
                    MakeAttack(maybeCaster, weapon, base.Target.Unit, penalty + 2);
                    penalty = penalty + 5;
                }
            }
        }

        public void MakeAttack(UnitEntityData Initiator, ItemEntityWeapon weapon, UnitEntityData Target, int AttackBonusPenalty)
        {

            WeaponStats = new RuleCalculateWeaponStats(Initiator, weapon, Kingmaker.Items.Slots.LimbType.PrimaryHand);
            Rulebook.Trigger(WeaponStats);
            RuleAttackRoll AttackRoll = new RuleAttackRoll(Initiator, Target, WeaponStats, AttackBonusPenalty)
            {
                AutoHit = false,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = (TacticalCombatHelper.IsActive || false),
                SuspendCombatLog = true,
                DoNotProvokeAttacksOfOpportunity = true,
                ForceFlatFooted = false,
                IgnoreConcealment = true

            };

            Rulebook.Trigger(AttackRoll);
            if (AttackRoll.IsHit)
            {
                RuleDealDamage ruleDealDamage = CreateRuleDealDamage(TacticalCombatHelper.IsActive,AttackRoll);
                if (ruleDealDamage.DamageBundle != null)
                {
                    Rulebook.Trigger(ruleDealDamage);
                }
            }
        }

        public RuleDealDamage CreateRuleDealDamage(bool enablePrecisionAndCritical, RuleAttackRoll AttackRoll)
        {
            DamageBundle damage = CreateDamage(enablePrecisionAndCritical,AttackRoll);
            return new RuleDealDamage(base.Context.MaybeCaster, base.Target.Unit, damage)
            {
                DisablePrecisionDamage = true,
                AttackRoll = AttackRoll
            };
        }

        public DamageBundle CreateDamage(bool enablePrecisionAndCritical,RuleAttackRoll AttackRoll)
        {
            DamageBundle damageBundle = null;
            for (int i = 0; i < WeaponStats.DamageDescription.Count; i++)
            {
                BaseDamage damage = WeaponStats.DamageDescription[i].CreateDamage();
                if (damageBundle == null)
                {
                    BaseDamage baseDamage = WeaponStats.DamageDescription[i].CreateDamage();
                    if (enablePrecisionAndCritical && AttackRoll.IsCriticalConfirmed)
                    {
                        baseDamage.CriticalModifier = ((!(Math.Abs(WeaponStats.TacticalCriticalModifier - 1f) < Mathf.Epsilon)) ? 1 : WeaponStats.CriticalMultiplier);
                        baseDamage.AdditionalCriticalMultiplier = WeaponStats.AdditionalCriticalMultiplier;
                        baseDamage.TacticalCriticalModifier = WeaponStats.TacticalCriticalModifier;
                    }

                    damageBundle = new DamageBundle(base.Context.MaybeCaster.GetFirstWeapon(), baseDamage);
                }
                else
                {
                    damageBundle.Add(damage);
                }
            }

            return damageBundle;
        }

    }
}
