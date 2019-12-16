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
    private CarController m_Car; // the car controller we want to use
    private GameObject Player;
    private float accel, handBrake, steering;
    private bool holdGas, boost;
    private Rigidbody thisRb;

    public bool debug = false;
    public float thrust = 500000f;

    private void Awake()
    {
      Player = GameObject.Find("Human");
      thisRb = GetComponent<Rigidbody>();
      // get the car controller
      m_Car = GetComponent<CarController>();
    }

    public void OnDriverExit()
    {
      m_Car.Move(0.0f, 0.0f, 0.0f, 0.0f);
    }

    private void FixedUpdate()
    {
      if (Player.GetComponent<Movement>().PlayerState == Movement.State.driving)
      {
        if (Player.GetComponent<VehicleController>().ChosenVehicle.transform == transform)
        {

#if !MOBILE_INPUT
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
            thisRb.AddForce(transform.forward * thrust);

          if (debug)
          {
            string l1, l2;
            l1 = string.Format("Accel: {0,-10}  HandBrake: {1,-10}", accel, handBrake);
            l2 = string.Format("Boost: {0,-10} Steering : {1,-10}", boost, steering);
            //print("Accel\t: "    + accel        + "\tHandBrake\t: " + handBrake + "\n" +
            //      "Steering\t: " + steering     + "\tBoost\t"       + );
            print(l1 + "\n" + l2);
          }
#else
          h           = CrossPlatformInputManager.GetAxis("Vertical");
          v           = CrossPlatformInputManager.GetAxis("Horizontal");
          handbrake   = CrossPlatformInputManager.GetAxis("HandBrake");
          m_Car.Move(h, v, v, hb);
          print("Vert :\t" + v + "\tHandBrake :" + hb + "\n" + "Horz :\t" + h);

#endif

        }
      }
      else
      {
        //m_Car.GetComponent<Rigidbody>().velocity = Vector3.zero;
      }
    }

    /// <summary>
    /// remaps a value within a given range to a new set of new bounds preserving the linear interopolation.
    /// </summary>
    /// <param name="value">Value to remap</param>
    /// <param name="minIn">current minimum</param>
    /// <param name="maxIn">current maximum</param>
    /// <param name="minOut">new minimum</param>
    /// <param name="maxOut">new maximum</param>
    /// <returns></returns>
    private float Map(float value, float minIn, float maxIn, float minOut, float maxOut)
    {
      return (value - minIn) * (maxOut - maxIn) / (maxIn - minIn) + minOut;
    }
  }
}
