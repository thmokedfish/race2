using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class Health : MonoBehaviour
{
    public int health;
    public int fullhealth;
    private Slider hpSlider;
    void Start()
    {
        GameObject hpSlidergo;
        if (!(hpSlidergo = GameObject.Find("Canvas/HPSlider" +( (int)this.GetComponent<CarUserControl>().player+1))))
        {
            Debug.LogError("Canvas/HPSlider" + ((int)this.GetComponent<CarUserControl>().player + 1) + " not exist");
            return;
        }
        hpSlider =hpSlidergo.GetComponent<Slider>();
        setPositionSide(hpSlider.transform);
        health = fullhealth;
    }

    void setPositionSide(Transform go)
    {
        go.localPosition = new Vector3(1, 0, 0) * Screen.width / 2 * ((int)this.GetComponent<CarUserControl>().player - 0.5f) +
            new Vector3(0, go.localPosition.y, go.localPosition.z);
    }

    public float dropAmount = 1;
    void Update()
    {
        if (hpSlider)
        {
            if(hpSlider.value > health /(float) fullhealth)
            {
                hpSlider.value -= dropAmount * Time.deltaTime;
            }
            if(hpSlider.value<health/(float)fullhealth)
            {
                hpSlider.value = health / (float)fullhealth;
            }
        }
        else
        {
            Debug.Log("no slider");
        }
    }
}
