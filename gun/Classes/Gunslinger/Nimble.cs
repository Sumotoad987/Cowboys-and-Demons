using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gun.Classes.Gunslinger
{
    internal static class Nimble
    {
        const string NimbleBuffGUID = "8f98c894661543689252645a8d757d15";
        const string NimbleGUID = "3a70483f08cc4a1691ca8d7d6e87ae97";
        public const string NimbleUnlockGUID = "d87ec12eaba048958e339f6acf7a1f10";

        
        public static void Configure()//Setup the Nimble Feature and all its associated parts.
        {
            Sprite icon = BlueprintTool.Get<BlueprintFeature>("3c08d842e802c3e4eb19d15496145709").Icon;//steal the icon from uncanny dodge

            ContextRankConfig NimbleRankConfig = new ContextRankConfig();//the rank config calculates the value to add based on its internal variables
            NimbleRankConfig.m_Flags = 0;//not sure if I need this or what it does
            NimbleRankConfig.m_Type = Kingmaker.Enums.AbilityRankType.DamageBonus;//This may seem odd but I'm fairly sure its just a way of the system linking the rank config with the context value so all that matters it they match
            NimbleRankConfig.m_BaseValueType = ContextRankBaseValueType.ClassLevel;//Nimble increases based on class level
            NimbleRankConfig.m_Progression = ContextRankProgression.DivStep;//it increments based on steps (I think that's what this means)
            NimbleRankConfig.m_StartLevel = 2;//We get nimble at level 2
            NimbleRankConfig.m_StepLevel = 4;//it increases every 4 levels after
            NimbleRankConfig.m_UseMin = false;//no minimum value (it starts at 1 anyway this is used for things like bonus equal to wisdom min 1)
            NimbleRankConfig.m_UseMax = true;//has a maximum in case you somehow get two gunslinger classes on legend I guess
            NimbleRankConfig.m_Max = 20;//caps at level 20
            NimbleRankConfig.m_Class = new BlueprintCharacterClassReference[] { BlueprintTool.GetRef<BlueprintCharacterClassReference>(Gunslinger.GunslingerClassGUID) };//linked to the Gunslinger class which mercifully doesn't need to be configured yet

            ContextValue NimbleContextValue = new ContextValue();//this is the part that links the context rank onfig and the stat bonus
            NimbleContextValue.ValueType = ContextValueType.Rank;//not sure what this does
            NimbleContextValue.Value = 0;//or this
            NimbleContextValue.ValueRank = Kingmaker.Enums.AbilityRankType.DamageBonus;//needs to match the rank config
            NimbleContextValue.ValueShared = Kingmaker.UnitLogic.Abilities.AbilitySharedValue.Damage;//not sure about this
            NimbleContextValue.m_AbilityParameter = AbilityParameterType.Level;//or this since it would seem to have been defined above

            AddContextStatBonus NimbleStatBonus = new AddContextStatBonus();
            NimbleStatBonus.m_Flags = 0;//not sure what this is or if I need it
            NimbleStatBonus.Descriptor = Kingmaker.Enums.ModifierDescriptor.Dodge;//its a dodge bonus
            NimbleStatBonus.Stat = StatType.AC;//it applies to AC
            NimbleStatBonus.Multiplier = 1;//no mulitpliation of computed value
            NimbleStatBonus.Value = NimbleContextValue;//the value is defined by the linking behavior above
            NimbleStatBonus.HasMinimal = false;//there is no minimal value


            BuffConfigurator.New("NimbleBuff", NimbleBuffGUID)//now we make the buff that actuall applies the effect
                .SetDisplayName(LocalizationTool.GetString("Nimble.Buff.Name"))
                .SetIsClassFeature(true)//the buff comes from a class feature
                .SetRanks(0)//has no ranks
                .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Replace)//does not stack
                .SetFrequency(Kingmaker.UnitLogic.Mechanics.DurationRate.Rounds)//not sure what this does
                .SetTickEachSecond(false)//or this
                .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi, Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)//the buff is not shown in UI and persists on death
                .AddComponent(NimbleRankConfig)//then it has the rank config and stat bonus to tell it what to do.
                .AddComponent(NimbleStatBonus)
                .SetIcon(icon)
                .Configure()
                ;

            AddFacts NimbleFacts = new AddFacts();//this is just to assosiate the feature with the buff
            NimbleFacts.m_Flags = 0;//again not sure what these are
            NimbleFacts.m_Facts = new BlueprintUnitFactReference[] { BlueprintTool.GetRef<BlueprintUnitFactReference>(NimbleBuffGUID) };//we want the nimble buff to be applied
            NimbleFacts.CasterLevel = 0;//not sure about these last 3
            NimbleFacts.DoNotRestoreMissingFacts = false;
            NimbleFacts.HasDifficultyRequirements = false;

            FeatureConfigurator.New("NimbleFeature", NimbleGUID)//This is the feature the character gets if they aren't wearing medium or heavy armor wich applies the buff
                .SetIsClassFeature(true)//it is a class feature
                .SetRanks(1)//it has one rank? not sure why I just copied from monk
                .SetReapplyOnLevelUp(true)//recompute every level since it might change
                .SetDisplayName(LocalizationTool.GetString("Nimble.Name"))
                .SetDescription(LocalizationTool.GetString("Nimble.Description"))
                .AddComponent(NimbleFacts)//it adds the facts as defined above
                .SetIcon(icon)
                .Configure()
                ;

            LightArmorUnlock NimbleArmorUnlock = new LightArmorUnlock();//defines the condition on which to add the above feature
            NimbleArmorUnlock.m_NewFact = BlueprintTool.GetRef<BlueprintUnitFactReference>(NimbleGUID);//tells it what feature to add


            FeatureConfigurator.New("NimbleUnlock", NimbleUnlockGUID)//this is the feature you actually add in the progression of the class
                .AddComponent(NimbleArmorUnlock)//It has the above conditional system to add everything else
                .SetDisplayName(LocalizationTool.GetString("Nimble.Name"))
                .SetDescription(LocalizationTool.GetString("Nimble.Description"))
                .SetRanks(1)//again not sure but copied from monk
                .SetIsClassFeature(true)//this is a class feature
                .SetIcon(icon)
                .Configure()
                ;

        }
    }
}
