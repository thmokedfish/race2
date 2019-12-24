using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Text WinText;
    public GameObject Crosshair;
    public Slider hpSlider;
    public Image TimingImage;
    public Text[] TeamScoresText = new Text[2];
    public Text RespawnTimingText;

    [Header("ShowDamageUI")]
    public Transform DamageText;
    public float DamageTextDisappearTime;
    public float DamageTextMoveRate;

    [Header("MiniMap")]
    //public GameObject MiniMapCamera;
    public Image MiniMap;
    public Transform PlayerIcon;
    public Transform MapCorner;//按照该点与玩家的向量差计算位置。目前是取边角
    public float Diameter; //计算图片width 与实际地图直径的比。若为正方形地图，则等于边长，圆形则等于直径
    private Transform LocalTransform;
    private Vector3 LocalRoot;

    [Header("BoxIcon")]
    public GameObject[] ActiveBoxIcons;
    public GameObject[] InactiveBoxIcons;

    [Header("ChoosingCar")]
    public GameObject ChoosingPanel;

    private void Awake()
    {
        Instance = this;
        //MiniMapCamera = Instantiate(MiniMapCamera);
        LocalRoot = new Vector3(-MiniMap.rectTransform.sizeDelta.x / 2, -MiniMap.rectTransform.sizeDelta.y / 2, 0);
        Debug.Log(LocalRoot);
        ChoosingPanel.SetActive(false);
    }

    private void Update()
    {
        RefreshMiniMap();
    }

    private void RefreshMiniMap()
    {
        if(!LocalTransform)
        {
            PlayerControl local = GameManager.Instance.LocalPlayer;
            if (!local) { return; }
            LocalTransform = local.transform;
        }
        if (!MapCorner)
        {
            Debug.LogError("mapCorner is null!");
            return;
        }
        Vector3 worldDiffXZ = LocalTransform.position - MapCorner.position;
        float mapDistance = MiniMap.rectTransform.sizeDelta.x;
        
        float proportion = mapDistance / Diameter;
        // Debug.Log(LocalRoot);
        PlayerIcon.localPosition = LocalRoot + new Vector3(worldDiffXZ.x, worldDiffXZ.z) * proportion;
        PlayerIcon.localEulerAngles = Vector3.forward * (90 - Camera.main.transform.eulerAngles.y);
    }

    public void SetWinner(int index)
    {
        if(index==-1)
        {
            WinText.text = "Draw!";
        }
        else
        {
            WinText.text = "Player" + index + " wins!";
        }
        WinText.gameObject.SetActive(true);
    }

    #region ScoreUI
    public void SetTeamScoreUI(Team team)
    {
        TeamScoresText[team.teamIndex].text = "队伍" + (team.teamIndex + 1).ToString() + "得分: " + team.teamScore.ToString();
        SetMemberScoreUI(team);
    }

    public void SetTeamScoreUI(int teamIndex)
    {
        Team team=GameManager.Instance.team[teamIndex];
        TeamScoresText[teamIndex].text= "队伍" + (teamIndex + 1).ToString() + "得分: " + team.teamScore.ToString();
        SetMemberScoreUI(team);
    }

    void SetMemberScoreUI(Team team)
    {
        Text score1 = TeamScoresText[team.teamIndex].transform.GetChild(0).GetComponent<Text>();
        Text score2 = TeamScoresText[team.teamIndex].transform.GetChild(1).GetComponent<Text>();
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
    #endregion
    public void SetRespawnTiming(int time)
    {
        if(time==-1)
        {
            RespawnTimingText.text = "";
        }
       else
        {
            RespawnTimingText.text = time.ToString();
        }
    }

}
