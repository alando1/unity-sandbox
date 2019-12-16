using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        private GameObject Player;

        float v, h;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            //Cursor.lockState = CursorLockMode.Confined;
            //Player = GameObject.Find("Dude_low");
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            
            {
                // pass the input to the car!
                //float h = CrossPlatformInputManager.GetAxis("MouseX");
                //float v = CrossPlatformInputManager.GetAxis("Vertical");

                //float h = CrossPlatformInputManager.GetAxis("Mouse X");
                //float v = CrossPlatformInputManager.GetAxis("Mouse Y");

#if !MOBILE_INPUT
                //float handbrake = CrossPlatformInputManager.VirtualAxisReference("jump").GetValue;


                v = Input.GetAxisRaw("Vertical");
                h = Input.GetAxisRaw("Horizontal");
                //v = Input.GetAxisRaw("Mouse Y");
                //h = Input.GetAxisRaw("Mouse X");

                //v = CrossPlatformInputManager.GetAxis("Vertical");
                //h = CrossPlatformInputManager.GetAxis("Horizontal");
                //v = CrossPlatformInputManager.VirtualAxisReference("Vertical").GetValue;
                //h = CrossPlatformInputManager.VirtualAxisReference("Horizontal").GetValue;

                //v = CrossPlatformInputManager.VirtualAxisReference("Mouse Y").GetValue;
                //h = CrossPlatformInputManager.VirtualAxisReference("Mouse X").GetValue;
                //v = CrossPlatformInputManager.VirtualAxisReference("Vertical").GetValue;
                //h = CrossPlatformInputManager.VirtualAxisReference("Horizontal").GetValue;

                m_Car.Move(h, v, v, 0);//handbrake);
                print("Vert\t:\t" + v + "\n" + "Horz\t:\t" + h);
#else
                v = CrossPlatformInputManager.VirtualAxisReference("Mouse Y").GetValue;
                h = CrossPlatformInputManager.VirtualAxisReference("Mouse X").GetValue;
            m_Car.Move(h, v, v, 0f);
#endif

            }
        }
    }
}
