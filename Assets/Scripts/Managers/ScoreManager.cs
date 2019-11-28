using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private int nextBallScore;
    private List<Text> scoreText = new List<Text>();      //每当有玩家连接，add++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private Image timingImage;
    private List<Sprite> timingSprites = new List<Sprite>(5);
    public float radius;
    public int totalBallCount;
    public Button startButton;
    private Text winText;
    public Team[] team = new Team[2];
    [SyncVar]public int nextPrefabID;
    [SyncVar]public int nextTeamID;
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
        startButton.onClick.AddListener(StartGame);
        nextBallScore = 1;
    }

    private void Start()
    {
        timingImage = UIManager.Instance.timingImage;
        winText = UIManager.Instance.winText;
        team = GameManager.instance.team;
    }


    public void GetBall(int teamID,int playerID)
    {
        //GetPoint(playerID, nextBallScore);
        totalBallCount--;
        if (totalBallCount <= 0)
        {
           // SetWinner
        }
        else
        {
            nextBallScore++;
            //StartCoroutine(StartTiming());
            RpcStartTiming();
        }
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
    }
    public void RefreshScoreText(int playerID)
    {
        //scoreText[playerID].text = "Player" + playerID + ": " + playerScore[playerID];
    }
    private void StartGame()
    {
        // StartCoroutine(StartTiming());
        RpcStartTiming();
    }
    [ClientRpc]
    void RpcStartTiming()
    {
        Debug.Log("rpc starttimging");
        StartCoroutine(StartTiming());
    }
    IEnumerator StartTiming()
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
        Vector3 ballPosition = new Vector3((Random.value*2 -1) * radius+this.transform.position.x, 25, (Random.value * 2 - 1) * radius+this.transform.position.z);
        RaycastHit hit;
        if (Physics.Raycast(ballPosition, Vector3.down, out hit))
        {
            if (hit.transform.tag == "Ground")
            {
                ballPosition = hit.point + new Vector3(0, 1.5f, 0);
            }
        }
        GameObject ball = Instantiate(Resources.Load<GameObject>("PointBall"),ballPosition,new Quaternion ());
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
    void RpcSetScoreText(int team1,int team2)
    {
        UIManager.Instance.setTeamScore(team1, team2);
    }
}
