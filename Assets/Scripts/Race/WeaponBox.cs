using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    // public Forge3D.F3DFXType WeaponType;
    public int index;
    private void OnTriggerEnter(Collider other)  //once,for local player
    {
        PlayerControl player = other.transform.root.GetComponent<PlayerControl>();
        if (player)
        {
            if (!player.isLocalPlayer) { return; }
            // player.SwitchWeapon(WeaponType);
            player.UpGradeWeapon();
            GameManager.Instance.StartBoxTiming(index);
            Destroy(this.gameObject);
        }
    }
}
