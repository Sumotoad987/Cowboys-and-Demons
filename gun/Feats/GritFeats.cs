using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using gun.Classes.Gunslinger;
using gun.Deeds;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gun.Feats
{
    internal static class GritFeats
    {

        public const string BlowoutShotAbility = "a8eef3424bf7416f8737a6d4fc384c7d";
        public const string BlowoutShotBuff = "ce03c5ed0d564bf49df59e805dd5d451";
        public const string BlowoutShotFeat = "9dc06bb0c680434d93ef2e0f0f2722c6";
        public static string[] DragonShotBuffs = {
            "76d533af8ab94e6ab3dc696c18509235",
            "31cc21ffa9eb4f61ad20bcd4fcd0fe58",
            "2c8bc17ef2d74663ab1ba2df2f37be52",
            "11f27012091d48f2a9f0b9bee6a480a0"
        };
        public const string DragonShotFeat = "bf17beaa834f4b9c9bd74a374807a217";
        public static string[] DragonShotAbility = {
            "3b7317de539443018a7eba78809fc83a",
            "8d4581d7ca0747a4a7a5227be57f6853",
            "d2221aa50ed84247805b2d4bf310e219",
            "0766ac9043a340df8f96c16b8309805c",
            "de5a24eaec1a4ae6a87f0dfd25db6bca"
        };
        public const string ExtraGrit = "5b2bf2bf37bb47269aaef49b09af3575";

        public static void Configure()
        {
            BlowoutShot();
            DragonShot();
            ExtraGritSetup();
        }

        //Blowout Shot
        //activatable ability that requires 1 grit
        //on a shot spends 1 grit to trigger
        //you move back 5 ft
        //target is makes save or is pushed 10ft
        //https://aonprd.com/FeatDisplay.aspx?ItemName=Blowout%20Shot%20Deed
        private static void BlowoutShot()
        {//not yet compatible with scatter shot (will only push one target)

            BuffConfigurator.New("BlowoutShotBuff", BlowoutShotBuff)
                .AddComponent(new BlowoutShot())
                .SetDisplayName(LocalizationTool.GetString("Feats.BlowoutShot.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.BlowoutShot.Description"))
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("7ab6f70c996fe9b4597b8332f0a3af5f").Icon)//copy the icon from bull rush
                .Configure();

            ActivatableAbilityConfigurator.New("BlowoutShot", BlowoutShotAbility)
                .SetBuff(BlueprintTool.GetRef<BlueprintBuffReference>(BlowoutShotBuff))
                .SetDisplayName(LocalizationTool.GetString("Feats.BlowoutShot.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.BlowoutShot.Description"))
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("7ab6f70c996fe9b4597b8332f0a3af5f").Icon)//copy the icon from bull rush
                .Configure();

            FeatureConfigurator.New("BlowoutShotFeat", BlowoutShotFeat, Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat, FeatureGroup.Feat)
                .SetDisplayName(LocalizationTool.GetString("Feats.BlowoutShot.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.BlowoutShot.Description"))
                .AddFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(BlowoutShotAbility)])
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("7ab6f70c996fe9b4597b8332f0a3af5f").Icon)
                .AddPrerequisiteFeaturesFromList([AmateurGunslinger.BaseGUID,AmateurGunslinger.DrifterGUID,Grit.GritFeatureGUID],1,true)//hope this is right
                .Configure();
        }



        //Dragon shot
        //swift action
        //costs 1 grit
        //has variants for each damage type
        //changes the damage you deal with weapon attacks to the chosen type
        //https://aonprd.com/FeatDisplay.aspx?ItemName=Dragon%20Shot

        private static void DragonShot()
        {
            DamageTypeDescription Acid = new DamageTypeDescription();
            Acid.Type = DamageType.Energy;
            Acid.Energy = Kingmaker.Enums.Damage.DamageEnergyType.Acid;
            BuffConfigurator.New("DragonShotAcidBuff", DragonShotBuffs[0])
                .AddComponent(new ChangeWeaponDamageType(Acid,Kingmaker.Enums.WeaponCategory.HandCrossbow,true))//hand crossbow is used for firearms
                .SetDisplayName(LocalizationTool.GetString("Feats.DragonShot.Acid.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.DragonShot.Description"))
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("ad84149c307247a381f17cc42acc38fa").Icon)//copy the icon from elemental fist Acid
                .Configure();

            AbilityConfigurator.New("DragonShotAcid", DragonShotAbility[0])
                .SetDisplayName(LocalizationTool.GetString("Feats.DragonShot.Acid.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.DragonShot.Description"))
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("ad84149c307247a381f17cc42acc38fa").Icon)//copy the icon from elemental fist acid
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuffWithDurationSeconds(DragonShotBuffs[0], 6))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: Grit.GritResource)
                .AddAbilityCasterHasNoFacts([DragonShotBuffs[0], DragonShotBuffs[1], DragonShotBuffs[2], DragonShotBuffs[3]])
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();





            DamageTypeDescription Fire = new DamageTypeDescription();
            Fire.Type = DamageType.Energy;
            Fire.Energy = Kingmaker.Enums.Damage.DamageEnergyType.Fire;
            BuffConfigurator.New("DragonShotFireBuff", DragonShotBuffs[1])
                .AddComponent(new ChangeWeaponDamageType(Fire, Kingmaker.Enums.WeaponCategory.HandCrossbow, true))//hand crossbow is used for firearms
                .SetDisplayName(LocalizationTool.GetString("Feats.DragonShot.Fire.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.DragonShot.Description"))
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("13e0e8560db9471aa7c0ff18be3e633b").Icon)//copy the icon from elemental fist Fire
                .Configure();

            AbilityConfigurator.New("DragonShotFire", DragonShotAbility[1])
                .SetDisplayName(LocalizationTool.GetString("Feats.DragonShot.Fire.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.DragonShot.Description"))
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("13e0e8560db9471aa7c0ff18be3e633b").Icon)//copy the icon from elemental fist fire
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuffWithDurationSeconds(DragonShotBuffs[1], 6))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: Grit.GritResource)
                .AddAbilityCasterHasNoFacts([DragonShotBuffs[0], DragonShotBuffs[1], DragonShotBuffs[2], DragonShotBuffs[3]])
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();



            DamageTypeDescription Electricity = new DamageTypeDescription();
            Fire.Type = DamageType.Energy;
            Fire.Energy = Kingmaker.Enums.Damage.DamageEnergyType.Electricity;
            BuffConfigurator.New("DragonShotElectricityBuff", DragonShotBuffs[2])
                .AddComponent(new ChangeWeaponDamageType(Electricity, Kingmaker.Enums.WeaponCategory.HandCrossbow, true))//hand crossbow is used for firearms
                .SetDisplayName(LocalizationTool.GetString("Feats.DragonShot.Electricity.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.DragonShot.Description"))
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("c5a35842eaf1409d892ca0ce9ad79063").Icon)//copy the icon from elemental fist Electricity
                .Configure();

            AbilityConfigurator.New("DragonShotElectricity", DragonShotAbility[2])
                .SetDisplayName(LocalizationTool.GetString("Feats.DragonShot.Electricity.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.DragonShot.Description"))
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("c5a35842eaf1409d892ca0ce9ad79063").Icon)//copy the icon from elemental fist electricity
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuffWithDurationSeconds(DragonShotBuffs[2], 6))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: Grit.GritResource)
                .AddAbilityCasterHasNoFacts([DragonShotBuffs[0], DragonShotBuffs[1], DragonShotBuffs[2], DragonShotBuffs[3]])
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();




            DamageTypeDescription Cold = new DamageTypeDescription();
            Cold.Type = DamageType.Energy;
            Cold.Energy = Kingmaker.Enums.Damage.DamageEnergyType.Cold;
            BuffConfigurator.New("DragonShotColdBuff", DragonShotBuffs[3])
                .AddComponent(new ChangeWeaponDamageType(Cold, Kingmaker.Enums.WeaponCategory.HandCrossbow, true))//hand crossbow is used for firearms
                .SetDisplayName(LocalizationTool.GetString("Feats.DragonShot.Cold.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.DragonShot.Description"))
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("d33e0b20052b407e98db4ec022d23fdb").Icon)//copy the icon from elemental fist Cold
                .Configure();

            AbilityConfigurator.New("DragonShotCold", DragonShotAbility[3])
                .SetDisplayName(LocalizationTool.GetString("Feats.DragonShot.Cold.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.DragonShot.Description"))
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("c5a35842eaf1409d892ca0ce9ad79063").Icon)//copy the icon from elemental fist cold
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuffWithDurationSeconds(DragonShotBuffs[3], 6))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: Grit.GritResource)
                .AddAbilityCasterHasNoFacts([DragonShotBuffs[0], DragonShotBuffs[1], DragonShotBuffs[2], DragonShotBuffs[3]])
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            AbilityConfigurator.New("DragonShot", DragonShotAbility[4])
                .SetDisplayName(LocalizationTool.GetString("Feats.DragonShot.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.DragonShot.Description"))
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("5d7c3a3eed0546a598e3d2a1c7e0026a").Icon)//copy the icon from elemental fist
                .AddAbilityVariants([DragonShotAbility[0], DragonShotAbility[1], DragonShotAbility[2], DragonShotAbility[3]])
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();



            FeatureConfigurator.New("DragonShotFeat", DragonShotFeat, Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat, FeatureGroup.Feat)
                .SetDisplayName(LocalizationTool.GetString("Feats.DragonShot.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.DragonShot.Description"))
                .AddFacts([BlueprintTool.GetRef<BlueprintUnitFactReference>(DragonShotAbility[4])])
                .SetIcon(BlueprintTool.Get<BlueprintAbility>("5d7c3a3eed0546a598e3d2a1c7e0026a").Icon)//copy the icon from elemental fist
                .AddPrerequisiteFeaturesFromList([AmateurGunslinger.BaseGUID, AmateurGunslinger.DrifterGUID, Grit.GritFeatureGUID], 1, true)//hope this is right
                .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana,5)
                .Configure();
        }

        //Extra Grit
        //https://aonprd.com/FeatDisplay.aspx?ItemName=Extra%20Grit
        //Gain 2 more grit points stacks and can be taken multiple times
        private static void ExtraGritSetup()
        {
            FeatureConfigurator.New("ExtraGrit", ExtraGrit, Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat, FeatureGroup.Feat)
                .SetDisplayName(LocalizationTool.GetString("Feats.ExtraGrit.Name"))
                .SetDescription(LocalizationTool.GetString("Feats.ExtraGrit.Description"))
                .AddIncreaseResourceAmount(Grit.GritResource,2)
                .SetRanks(20)//I think this means it can be selected multiple times
                .AddPrerequisiteFeaturesFromList([AmateurGunslinger.BaseGUID, AmateurGunslinger.DrifterGUID, Grit.GritFeatureGUID], 1, true)//hope this is right
                .Configure();
        }
        

        //Mythic Feat: Reserve Grit
        //Custom feat which allows all deeds which case about you only having grit but don't spend it to work even on 0 grit

        //Recall Ammunition
        //may or may not do this one, its an actiaable which on a miss spends 2 grit and grants 1 ammo
        //https://aonprd.com/FeatDisplay.aspx?ItemName=Recall%20Ammunition

        //Ricochet Shot Deed
        //Simplify this to costs 1 grit per attack as activatable to ignore concealment
        //https://aonprd.com/FeatDisplay.aspx?ItemName=Ricochet%20Shot%20Deed

        //Signature Deed
        //Somehow saves you a point of grit when you use the chosen deed needs you to have 1 grit in the bank
        //https://aonprd.com/FeatDisplay.aspx?ItemName=Signature%20Deed






    }
}
