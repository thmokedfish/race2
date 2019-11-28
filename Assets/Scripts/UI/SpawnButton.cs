using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpawnButton : MonoBehaviour
{
    public int prefabID;
    public int teamID;
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(SpawnButton_OnClick);
    }
    void SpawnButton_OnClick()
    {
        ScoreManager.Instance.nextPrefabID = prefabID;
        ScoreManager.Instance.nextTeamID = teamID;
        GameManager.instance.networkManager.customAddplayer();
    }
}
