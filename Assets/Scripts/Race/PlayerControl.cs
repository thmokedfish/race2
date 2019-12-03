using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Cinemachine;

public class PlayerControl :NetworkBehaviour
{
    bool isFiring; // Is turret currently in firing state
    public CinemachineFreeLook FreeLookCam;
    public Forge3D.F3DPlayerTurretController turrent;
    [Range(0,1)]
    public int prefabIndex;
    [SyncVar]
    public int playerID;//在队伍中的ID,0 or 1
    [SyncVar]
    public int score;
    [SyncVar]
    public int teamID;//0 or 1
    public Transform cameraTarget;
    bool isCursorLocked;
    Crosshair crosshair;
    private void Awake()
    {
        crosshair = UIManager.Instance.crosshair.GetComponent<Crosshair>();
    }
    public override void OnStartLocalPlayer()
    {
        GameManager.Instance.localPlayer = this;
        CursorLock(true);
        FreeLookCam.gameObject.SetActive(true);
    }
    private void Start()  //不区分是否是localPlayer的部分
    {
        InitTurret();
        GameManager.Instance.team[teamID].AddPlayer(this);
        UIManager.Instance.setTeamScoreUI(teamID);
    }
    void InitTurret()
    {
        turrent.isLocal = isLocalPlayer;
    }
    float timer=0;

    float coolDown=0.5f;
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CursorLock(!isCursorLocked);
        }

        if(Input.GetMouseButtonDown(0))
        {
            CursorLock(true);
        }

        if (!isFiring && Input.GetKeyDown(KeyCode.Mouse0))
        {
            CmdFire();
            isFiring = true;
        }
        if(isFiring&&Input.GetKey(KeyCode.Mouse0))
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2));
            CmdDamage(cameraRay);
        }

        if (isFiring && Input.GetKeyUp(KeyCode.Mouse0))
        {
            isFiring = false;
            CmdStopFire();
        }
        crosshair.increase = isFiring;

        if (timer < coolDown)
        {
            timer += Time.deltaTime;
        }
    }


    void CursorLock(bool locked)
    {
        isCursorLocked = locked;

        if (!locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    [Command]
    void CmdDamage(Ray cameraRay)
    {
        RaycastHit hit;
        if (timer < coolDown)
        { return; }
        timer = 0;
        if (Physics.Raycast(cameraRay, out hit))
        {
            if (hit.transform.root.tag != "Player")
            {
                return;
            }
            Debug.Log("damage");
            PlayerControl target = hit.transform.root.GetComponent<PlayerControl>();
            if (target.teamID== this.teamID)
            { return; }
            target.GetComponent<Health>().TakeDamage(GameManager.Instance.VulcanDamage);
        }
    }
    [Command]
    void CmdFire()
    {
        //network identity "spawn"
        Debug.Log("cmdfire");
        RpcFire();
    }

    [ClientRpc]
    void RpcFire()
    {
        turrent.fxController.Fire();
    }

    [Command]
    void CmdStopFire()
    {
        RpcStopFire();
    }

    [ClientRpc]
    void RpcStopFire()
    {
        turrent.fxController.Stop();
    }
    /*
    private void Aim()
    {
        if (!crosshair.IsActive())
        {
            crosshair.gameObject.SetActive(true);
        }
        float radius = 0;
        if (crosshair.transform.childCount > 0)
        {
            if (crosshair.transform.childCount==0)
            {
                Debug.LogError("crosshair has no child");
                return;
            }
            radius = Vector3.Distance(new Vector3(), crosshair.transform.GetChild(0).localPosition);
        }
        Vector2 targetOnScreen;
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), playerCamera.WorldToScreenPoint(target.position),null, out targetOnScreen);
        bool inRange = Vector3.Distance(crosshair.transform.localPosition,new Vector3(targetOnScreen.x,targetOnScreen.y) ) < radius;
        bool inFront = Vector3.Dot(playerCamera.transform.forward, target.position - this.transform.position)>0;
        if (inRange&&inFront)
        {
            crosshair.color = Color.green;//new Color(125, 255, 125);
            canShoot = true;
        }
        else
        {
            crosshair.color = Color.white;
            canShoot = false;
        }

    }

    private void Shoot()
    {
        if (!target)
        {
            return;
        }
        GameObject missile = Instantiate(Resources.Load("Missile") as GameObject, muzzle.position, muzzle.rotation);
        missile.GetComponent<Missile>().target = target;
    }
    */
    void SetAutoCam()
    {
        UnityStandardAssets.Cameras.AutoCam autoCam;
        if (autoCam = Camera.main.transform.root.GetComponent<UnityStandardAssets.Cameras.AutoCam>())
        {
            autoCam.SetTarget(this.cameraTarget);
        }
    }
    
    void SetFreeLookCam()
    {
        CinemachineFreeLook freeLookCam;
        freeLookCam = GameObject.FindObjectOfType<CinemachineFreeLook>();
        if (freeLookCam)
        {
            freeLookCam.LookAt = this.cameraTarget;
            freeLookCam.Follow = this.cameraTarget;
        }
    }

    
    /*
    void setPositionSide(Transform go)
    {
        go.localPosition = new Vector3(1, 0, 0) * Screen.width / 2 * ((int)this.GetComponent<CarUserControl>().player - 0.5f) +
            new Vector3(0, go.localPosition.y, go.localPosition.z);
    }
    */
}
