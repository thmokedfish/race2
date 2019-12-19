using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Team :NetworkBehaviour
{
    [Range(0,1)]
    public int teamIndex;//0 or 1
    [SyncVar]
    public int teamScore;
    // SyncListInt playerScore = new SyncListInt();

    //public List<Health> respawnWaiting = new List<Health>();

    public PlayerControl[] player = new PlayerControl[2];
   public  int playerCount { get; private set; }
    public void RemovePlayer(int playerID)
    {
        if (playerID>=playerCount)
        {
            Debug.LogError("playerID out of index");
            return;
        }
        player[0] = player[1];
        player[1] = null;
        if (player[0])
        {
            player[0].playerID = 0;
        }
        playerCount--;
    }

    public void AddPlayer(PlayerControl newplayer)
    {
        if(playerCount>=2)
        {
            Debug.LogError("team is full");
            return;
        }

        newplayer.playerID = playerCount;
        newplayer.teamID = this.teamIndex;
        newplayer.score = 0;

        player[playerCount] = newplayer;

        playerCount++;
    }

   
}
