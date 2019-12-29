using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  public GameObject player;
  public Transform target;
  public Transform firstPersonView;
  public float distance = 2.0f;
  public float xSpeed = 120.0f;
  public float ySpeed = 120.0f;

  public float yMinLimit = -80f;
  public float yMaxLimit = 80f;

  public float distanceMin = .5f;
  public float distanceMax = 15f;

  [Header("Over the shoulder")]
  public Vector3 shoulderOffset;

  #region External Properties
  private bool ResetCamera
  {
    get { return GetComponentInParent<InputController>().ResetCam;  }
    set { GetComponentInParent<InputController>().ResetCam = value; }
  }

  private bool FirstPersonView
  {
    get { return GetComponentInParent<InputController>().FpsView; }
  }
  #endregion

  float x = 0.0f;
  float y = 0.0f;

  // Use this for initialization
  void Start()
  {
    Vector3 angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;
  }

  void LateUpdate()
  {
    var pState = GetComponentInParent<Movement>().PlayerState;
    switch(pState)
    {
      case Movement.State.driving: DrivingCamera(); break;
      case Movement.State.onGround: GroundCamera(); break;
      default: break;
    }
  }

  public static float ClampAngle(float angle, float min, float max)
  {
    if (angle < -360F)
      angle += 360F;
    if (angle > 360F)
      angle -= 360F;
    return Mathf.Clamp(angle, min, max);
  }

  private void DrivingCamera()
  {
    if (GetComponentInParent<InputController>().ResetCam)
    {
      x = 0f; y = 0f;
      GetComponentInParent<InputController>().ResetCam = false;
    }

    x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
    y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
    y = ClampAngle(y, yMinLimit, yMaxLimit);

    if (FirstPersonView)
    {
      x = ClampAngle(x, -120.0f, 120.0f);
      Quaternion rotation = Quaternion.Euler(y, x, 0);
      transform.position = firstPersonView.position;
      transform.rotation = firstPersonView.rotation * rotation;
    }
    else
    {
      if (target != null)
      {
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        distance = Mathf.Clamp(distance - Input.mouseScrollDelta.y, distanceMin, distanceMax);

        RaycastHit hit;
        Vector3 negDistance;
        Vector3 direction = transform.position - target.position;
        if (Physics.SphereCast(target.position, 0.35f, direction, out hit, distance))
        {
          negDistance = (hit.distance < distanceMin) ?  
               new Vector3(0.0f, 0.0f, -distanceMin) : 
               new Vector3(0.0f, 0.0f, -hit.distance);
          //Debug.Log(hit.collider.name);
        }
        else
          negDistance = new Vector3(0.0f, 0.0f, -distance + 0.35f);

        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
      }
    }
  }
  private void GroundCamera()
  {
    y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
    y = ClampAngle(y, yMinLimit, yMaxLimit);
    distance = Mathf.Clamp(distance - Input.mouseScrollDelta.y, distanceMin, distanceMax);

    if (FirstPersonView)
    {
      transform.position = firstPersonView.position;
      transform.forward = Quaternion.AngleAxis(y, transform.parent.right) * transform.parent.forward;
    }
    else
    {
      Quaternion rotation = transform.parent.rotation * Quaternion.Euler(y, 0, 0);
      Vector3 position = transform.parent.position + transform.parent.rotation * shoulderOffset;
      Vector3 cameraOffset = new Vector3(0, 0, -distance);
      Vector3 newCamPos = rotation * cameraOffset + position;

      RaycastHit hit;
      // change this to sphere cast
      if (Physics.Linecast(position, newCamPos, out hit))
      {
        cameraOffset.z = -hit.distance;
        newCamPos = rotation * cameraOffset + position;
      }

      transform.rotation = rotation;
      transform.position = newCamPos;
    }
  }
}
