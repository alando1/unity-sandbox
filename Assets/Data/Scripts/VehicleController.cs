using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
  public GameObject VehicleContainer;
  private bool playerIsDriving = false;
  private GameObject EnterCarButton;
  private GameObject ExitCarButton;
  public GameObject ChosenVehicle { get; set; }
  private Animator Animator;
  private Rigidbody Rigidbody;
  private Transform seat;
  private Stack<BoxCollider> PlayerBoxColliders = new Stack<BoxCollider>();
  private Stack<CapsuleCollider> PlayerCapColliders = new Stack<CapsuleCollider>();
  private GameObject Player;
  public GameObject GetPlayer { get { return Player; } }
  private Transform playerRoot;

  [Header("Seat positions")]
  public Vector3 SeatOffset;
  public Vector3 SeatRotation;

  // Use this for initialization
  void Start()
  {
    Player = GetComponent<InputController>().Player;
    playerRoot = Player.transform.parent;
    FindAllColliders();

    Rigidbody = GetComponent<Rigidbody>();
    Animator = GetComponent<Animator>();

    // To Do: make external canvas child of each player
    EnterCarButton = GameObject.Find("Enter Button");
    ExitCarButton = GameObject.Find("Exit Button");
    EnterCarButton.SetActive(false);
    ExitCarButton.SetActive(false);
  }

  private void FindCollider()
  {
    BoxCollider box = GetComponent<BoxCollider>();
    if (box != null)
      PlayerBoxColliders.Push(box);

    CapsuleCollider cap = GetComponent<CapsuleCollider>();
    if (cap != null)
      PlayerCapColliders.Push(cap);
  }

  /// <summary>
  /// recursively find all colliders and stores in a stack to reference.
  /// </summary>
  private void FindAllColliders()
  {
    // Find any colliders in this transform.
    FindCollider();

    // Find any colliders in children of this tranform.
    foreach (Transform t in transform)
    {
      GameObject go = t.gameObject;
      if (go != null)
        FindCollider();
    }
  }

  // Update is called once per frame
  void Update()
  {
    CheckForCarsUpdate();
    DrivingUpdate();

  }

  void CheckForCarsUpdate()
  {
    // if the player is not driving...
    if (!playerIsDriving)
    {
      var colliders = Physics.OverlapSphere(transform.position, 1.0f);
      bool noCars = true;
      // check all colliders within one meter radius.
      foreach (Collider collider in colliders)
      {
        if (collider.tag == "Car")
        {
          ChosenVehicle = collider.gameObject;

          // Enable "enter car" message
          DisplayMessageToEnterCar();
          noCars = false;

          // If the enter car message is displayed and the enter button is pressed ...
          if (EnterCarButton.activeSelf && Input.GetButtonDown("Action"))
          {
            // remove enter car message and change state
            EnterCarButton.SetActive(false);
            ChangeToDriving();
            print("Entering " + ChosenVehicle.name);
            break;
          }
        }
      }

      // if there are no cars within range and the Enter Car button is active, set false.
      if (noCars && EnterCarButton.activeSelf)
      {
        EnterCarButton.SetActive(false);
      }
    }

  }

  void DisplayDrivingGui()
  {
    EnterCarButton.SetActive(false);
    ExitCarButton.SetActive(true);
  }

  void DisplayMessageToEnterCar()
  {
    if (!EnterCarButton.activeSelf)
    {
      ExitCarButton.SetActive(false);
      EnterCarButton.SetActive(true);
    }
  }

  void ChangeToDriving()
  {
    // Get the driver seat position and rotation...
    seat = FindDriverSeat(ChosenVehicle.transform);
    print(seat);

    if (seat != null)
    {
      playerIsDriving = true;

      // disable player's rigidbody and colliders
      Rigidbody.isKinematic = true;
      SetColliderStates(false);

      // make player a child of the car
      transform.SetParent(ChosenVehicle.transform);
      transform.localPosition = Vector3.zero;

      // assign player the driver seat position and rotation
      transform.SetPositionAndRotation(seat.position, seat.rotation);
      //playerAnchor = transform;

      // change the player state and trigger the driving animation
      GetComponent<Movement>().PlayerState = Movement.State.driving;
      //Animator.applyRootMotion = false;
      Animator.SetBool("isDriving", true);
    }
  }

  void ChangeToOnGround()
  {
    playerIsDriving = false;

    // enable the player's rigidbody and box colliders
    Rigidbody.isKinematic = false;
    SetColliderStates(true);

    // compute left offset
    Vector3 offset = seat.right * -2.0f;

    // remove player from car
    transform.SetParent(Player.transform);

    // reposition the player to outside of the driver seat
    transform.SetPositionAndRotation(seat.position + offset, Quaternion.LookRotation(Vector3.forward, Vector3.up));

    Player.transform.parent = playerRoot;

    // kill the engine on driver exit
    if (ChosenVehicle != null)
    {
      ChosenVehicle.GetComponent<UnityStandardAssets.Vehicles.Car.MyCarUserControl>().OnDriverExit();
    }

    // change the player state and trigger the onGround animation
    GetComponent<Movement>().PlayerState = Movement.State.onGround;
    //Animator.applyRootMotion = true;
    Animator.SetBool("isDriving", false);



  }

  void DrivingUpdate()
  {
    // while driving ...
    if (playerIsDriving)
    {
      // first frame of driving ...
      if (!ExitCarButton.activeSelf)
      {
        DisplayDrivingGui();
        ChangeToDriving();
      }
      // driving input
      else
      {
        //**************//
        // Exit vehicle //
        //**************//
        if (Input.GetButtonDown("Action"))
        {
          // remove exit car message and change state
          ExitCarButton.SetActive(false);
          ChangeToOnGround();
          Debug.Log("Exiting " + ChosenVehicle.name);
        }
      }
    }
  }

  // needed to enter car and not collide...
  void SetColliderStates(bool state)
  {
    if(PlayerBoxColliders != null)
      foreach(BoxCollider box in PlayerBoxColliders)
        box.enabled = state;

    if (PlayerCapColliders != null)
      foreach (CapsuleCollider cap in PlayerCapColliders)
        cap.enabled = state;
  }

  Transform FindDriverSeat(Transform vehicle)
  {
    return vehicle.Find("Seat Positions/Driver Seat");
    //foreach (Transform child in vehicle.transform)
    //{
    //    if (child.tag == "Driver Seat")
    //        return child.transform;
    //    else
    //        FindDriverSeat(child);

    //}
    //return null;
  }


}
