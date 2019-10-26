using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Camera[] playerCameras = new Camera[2];
    public CarUserControl[] cars;
    private void Awake()
    {
        instance = this;
    }

    public float CarDistance()
    {
        if (playerCameras == null) return 0;
        if (playerCameras.Length < 2) return 0;
        return Vector3.Distance(playerCameras[0].transform.position, playerCameras[1].transform.position);
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
