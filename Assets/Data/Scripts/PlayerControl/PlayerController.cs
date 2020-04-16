using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
  #region Variables
  // Components
  private Animator animator;
  private InputController inputController;
  private CharacterController character;
  private Camera playerCam;

  // Header sections
  [Header("Ground Contact")]
  public Transform groundContact;
  public float groundDistance;
  public LayerMask groundMask;
  private bool isJumping;

  [Header("Movement Settings")]
  public float groundSpeed = 10f;
  public float crouchSpeed = 5f;
  public float jumpHeight = 3f;
  public float gravity = -9.81f;
  public float turnSpeed = 5.0f;
  public float turnOffset = 0.0f;
  public float moveMag;
  public float terminalVelocity = 50F;
  public bool turning = false;
  private bool prevTurn;
  private bool stopTurning;
  private Vector3 newDesiredLookDir;
  private float turnThreshold = 85F;
  private float deltaTurn = 0F;
  private float speed;

  //turning
  public bool TurningInPlace { get { return turning; } }
  private Vector3 desiredFacingDir = Vector3.zero;

  [Header("Collider Settings")]
  public float standingHeight = 1.9F;
  public float crouchingHeight = 1.4F;
  public Vector3 crouchingCenter = new Vector3(0F, 0.62F, 0F);
  public Vector3 standingCenter = new Vector3(0F, 0.97F, 0F);

  private Vector3 velocity;
  private Vector3 move;
  private Vector2 movement;
  public Vector2 groundMovement { get { return movement; } }
  public bool isGrounded;
  private float deltaY;

  public enum ControlStates { GROUND_CONTROL, CAR_CONTROL };
  [SerializeField] public ControlStates PlayerState;
  #endregion

  void Start ()
  {
    PlayerState = ControlStates.GROUND_CONTROL;

    animator = GetComponent<Animator>();
    playerCam = Camera.main;
    inputController = GetComponent<InputController>();
    character = GetComponent<CharacterController>();
    deltaY = 0;
    move = Vector3.zero;
    stopTurning = prevTurn = turning;
    isJumping = false;
	}
	
  void Update()
  {
    switch(PlayerState)
    {
      case ControlStates.GROUND_CONTROL: GroundUpdate(); break;
      case ControlStates.CAR_CONTROL: break;
      default: break;
    }
  }

	void GroundUpdate()
  {

    // Check if now grounded
    isGrounded = Physics.CheckSphere(groundContact.position, groundDistance, groundMask);

    // reset downward velocity when grounded
    if (isGrounded && velocity.y < 0)
    {
      velocity.y = -5F;
      isJumping = false;
    }

    HandleCrouchMovement();
    HandleHorizontalMovement();
    HandleVerticalMovement();
    character.Move(move * speed * Time.deltaTime + velocity * Time.deltaTime);
    HandleLookRotation();
    SetAnimations();

    // need to set after animations to avoid skipping jump animation transition
    if (!isGrounded)
    {
      deltaY = velocity.y;
    }
	}

  // Set animation parameters
  void SetAnimations()
  {
    // ground movement variables
    animator.SetFloat("Side", movement.x);          // left/right movement
    animator.SetFloat("Forward", movement.y);       // forward/back movement
    animator.SetBool("Crouch", inputController.Crouch); // crouching
    animator.SetBool("Grounded", isGrounded);       // is this used by animator anymore?

    // jump variables
    animator.SetFloat("VelocityY", velocity.y); // triggers jump transition
    animator.SetFloat("DeltaY", deltaY);        // triggers jump blending animations

    // turning
    animator.SetFloat("TurnOffset", turnOffset);
    animator.SetBool("Aiming", inputController.IsAiming);
  }

  /// <summary>
  /// rotate player when turn threshold is exceeded
  /// </summary>
  void HandleLookRotation()
  {
    // take the current camera look vector, flatten to XZ plane and normalize;
    var cameraLookVector = playerCam.transform.forward;
    cameraLookVector.y = 0F; cameraLookVector.Normalize();

    // take the current player look vector, flatten to XZ plane and normalize;
    var playerLookVector = transform.forward;
    playerLookVector.y = 0F; playerLookVector.Normalize();

    // GROUND TURNING
    if (isGrounded)
    {
      // if starting to move
      if (moveMag > 0.1F)
      {
        turnOffset = 0F;
        turning = false;
      }
      else
      {
        turnOffset = Vector3.SignedAngle(playerLookVector, cameraLookVector, Vector3.up);
        float absTurnOffset = Mathf.Abs(turnOffset);
        turning = absTurnOffset > turnThreshold;
        float turnDir = turnOffset > 0F ? -1F : 1F;

        if (turning)
        {
          Vector3 forward = Quaternion.Euler(new Vector3(0F, turnThreshold * turnDir, 0F)) * cameraLookVector;
          transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
        else
        {
          if (prevTurn && !turning)
          {
            stopTurning = true;
            newDesiredLookDir = cameraLookVector;
          }

          if (stopTurning)
          {
            var stopThresh = Vector3.SignedAngle(playerLookVector, newDesiredLookDir, Vector3.up);
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(playerLookVector, newDesiredLookDir, turnSpeed * Time.deltaTime, 1F), Vector3.up);
            if (Mathf.Abs(stopThresh) < 5f)
            {
              stopTurning = false;
            }
          }
        }
      }

      prevTurn = turning;
    }
    else
    {
      transform.rotation = Quaternion.LookRotation(cameraLookVector);
    }
  }

  // handle crouch transitions
  void HandleCrouchMovement()
  {
    if (inputController.Crouch && isGrounded)
    {
      speed = crouchSpeed;
      character.center = crouchingCenter;
      character.height = crouchingHeight;
    }
    else
    {
      speed = groundSpeed;
      character.center = standingCenter;
      character.height = standingHeight;
    }
  }

  // handle horizontal movement input
  void HandleHorizontalMovement()
  {
    if(move.magnitude > 0.1f)
    {
      // take the current camera look vector, flatten to XZ plane and normalize;
      var cameraLookVector = playerCam.transform.forward;
      cameraLookVector.y = 0F; cameraLookVector.Normalize();
      transform.forward = cameraLookVector; 
    }

      movement = new Vector2(inputController.Side, inputController.Forward);
      movement = movement.magnitude > 1F ? movement.normalized : movement;
      move = transform.right * movement.x + transform.forward * movement.y;
      moveMag = move.magnitude;
  }

  // handle vertical movement input
  void HandleVerticalMovement()
  {
    if (inputController.Jump && isGrounded)
    {
      if (inputController.Crouch)
        inputController.Crouch = !inputController.Crouch;
      else
      {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        isJumping = true;
      }

      deltaY = 0F;
    }

    // handle gravity
    if (velocity.y > -terminalVelocity)
      velocity.y += gravity * Time.deltaTime;

  }
}
