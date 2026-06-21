using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
namespace gun.Firearms
{

    //
    // Summary:
    //     This is the logic for a weapon with the misfire enhacement to handle missing within misfire range and exploding if damaged

    public class Misfire : WeaponEnchantmentLogic, IInitiatorRulebookHandler<Kingmaker.RuleSystem.Rules.RuleAttackWithWeaponResolve>, IRulebookHandler<Kingmaker.RuleSystem.Rules.RuleAttackWithWeaponResolve>, ISubscriber, IInitiatorRulebookSubscriber
    {
        int Range;
        Feet Radius;
        bool explodes;
        public Misfire(int RangeIn, Feet RadiusIn, bool explodeIn = true)
        {
            Range = RangeIn;
            Radius = RadiusIn;
            explodes = explodeIn;
        }

        public void OnEventAboutToTrigger(RuleAttackWithWeaponResolve evt)
        {

        }

        public void OnEventDidTrigger(RuleAttackWithWeaponResolve evt)
        {
            if ((evt.Reason.Ability != null && evt.Reason.Ability.Blueprint.UseCurrentWeaponAsReasonItem && evt.Reason.Caster?.GetFirstWeapon() == base.Owner) || evt.Reason.Item == base.Owner)
            {
                int tempRange = Range;
                if (evt.Initiator.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(DamagedFirearm.DamagedFirearmGUID)) != null)//if the user has a damaged firearm
                {
                    tempRange += 2;
                }
                if (evt.AttackRoll.Roll <= tempRange)//if we are within the missfire range
                {
                    if (evt.Initiator.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(DamagedFirearm.DamagedFirearmGUID)) != null)//if the user has a damaged firearm
                    {
                        if (explodes)//advanced firearms don't explode so will not trigger this
                        {
                            //blow up
                            foreach (UnitEntityData item in GameHelper.GetTargetsAround(evt.Initiator.Position, Radius))//find all targets in the misfire explosion radius around the target
                            {
                                RuleSavingThrow misfireSave = new RuleSavingThrow(item, SavingThrowType.Reflex, 12);//make a dc 12 reflex save
                                Rulebook.Trigger(misfireSave);//roll the save
                                if (!(misfireSave.Success && misfireSave.ImprovedEvasion))//if the target succeeds and has improved evasion do nothing otherwise
                                {
                                    //deal misfire damage
                                    DealMisfireDamage(evt.Initiator, item, evt.AttackWithWeapon.Weapon, (misfireSave.Success || misfireSave.Evasion));
                                }
                            }
                        }
                    }
                    else//otherwise
                    {
                        //damage the firearm
                        evt.Initiator.AddBuff(BlueprintTool.Get<BlueprintBuff>(DamagedFirearm.DamagedFirearmGUID), evt.Initiator,System.TimeSpan.FromHours(1));
                    }
                }
            }
        }

        public void DealMisfireDamage(UnitEntityData user, UnitEntityData target, ItemEntityWeapon weapon, bool half)
        {
            RuleCalculateWeaponStats ruleCalculateWeaponStats = new RuleCalculateWeaponStats(user, weapon);
            Rulebook.Trigger(ruleCalculateWeaponStats);
            DamageBundle damageBundle = null;
            foreach (DamageDescription item in ruleCalculateWeaponStats.DamageDescription)
            {
                BaseDamage damage = item.CreateDamage();

                damage.Half = new ValueWithSource<bool>(half);//hopefully this will half damage on sucessful save
                if (damageBundle == null)
                {
                    damageBundle = new DamageBundle(weapon, item.CreateDamage());
                }
                else
                {
                    damageBundle.Add(item.CreateDamage());
                }
            }
            if (damageBundle != null)
            {
                Rulebook.Trigger(new RuleDealDamage(user, target, damageBundle));
            }
        }
    }

    //
    // Summary:
    //     This sets up the Damaged Firearm condition that can be applied to a character when they misfire as well as housing the associated GUIDs

    public static class DamagedFirearm
    {
        public const string DamagedFirearmGUID = "7357576705e948a59e90e8653871762c";
        public const string MisfireExplodeGUID = "9d956b241087425da79fe8cb6e765b4e";

        //
        // Summary:
        //     This sets up the Damaged Firearm condition that can be applied to a character when they misfire.
        public static void Configure()
        {
            BuffConfigurator.New("DamagedFirearm", DamagedFirearmGUID)
                .SetDescription(LocalizationTool.GetString("DamagedFirearm.Description"))
                .SetDisplayName(LocalizationTool.GetString("DamagedFirearm.Name"))
                ;
        }
    }

    public class HasDamagedFirearm : BlueprintComponent, IAbilityCasterRestriction
    {
        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            return true;// (caster.GetFeature(BlueprintTool.Get<BlueprintFeature>(DamagedFirearm.DamagedFirearmGUID)) != null);
        }

        public string GetAbilityCasterRestrictionUIText()
        {
            return "Requires Damaged Firearm";
        }
    }

    //
    // Summary:
    //     This class defines the "weapon enhancements" associated with the misfire mechanic and hosts the GUIDs used to reference them elsewhere
    public static class MisfireEnhancement
    {
        public const string Misfire1_5 = "e5764b859a424a309df4c3176bbaa118";
        public const string Misfire12_5 = "3bd12b32f2fc4fc0a765d7c697b53659";
        public const string Misfire12_10 = "5aba229ccb5e4bc0aab9061393e661e6";
        public const string Misfire1_A = "e28e3d5fb6a24d4ab9a3bd4f977479e2";
        public const string Misfire12_A = "1d2a36a00e154e688259121eadd71014";

        //
        // Summary:
        //     This function defines the "weapon enhancements" for misfire
        public static void Configure()
        {
            DamagedFirearm.Configure();
            WeaponEnchantmentConfigurator.New("Misfire1_5", Misfire1_5)
                .SetDescription(LocalizationTool.GetString("Misfire.Early.Description"))
                .SetEnchantName(LocalizationTool.GetString("Misfire.1.5.Name"))
                .AddComponent(new Misfire(1, FeetExtension.Feet(5)))
                .Configure();
            WeaponEnchantmentConfigurator.New("Misfire12_5", Misfire12_5)
                .SetDescription(LocalizationTool.GetString("Misfire.Early.Description"))
                .SetEnchantName(LocalizationTool.GetString("Misfire.12.5.Name"))
                .AddComponent(new Misfire(2, FeetExtension.Feet(5)))
                .Configure();
            WeaponEnchantmentConfigurator.New("Misfire12_10", Misfire12_10)
                .SetDescription(LocalizationTool.GetString("Misfire.Early.Description"))
                .SetEnchantName(LocalizationTool.GetString("Misfire.12.10.Name"))
                .AddComponent(new Misfire(2, FeetExtension.Feet(10)))
                .Configure();
            WeaponEnchantmentConfigurator.New("Misfire1_A", Misfire1_A)
                .SetDescription(LocalizationTool.GetString("Misfire.Advanced.Description"))
                .SetEnchantName(LocalizationTool.GetString("Misfire.1.A.Name"))
                .AddComponent(new Misfire(1, FeetExtension.Feet(0), false))
                .Configure();
            WeaponEnchantmentConfigurator.New("Misfire12_A", Misfire12_A)
                .SetDescription(LocalizationTool.GetString("Misfire.Advanced.Description"))
                .SetEnchantName(LocalizationTool.GetString("Misfire.12.A.Name"))
                .AddComponent(new Misfire(2, FeetExtension.Feet(0), false))
                .Configure();
        }
    }
}

