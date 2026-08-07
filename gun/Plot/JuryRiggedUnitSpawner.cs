using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Designers;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Groups;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.View;
using Kingmaker.View.Spawners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace gun.Plot
{
    internal class JuryRiggedUnitSpawner : IAreaActivationHandler, ISubscriber, IGlobalSubscriber
    {
        BlueprintArea area;
        BlueprintUnit ToSpawn;
        Vector3 position;
        Quaternion rotation;
        BlueprintUnlockableFlag Condition;
        BlueprintUnlockableFlag Flag;//this is the one controled in here to note that the unit is now on the map
        private bool hasRemover;
        int Chapter;
        string dialog;

        public JuryRiggedUnitSpawner(string area, string ToSpawn, Vector3 position, Vector3 rotation, string Requires, int chapter, string dialog = "NULL") 
        {
            this.area = BlueprintTool.Get<BlueprintArea>(area);
            this.ToSpawn = BlueprintTool.Get<BlueprintUnit>(ToSpawn);
            this.position = position;
            this.rotation = Quaternion.Euler(rotation);
            this.Condition = BlueprintTool.Get<BlueprintUnlockableFlag>(Requires);
            this.Chapter = chapter;


            this.dialog = dialog;
        }
        public void OnAreaActivated()
        {
            
            if (Game.Instance.CurrentlyLoadedArea == area && Game.Instance.Player.Chapter >= Chapter)
            {//if we are on the right map
                UnitEntityData data = null;
                UnityEngine.Object[] objects = GameObject.FindObjectsByType(typeof(UnitEntityView), FindObjectsSortMode.InstanceID);

                /*bool UnitPresent = (!Game.Instance.State.LoadedAreaState.MainState.AllEntityData.Any((EntityDataBase data) =>
                {
                    if (data.GetType() == typeof(UnitEntityData)) 
                    {
                        Main.Log.Log(((UnitEntityData)data).CharacterName);
                        return ((UnitEntityData)data).Blueprint == ToSpawn;
                    }
                    else
                    {
                        return false;
                    }

   
                }));//is the unit currently on the map?*/
                bool UnitPresent = objects.Any((UnityEngine.Object unitobject) => {
                    Main.Log.Log(((UnitEntityView)unitobject).Blueprint.name);
                    if (((UnitEntityView)unitobject).Blueprint == ToSpawn)
                    {
                        data = ((UnitEntityView)unitobject).Data;
                        return true;
                    }
                    return false;
                }
                );
                
                if (UnitPresent)
                {
                    Main.Log.Log("Found Cowgirl");
                    if (Game.Instance.Player.UnlockableFlags.GetFlagValue(Condition) == 0)
                    {
                        //if the unit's condition is no longer met
                        data.Position = new Vector3(-9999, -9999, -9999);//just teleport them really far away first in case the below doesn't work
                        Game.Instance.UnitGroupsController.HandleUnitDestroyed(data); //not sure this will work 
                        data.Destroy();//maybe this will
                    }
                }else if (Game.Instance.Player.UnlockableFlags.GetFlagValue(Condition) != 0)
                {//if it is not already spawned and its requirements are met
                    //used toybox spawn unit as a reference for this one
                    Main.Log.Log("spawning her now");
                    data = Game.Instance.EntityCreator.SpawnUnit(ToSpawn, position, rotation, Game.Instance.State.LoadedAreaState.MainState);
                    
                }
            }
        }
    }
}
