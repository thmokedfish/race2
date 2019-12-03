using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cinemachine;
public class GameManager:MonoBehaviour
{

   // public SyncListBool isAvailable;      //几种车型是否可选
    //private bool[] IsAvailable;
    public static GameManager Instance;
    //public Camera playerCamera;
    public GameObject[] carPrefabs;
    public PlayerControl localPlayer;
    [HideInInspector]public MyNetworkManager networkManager;
    public Team[] team = new Team[2];
    public int VulcanDamage;
    private void Awake()
    {
        Instance = this;
        Camera.main.gameObject.AddComponent(typeof(CinemachineBrain));
        networkManager = GameObject.Find("NetworkManager").GetComponent<MyNetworkManager>();
        Transform TeamManager = GameObject.Find("Scripts/TeamManager").transform;
        team[0] = TeamManager.GetChild(0).GetComponent<Team>();
        team[1] = TeamManager.GetChild(1).GetComponent<Team>();
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
