using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
public class PointBall : NetworkBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        PlayerControl playerControl = collider.transform.root.GetComponent<PlayerControl>();
        Debug.Log(collider.name);
        if (playerControl)
        {
            Debug.Log("trigger enter");
            if (isServer)
            {
                ScoreManager.Instance.GetBall(playerControl.teamID, playerControl.playerID);
            }
            Destroy(this.gameObject);
        }
    }
}
