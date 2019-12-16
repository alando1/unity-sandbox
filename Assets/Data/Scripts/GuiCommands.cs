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

  private Animator anim;

  // Use this for initialization
  void Start ()
  {
    anim = GetComponent<Animator>();
  }

  private Vector2 Joystick { get { return GetComponent<Movement>().JoyStick; } }
	
	// Update is called once per frame
	void Update ()
  {
		
	}

  private void DisplayJoystickInfo()
  {
    // JoyStick Background
    Vector2 size = new Vector2(joystickIconSize, joystickIconSize);
    Rect display = new Rect(jsScreenOffset, size);
    GUI.Label(display, joystickIcon);

    // scale joystick values to icon size
    float widthOffset = joystickIconSize / 2;
    Vector2 joystickPos = new Vector2(Utilities.Map(Joystick.x, -1.0f, 1.0f, -widthOffset + 10, widthOffset - 10),
                                      Utilities.Map(Joystick.y, -1.0f, 1.0f, widthOffset - 10, -widthOffset + 10));

    // find center point of JoyStick Background rectangle
    size = new Vector2(widthOffset, widthOffset);
    Vector2 centerPos = jsScreenOffset + size;

    size = new Vector2(thumbstickSize, thumbstickSize);
    display = new Rect(centerPos + joystickPos - size / 2, size);
    GUI.Label(display, joystickIcon);

    // print raw values to screen
    display = new Rect(jsScreenOffset.x, jsScreenOffset.y + joystickIconSize, 50, 80);
    string debugx, debugy, formatted;
    debugx = Joystick.x.ToString("F2");
    debugy = Joystick.y.ToString("F2");
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
    try
    {
      DisplayCurrentAnimation();
      DisplayJoystickInfo();
    }
    catch (Exception e)
    {
      Debug.Log(e.StackTrace);
    }

  }
}
