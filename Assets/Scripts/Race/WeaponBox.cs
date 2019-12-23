using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    public Forge3D.F3DFXType WeaponType;
    private void OnTriggerEnter(Collider other)
    {
        PlayerControl player = other.transform.root.GetComponent<PlayerControl>();
        if (player)
        {
            if (!player.isLocalPlayer) { return; }
            player.SwitchWeapon(WeaponType);
        }
    }
}
