using Assets.Data.Scripts;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[RequireComponent(typeof(GuiCommands))]
[RequireComponent(typeof(Build))]
public class Movement : MonoBehaviour
{
  #region variables
  [Header("Velocities")]
  public float runSpeed         = 5.0f;
  public float jumpVelocity     = 5.0f;
  public float crouchSpeed      = 4.0f;
  public float boostMultiplier  = 2.0f;
  private float vertical;
  private float horizontal;
  private Vector3 deltaPosition;
  private Vector2 debugJoystick;
  public Vector2 JoyStick { get { return debugJoystick; } }

  // jump variables
  private bool onGround;
  private float initJumpHeight;
  private bool isJumping;
  //private float elapsedJumpTime;
  private float flyMultiplier;
  private float prevHeight;
  public LayerMask groundLayer;

  [Header("Aim Settings")]
  public Transform aimObject;
  public Vector3 aimOffset;
  public float aimSpeed;
  private float aimWeight;
  private Transform chest;


  public static Action<Vector2> JoyStickGui;
  public static Action<Vector2, string> CurrentAnimationGui;


  // Components
  private Camera Camera;
  private Animator anim;
  private Rigidbody rig;
  private GameObject pauseMenu;

  // IK variables
  private Quaternion desiredChestRot;
  Quaternion aimRotation;
  //private float aimStrength;

  public enum State { onGround, driving };
  private State mPlayerState = State.onGround;

  #endregion

  #region events
  public static event Action<Vector3> Respawn; 
  public static event Action<object> ADS;
  public static event Action<object> StopADS;
  #endregion

  #region Properties
  // Movement Input
  private bool Sprint  { get { return GetComponent<InputController>().Sprint;  } }
  private bool Jump    { get { return GetComponent<InputController>().Jump;    } }
  private bool AutoRun { get { return GetComponent<InputController>().AutoRun; } }
  private bool Fly     { get { return GetComponent<InputController>().Fly;     } }
  private bool Crouch  { get { return GetComponent<InputController>().Crouch;  } }

  private float MouseHorizontal { get { return GetComponent<InputController>().MouseHorz; } }
  public bool IsAiming          { get { return (GetComponent<InputController>().IsAiming || AutoRun); } }
  private bool Paused
  {
    get
    {
      try
      {
        return pauseMenu.GetComponent<PauseMenuControl>().Paused;
      }
      catch
      {
        return false;
      }
    }
  }
  /// <summary>Current state of the player.</summary>
  public State PlayerState
  {
    get { return mPlayerState; }
    set { mPlayerState = value; }
  }

  public bool OnGround { get { return onGround; } }

  #endregion

  void Start()
  {
    Application.targetFrameRate = 144;

    print("initial velocity needed for a jump of 6ft: " + Utilities.CalculateJumpSpeed(1.8288f, 9.8f));

    // initialize variables
    mPlayerState = State.onGround;
    aimWeight       = 0.0f;
    //elapsedJumpTime = 0.0f;
    flyMultiplier   = 50.0f;
    deltaPosition = new Vector3(0f, 0f, 0f);
    debugJoystick = new Vector2(0f, 0f);

    onGround = false;
    prevHeight = transform.position.y;

    // Components
    Camera = GetComponentInChildren<Camera>();
    anim = GetComponent<Animator>();
    rig = GetComponent<Rigidbody>();
    pauseMenu = GameObject.FindGameObjectWithTag("Pause Menu");
    chest = anim.GetBoneTransform(HumanBodyBones.Chest);
    rig.useGravity = true;
    rig.isKinematic = false;
    isJumping = false;
  }

  public void SetPlayerState(State state)
  {
    mPlayerState = state;
  }

  public void OnGroundMovement()
  {
    if (!Paused)
    { // replace this with actual animation rotation
      if (MouseHorizontal != 0)
      {
        transform.Rotate(Vector3.up, MouseHorizontal);
      }
    }

    #region Flight
    if (Fly)
    {
      if (isJumping)
        isJumping = false;

      if (rig.useGravity)
        rig.useGravity = false;

      if (Input.GetButton("Sprint"))
        transform.position += Camera.transform.forward * flyMultiplier * Time.deltaTime;
    }
    #endregion
    else
    {
      if (!rig.useGravity)
        rig.useGravity = true;

      #region Locomotion
      deltaPosition *= Crouch ? crouchSpeed : runSpeed;
      deltaPosition *= Sprint ? boostMultiplier : 1.0f;
      transform.position += Quaternion.AngleAxis(transform.localEulerAngles.y, transform.up) * deltaPosition * Time.deltaTime;
      #endregion

      #region Jump
      onGround = Physics.CheckSphere(transform.position + new Vector3(0, 0.14f, 0f), 0.17f, groundLayer);

      if (Jump && !isJumping && onGround)
      {
        rig.velocity = new Vector3(0, jumpVelocity, 0);
        isJumping = true;
        anim.SetBool("IsJumping", isJumping);
      }
      else if (isJumping && onGround)
      {
        isJumping = false;
        anim.SetBool("IsJumping", isJumping);
      }

      #endregion
    }

  }

  #region Update Functions

  private void Update()
  {
    HandleMovement();
    SetAnimationValues();
    LastUpdate();
  }

  private void FixedUpdate()
  {
    prevHeight = transform.position.y;
  }

  private void LateUpdate()
  {
    if (mPlayerState == State.onGround)
      chest.rotation = desiredChestRot;
  }

  private void LastUpdate()
  {


    switch (mPlayerState)
    {
      case State.onGround:
        if (IsAiming)
        {
          aimRotation = Quaternion.LookRotation(aimObject.position - chest.position, Vector3.up);
          desiredChestRot = Quaternion.Lerp(chest.rotation, aimRotation, aimWeight);
        }
        else
          desiredChestRot = Quaternion.Lerp(chest.rotation, Quaternion.LookRotation(transform.forward, transform.up), aimWeight);

        break;
      default: break;
    }
  }
  #endregion

  #region Helper Functions

  private void HandleMovement()
  {
    // read player movement input
    vertical = Input.GetAxis("Vertical");
    horizontal = Input.GetAxis("Horizontal");
    deltaPosition = new Vector3(horizontal, 0, vertical);
    deltaPosition = deltaPosition.magnitude > 1.0f ? deltaPosition.normalized : deltaPosition;
    debugJoystick.x = deltaPosition.x;
    debugJoystick.y = deltaPosition.z;

    switch (mPlayerState)
    {
      case State.onGround: OnGroundMovement(); break;
      case State.driving: break;
      default: break;
    }
  }

  void SetAnimationValues()
  {
    // locomotion variables
    anim.SetFloat("Forward", vertical);
    anim.SetFloat("Side", horizontal);
    anim.SetFloat("yDelta", transform.position.y - prevHeight);
    anim.SetBool("Crouch", Crouch);


    //anim.SetLayerWeight(anim.GetLayerIndex("AimingLayer"), aimWeight);
    //anim.SetFloat("AimingWeight", aimWeight);
    anim.SetBool("OnGround", onGround);

    anim.SetBool("Tab", AutoRun);
  }

  private void OnAnimatorIK(int layerIndex)
  {
    // aiming variables
    aimWeight = Mathf.MoveTowards(aimWeight,
                                 (IsAiming ? 1.0f : 0.0f),
                                 5.0f * Time.deltaTime);
    anim.SetLayerWeight(anim.GetLayerIndex("AimingLayer"), aimWeight);
  }
}
  #endregion
