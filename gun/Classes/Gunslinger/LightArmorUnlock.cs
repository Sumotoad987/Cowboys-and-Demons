using BepInEx.AssemblyPublicizer;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace gun.Classes.Gunslinger
{
    public class LightArmorUnlockData
    {//fully just copied from monk and renamed to keep it separate
        [JsonProperty]
        public EntityFact AppliedFact;
    }


    [ComponentName("Add feature if owner has light or no armor")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("f9a62f2a-1da2-496b-b2ec-a0507ec6871d")]
    public class LightArmorUnlock : UnitFactComponentDelegate<LightArmorUnlockData>, IUnitActiveEquipmentSetHandler, IGlobalSubscriber, ISubscriber, IUnitEquipmentHandler, IPolymorphActivatedHandler, IPolymorphDeactivatedHandler
    {//this is the system copied from monks no armor setup which adds a fact if the
        [SerializeField]
        [FormerlySerializedAs("NewFact")]
        public BlueprintUnitFactReference m_NewFact;

        public BlueprintUnitFact NewFact => m_NewFact?.Get();

        public override void OnActivate()
        {
            CheckEligibility();
        }

        public override void OnDeactivate()
        {
            RemoveFact();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit)
        {
            UpdateBuffStateIfCorrectUnit(unit);
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
        {
            UpdateBuffStateIfCorrectUnit(slot.Owner);
        }

        public void OnPolymorphActivated(UnitEntityData unit, Polymorph polymorph)
        {
            UpdateBuffStateIfCorrectUnit(unit);
        }

        public void OnPolymorphDeactivated(UnitEntityData unit, Polymorph polymorph)
        {
            UpdateBuffStateIfCorrectUnit(unit);
        }

        public void UpdateBuffStateIfCorrectUnit(UnitEntityData unit)
        {
            if (!(unit != base.Owner))
            {
                CheckEligibility();
            }
        }

        public void CheckEligibility()
        {//this is the only fact of the class I changed
            UnitBody body = base.Owner.Body;
            //removed check for shield
            bool flag = body.Armor.HasArmor && body.Armor.Armor.Blueprint.IsArmor;//true if the character is wearing any armor
            flag = flag && (body.Armor.Armor.ArmorType() != ArmorProficiencyGroup.Light);//true if the armor is not light
            if ((body.IsPolymorphed && !body.IsPolymorphKeepSlots) || (!flag))
            {
                AddFact();
            }
            else
            {
                RemoveFact();
            }
        }

        public void AddFact()
        {
            if (base.Data.AppliedFact == null)
            {
                base.Data.AppliedFact = base.Owner.AddFact(NewFact);
            }
        }
        public void RemoveFact()
        {
            if (base.Data.AppliedFact != null)
            {
                base.Owner.RemoveFact(base.Data.AppliedFact);
                base.Data.AppliedFact = null;
            }
        }
    }
}
