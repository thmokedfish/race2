using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
public class MyNetworkManager : NetworkManager
{
    public GameObject[] carPrefabs;
    private GameObject prefabToSpawn;
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
        if(!prefabToSpawn)
        {
            Debug.LogError("prefab to spawn is null");
        }
        if (prefabToSpawn.GetComponent<NetworkIdentity>() == null)
        {
            if (LogFilter.logError) { Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab."); }
            return;
        }

        if (startPos != null)
        {
            player = (GameObject)Instantiate(prefabToSpawn, startPos.position, startPos.rotation);
        }
        else
        {
            player = (GameObject)Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
        }
        //为playerID与teamID赋值
        //ScoreManager.Instance.添加分数
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.I))  //模拟选择车型触发
        //{
         //   customAddplayer(carPrefabs[0]);
       // }
    }
    public void customAddplayer(GameObject player,int teamID)
    {

        int playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
        Debug.Log("playerCount " + playerCount);
        this.prefabToSpawn = player;
        ClientScene.AddPlayer(0);  //send message to call OnServerAddPlayer
    }

    public override void OnStopClient()
    {
        //更新playerID,playerscores
        Debug.Log("on stop client");
        if (GameManager.instance.localPlayer)
        {
            int teamid = GameManager.instance.localPlayer.teamID;
            int playerid = GameManager.instance.localPlayer.playerID;
            ScoreManager.Instance.RemovePlayer(teamid, playerid);
        } 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public override void OnStartHost()
    {
        Debug.Log("on start host");
    }
    public override void OnStartServer()
    {
        Debug.Log("on start server");
    }
    public override void OnStartClient(NetworkClient client)
    {
        Debug.Log("on start client");
    }

}
