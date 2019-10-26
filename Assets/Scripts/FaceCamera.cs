using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class FaceCamera : MonoBehaviour
{
    private Camera target;
    private void Start()
    {
        CarUserControl carController;
        if(carController=transform.parent.GetComponent<CarUserControl>())
        {
            target=GameManager.instance.playerCameras[1 - (int)carController.player];
        }
        //gameObject.layer = LayerMask.GetMask("Camera" +(int)carController.player + "Ignore");
       // Debug.Log("Camera" +((int)carController.player+1) + "Ignore");
    }
    void Update()
    {
        transform.LookAt(target.transform);
    }
}
