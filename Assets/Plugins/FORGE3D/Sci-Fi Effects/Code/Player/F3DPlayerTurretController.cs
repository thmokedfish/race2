using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
namespace Forge3D
{
    public class F3DPlayerTurretController :MonoBehaviour
    {
        public bool isLocal = false;
        RaycastHit hitInfo; // Raycast structure
        public F3DTurret turret;
        bool isFiring; // Is turret currently in firing state
        public F3DFXController fxController;

        void Update()
        {
            if(!isLocal)
            { return; }
            CheckForTurn();
            // CheckForFire();
            /*
            if (!isFiring && Input.GetKeyDown(KeyCode.Mouse0))
            {
                CmdFire();
                isFiring = true;
            }

            if (isFiring && Input.GetKeyUp(KeyCode.Mouse0))
            {
                isFiring = false;
                CmdStopFire();
            }
            */
        }

        void CheckForFire()
        {
            // Fire turret
            if (!isFiring && Input.GetKeyDown(KeyCode.Mouse0))
            {
                isFiring = true;
                fxController.Fire();
            }

            // Stop firing
            if (isFiring && Input.GetKeyUp(KeyCode.Mouse0))
            {
                isFiring = false;
                fxController.Stop();
            }
        }
        /*
        [Command]
        void CmdFire()
        {
            //network identity "spawn"
            Debug.Log("cmd fire");
            RpcFire();
        }

        [ClientRpc]
        void RpcFire()
        {
            Debug.Log("rpc fire");
            fxController.Fire();
        }

        [Command]
        void CmdStopFire()
        {
            RpcStopFire();
        }

        [ClientRpc]
        void RpcStopFire()
        {
            fxController.Stop();
        }
        */
        void CheckForTurn()
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
            // Construct a ray pointing from screen mouse position into world space
            Vector3 centerPos=Camera.main.ScreenToWorldPoint(screenCenter);
           // turret.SetNewTarget(turret.Mount.transform.position * 2 - mousePos);
            Ray cameraRay = Camera.main.ScreenPointToRay(screenCenter);
            // Raycast
            if(Physics.Raycast(cameraRay,out hitInfo))//(cameraRay, out hitInfo, 5000f);
            {
                turret.SetNewTarget(hitInfo.point);
            }
            else
            {
                turret.SetNewTarget(centerPos + Camera.main.transform.forward * 5000);
            }
        }
    }
}