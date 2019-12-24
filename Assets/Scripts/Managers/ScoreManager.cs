using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int TotalSeconds = 300;
    public GameObject ballPrefab;
    public int BallScore;
    //private List<Text> scoreText = new List<Text>();      //每当有玩家连接，add
    private Image timingImage;
    private List<Sprite> timingSprites = new List<Sprite>(5);
    public float radius;
    public Button startButton;
    private Text winText;
    public Team[] team = new Team[2];
    //[SyncVar]public int nextPrefabID;
    //[SyncVar]public int nextTeamID;
    void Awake()
    {
        Instance = this;
        //timingSprite赋初值 
        int i = 0;
        Sprite s;
        while (true)
        {
            i++;
            s = Resources.Load<Sprite>("Textures/" + i);
            if(s==null)
            { break; }
            timingSprites.Add(s);
        }
    }
    public override void OnStartServer()
    {
        startButton=Instantiate(this.startButton, UIManager.Instance.transform);
        startButton.onClick.AddListener(ServerStartGame);
        BallScore = 1;
    }

    public override void OnStartClient()
    {
        timingImage = UIManager.Instance.TimingImage;
        winText = UIManager.Instance.WinText;
        team = GameManager.Instance.team;
        UIManager.Instance.SetTeamScoreUI(team[0]);
        UIManager.Instance.SetTeamScoreUI(team[1]);
    }

    public void ServerGetBall(int teamID, int playerID)
    {
        GetPoint(teamID, playerID, BallScore);
        //StartCoroutine(StartTiming());
        RpcStartBallTiming();

        RpcSetScoreText(teamID);
    }
    public void GetPoint(int teamID,int playerID,int score)
    {
        team[teamID].player[playerID].score += score;
        team[teamID].teamScore += score;
    }

    public void RemovePlayer(int teamID,int playerID)
    {
        //remove player from list and refresh score text
        team[teamID].RemovePlayer(playerID);
        RpcSetScoreText(teamID);
    }
    public void RefreshScoreText(int playerID)
    {
        //scoreText[playerID].text = "Player" + playerID + ": " + playerScore[playerID];
    }
    private void ServerStartGame()
    {
        GameManager.Instance.ServerStartBoxTiming();
        // StartCoroutine(StartTiming());
        RpcStartBallTiming();
        RpcStartGameTiming();
    }
    [ClientRpc]
    void RpcStartGameTiming()
    {
        UIManager.Instance.GameTimingText.GetComponent<Race.Timing>().ClientStartGameTiming(TotalSeconds);
    }

    [ClientRpc]
    void RpcStartBallTiming()
    {
        Debug.Log("rpc starttimging");
        StartCoroutine(StartBallTiming());
    }
    IEnumerator StartBallTiming()
    {
        timingImage.gameObject.SetActive(true);
        for (int i =timingSprites.Count-1; i >=0; i--)
        {
            timingImage.sprite = timingSprites[i];
            yield return new WaitForSeconds(1);
        }
        timingImage.gameObject.SetActive(false);

        if (isServer)                             //server calling create ball
        {
            ServerCreateBall();
        }
    }
    void ServerCreateBall()
    {
        //radius 128
        Vector3 ballPosition = new Vector3((Random.value*2 -1) * radius+this.transform.position.x, 100, (Random.value * 2 - 1) * radius+this.transform.position.z);
        RaycastHit hit;
        if (Physics.Raycast(ballPosition, Vector3.down, out hit))
        {
            if (hit.transform.tag == "Ground")
            {
                ballPosition = hit.point + new Vector3(0,1f, 0);
            }
        }
        //GameObject ball = Instantiate(Resources.Load<GameObject>("PointBall"),ballPosition,new Quaternion ());
        GameObject ball = Instantiate(ballPrefab, ballPosition, new Quaternion());
        NetworkServer.Spawn(ball);//spawn ball for clients
        //ball.GetComponent<PointBall>().ballTrigged.AddListener();
    }
    public void SetWinner(int index)
    {
        if (index == -1)
        {
            winText.text = "Draw!";
        }
        else
        {
            winText.text = "Player" + index + " wins!";
        }
        winText.gameObject.SetActive(true);
    }

    public int GetTeamScore(int teamIndex)
    {
        return team[teamIndex].teamScore;
    }

    [ClientRpc]
    void RpcSetScoreText(int teamID)
    {
        UIManager.Instance.SetTeamScoreUI(team[teamID]);
    }

    public void ServerDropPoint(int teamID,int playerID)
    {
        PlayerControl player = team[teamID].player[playerID];
        int dropped = player.score / 2;
        player.score -= dropped;
        team[teamID].teamScore -= dropped;
        //spawn point box
        RpcSetScoreText(teamID);
    }


    public void ServerEndGame()
    {
        if (!isServer) { return; }
        int winner = -1;
        if(team[0].teamScore>team[1].teamScore)
        {
            winner = 0;
        }
        else if(team[0].teamScore<team[1].teamScore)
        {
            winner = 1;
        }
        RpcSetWinner(winner);
    }


    void RpcSetWinner(int team)
    {
        UIManager.Instance.WinPanel.SetActive(true);
        switch(team)
        {
            case 0:
                winText.text = "队伍一胜利";
                break;
            case 1:
                winText.text = "队伍二胜利";
                break;
            default:
                winText.text = "平局";
                break;
        }
        
    }
}
