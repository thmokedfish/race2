﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//这个脚本我挂在了我用于显示百分比的Text下
public class LoadAsyncScene : MonoBehaviour
{

    //显示进度的文本
    private Text progress;
    //进度条的数值
    private float progressValue;
    //进度条
    private Slider slider;
    [Tooltip("下个场景的名字")]
    public string nextSceneName;

    private AsyncOperation async = null;

    private void Start()
    {
        progress = GetComponent<Text>();
        slider = FindObjectOfType<Slider>();
        StartCoroutine("LoadScene");
    }

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress < 0.9f)
                progressValue = async.progress;
            else
                progressValue = 1.0f;

            slider.value = progressValue;
            progress.text = (int)(slider.value * 100) + " %";

            if (progressValue >= 0.9)
            {
                progress.text = "按任意键继续";
                if (Input.anyKeyDown)
                {
                    async.allowSceneActivation = true;
                }
            }

            yield return null;
        }

    }

}
