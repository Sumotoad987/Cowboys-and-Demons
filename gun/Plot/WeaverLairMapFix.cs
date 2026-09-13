using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.AreaLogic;
using Kingmaker.Blueprints.Area;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.PubSubSystem;
using Kingmaker.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Kingmaker.UI.Context.MenuItem;

namespace gun.Plot
{
    internal class WeaverLairMapFix : IAreaActivationHandler, ISubscriber, IGlobalSubscriber
    {

        public void OnAreaActivated()
        {
            if (Game.Instance.CurrentlyLoadedArea == BlueprintTool.Get<BlueprintArea>(Act4.WeaversLairArea))
            {
                Main.Log.Log(GameObject.Find("PlayerSpawnPlaceCave").name);
            }
            else
            {
                Game.Instance.LoadArea(BlueprintTool.Get<BlueprintArea>(Act4.WeaversLairArea), BlueprintTool.Get<BlueprintAreaEnterPoint>("d44f991b94581de43936b1ce0bf63add"), AutoSaveMode.None, forceUnload: false, null, null);
            }
        }
    }
}
