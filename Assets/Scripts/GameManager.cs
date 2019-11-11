using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cinemachine;
public class GameManager:MonoBehaviour
{

   // public SyncListBool isAvailable;      //几种车型是否可选
    //private bool[] IsAvailable;
    public static GameManager instance;
    //public Camera playerCamera;
    public GameObject[] carPrefabs;
    public PlayerControl localPlayer;
    private void Awake()
    {
        instance = this;
        Camera.main.gameObject.AddComponent(typeof(CinemachineBrain));
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
