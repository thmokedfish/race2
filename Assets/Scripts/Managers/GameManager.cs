using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cinemachine;
public class GameManager : NetworkBehaviour
{

    // public SyncListBool isAvailable;      //几种车型是否可选
    //private bool[] IsAvailable;
    public static GameManager Instance;
    //public Camera playerCamera;
    //public GameObject[] carPrefabs;
    private PlayerControl _localPlayer;
    public PlayerControl LocalPlayer { get { return _localPlayer; } set { _localPlayer = value; } }

    [HideInInspector] public MyNetworkManager networkManager;
    [HideInInspector] public NWH.VehiclePhysics.DesktopInputManager inputManager;
    [HideInInspector] public Team[] team = new Team[2];
    public GameObject boomEffect;
    public Forge3D.F3DFXType[] WeaponTypes;
    public int[] WeaponDamage;
    public Dictionary<Forge3D.F3DFXType, int> DamageDic = new Dictionary<Forge3D.F3DFXType, int>();
    public float BoxSpawnTime;
    public GameObject BoxPrefab;
    public Transform BoxPos;
    private void Awake()
    {
        Instance = this;
        Camera.main.gameObject.AddComponent(typeof(CinemachineBrain));
        networkManager = GameObject.Find("NetworkManager").GetComponent<MyNetworkManager>();
        inputManager = GameObject.Find("Scripts/VehicleManager").GetComponent<NWH.VehiclePhysics.DesktopInputManager>();
        Transform TeamManager = GameObject.Find("Scripts/TeamManager").transform;
        team[0] = TeamManager.GetChild(0).GetComponent<Team>();
        team[1] = TeamManager.GetChild(1).GetComponent<Team>();
        InitDamageDic();
    }
    private void InitDamageDic()
    {
        for (int i = 0; i < WeaponTypes.Length; i++)
        {
            if (i == WeaponDamage.Length) { break; }
            if (DamageDic.ContainsKey(WeaponTypes[i])) { continue; }
            DamageDic.Add(WeaponTypes[i], WeaponDamage[i]);
        }
    }

    #region BoomEffect
    public void PlayBoomEffect(Vector3 pos)
    {
        StartCoroutine(BoomEffect(pos));
    }
    private IEnumerator BoomEffect(Vector3 pos)
    {
        GameObject boom = Instantiate(boomEffect, pos, new Quaternion());
        yield return new WaitForSeconds(5);
        Destroy(boom);
    }
    #endregion

    #region PlayerRespawning
    public void StartRespawnTiming(int time, GameObject go)
    {
        StartCoroutine(RespawnTiming(time, go));
    }
    public IEnumerator RespawnTiming(int time, GameObject go)
    {
        for (; time > 0; time--)
        {
            UIManager.Instance.SetRespawnTiming(time);
            yield return new WaitForSeconds(1f);
        }
        CmdRespawn(go);
        UIManager.Instance.SetRespawnTiming(-1);
    }
    [Command]
    private void CmdRespawn(GameObject go)
    {
        Health health = go.GetComponent<Health>();
        health.RpcSetHealth(health.fullhealth);
        RpcRespawn(go);
    }

    [ClientRpc]
    private void RpcRespawn(GameObject go) /////////////////Need checkout
    {
        go.SetActive(true);
        PlayerControl player = go.GetComponent<PlayerControl>();
        if (!player) { return; }
        if (!player.isLocalPlayer) { return; }
        go.transform.position = player.spawnPoint.position;
        go.transform.rotation = player.spawnPoint.rotation;
    }
    #endregion

    #region BoxSpawning

    public void StartBoxTiming(int boxIndex)
    {
        CmdStartBoxTiming(boxIndex);
    }

    public void ServerStartGameBoxTiming()
    {
        RpcSetIcon(0, false);
        RpcSetIcon(1, false);
        StartCoroutine(BoxTiming(0));
        StartCoroutine(BoxTiming(1));
    }

    [Command]
    public void CmdStartBoxTiming(int index)
    {
        RpcSetIcon(index, false);
        StartCoroutine(BoxTiming(index));
    }

    IEnumerator BoxTiming(int index)
    {
        yield return new WaitForSeconds(BoxSpawnTime);
        Vector3 pos = BoxPos.GetChild(index).position;
        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit))
        {
            if (hit.transform.tag == "Ground")
            {
                pos = hit.point + new Vector3(0, 0.7f, 0);
            }
        }
        GameObject box = Instantiate(BoxPrefab,pos, BoxPos.GetChild(index).rotation);
        box.GetComponent<WeaponBox>().index = index;
        NetworkServer.Spawn(box);
        RpcSetIcon(index, true);
    }
    [ClientRpc]
    void RpcSetIcon(int index,bool isActive)
    {
        SwitchBoxIcon(index, isActive);
    }
    private void SwitchBoxIcon(int index, bool isActive)
    {
        UIManager.Instance.ActiveBoxIcons[index].SetActive(isActive);
        UIManager.Instance.InactiveBoxIcons[index].SetActive(!isActive);
    }

    #endregion 

}
