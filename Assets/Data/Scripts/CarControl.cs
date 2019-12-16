using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    public class CarControl : MonoBehaviour
    {
        private void Awake()
        {
        }


        private void FixedUpdate()
        {

#if !MOBILE_INPUT

#else
                //m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
