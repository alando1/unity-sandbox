using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
  #region Variables

  // player locomotion
  private bool mForward;
  private bool mBack;
  private bool mLeft;
  private bool mRight;
  private bool mSprint;
  private bool mJump;
  private bool mCrouch;
  private bool mFly;
  private bool mDebug;
  private bool mAutoRun;
  private bool mIsAiming;
  private bool mAimingToggle;

  // mouse input
  private bool mFpsView;
  private bool mResetCam;
  private bool mLeftClick;
  private bool mMiddleClick;
  private bool mBackSideClick;
  private bool mFrontSideClick;
  private bool mLeftClickDown;
  private bool mRightClickDown;
  private bool mMiddleClickDown;
  private bool mBackSideClickDown;
  private bool mFrontSideClickDown;
  private float mMouseVert;
  private float mMouseHorz;
  [Header("Mouse Settings")]
  public float VerticalSensitivity;
  public float HorizontalSensitivity;

  #endregion

  #region Properties

  #region Mouse Input
  public float MouseVert         { get { return mMouseVert;          } }
  public float MouseHorz         { get { return mMouseHorz;          } }
  public bool FpsView            { get { return mFpsView;            } }
  public bool LeftClick          { get { return mLeftClick;          } }
  public bool MiddleClick        { get { return mMiddleClick;        } }
  public bool BackSideClick      { get { return mBackSideClick;      } }
  public bool FrontSideClick     { get { return mFrontSideClick;     } }
  public bool LeftClickDown      { get { return mLeftClickDown;      } }
  public bool RightClickDown     { get { return mRightClickDown;     } }
  public bool MiddleClickDown    { get { return mMiddleClickDown;    } }
  public bool BackSideClickDown  { get { return mBackSideClickDown;  } }
  public bool FrontSideClickDown { get { return mFrontSideClickDown; } }
  #endregion

  #region Movement Input
  public bool Forward     { get { return mForward; } }
  public bool Back        { get { return mBack;    } }
  public bool Left        { get { return mLeft;    } }
  public bool Right       { get { return mRight;   } }
  public bool Sprint      { get { return mSprint;  } }
  public bool Jump        { get { return mJump;    } }
  public bool Crouch      { get { return mCrouch;  } }
  public bool Fly         { get { return mFly;     } }
  public bool Debug       { get { return mDebug;   } }
  public bool AutoRun     { get { return mAutoRun; } }
  public bool IsAiming    { get { return mIsAiming; } }
  public bool ResetCam    { get { return mResetCam; } set { mResetCam = value; } }
  public bool AimingToggle { get { return mAimingToggle; } set { mAimingToggle = value; } }

  public GameObject Player { get { return this.gameObject; }  }
  #endregion

  #endregion

  void Start ()
  {
    Application.targetFrameRate = 60;
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    // initialize mouse data
    mMouseVert = 0.0f;
    mMouseHorz = 0.0f;
    mFpsView = false;
    mIsAiming = false;
    mLeftClick = false;
    mMiddleClick = false;
    mBackSideClick = false;
    mFrontSideClick = false;
    mLeftClickDown = false;
    mRightClickDown = false;
    mMiddleClickDown = false;
    mBackSideClickDown = false;
    mFrontSideClickDown = false;

    // initialize movement data
    mForward = false;
    mBack    = false;
    mLeft    = false;
    mRight   = false;
    mSprint  = false;
    mJump    = false;
    mCrouch  = false;
    mAutoRun = false;
  }
	
	// Update is called once per frame
	void Update ()
  {
    // Update Mouse Input
    mMouseHorz = Input.GetAxis("Mouse X") * Time.deltaTime * 120f * HorizontalSensitivity;
    mMouseVert = Input.GetAxis("Mouse Y") * Time.deltaTime * 120f * VerticalSensitivity * -1.0f;
    mLeftClick = Input.GetMouseButton(0);
    mMiddleClick = Input.GetMouseButton(2);
    mBackSideClick = Input.GetMouseButton(3);
    mFrontSideClick = Input.GetMouseButton(4);
    mLeftClickDown = Input.GetMouseButtonDown(0);
    mRightClickDown = Input.GetMouseButtonDown(1);
    mMiddleClickDown = Input.GetMouseButtonDown(2);
    mBackSideClickDown= Input.GetMouseButtonDown(3);
    mFrontSideClickDown = Input.GetMouseButtonDown(4);

    mIsAiming = Input.GetButton("Aim");
    mForward = Input.GetButton("Forward");
    mBack    = Input.GetButton("Back");
    mLeft    = Input.GetButton("Left");
    mRight   = Input.GetButton("Right");
    mSprint  = Input.GetButton("Sprint");
    mFly     = Input.GetButtonDown("Fly") ? !mFly : mFly;
    mJump    = Input.GetButtonDown("Jump");
    mCrouch  = Input.GetButtonDown("Crouch") ? !mCrouch : mCrouch;
    mDebug = Input.GetButtonDown("CapsLock");// ? !mDebug : mDebug;
    mAutoRun = Input.GetButtonDown("AutoRun") ? !mAutoRun : mAutoRun;

    if (mDebug)
    {
      mFpsView = !mFpsView;
      mResetCam = true;
    }
  }
}
