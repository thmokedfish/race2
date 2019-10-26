using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _Instance;
    public static ScoreManager Instance
    {
        get { return _Instance; }
    }
    private int nextBallScore;
    private float[] playerScore;
    private Text[] scoreText;
    public Image timingImage;
    public Sprite[] timingSprite;
    public Vector3 centerPoint;
    public float radius;
    public int totalBallCount;
    private Text winText;
    void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        // ballRefreshTiming = GameObject.Find("Canvas/RefreshTiming").GetComponent<Slider>();
        //ballRefreshTiming.gameObject.SetActive(false);
        newInitiate();
        varInitiate();
        refreshScoreText();
        timingImage.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        StartCoroutine(nextBall());
    }

    private void newInitiate()
    {
        scoreText = new Text[2];
        playerScore = new float[2];
        scoreText[0] = GameObject.Find("Canvas/Score1").GetComponent<Text>();
        scoreText[1] = GameObject.Find("Canvas/Score2").GetComponent<Text>();
        winText = GameObject.Find("Canvas/WinText").GetComponent<Text>();
    }
    private void varInitiate()
    {
        nextBallScore = 1;
        playerScore[0] = 0;
        playerScore[1] = 0;
    }
    public void getBall(int playernum)
    {
        getScore(playernum, nextBallScore);
        totalBallCount--;
        if (totalBallCount <= 0)
        {
            if (playerScore[0] > playerScore[1])
            {
                winText.text = "Player1 Win!";
            }
            else if (playerScore[0] < playerScore[1])
            {
                winText.text = "Player2 Win!";
            }
            else if (playerScore[0] == playerScore[1])
            {
                winText.text = "Draw!";
            }
            winText.gameObject.SetActive(true);
        }
        else
        {
            nextBallScore++;
            StartCoroutine(nextBall());
        }
    }
    public void getScore(int playernum,int score)
    {
        playerScore[playernum] +=score;
        refreshScoreText();
    }
    public void refreshScoreText()
    {
        scoreText[0].text = "player1 score:" + playerScore[0];
        scoreText[1].text = "player2 score:" + playerScore[1];
    }
    IEnumerator nextBall()
    {
        timingImage.gameObject.SetActive(true);
        for (int i =0; i <timingSprite.Length; i++)
        {
            timingImage.sprite = timingSprite[i];
            yield return new WaitForSeconds(1);
        }
        timingImage.gameObject.SetActive(false);
        createBall();
    }
    void createBall()
    {
        Vector3 ballPosition = new Vector3((Random.value*2 -1) * radius+centerPoint.x, 25, (Random.value * 2 - 1) * radius+centerPoint.z);
        RaycastHit hit;
        if (Physics.Raycast(ballPosition, Vector3.down, out hit))
        {
            if (hit.transform.tag == "Ground")
            {
                ballPosition = hit.point + new Vector3(0, 1.5f, 0);
            }
            else
            {
                ballPosition = new Vector3((Random.value + 0.5f) * 100, 25, (Random.value + 0.5f) * 100);
            }
        }
        GameObject ball = Instantiate(Resources.Load<GameObject>("PointBall"),ballPosition,new Quaternion ());
    }
}
