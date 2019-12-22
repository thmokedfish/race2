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
    public Text[] teamScoresText = new Text[2];
    public Text respawnTimingText;
    public Transform DamageText;
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
    public void setTeamScoreUI(Team team)
    {
        teamScoresText[team.teamIndex].text = "队伍" + (team.teamIndex + 1).ToString() + "得分: " + team.teamScore.ToString();
        setMemberScoreUI(team);
    }

    public void setTeamScoreUI(int teamIndex)
    {
        Team team=GameManager.Instance.team[teamIndex];
        teamScoresText[teamIndex].text= "队伍" + (teamIndex + 1).ToString() + "得分: " + team.teamScore.ToString();
        setMemberScoreUI(team);
    }

    void setMemberScoreUI(Team team)
    {
        Text score1 = teamScoresText[team.teamIndex].transform.GetChild(0).GetComponent<Text>();
        Text score2 = teamScoresText[team.teamIndex].transform.GetChild(1).GetComponent<Text>();
        if (team.player[0] == null)
        {
            score1.text = "";
        }
        else
        {
            score1.text = team.player[0].score.ToString();
        }
        if (team.player[1] == null)
        {
            score2.text = "";
        }
        else
        {
            score2.text = team.player[1].score.ToString();
        }
    }

    public void setRespawnTiming(int time)
    {
        if(time==-1)
        {
            respawnTimingText.text = "";
        }
       else
        {
            respawnTimingText.text = time.ToString();
        }
    }
}
