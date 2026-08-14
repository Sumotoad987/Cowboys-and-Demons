using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using HarmonyLib;
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
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.View.Animation;
using Kingmaker.View.Equipment;
using Kingmaker.Visual.Animation;
using Kingmaker.Visual.Animation.Kingmaker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            ContextDurationValue duration = new ContextDurationValue();
            duration.Rate = DurationRate.Rounds;
            duration.DiceType = DiceType.Zero;
            duration.DiceCountValue = new ContextValue();
            duration.BonusValue = new ContextValue();
            duration.BonusValue.Value = 1;
            AbilityConfigurator.New("ScatterAbility30ft", Ability30ft)
                .AddAbilityDeliverProjectile(length: new Feet(30), lineWidth: new Feet(5), projectiles: Bullet,needAttackRoll:false,type: Kingmaker.UnitLogic.Abilities.Components.AbilityProjectileType.Cone)
                .AddAbilityEffectRunAction(ActionsBuilder.New().Add(new ScatterAction()))
                .AddAbilityExecuteActionOnCast(ActionsBuilder.New().Add(new ScatterAnim()).Conditional(ConditionsBuilder.New().HasBuff(BaseFirearm.RoundsGUID),ifTrue:ActionsBuilder.New().ReduceBuffDuration(duration,BaseFirearm.RoundsGUID)))//hoping this sets all amoo to go away at end of round
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
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction()
                .SetUseCurrentWeaponAsReasonItem()
                .SetIcon(Utilities.MakeIcon("ScatterShot.png"))
                .Configure()
                ;
            AbilityConfigurator.New("ScatterAbility15ft", Ability15ft)
                .AddAbilityDeliverProjectile(length: new Feet(15), lineWidth: new Feet(5), projectiles: Bullet, needAttackRoll: false, type: Kingmaker.UnitLogic.Abilities.Components.AbilityProjectileType.Cone)
                .AddAbilityEffectRunAction(ActionsBuilder.New().Add(new ScatterAction()))
                .AddAbilityExecuteActionOnCast(ActionsBuilder.New().Add(new ScatterAnim()).Conditional(ConditionsBuilder.New().HasBuff(BaseFirearm.RoundsGUID), ifTrue: ActionsBuilder.New().ReduceBuffDuration(duration, BaseFirearm.RoundsGUID)))
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
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction()
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetUseCurrentWeaponAsReasonItem()
                .SetIcon(Utilities.MakeIcon("ScatterShot.png"))
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
            int num = 1;
            if (ReloadSpeedCalc.Calculate(maybeCaster) == -1)
            {//if we have free action reload make as many attacks as BAB allows
                while (BAB - penalty > 0 || penalty == 0)
                {
                    MakeAttack(maybeCaster, weapon, base.Target.Unit, penalty + 2,num);
                    penalty = penalty + 5;
                    Main.Log.Log("Full attack varient");
                    num++;
                }
                
            }
            else
            {//otherwise make as many attacks as rounds in the clip or up to BAB whichever comes first
               
                Buff buff = maybeCaster.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(BaseFirearm.RoundsGUID));
                if (buff != null)
                {
                    int rounds = buff.Rank;
                    while ((BAB - penalty > 0 || penalty == 0) && rounds > 0)
                    {

                        MakeAttack(maybeCaster, weapon, base.Target.Unit, penalty + 2, num);
                        penalty = penalty + 5;
                        Main.Log.Log("Full attack varient");
                        rounds--;
                        num++;
                    }
                }
            }
        }
        
        public async Task MakeAttack(UnitEntityData Initiator, ItemEntityWeapon weapon, UnitEntityData Target, int AttackBonusPenalty, int num)
        {
            //yield return new WaitForSeconds(5);
            await Task.Delay(1000 * num);
            WeaponStats = new RuleCalculateWeaponStats(Initiator, weapon, Kingmaker.Items.Slots.LimbType.PrimaryHand);
            Rulebook.Trigger(WeaponStats);
            RuleAttackRoll AttackRoll = new RuleAttackRoll(Initiator, Target, WeaponStats, AttackBonusPenalty)
            {
                AutoHit = false,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = (TacticalCombatHelper.IsActive || false),
                SuspendCombatLog = false,
                DoNotProvokeAttacksOfOpportunity = true,
                ForceFlatFooted = false,
                IgnoreConcealment = true

            };

            Rulebook.Trigger(AttackRoll);
            if (AttackRoll.IsHit)
            {
                Main.Log.Log("Attack Hit");
                RuleDealDamage ruleDealDamage = CreateRuleDealDamage(Initiator, Target, true, AttackRoll);
                Main.Log.Log("Created Damage");
                if (ruleDealDamage.DamageBundle != null)
                {
                    Rulebook.Trigger(ruleDealDamage);
                    Main.Log.Log("Dealt Damage");
                }
            }
        }

        public RuleDealDamage CreateRuleDealDamage(UnitEntityData Initiator, UnitEntityData Target, bool enablePrecisionAndCritical, RuleAttackRoll AttackRoll)
        {
            DamageBundle damage = CreateDamage(Initiator, enablePrecisionAndCritical, AttackRoll);
            return new RuleDealDamage(Initiator, Target, damage)
            {
                DisablePrecisionDamage = true,
                AttackRoll = AttackRoll
            };
        }

        public DamageBundle CreateDamage(UnitEntityData Initiator, bool enablePrecisionAndCritical, RuleAttackRoll AttackRoll)
        {
            DamageBundle damageBundle = null;
            for (int i = 0; i < WeaponStats.DamageDescription.Count; i++)
            {
                BaseDamage damage = WeaponStats.DamageDescription[i].CreateDamage();
                if (AttackRoll.IsCriticalConfirmed)
                {
                    damage.CriticalModifier = ((!(Math.Abs(WeaponStats.TacticalCriticalModifier - 1f) < Mathf.Epsilon)) ? 1 : WeaponStats.CriticalMultiplier);
                    damage.AdditionalCriticalMultiplier = WeaponStats.AdditionalCriticalMultiplier;
                    damage.TacticalCriticalModifier = WeaponStats.TacticalCriticalModifier;
                }
                if (damageBundle == null)
                {                   
                    damageBundle = new DamageBundle(Initiator.GetFirstWeapon(), damage);
                }
                else
                {
                    damageBundle.Add(damage);
                }
            }

            return damageBundle;
        }

    }

    class ScatterAnim : ContextAction
    {
        public override string GetCaption()
        {
            return "Scatter Animation";
        }
        public override void RunAction()
        {
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing");
                return;
            }
            ItemEntityWeapon weapon = maybeCaster.GetFirstWeapon();
            int BAB = maybeCaster.Stats.BaseAttackBonus;
            int penalty = 0;
            if (ReloadSpeedCalc.Calculate(maybeCaster) == -1)
            {//if thay have free action reload fire as many times as BAB allows
                while (BAB - penalty > 0 || penalty == 0)
                {
                    UnitAnimationActionHandle handle = maybeCaster.View.AnimationManager.CreateHandle(UnitAnimationType.MainHandAttack);//then play the attack anim
                    handle.AttackWeaponStyle = WeaponAnimationStyle.Crossbow;
                    maybeCaster.View.AnimationManager.Execute(handle);
                    penalty = penalty + 5;
                }
                


            }
            else
            {//otherwise make attacks until no more rounds or BAB limit whichever comes first
                Buff buff = maybeCaster.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(BaseFirearm.RoundsGUID));
                if (buff != null)
                {
                    int rounds = buff.Rank;
                    while ((BAB - penalty > 0 || penalty == 0) && rounds > 0)
                    {
                        UnitAnimationActionHandle handle = maybeCaster.View.AnimationManager.CreateHandle(UnitAnimationType.MainHandAttack);//then play the attack anim
                        handle.AttackWeaponStyle = WeaponAnimationStyle.Crossbow;
                        Main.Log.Log(handle.CastingTime.ToString());
                        maybeCaster.View.AnimationManager.Execute(handle);
                        penalty = penalty + 5;
                        rounds--;
                    }
                }
                
            }
        }
    }
}
