using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//实现PointerClickHandler接口用于监听UGUI鼠标点击操作
public class ToLoadingScene : MonoBehaviour
{

    [Tooltip("下个场景的名字")]
    public string nextSceneName;
    public void OnPointerClick()
    {
        SceneManager.LoadScene(nextSceneName);
    }

}
