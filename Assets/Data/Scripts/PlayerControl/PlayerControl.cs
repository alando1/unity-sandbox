using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
  private CharacterController character;
  private InputController playerInput;
  private PauseMenuControl pauseMenu;
  public float speed = 10f;
  public float height = 3f;
  public float gravity = -9.81f;

  [Header("Ground Contact")]
  public Transform groundCheck;
  public float groundDistance;
  public LayerMask groundMask;

  public static Action UpdateCamera;
  Vector3 velocity;
  bool isGrounded;

	void Start ()
  {
    character = GetComponent<CharacterController>();
    playerInput = GetComponent<InputController>();
    pauseMenu = GameObject.FindGameObjectWithTag("Pause Menu").GetComponent<PauseMenuControl>();
	}
	
	void Update ()
  {
    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

    // reset downward velocity when grounded
    if (isGrounded && velocity.y < 0)
    {
      velocity.y = -2f;
    }

    // rotational movement
    if (!pauseMenu.Paused)
    { // replace this with actual animation rotation
      if (playerInput.MouseHorz != 0)
      {
        transform.Rotate(Vector3.up, playerInput.MouseHorz);
      }
    }

    // horizontal movement - wasd
    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");
    Vector3 move = transform.right * x + transform.forward * z;
    character.Move(move * speed * Time.deltaTime);

    // handle jump input
    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      velocity.y = Mathf.Sqrt(height * -2f * gravity);
    }

    // handle gravity
    velocity.y += gravity * Time.deltaTime;
    character.Move(velocity * Time.deltaTime);

    UpdateCamera();
	}

  private void FixedUpdate()
  {
    
  }
}
