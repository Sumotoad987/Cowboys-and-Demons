using BlueprintCore.Utils;
using BlueprintCore.Utils.Assets;
using HarmonyLib;
using Kingmaker;
using Kingmaker.AreaLogic.Cutscenes.Commands;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Globalmap.View;
using Kingmaker.PubSubSystem;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gun.Plot
{
    public class WorldMapEncounter : IAreaActivationHandler, ISubscriber, IGlobalSubscriber
    {
        private float x0;
        private float z0;
        private float x1;
        private float z1;
        private BlueprintUnlockableFlag Requires;//the flag required to spawn it
        private BlueprintUnlockableFlag RemovedBy;//the flag which deletes it (typically the one that is set when it is triggered)
        private BlueprintDialogReference Event;
        private bool hasRequirement = false;
        private string ImageFile = "";
        public WorldMapEncounter (float x0, float z0, float x1, float z1, string removedBy, string Dialogue, string image = "", string requires = "NULL")
        {
            this.x0 = x0;
            this.z0 = z0;
            this.x1 = x1;
            this.z1 = z1;
            RemovedBy = BlueprintTool.Get<BlueprintUnlockableFlag>(removedBy);
            if (requires != null)
            {
                hasRequirement = true;
                Requires = BlueprintTool.Get<BlueprintUnlockableFlag>(requires);
            }
            Event = BlueprintTool.GetRef<BlueprintDialogReference>(Dialogue);
            ImageFile = image;
        }

        public void OnAreaActivated()
        {
            if (Game.Instance.CurrentlyLoadedArea.IsGlobalMap)
            {//if we are on the world map
                if (hasRequirement)//if there is a requirement
                {
                    if (Game.Instance.Player.UnlockableFlags.GetFlagValue(Requires) != 0)//and it is not met
                    {
                        return;//do nothing
                    }
                }
                GlobalMapPlayerPawn player = GlobalMapView.Instance.PlayerPawn;
                
                if (Game.Instance.Player.UnlockableFlags.GetFlagValue(RemovedBy) == 0)//if the thing that removes this is not true
                {
                    JuryRiggedEncounter Encounter = player.gameObject.AddComponent<JuryRiggedEncounter>();//make the encoutner
                    Encounter.x0 = x0;
                    Encounter.z0 = z0;
                    Encounter.x1 = x1;
                    Encounter.z1 = z1;
                    Encounter.dialog = Event;
                    if (ImageFile != "")
                    {
                        float width = Mathf.Abs(x1 - x0);
                        float height = Mathf.Abs(z1 - z0);
                        Texture2D texture = new Texture2D(100, 100);//this should be size of the image file
                        byte[] data = File.ReadAllBytes(Main.ModPath + "/Media/Map/" + ImageFile);
                        texture.LoadImage(data);


                        Rect SpriteRect = new Rect(0, 0, 100, 100);
                        Sprite Icon = Sprite.Create(texture, SpriteRect, Vector2.zero);
                        SpriteRenderer IconRender = new SpriteRenderer();
                        IconRender.transform.position = new Vector3((x0 + x1) / 2, 0, (z0 + z1) / 2);
                        IconRender.transform.localScale = new Vector3(width, height, 0);
                        IconRender.transform.rotation = Quaternion.Euler(-90, 0, 0);
                        IconRender.sprite = Icon;
                        SpriteRenderer.Instantiate(Icon, GlobalMapView.Instance.transform,true);
                        //Icon.transform.position = new Vector3((x0 + x1) / 2, 0, (z0 + z1) / 2);//place it in the center of the encoutner area
                    }
                }
            }
        }
    }
}
