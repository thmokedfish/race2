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
            /*
            if (autoCreatePlayer)
            {
                Debug.LogWarning("auto create player");
                ClientScene.AddPlayer(0);
            }
            */
        }
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (carPrefabs.Length==0)
        {
            if (LogFilter.logError) { Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object."); }
            return;
        }

        if (playerControllerId < conn.playerControllers.Count && conn.playerControllers[playerControllerId].IsValid && conn.playerControllers[playerControllerId].gameObject != null)
        {
            if (LogFilter.logError) { Debug.LogError("There is already a player at that playerControllerId for this connections."); }
            return;
        }

        GameObject player;
        Transform startPos = GetStartPosition();
        GameObject prefab;
        if(GameObject.FindObjectOfType<PlayerControl>())
        {
            prefab = carPrefabs[1];
        }
        else
        {
            prefab = carPrefabs[0];
        }
        if (prefab.GetComponent<NetworkIdentity>() == null)
        {
            if (LogFilter.logError) { Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab."); }
            return;
        }

        if (startPos != null)
        {
            player = (GameObject)Instantiate(prefab, startPos.position, startPos.rotation);
        }
        else
        {
            player = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))  //模拟选择车型触发
        {
            customAddplayer();
        }
    }
    public void customAddplayer()
    {

        GameObject car;
        int playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
        Debug.Log("playerCount " + playerCount);
        if (playerCount > 1)
        {
            return;
        }
        /*
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
        */
        ClientScene.AddPlayer(0);
    }
}
