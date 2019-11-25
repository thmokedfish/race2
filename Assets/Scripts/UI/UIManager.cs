using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text winText;
    public GameObject crosshair;
    public Slider hpSlider;
    public Image timingImage;
    public Text team1Score;
    public Text team2Score;
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
    public void setTeamScore(int team1,int team2)
    {
        team1Score.text = "队伍1" + team1.ToString();
        team2Score.text = "队伍2" + team2.ToString();
    }

}
