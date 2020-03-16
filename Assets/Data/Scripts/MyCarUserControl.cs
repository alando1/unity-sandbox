using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
  [RequireComponent(typeof(CarController))]
  public class MyCarUserControl : MonoBehaviour
  {
    private CarController m_Car;
    private GameObject player;
    private float accel, handBrake, steering;
    private bool holdGas, boost;
    private Rigidbody rb;

    public bool debug = false;
    public float thrust = 500000f;

    private void Awake()
    {
      player = GameObject.Find("Human");
      rb = GetComponent<Rigidbody>();
      m_Car = GetComponent<CarController>();
    }

    public void OnDriverExit()
    {
      //m_Car.Move(0.0f, 0.0f, 0.0f, 0.0f);
    }

    private void FixedUpdate()
    {
      if (player.GetComponent<PlayerController>().PlayerState == PlayerController.ControlStates.CAR_CONTROL)
      {
        if (player.GetComponent<VehicleController>().ChosenVehicle == transform.gameObject)
        {
          // Check if Hold Gas button has been pressed
          if (Input.GetKeyDown(KeyCode.Tab))
            holdGas = !holdGas;

          if (holdGas)
            accel = 1.0f;
          else
            accel = Input.GetAxis("Vertical");

          // make shift boost
          steering = Input.GetAxis("Horizontal");
          handBrake = Input.GetAxis("HandBrake");
          boost = Input.GetKey(KeyCode.LeftShift);
          m_Car.Move(steering, accel, accel, handBrake);

          // fix this trash
          if (boost)
          {
            rb.AddForce(transform.forward * thrust);
          }

          if (debug)
          {
            string l1, l2;
            l1 = string.Format("Accel: {0,-10}  HandBrake: {1,-10}", accel, handBrake);
            l2 = string.Format("Boost: {0,-10} Steering : {1,-10}", boost, steering);
            print(l1 + "\n" + l2);
          }
        }
      }
      else
      {
        m_Car.Move(0F, 0F, 0F, 0F);
      }
    }
  }
}
