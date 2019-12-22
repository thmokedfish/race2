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
    private GameObject Smoke;
    private Transform DamageText;

    private void Awake()
    {
        Smoke = transform.Find("Smoke").gameObject;
    }
    private void Start()
    {
        DamageText = UIManager.Instance.DamageText;
    }
    public override void OnStartLocalPlayer()
    {
        hpSlider = UIManager.Instance.hpSlider;
        if (!hpSlider)
        {
            Debug.Log("no hpSlider");
            return;
        }
        hpSlider.gameObject.SetActive(true);
        health = fullhealth;
    }
    private void OnEnable()
    {
        Smoke.SetActive(false);
        Brightness brightness = Camera.main.GetComponent<Brightness>();
        brightness.saturation = 1;
    }

    public float dropAmount = 1;
    void Update()
    {
        if (!isLocalPlayer)
        { return; }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Die();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            TakeDamage(1);
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
    public void TakeDamage(int damage)
    {
        //ui 报数
        Forge3D.F3DPoolManager.Pools["GeneratedPool"].Spawn(DamageText.transform,UIManager.Instance.transform, this.transform.position,new Quaternion());
        CmdTakeDamage(damage);
    }

    [Command]
    public void CmdTakeDamage(int damage)//only on server
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
        Debug.Log("health changed" + health);
        if (health < fullhealth / 2)
        {
            Smoke.SetActive(true);
        }
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()      //rpc called
    {
        Smoke.SetActive(false);
        GameManager.Instance.PlayBoomEffect(this.transform.position);
        if (isLocalPlayer)
        {
            Brightness brightness = Camera.main.GetComponent<Brightness>();
            brightness.saturation = 0;
            GameManager.Instance.StartRespawnTiming(respawnTime, this.gameObject); //start respawn timing.
        }
        if (isServer)
        {
            PlayerControl playerControl = this.GetComponent<PlayerControl>();
            ScoreManager.Instance.ServerDropPoint(playerControl.teamID, playerControl.playerID);
        }
        this.gameObject.SetActive(false);
    }

}
