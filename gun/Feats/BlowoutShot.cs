using BlueprintCore.Utils;
using gun.Classes.Gunslinger;
using gun.Deeds;
using gun.Firearms;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gun.Feats
{
    internal class BlowoutShot : AbstractWeaponTrigger, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {

        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {

        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.AttackRoll.IsHit &&//if we hit the target
                evt.Weapon.Blueprint.Category == BaseFirearm.FirearmCategory && //and the weapon is a firearm
                evt.Initiator.Resources.HasEnoughResource(Grit.GritResource, 1)//and have enough grit
                )
            {
                evt.Initiator.Resources.Spend(Grit.GritResource,1);//spend 1 grit
                evt.Initiator.Buffs.GetBuff(BlueprintTool.Get<BlueprintBuff>(Feats.GritFeats.BlowoutShotBuff)).Remove();//remove blowoutshot buff
                
                //copied the below from the bullrush manuever before editing
                Vector3 normalized3 = (evt.Initiator.Position - evt.Target.Position).normalized;
                evt.Initiator.Ensure<UnitPartForceMove>().Push(normalized3, 5.Feet().Meters, false);//push the user back 5 feet

                int DC = 10;

                ClassData GunslingerData = evt.Initiator.Progression.Classes.First((ClassData Class) =>
                {
                    return Class.CharacterClass.AssetGuid == Gunslinger.GunslingerClassGUID;
                });

                if (GunslingerData != null) {
                    DC += GunslingerData.Level / 2;
                }

                DC += evt.Initiator.Stats.Intelligence;
               

                RuleSavingThrow save = new RuleSavingThrow(evt.Target,Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex, DC);
                Context.TriggerRule(save);
                if (!save.IsPassed)
                {//if they failed the save
                    evt.Target.Ensure<UnitPartForceMove>().Push(-normalized3, 10.Feet().Meters, false);//push them back 10 feet
                }
                
            }
        }
    }
}
