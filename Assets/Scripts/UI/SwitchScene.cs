using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    public void SwitchButton_OnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
