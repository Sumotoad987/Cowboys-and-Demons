using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.ContextData;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace gun.Plot
{
    public class CowgirlDialogComponent : UnitInteractionComponent, IDialogReference
    {
        [SerializeField]
        [FormerlySerializedAs("Dialog")]
        public BlueprintDialogReference m_Dialog;

        public ActionList NoDialogActions;

        public BlueprintUnlockableFlagReference m_Condition;

        public BlueprintUnlockableFlag Condition => m_Condition.Get();

        public BlueprintDialog Dialog => m_Dialog?.Get();

        public override bool IsAvailable(UnitEntityData initiator, UnitEntityData target)
        {
            if (!base.IsAvailable(initiator, target))
            {
                return false;
            }

            if (Dialog == null || Dialog.FirstCue.Cues.Count <= 0)
            {
                return NoDialogActions.HasActions;
            }

            return true;
        }

        public override UnitCommand.ResultType Interact(UnitEntityData user, UnitEntityData target)
        {
            if (Dialog == null)
            {
                using (ContextData<ClickedUnitData>.Request().Setup(target))
                {
                    NoDialogActions.Run();
                }

                return UnitCommand.ResultType.Fail;
            }

            Game.Instance.DialogController.StartDialogWithUnit(Dialog, target, user);
            return UnitCommand.ResultType.Success;
        }

        public DialogReferenceType GetUsagesFor(BlueprintDialog dialog)
        {
            if (dialog != Dialog)
            {
                return DialogReferenceType.None;
            }

            return DialogReferenceType.Start;
        }
    }
}
