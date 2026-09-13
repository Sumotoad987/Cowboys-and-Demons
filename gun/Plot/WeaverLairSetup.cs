using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.AreaEx;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.AreaLogic.Etudes;
using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.AreaLogic;
using Kingmaker.AreaLogic.Cutscenes.Commands;
using Kingmaker.Armies.State;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Controllers;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RandomEncounters.Settings;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Kingmaker.View.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.TerrainAPI;

namespace gun.Plot
{
    internal class WeaverLairSetup : IAreaActivationHandler, ISubscriber, IGlobalSubscriber
    {
        Vector3[] ExtraObjects = {
            new Vector3(67.4f, 40.8f, 38.4f),//A
            new Vector3(66.9f, 41.1f, 19.9f),//B
            new Vector3(69.9f, 41.1f, 25.5f),//Second portal for B because corridor too wide
            new Vector3(108.1f, 40.8f, 12.6f),//C
            new Vector3(138.2f, 40.8f, 11.0f),//D
            new Vector3(140.6f, 40.9f, 35.0f),//E
            new Vector3(138.4f, 41.1f, 56.7f),//F
            new Vector3(111.7f, 41.4f, 57.5f),//Second portal for H because corridor wide
            new Vector3(115.4f, 41.0f, 60.5f),//H
            new Vector3(167.0f, 40.9f, 100.4f),//G
            new Vector3(163.7f, 41.5f, 105.1f)//second portal for G because the corridor is too wide


        };
        public void OnAreaActivated()
        {
            if (Game.Instance.CurrentlyLoadedArea == BlueprintTool.Get<BlueprintArea>(Act4.WeaversLairArea))
            {
                
                //new approach  will be to create a spell effect zone at the target positions that will on entered teleport the target to a new position.
                if (GameObject.FindAnyObjectByType<AreaEffectView>() != null)
                {
                    AreaEffectView[] views = GameObject.FindObjectsOfType<AreaEffectView>();
                    if (!views.Any((AreaEffectView view) => {//if there are no views of the portal than we need to make them
                        return view.m_Blueprint == BlueprintTool.GetRef<BlueprintAbilityAreaEffectReference>(Act4.WeaverLairPortalEffect);
                    }
                    ))
                    {
                        ContextDurationValue InfinateDuration = new ContextDurationValue();
                        InfinateDuration.Rate = DurationRate.Days;
                        InfinateDuration.m_IsExtendable = true;
                        InfinateDuration.BonusValue = new ContextValue();
                        InfinateDuration.BonusValue.Value = 99999;
                        InfinateDuration.DiceCountValue = new ContextValue();
                        foreach (Vector3 vector in ExtraObjects)
                        {
                            //AreaEffectsController.Spawn(null, BlueprintTool.Get<BlueprintAbilityAreaEffect>(Act4.WeaverLairPortalEffect), new TargetWrapper(vector), null);
                            SceneEntitiesState state =  Game.Instance.LoadedAreaState.MainState;
                            AreaEffectView areaEffectView = new GameObject($"AreaEffect [{BlueprintTool.Get<BlueprintAbilityAreaEffect>(Act4.WeaverLairPortalEffect)}]").AddComponent<AreaEffectView>();
                            areaEffectView.UniqueId = Game.Instance.Player.GetNewUniqueId();
                            areaEffectView.OnUnit = false;
                            areaEffectView.InitAtRuntime(null, BlueprintTool.Get<BlueprintAbilityAreaEffect>(Act4.WeaverLairPortalEffect), new TargetWrapper(vector), Game.Instance.TimeController.GameTime, null);
                            AreaEffectEntityData data = (AreaEffectEntityData)Game.Instance.EntityCreator.SpawnEntityWithView(areaEffectView, state);
                            EventBus.RaiseEvent(delegate (IAreaEffectHandler h)
                            {
                                h.HandleAreaEffectSpawned(data);
                            });
                            
                        }
                    }
                }
            }
        }
    }

    internal class WeaverLairPorterPos : PositionEvaluator
    {
        static Vector3[] PortalPositions =
        {
            new Vector3(67.4f, 40.8f, 38.4f),//A
            new Vector3(68.9f, 40.9f, 22.4f),//B
            new Vector3(108.1f, 40.8f, 12.6f),//C
            new Vector3(138.2f, 40.8f, 11.0f),//D
            new Vector3(140.6f, 40.9f, 35.0f),//E
            new Vector3(138.4f, 41.1f, 56.7f),//F
            new Vector3(167.0f, 40.9f, 100.4f),//G
            new Vector3(115.4f, 41.0f, 60.5f)//H
            
        };
        static Vector3[] ExitPositions =
        {
            new Vector3(74.1f, 40.5f, 19.2f),//A->B
            new Vector3(113.1f, 40.7f, 11.7f),//B->C
            new Vector3(141.9f, 40.7f, 13.9f),//C->D
            new Vector3(138.3f, 40.8f, 39.6f),//D->E
            new Vector3(142.7f, 40.6f, 61.1f),//E->F
            new Vector3(169.5f, 40.9f, 104.7f),//F->G
            new Vector3(108.4f, 41.2f, 63.3f),//G->H
            new Vector3(63.6f, 40.5f, 34.9f),//H->A
            
            
            
        };
        public override string GetCaption()
        {
            return "Finding correct portal exit";
        }

        public override Vector3 GetValueInternal()
        {
            Vector3 startpoint = ContextData<MechanicsContext.Data>.Current.CurrentTarget.Unit.Position;
            //Vector3 startpoint = ContextData<MechanicsContext.Data>.Current.Context.SourceAbilityContext.MainTarget.m_Point;
            Main.Log.Log("" + startpoint);
            int targetnum = 0;
            float distance = 999;
            int i = -1;
            foreach (Vector3 Portal in PortalPositions)
            {
                i++;
                if (Vector3.Distance(startpoint, Portal) < distance)//if this point is closer then
                {
                    targetnum = i;//that our new best bet for which portal this is
                    distance = Vector3.Distance(startpoint, Portal);//update distance
                }
            }
            if (targetnum == 10)//if we're teleporting into the final room then we will start the dialog with the brainweaver
            {

            }
            Main.Log.Log("" + targetnum);
            return ExitPositions[targetnum];
        }
    }
}
