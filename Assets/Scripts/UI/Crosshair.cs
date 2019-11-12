using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Crosshair : MonoBehaviour
{
    public RawImage center;
    public RawImage top;
    public RawImage bottom;
    public RawImage left;
    public RawImage right;
    public float MaxDistance;
    public float curDistance;
    public bool increase;
    public float intense = 10;
    private void Update()
    {
        curDistance = Mathf.Lerp(curDistance, increase ? MaxDistance : 0, intense*Time.deltaTime);
        //Debug.Log(change);
        //float changeAmount = increase ? change : -change;
        //curDistance = change;
        if(curDistance>MaxDistance)
        {
            curDistance = MaxDistance;
        }
        if(curDistance<0)
        {
            curDistance = 0;
        }
        ResetSingleCrossPerFrame(top.transform, curDistance, Vector3.up);
        ResetSingleCrossPerFrame(bottom.transform, curDistance, Vector3.down);
        ResetSingleCrossPerFrame(right.transform, curDistance, Vector3.right);
        ResetSingleCrossPerFrame(left.transform, curDistance, Vector3.left);
    }
    public IEnumerator ResetCrosshair(float max,float targetDistance,bool increase,float intense)
    {
        /*
        while (true)
        {
            curDistance = top.transform.position.y - center.transform.position.y;
            float distanceDiff = targetDistance - curDistance;
            ResetSingleCrossPerFrame(top.transform, intense, Vector3.up*distanceDiff);
            ResetSingleCrossPerFrame(bottom.transform, intense, Vector3.down * distanceDiff);
            ResetSingleCrossPerFrame(right.transform, intense, Vector3.right * distanceDiff);
            ResetSingleCrossPerFrame(left.transform, intense, Vector3.left * distanceDiff);
            yield return null;
        }
        */
        float changeAmount = increase ? 1 : -1;
        while (true)
        {
            //if()
            ResetSingleCrossPerFrame(top.transform, changeAmount*intense, Vector3.up);
            ResetSingleCrossPerFrame(bottom.transform, changeAmount * intense, Vector3.down);
            ResetSingleCrossPerFrame(right.transform, changeAmount * intense, Vector3.right);
            ResetSingleCrossPerFrame(left.transform, changeAmount * intense, Vector3.left);
            yield return null;
        }
    }

    private void ResetSingleCrossPerFrame(Transform transform,float distance,Vector3 direction)
    {
        transform.localPosition = Vector3.Normalize(direction) * distance;
    }
}
