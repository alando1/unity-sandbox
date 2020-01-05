using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
  // Components
  private Animator animator;
  private CharacterController character;
  private InputController playerInput;

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

  public static Action UpdateCamera;
  private Vector3 velocity;
  private Vector2 movement;
  public bool isGrounded;
  private float deltaY;
  private float prevY;


  public float standingHeight = 1.9F;
  private float crouchingHeight = 1.4F;
  private Vector3 crouchingCenter = new Vector3(0F, 0.62F, 0F);
  public Vector3 standingCenter = new Vector3(0F, 0.97F, 0F);

	void Start ()
  {
    animator = GetComponent<Animator>();
    character = GetComponent<CharacterController>();
    playerInput = GetComponent<InputController>();
    deltaY = 0;
    prevY = transform.position.y;
	}
	
	void Update ()
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

    UpdateCamera();
    SetAnimations();

    if (!isGrounded)
    {
      deltaY = velocity.y;
    }

    prevY = transform.position.y;
	}

  void SetAnimations()
  {
    // movement variables
    animator.SetFloat("Side", movement.x);
    animator.SetFloat("Forward", movement.y);
    animator.SetBool("Crouch", playerInput.Crouch);
    animator.SetBool("Grounded", isGrounded);
    animator.SetFloat("VelocityY", velocity.y);
    animator.SetFloat("DeltaY", deltaY);
  }

  private void FixedUpdate()
  {
    
  }
}
