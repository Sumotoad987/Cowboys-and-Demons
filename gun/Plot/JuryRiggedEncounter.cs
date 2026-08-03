using Kingmaker;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gun.Plot
{
    public class JuryRiggedEncounter : MonoBehaviour
    {
        public float x0;
        public float z0;
        public float x1;
        public float z1;
        public BlueprintDialogReference dialog;
        private float t;

        public JuryRiggedEncounter(float X0, float Y0, float X1, float Y1)
        {
            x0 = X0;
            z0 = Y0;
            x1 = X1;
            z1 = Y1;
        }
        public void Update()
        {
            //Main.Log.Log("Jury Rig finds player at:" + this.gameObject.transform.position);
            if (this.gameObject.transform.position.x >= x0 && this.gameObject.transform.position.x <= x1 && this.gameObject.transform.position.z >= z0 && this.gameObject.transform.position.z <= z1)
            {
                Game.Instance.DialogController.StartDialogWithoutTarget(dialog, null);
                Destroy(this);
            }
                 
        }
    }
}
