using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class Health : NetworkBehaviour
{
    [SyncVar]public int health;
    public int fullhealth;
    private Slider hpSlider;

    public override void OnStartLocalPlayer()
    {
        hpSlider = UIManager.Instance.hpSlider;
        if(!hpSlider)
        {
            Debug.Log("no hpSlider");
            return;
        }
        hpSlider.gameObject.SetActive(true);
        health = fullhealth;
    }

    public float dropAmount = 1;
    void Update()
    {
        if(!isLocalPlayer)
        { return; }
        if (hpSlider.value > health / (float)fullhealth)
        {
            hpSlider.value -= dropAmount * Time.deltaTime;
        }
        else
        {
            hpSlider.value = health / (float)fullhealth;
        }
    }
    public void TakeDamage(int damage)//only on server
    {
        if(!isServer)
        { return; }
        Debug.Log("take");
        health -= damage;
        if(health<=0)
        { Die(); }
    }
    public void Die()
    {
        health = 0;
    }
}
