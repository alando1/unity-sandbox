using Assets.Data.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiCommands : MonoBehaviour
{
  [Header("Joystick Settings")]
  public int joystickIconSize = 50;
  public Texture joystickIcon;
  public Vector2 jsScreenOffset;
  public float thumbstickSize = 20;
  private bool displayDebug = false;
  private Vector2 joystick;
  private Vector2 groundMovement;

  private Animator anim;

  void Start ()
  {
    anim = GetComponent<Animator>();
    joystick = Vector2.zero;
    groundMovement = Vector2.zero;
  }
	
	void Update ()
  {
    joystick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    joystick = joystick.magnitude > 1F ? joystick.normalized : joystick;

    groundMovement = GetComponent<PlayerController>().groundMovement;
    
  }

  private void DisplayJoystickInfo(Vector2 joystick, Vector2 offset)
  {
    // JoyStick Background
    Vector2 size = new Vector2(joystickIconSize, joystickIconSize);
    Rect display = new Rect(jsScreenOffset + offset, size);
    GUI.Label(display, joystickIcon);

    // scale joystick values to icon size
    float widthOffset = joystickIconSize / 2;
    Vector2 joystickPos = new Vector2(Utilities.Map(joystick.x, -1.0f, 1.0f, -widthOffset + 10, widthOffset - 10),
                                      Utilities.Map(joystick.y, -1.0f, 1.0f, widthOffset - 10, -widthOffset + 10));

    // find center point of JoyStick Background rectangle
    size = new Vector2(widthOffset, widthOffset);
    Vector2 centerPos = jsScreenOffset + offset + size;

    size = new Vector2(thumbstickSize, thumbstickSize);
    display = new Rect(centerPos + joystickPos - size / 2, size);
    GUI.Label(display, joystickIcon);

    // print raw values to screen
    display = new Rect(jsScreenOffset.x + offset.x,
                       jsScreenOffset.y + joystickIconSize + offset.y,
                       50F,
                       80F);

    string debugx, debugy, formatted;
    debugx = joystick.x.ToString("F2");
    debugy = joystick.y.ToString("F2");
    formatted = string.Format("x: {0, 5}\ny: {1, 5}", debugx, debugy);
    GUI.Label(display, formatted);
  }

  void DisplayCurrentAnimation()
  {
    GUI.Label(new Rect( Screen.width / 2 - 100,
                        Screen.height - 25,
                        500,
                        25),
              "Current State : " +
              anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
  }

  private void OnGUI()
  {
    if (displayDebug)
    {
      try
      {
        DisplayCurrentAnimation();
        DisplayJoystickInfo(joystick, Vector2.zero);
        DisplayJoystickInfo(groundMovement, new Vector2(50F, 0F));
      }
      catch (Exception e)
      {
        Debug.Log(e.StackTrace);
      }
    }
  }

  public void ToggleDebug()
  {
    displayDebug = !displayDebug;
  }
}
