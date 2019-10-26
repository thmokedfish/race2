using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBall : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if(collider.name=="PlayerCollider1")
        {
            ScoreManager.Instance.getBall(0);
            Destroy(this.gameObject);
        }
        if (collider.name == "PlayerCollider2")
        {
            ScoreManager.Instance.getBall(1);
            Destroy(this.gameObject);
        }
    }
}
