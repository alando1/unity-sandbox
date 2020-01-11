using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
  // Components
  private Animator animator;
  private InputController playerInput;
  private CharacterController character;

  // Header sections
  [Header("Ground Contact")]
  public Transform groundContact;
  public float groundDistance;
  public LayerMask groundMask;

  [Header("Movement Settings")]
  public float groundSpeed = 10f;
  public float crouchSpeed = 5f;
  public float jumpHeight = 3f;
  public float gravity = -9.81f;

  [Header("Collider Settings")]
  public float standingHeight = 1.9F;
  public float crouchingHeight = 1.4F;
  public Vector3 crouchingCenter = new Vector3(0F, 0.62F, 0F);
  public Vector3 standingCenter = new Vector3(0F, 0.97F, 0F);

  private Vector3 velocity;
  private Vector2 movement;
  public Vector2 Joystick { get { return movement; } }
  public bool isGrounded;
  private float deltaY;

  public enum ControlStates { GROUND_CONTROL, CAR_CONTROL };
  [SerializeField] public ControlStates PlayerState;


	void Start ()
  {
    PlayerState = ControlStates.GROUND_CONTROL;

    animator = GetComponent<Animator>();
    playerInput = GetComponent<InputController>();
    character = GetComponent<CharacterController>();
    deltaY = 0;
	}
	
  void Update()
  {
    switch(PlayerState)
    {
      case ControlStates.GROUND_CONTROL: GroundUpdate(); break;
      default: break;
    }
  }

	void GroundUpdate()
  {

    isGrounded = Physics.CheckSphere(groundContact.position, groundDistance, groundMask);

    // reset downward velocity when grounded
    if (isGrounded && velocity.y < 0)
    {
      velocity.y = -2f;
    }

    // horizontal movement - wasd
    movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    movement = movement.magnitude > 1F ? movement.normalized : movement;
    Vector3 move = transform.right * movement.x + transform.forward * movement.y;

    float speed;
    if (playerInput.Crouch)
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

    character.Move(move * speed * Time.deltaTime);

    // rotational movement
    if (playerInput.MouseHorz != 0)
    {
      transform.Rotate(Vector3.up, playerInput.MouseHorz);
    }

    // vertical movement
    if (playerInput.Jump && isGrounded)
    {
      if (playerInput.Crouch)
        playerInput.Crouch = !playerInput.Crouch;
      else
      velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

      deltaY = 0F;
    }

    // handle gravity
    velocity.y += gravity * Time.deltaTime;
    character.Move(velocity * Time.deltaTime);

    SetAnimations();

    // need to set after animations to avoid skipping jump animation transition
    if (!isGrounded)
    {
      deltaY = velocity.y;
    }
	}

  void SetAnimations()
  {
    // movement
    animator.SetFloat("Side", movement.x);
    animator.SetFloat("Forward", movement.y);
    animator.SetBool("Crouch", playerInput.Crouch);
    animator.SetBool("Grounded", isGrounded);

    // triggers jump transition
    animator.SetFloat("VelocityY", velocity.y);
    // triggers jump blending animations
    animator.SetFloat("DeltaY", deltaY);
  }

  private void FixedUpdate()
  {
    
  }
}
