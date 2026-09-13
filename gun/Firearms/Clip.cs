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
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.TextTools;
using Kingmaker.UI.Log;
using Kingmaker.UI.Models.Log;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UI.Models.Log.Events;
using Kingmaker.UI.Tooltip;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using Kingmaker.Visual.Sound;
using Owlcat.Runtime.Core.Logging;
using Owlcat.Runtime.UI.Tooltips;
using System.Drawing;
using System.Media;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;
using static Kingmaker.UI.MVVM._VM.ServiceWindows.Spellbook.MagicHack.SpellbookMagicHackMixerView;

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
                    evt.AutoHit = true;
                    evt.SkipAnimation = true;
                    evt.ReplaceTarget = true;
                    evt.NewTarget = null;
                    Kingmaker.UI.Models.Log.Events.GameLogEventMessage eventmessage = new GameLogEventMessage(evt.Initiator.CharacterName + ": No Ammo");
                    Kingmaker.Game.Instance.GameLogController.AddReadyEvent(eventmessage);
                    
                }
                else
                {//if there are rounds 
                    if (evt.Weapon.Enchantments.Any((ItemEnchantment enhancement) =>
                    {
                        Capacity cap = enhancement.GetComponent<Capacity>();
                        if (cap != null)
                        {
                            return cap.count == 1;
                        }
                        return false;
                    }))
                    {
                        evt.Initiator.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(BaseFirearm.RoundsGUID)).Remove();//if the clip is just one then remove all rounds (in case you load a thing with a large clip them switch to a thing with a smaller clip
                    }
                    else
                    {
                        evt.Initiator.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(BaseFirearm.RoundsGUID)).RemoveRank();//then remove one round
                    }
                    SoundPlayer Bang = new SoundPlayer(GunSound.GetSoundFile());
                    Bang.Play();//this need to be replaced with proper sound system
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
                        evt.AutoHit = true;
                        evt.SkipAnimation = true;
                        evt.ReplaceTarget = true;
                        evt.NewTarget = null;
                        
                        Kingmaker.UI.Models.Log.Events.GameLogEventMessage eventmessage = new GameLogEventMessage(evt.Initiator.CharacterName + ": No Ammo");
                        Kingmaker.Game.Instance.GameLogController.AddReadyEvent(eventmessage);
                        return;
                       //Rulebook.Trigger(log)
                    }
                    else
                    {//if there are rounds 
                        if (evt.Weapon.Enchantments.Any((ItemEnchantment enhancement) =>
                        {
                            Capacity cap = enhancement.GetComponent<Capacity>();
                            if (cap != null) 
                            {
                                return cap.count == 1;
                            }
                            return false;
                        }))
                        {
                            Rounds.Remove();//if the clip is just one then remove all rounds (in case you load a thing with a large clip them switch to a thing with a smaller clip
                        }
                        else
                        {
                            Rounds.RemoveRank();//then remove one round
                        }
                            
                        
                    }
                    
                }
                SoundPlayer Bang = new SoundPlayer(GunSound.GetSoundFile());
                Bang.Play();

            }
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

    public class Capacity : WeaponEnchantmentLogic
    {
        public int count = 1;

        public Capacity(int cap)
        {
            count = cap;
        }
    }

    public static class GunSound
    {
        public static string GetSoundFile()
        {
            float vol = Kingmaker.Settings.SettingsController.SoundSettingsController.m_Settings.VolumeMaster/100;
            vol *= Kingmaker.Settings.SettingsController.SoundSettingsController.m_Settings.VolumeSFX/100;
            vol *= 10;
            int volume = Mathf.FloorToInt(vol) * 10;
            Main.Log.Log(volume.ToString());
            return System.IO.Path.Combine(Main.ModPath, "Media\\Sounds\\bang" + volume + ".wav");
        }
    }
}
