using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Race
{
    public class Timing : MonoBehaviour
    {
        public void ClientStartGameTiming(int totalSeconds)
        {
            StartCoroutine(GameTiming(totalSeconds));
        }
        private IEnumerator GameTiming(int totalSeconds)
        {
            int minute;
            int second;
            int secondsLeft = totalSeconds;
            while (secondsLeft > 0)
            {
                secondsLeft--;
                this.GetComponent<Text>().text = secondsLeft / 60 + ":" + secondsLeft % 60;
                yield return new WaitForSeconds(1);
            }
            ScoreManager.Instance.ServerEndGame();
        }
    }
}