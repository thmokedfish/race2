using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
public class PointBall : NetworkBehaviour
{
    public UnityEvent ballTrigged = new UnityEvent();
    void OnTriggerEnter(Collider collider)
    {

        PlayerControl playerControl;
        Debug.Log(collider.name);
        if ((playerControl = collider.transform.root.GetComponent<PlayerControl>()))
        {
            Debug.Log("trigger enter");
            if (isServer)
            {
                ScoreManager.Instance.GetBall(playerControl.playerID);
            }
            Destroy(this.gameObject);
        }
    }
}
