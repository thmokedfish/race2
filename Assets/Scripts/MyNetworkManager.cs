using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
public class MyNetworkManager : NetworkManager
{
    public GameObject[] carPrefabs;
    public override void OnClientConnect(NetworkConnection conn)
    {
        if (!clientLoadedScene)
        {
            // Ready/AddPlayer is usually triggered by a scene load completing. if no scene was loaded, then Ready/AddPlayer it here instead.
            ClientScene.Ready(conn);
            if (autoCreatePlayer)
            {
                Debug.LogWarning("auto create player");
                ClientScene.AddPlayer(0);
            }
            else
            {
                //customOnConnect();
            }
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            customOnConnect();
        }
    }
    public void customOnConnect()
    {

        GameObject car;
        int playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
        Debug.Log("playerCount " + playerCount);
        if (playerCount > 1)
        {
            return;
        }
        if (playerCount == 1)
        {
           //playerPrefab = carPrefabs[1 - GameObject.FindObjectOfType<PlayerControl>().prefabIndex];
            playerPrefab = carPrefabs[1];
            //car = Instantiate(carPrefabs[1 - GameObject.FindObjectOfType<PlayerControl>().prefabIndex]);
        }
        else
        {
            playerPrefab = carPrefabs[0];
            // car = Instantiate(carPrefabs[0]);
        }
        //localPlayer = car.GetComponent<PlayerControl>();

        ClientScene.AddPlayer(0);
    }
}
