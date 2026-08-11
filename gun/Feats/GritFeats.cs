using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using gun.Deeds;
using Kingmaker.Blueprints;
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

        public const string BlowoutShotAbility = "";
        public const string BlowoutShotBuff = "";


        public static void Configure()
        {
           
        }


        public static void BlowoutShot()
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
        }

        //Blowout Shot
        //activatable ability that requires 1 grit
        //on a shot spends 1 grit to trigger
        //you move back 5 ft
        //target is makes save or is pushed 10ft
        //https://aonprd.com/FeatDisplay.aspx?ItemName=Blowout%20Shot%20Deed

        //Dragon shot
        //swift action
        //costs 1 grit
        //has variants for each damage type
        //changes the damage you deal with weapon attacks to the chosen type
        //https://aonprd.com/FeatDisplay.aspx?ItemName=Dragon%20Shot

        //Extra Grit
        //https://aonprd.com/FeatDisplay.aspx?ItemName=Extra%20Grit
        //Gain 2 more grit points stacks and can be taken multiple times

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

        //Sizling Shot
        //If you have one grit can be activated to deal have your damage as fire (this just seems bad will probably ignore this part)
        //can spend 1 grit to grant weapon flaming rune for 1 attack (could I say for a full attack)





    }
}
