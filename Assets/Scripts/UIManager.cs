using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text winText;
    public Image crosshair;
    public Slider hpSlider;
    public Image timingImage;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
    }

    public void setWinner(int index)
    {
        if(index==-1)
        {
            winText.text = "Draw!";
        }
        else
        {
            winText.text = "Player" + index + " wins!";
        }
        winText.gameObject.SetActive(true);
    }

}
