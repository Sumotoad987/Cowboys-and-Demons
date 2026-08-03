using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.QA.Clockwork;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gun
{
    internal static class Utilities
    {
        public static WeaponVisualParameters Clone (WeaponVisualParameters origional)
        {
            WeaponVisualParameters clone = new WeaponVisualParameters ();
            clone.m_Projectiles = origional.m_Projectiles;
            clone.m_WeaponAnimationStyle = origional.m_WeaponAnimationStyle;
            clone.m_SpecialAnimation = origional.m_SpecialAnimation;
            clone.m_WeaponModel = origional.m_WeaponModel;
            clone.m_WeaponBeltModelOverride = origional.m_WeaponBeltModelOverride;
            clone.m_WeaponSheathModelOverride = origional.m_WeaponSheathModelOverride;
            clone.m_OverrideAttachSlots = origional.m_OverrideAttachSlots;
            clone.m_PossibleAttachSlots = origional.m_PossibleAttachSlots;
            clone.m_ReachFXThresholdBonus = origional.m_ReachFXThresholdBonus;
            clone.m_CachedBeltModel = origional.m_CachedBeltModel;
            clone.m_CachedSheathModel = origional.m_CachedSheathModel;
            clone.m_CachedEquipLinksUpToDate = origional.m_CachedEquipLinksUpToDate;
            clone.m_SoundSize = origional.m_SoundSize;
            clone.m_SoundType = origional.m_SoundType;
            clone.m_WhooshSound = origional.m_WhooshSound;
            clone.m_MissSoundType = origional.m_MissSoundType;
            clone.m_EquipSound = origional.m_EquipSound;
            clone.m_UnequipSound = origional.m_UnequipSound;
            clone.m_InventoryEquipSound = origional.m_InventoryEquipSound;
            clone.m_InventoryPutSound = origional.m_InventoryPutSound;
            clone.Prototype = origional.Prototype;
            return clone;
        }

        public static ActionList Clone(ActionList origional)
        {
            ActionList clone = new ActionList();
            clone.Actions = new GameAction[origional.Actions.Length];
            int i = 0;
            foreach (GameAction action in origional.Actions)
            {
                clone.Actions[i++] = action;
            }
            return clone;
        }
        public static AddFactContextActions Clone (AddFactContextActions origional)
        {//copies as deep and individual game action but those actions will have to be individually copied to be editied independantly
            AddFactContextActions clone = new AddFactContextActions ();
            clone.Activated = Clone(origional.Activated);
            clone.Deactivated = Clone(origional.Deactivated);
            clone.NewRound = Clone(origional.NewRound);
            clone.Dispose = Clone(origional.Dispose);
            return clone;
        }
        public static ContextValue Clone (ContextValue origional)
        {
            ContextValue clone = new ContextValue();
            clone.ValueType = origional.ValueType;
            clone.Value = origional.Value;
            clone.ValueRank = origional.ValueRank;
            clone.ValueShared = origional.ValueShared;
            clone.Property = origional.Property;
            clone.m_CustomProperty = origional.m_CustomProperty;//not a deep copy here
            clone.m_AbilityParameter = origional.m_AbilityParameter;
            clone.PropertyName = origional.PropertyName;
            return clone;
        }
        public static ContextDiceValue Clone (ContextDiceValue origional)
        {
            ContextDiceValue clone = new ContextDiceValue();
            clone.DiceType = origional.DiceType;
            clone.DiceCountValue = Clone(origional.DiceCountValue);
            clone.BonusValue = Clone(origional.BonusValue);
            return clone;
        }
        public static ContextActionDealDamage Clone (ContextActionDealDamage origional)
        {//not a deep copy in everything just enough for my purposes
            ContextActionDealDamage clone = new ContextActionDealDamage();
            clone.m_Type = origional.m_Type;
            clone.DamageType = origional.DamageType;
            clone.Drain = origional.Drain;
            clone.AbilityType = origional.AbilityType;
            clone.EnergyDrainType = origional.EnergyDrainType;
            clone.Duration = origional.Duration;//not deep copy here
            clone.ReadPreRolledFromSharedValue = origional.ReadPreRolledFromSharedValue;
            clone.PreRolledSharedValue = origional.PreRolledSharedValue;
            clone.Value = Clone(origional.Value);
            clone.Half = origional.Half;
            clone.DisableSneakDamage = origional.DisableSneakDamage;
            clone.AlreadyHalved = origional.AlreadyHalved;
            clone.IsAoE = origional.IsAoE;
            clone.HalfIfSaved = origional.HalfIfSaved;
            clone.IgnoreCritical = origional.IgnoreCritical;
            clone.IgnoreUnitModifiers = origional.IgnoreUnitModifiers;
            clone.DisableKineticCache = origional.DisableKineticCache;
            clone.AddAdditionalDamage = origional.AddAdditionalDamage;
            clone.AddFavoredEnemyDamage = origional.AddFavoredEnemyDamage;
            clone.UseWeaponDamageModifiers = origional.UseWeaponDamageModifiers;
            clone.UseMinHPAfterDamage = origional.UseMinHPAfterDamage;
            clone.MinHPAfterDamage = origional.MinHPAfterDamage;
            clone.WriteResultToSharedValue = origional.WriteResultToSharedValue;
            clone.WriteRawResultToSharedValue = origional.WriteRawResultToSharedValue;
            clone.ResultSharedValue = origional.ResultSharedValue;
            clone.WriteCriticalToSharedValue = origional.WriteCriticalToSharedValue;
            clone.CriticalSharedValue = origional.CriticalSharedValue;
            clone.SetFactAsReason = origional.SetFactAsReason;
            return clone;
        }

        public static CueSelection MakeCueSelection (params string[] cues)
        {
            CueSelection selection = new CueSelection ();
            selection.Cues = new List<BlueprintCueBaseReference> ();
            foreach (string cue in cues)
            {
                selection.Cues.Add(BlueprintTool.GetRef<BlueprintCueBaseReference>(cue));
            }
            selection.Strategy = Strategy.First;
            return selection;
        }

        //makes a conditions builder for is the user has seen the following cues. Or if they have not seen any of the following cues
        public static ConditionsBuilder MakeSeenCue (bool Not, params string[] Cues)
        {
            ConditionsBuilder output = ConditionsBuilder.New();
            foreach (string cue in Cues)
            {
                CueSeen Condition = new CueSeen();
                Condition.m_Cue = BlueprintTool.GetRef<BlueprintCueBaseReference>(cue);
                Condition.Not = Not;
                
                output.Add(Condition);
                
            }
            return output;
        }

        public static ConditionsBuilder MakeSeenAny (bool Not, params string[] Cues)
        {
            ConditionsBuilder output = MakeSeenCue(Not, Cues);
            output.UseOr();
            return output;
        }

        public static ConditionsBuilder MakeSelectedAnswer (params string[] Answers)
        {
            ConditionsBuilder output = ConditionsBuilder.New();
            foreach (string answer in Answers)
            {
                AnswerSelected Condition = new AnswerSelected();
                Condition.m_Answer = BlueprintTool.GetRef<BlueprintAnswerReference>(answer);
                output.Add(Condition);

            }
            return output;
        }

        public static ConditionsBuilder MakeFlagCheck(string flag, int min, int max)
        {
            ConditionsBuilder output = ConditionsBuilder.New();
            FlagInRange FlagCondition = new FlagInRange();
            FlagCondition.m_Flag = BlueprintTool.GetRef<BlueprintUnlockableFlagReference>(flag);
            FlagCondition.MinValue = min;
            FlagCondition.MaxValue = max;
            output.Add(FlagCondition);
            return output;
        }


        //make a condition builder for if all check were passed (or all were failed)
        public static ConditionsBuilder MakePassedCheck (bool passed, params string[] Checks)
        {
            ConditionsBuilder output = ConditionsBuilder.New();
            if (passed)
            {
                foreach (string check in Checks)
                {
                    CheckPassed Condition = new CheckPassed();
                    Condition.m_Check = BlueprintTool.GetRef<BlueprintCheckReference>(check);

                    output.Add(Condition);

                }
            }
            else
            {
                foreach (string check in Checks)
                {
                    CheckFailed Condition = new CheckFailed();
                    Condition.m_Check = BlueprintTool.GetRef<BlueprintCheckReference>(check);

                    output.Add(Condition);

                }
            }
                
            return output;
        }

        public static ActionsBuilder LootBuilder(params string[] items)
        {
            ActionsBuilder output = ActionsBuilder.New();
            foreach (string item in items)
            {
                output.GiveItemToPlayer(item);
            }
            return output;
        }
        public static Sprite MakeIcon (string file)
        {
            //creates an Icon
            byte[] data = File.ReadAllBytes(Main.ModPath + "/Media/Icons/" + file);
            Texture2D texture2D = new Texture2D(64, 64);
            texture2D.LoadImage(data);
            Sprite icon = Sprite.Create(texture2D, new Rect(0f, 0f, 64, 64), new Vector2(0f, 0f));
            return icon;
        }

    }
}
