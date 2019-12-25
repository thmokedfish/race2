using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwitchCar : MonoBehaviour
{
    public Transform CarImages;
    public int current = 0;
    public float duration = 0.5f;
    [HideInInspector]public Transform[] Icons;
    public float edgeX;
    public void ArrowButton_OnClick(bool forward)
    {
        SwitchChoice(forward);
    }
    private void Awake()
    {
        int count=CarImages.childCount;
        Icons = new Transform[count];
        for(int i=0;i<count;i++)
        {
            Icons[i] = CarImages.GetChild(i);
            Icons[i].localPosition = new Vector3(Screen.width / 2 + edgeX, 0, 0);
        }

        Icons[current].localPosition = Vector3.zero;
    }
    private void Update()
    {
       // float x = Mathf.Lerp(Icons[current].localPosition.x, 0, duration);
        //Icons[current].localPosition = new Vector3(x, 0, 0);
    }


    public void SwitchChoice(bool forward)
    {
        float edge = Screen.width / 2 + edgeX;
        if (forward)
        {
            current++;
            current = current % Icons.Length;
            StartCoroutine(MoveIcon(current, 0,edge));
            StartCoroutine(MoveIcon((current - 1 + Icons.Length) % Icons.Length, -edge));
        }
        else
        {
            current--;
            current = (current+ Icons.Length) % Icons.Length;
            StartCoroutine(MoveIcon(current, 0,-edge));
            StartCoroutine(MoveIcon((current + 1 + Icons.Length) % Icons.Length, edge));
        }
    }
    
    IEnumerator MoveIcon(int index,float goal,float from)
    {
        float time = 0;
        Icons[index].localPosition = new Vector3(from, 0, 0);
        while (time<duration)
        {
            time += Time.deltaTime;
            float x = Mathf.Lerp(Icons[index].localPosition.x, goal, duration);
            Icons[index].localPosition = new Vector3(x, 0, 0);
            yield return null;
        }
    }
    IEnumerator MoveIcon(int index, float goal)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float x = Mathf.Lerp(Icons[index].localPosition.x, goal, duration);
            Icons[index].localPosition = new Vector3(x, 0, 0); ;
            yield return null;
        }
    }

}
