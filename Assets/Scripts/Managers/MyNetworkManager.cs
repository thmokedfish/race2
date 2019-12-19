using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
public class MyNetworkManager : NetworkManager
{
    public GameObject[] carPrefabs;
    public GameObject StartPositions;
    public NetworkConnection myConn;

    private void Awake()
    {
        StartPositions = Instantiate(StartPositions);
    }
    public class CustomMessage :MessageBase
    {
        public int prefabID;
        public int teamID;
        public int spawnPointID;
    }
    public override void OnClientConnect(NetworkConnection conn) //call on client too
    {
        Debug.Log("client connect");
        myConn = conn;
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
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        Debug.Log("with extra message");
        if (carPrefabs.Length == 0)
        {
            if (LogFilter.logError) { Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object."); }
            return;
        }

        if (playerControllerId < conn.playerControllers.Count && conn.playerControllers[playerControllerId].IsValid && conn.playerControllers[playerControllerId].gameObject != null)
        {
            if (LogFilter.logError) { Debug.LogError("There is already a player at that playerControllerId for this connections."); }
            return;
        }
        CustomMessage message = extraMessageReader.ReadMessage<CustomMessage>();

        GameObject player;
        //Transform startPos = GetStartPosition();
        GameObject prefabToSpawn = carPrefabs[message.prefabID];

        if (!prefabToSpawn)
        {
            Debug.LogError("prefab to spawn is null");
        }
        if (prefabToSpawn.GetComponent<NetworkIdentity>() == null)
        {
            if (LogFilter.logError) { Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab."); }
            return;
        }
        Transform startPos;
        startPos = StartPositions.transform.GetChild(message.spawnPointID);
        
        if (startPos != null)

        {
            player = (GameObject)Instantiate(prefabToSpawn, startPos.position, startPos.rotation);
        }
        else
        {
            Debug.LogError("startPositions loss");
            player = (GameObject)Instantiate(prefabToSpawn,Vector3.zero,new Quaternion());
        }
        player.GetComponent<PlayerControl>().teamID = message.teamID;
        player.GetComponent<PlayerControl>().spawnPoint = StartPositions.transform.GetChild(message.spawnPointID).position;
        //ScoreManager.Instance.添加分数
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        //maybe call rpc team.addplayer here? Try call it on player if not work
    }
    /*
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
        GameObject prefabToSpawn = carPrefabs[nextPrefabID];
        
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
        player.GetComponent<PlayerControl>().teamID = nextTeamID;
        //ScoreManager.Instance.添加分数
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        //maybe call rpc team.addplayer here? Try call it on player if not work
    }*/


    public void customAddplayer(int prefabID,int teamID,int spawnPointID)
    {
        CustomMessage message = new CustomMessage();
        message.prefabID = prefabID;
        message.teamID = teamID;
        message.spawnPointID = spawnPointID;
       //传参用cmd,传下标
        ClientScene.AddPlayer(myConn,0,message);  //send message to call OnServerAddPlayer
        
    }

    public override void OnStopClient()
    {
        //更新playerID,playerscores
        Debug.Log("on stop client"); //call on client
        if (GameManager.Instance.localPlayer)
        {
            int teamid = GameManager.Instance.localPlayer.teamID;
            int playerid = GameManager.Instance.localPlayer.playerID;
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
    }
    public override void OnStartServer()
    {
    }
    public override void OnStartClient(NetworkClient client)
    {
        //call on client
        Debug.Log("start client");
    }

    public override void OnServerConnect(NetworkConnection conn) //when client connect,call on server too !
    {
        //Debug.Log("server connect");
    }

}
