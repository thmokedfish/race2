using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpawnButton : MonoBehaviour
{
    public int prefabID;
    public int teamID;
    public int spawnPointID;
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(SpawnButton_OnClick);
    }
    void SpawnButton_OnClick()
    {
        GameManager.Instance.networkManager.CustomAddplayer(prefabID,teamID,spawnPointID);
        UIManager.Instance.ChoosingPanel.SetActive(false);
    }
}
