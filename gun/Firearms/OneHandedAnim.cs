using Kingmaker;
using Kingmaker.AreaLogic.Cutscenes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.ResourceManagement;
using Kingmaker.Visual.Animation;
using Kingmaker.Visual.Animation.Actions;
using Kingmaker.Visual.Animation.Kingmaker;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gun.Firearms
{
    internal static class OneHandedAnim
    {
        public static void PlayAnimation(UnitEntityData unit)
        {
            BundledResourceHandle<AnimationClipWrapper> CutsceneClipWrapperHandle = BundledResourceHandle<AnimationClipWrapper>.Request("e876cf2034488084595fdc6d38cf63a9", hold: true);

            AnimationClipWrapper animationClipWrapper = CutsceneClipWrapperHandle.Object;
            if (unit?.RiderPart != null && animationClipWrapper.MountedAlternativeAnimation != null)
            {
                animationClipWrapper = animationClipWrapper.MountedAlternativeAnimation;
            }

            if (!animationClipWrapper)
            {
                PFLog.Default.Error($"No clip found for");
                return;
            }

            UnitAnimationManager animationManager = unit.View.AnimationManager;
            if (!animationManager)
            {
                return;
            }
            UnitAnimationActionClip unitAnimationActionClip = UnitAnimationActionClip.Create(animationClipWrapper, "CommandUnitPlayCutsceneAnimation/PlayAnimation");
            unitAnimationActionClip.ExecutionMode = ExecutionMode.Sequenced;
            UnitAnimationActionHandle unitAnimationActionHandle = (UnitAnimationActionHandle)animationManager.CreateHandle(unitAnimationActionClip);
            
            animationManager.Execute(unitAnimationActionHandle);
        }
    }
}
