using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class Health : NetworkBehaviour
{
    public int health;
    public int fullhealth;
    public int respawnTime;
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
    private void OnEnable()
    {
        Brightness brightness = Camera.main.GetComponent<Brightness>();
        brightness.saturation = 1;
    }

    public float dropAmount = 1;
    void Update()
    {
        if(!isLocalPlayer)
        { return; }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Die();
        }
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
        health -= damage;
        if (health < 0)
            health = 0;
        RpcSetHealth(health);
    }
    [ClientRpc]
    public void RpcSetHealth(int health)
    {
        this.health = health;
        OnHealthChanged(health);
    }

    void OnHealthChanged(int health) //SHOULD be called from both
    {
        Debug.Log("health changed"+health);
        if(health<=0)
        {
            Die();
        }
    }

    void Die()      //rpc called
    {
        //play boom animation
        if (isLocalPlayer)
        {
            Brightness brightness = Camera.main.GetComponent<Brightness>();
            brightness.saturation = 0;
            ScoreManager.Instance.StartRespawnTiming(respawnTime, this.gameObject); //start respawn timing.
        }
        if (isServer)
        {
            PlayerControl playerControl = this.GetComponent<PlayerControl>();
            ScoreManager.Instance.ServerDropPoint(playerControl.teamID, playerControl.playerID);
        }
        this.gameObject.SetActive(false);
    }
    
}
