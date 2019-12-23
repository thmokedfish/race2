using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cinemachine;
public class GameManager:NetworkBehaviour
{

   // public SyncListBool isAvailable;      //几种车型是否可选
    //private bool[] IsAvailable;
    public static GameManager Instance;
    //public Camera playerCamera;
    //public GameObject[] carPrefabs;
    public PlayerControl localPlayer;
    [HideInInspector]public MyNetworkManager networkManager;
    [HideInInspector] public NWH.VehiclePhysics.DesktopInputManager inputManager;
    [HideInInspector]public Team[] team = new Team[2];
    public GameObject boomEffect;
    public Forge3D.F3DFXType[] WeaponTypes;
    public int[] WeaponDamage;
    public Dictionary<Forge3D.F3DFXType, int> DamageDic = new Dictionary<Forge3D.F3DFXType, int>();
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
    public void PlayBoomEffect(Vector3 pos)
    {
        StartCoroutine(BoomEffect(pos));
    }
    private IEnumerator BoomEffect(Vector3 pos)
    {
        GameObject boom = Instantiate(boomEffect, pos,new Quaternion());
        yield return new WaitForSeconds(5);
        Destroy(boom);
    }
    public void StartRespawnTiming(int time, GameObject go)
    {
        StartCoroutine(RespawnTiming(time, go));
    }
    public IEnumerator RespawnTiming(int time, GameObject go)
    {
        for (; time > 0; time--)
        {
            UIManager.Instance.setRespawnTiming(time);
            yield return new WaitForSeconds(1f);
        }
        CmdRespawn(go);
        UIManager.Instance.setRespawnTiming(-1);
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
        PlayerControl player= go.GetComponent<PlayerControl>();
        if (!player) { return; }
        if (!player.isLocalPlayer) { return; }
        go.transform.position = player.spawnPoint.position;
        go.transform.rotation = player.spawnPoint.rotation;
    }


    /*
    public override void OnStartClient()
    {
        GameObject car;
        NetworkManager networkManager = GameObject.FindObjectOfType<NetworkManager>();
        int playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
        if (playerCount > 1)
        {
            return;
        }
        if (playerCount == 1)
        {
            networkManager.playerPrefab = carPrefabs[1 - GameObject.FindObjectOfType<PlayerControl>().prefabIndex];
            //car = Instantiate(carPrefabs[1 - GameObject.FindObjectOfType<PlayerControl>().prefabIndex]);
        }
        else
        {
            networkManager.playerPrefab = carPrefabs[0];
            // car = Instantiate(carPrefabs[0]);
        }
        //localPlayer = car.GetComponent<PlayerControl>();

        ClientScene.AddPlayer(0);
    }
    */
}
