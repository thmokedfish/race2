using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpawnButton : MonoBehaviour
{
    public GameObject carPrefab;
    public int teamID;
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(SpawnButton_OnClick);
    }
    void SpawnButton_OnClick()
    {
        GameManager.instance.networkManager.customAddplayer(carPrefab,teamID);
    }
}
