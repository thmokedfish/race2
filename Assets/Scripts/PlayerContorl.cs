using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class PlayerContorl : MonoBehaviour
{
    public KeyCode shootButton;
    public Camera playerCamera;
    public Transform muzzle;
    public Transform target;
    private Image crosshair;
    private CarUserControl.PlayerName player;
    private bool canShoot;

    void Start()
    {
        GameObject crosshairgo;
        if (!(crosshairgo = GameObject.Find("Canvas/Crosshair" + ((int)this.GetComponent<CarUserControl>().player + 1))))
        {
            Debug.LogError("Canvas/Crosshair" + ((int)this.GetComponent<CarUserControl>().player + 1) + " not exist");
            return;
        }
        crosshair = crosshairgo.GetComponent<Image>();
        crosshair.gameObject.SetActive(false);
        setPositionSide(crosshair.transform);
        player = this.GetComponent<CarUserControl>().player;
        target = GameManager.instance.cars[1 - (int)player].transform;
        muzzle = this.transform.Find("Muzzle");
        if(player==CarUserControl.PlayerName.player1)
        {
            shootButton = KeyCode.LeftShift;
        }
        if(player == CarUserControl.PlayerName.player2)
        {
            shootButton = KeyCode.RightShift;
        }
    }

    void setPositionSide(Transform go)
    {
        go.localPosition = new Vector3(1, 0, 0) * Screen.width / 2 * ((int)this.GetComponent<CarUserControl>().player - 0.5f) +
            new Vector3(0, go.localPosition.y, go.localPosition.z);
    }

    void Update()
    {
        if (Input.GetKey(shootButton))
        {
            Aim();
        }

        if (Input.GetKeyUp(shootButton))
        {
            crosshair.gameObject.SetActive(false);
            if (!canShoot) return;
            Shoot();
        }
    }
    private void Aim()
    {
        if (!crosshair.IsActive())
        {
            crosshair.gameObject.SetActive(true);
        }
        float radius = 0;
        if (crosshair.transform.childCount > 0)
        {
            if (crosshair.transform.childCount==0)
            {
                Debug.LogError("crosshair has no child");
                return;
            }
            radius = Vector3.Distance(new Vector3(), crosshair.transform.GetChild(0).localPosition);
        }
        Vector2 targetOnScreen;
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), playerCamera.WorldToScreenPoint(target.position),null, out targetOnScreen);
        bool inRange = Vector3.Distance(crosshair.transform.localPosition,new Vector3(targetOnScreen.x,targetOnScreen.y) ) < radius;
        bool inFront = Vector3.Dot(playerCamera.transform.forward, target.position - this.transform.position)>0;
        if (inRange&&inFront)
        {
            crosshair.color = Color.green;//new Color(125, 255, 125);
            canShoot = true;
        }
        else
        {
            crosshair.color = Color.white;
            canShoot = false;
        }

    }

    private void Shoot()
    {
        if (!target)
        {
            return;
        }
        GameObject missile = Instantiate(Resources.Load("Missile") as GameObject, muzzle.position, muzzle.rotation);
        missile.GetComponent<Missile>().target = target;
    }
}
