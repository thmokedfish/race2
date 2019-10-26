using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        public enum PlayerName { player1 = 0, player2 = 1 }
        public KeyCode[] shootButton;
        public PlayerName player;
        public Camera playerCamera;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal" + ((int)player + 1));
            float v = CrossPlatformInputManager.GetAxis("Vertical" + ((int)player + 1));
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
            if (Input.GetKey(shootButton[(int)player]))
            {
                aim();
            }

            if (Input.GetKeyUp(shootButton[(int)player]))
            {
                shoot();
            }

        }
        private void aim()
        {
            //if()
        }

        private void shoot()
        {

        }
    }
}
