using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text winText;
    public Image crosshair;
    public Slider hpSlider;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        winText = Instantiate(winText,transform)as Text;
        winText.gameObject.SetActive(false);
        crosshair = Instantiate(crosshair, transform) as Image;
        crosshair.gameObject.SetActive(false);
        hpSlider = Instantiate(hpSlider,transform) as Slider;
        hpSlider.gameObject.SetActive(false);
    }

}
