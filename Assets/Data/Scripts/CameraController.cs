using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  public Transform target;
  public Transform firstPersonView;
  public LayerMask ignoreMask;

  public float distance = 2.0f;
  public float xSpeed = 120.0f;
  public float ySpeed = 120.0f;

  public float yMinLimit = -80f;
  public float yMaxLimit = 80f;

  public float distanceMin = .5f;
  public float distanceMax = 15f;
  public float smoothTime = 0.1f;

  [Header("Over the shoulder")]
  public Vector3 shoulderOffset;
  public Transform shoulderPivot;

  private Vector3 velocity;

  #region External Properties

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
    velocity = Vector3.zero;
  }

  void LateUpdate()
  {
    switch (GetComponentInParent<PlayerController>().PlayerState)
    {
      case PlayerController.ControlStates.GROUND_CONTROL: GroundCamera(); break;
      case PlayerController.ControlStates.CAR_CONTROL: DrivingCamera(); break;
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
    Vector3 currentPosition = transform.position;
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
        Vector3 desiredOffset = Vector3.zero;
        float radius = 0.5f;
        Vector3 direction = transform.position - target.position;
        Vector3 offset = new Vector3(0F, 0F, -direction.magnitude);
        if (Physics.SphereCast(target.position, radius, direction, out hit, distance))
        {
          desiredOffset.z = (hit.distance < distanceMin) ? -distanceMin : -hit.distance;
          //Debug.Log(hit.collider.name);
        }
        else
          desiredOffset.z = -distance;

        // smooth offset reposition
        offset = Vector3.SmoothDamp(offset, desiredOffset, ref velocity, smoothTime);
        Vector3 position = rotation * offset + target.position;

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
      Vector3 cameraOffset = new Vector3(0, 0, -distance);
      Quaternion rotation = transform.parent.rotation * Quaternion.Euler(y, 0, 0);
      Vector3 desiredCameraPos = shoulderPivot.position + rotation * cameraOffset;

      Vector3 direction = transform.position - shoulderPivot.position;
      Vector3 offset = new Vector3(0F, 0F, -direction.magnitude);
      Vector3 desiredOffset = Vector3.zero;
      RaycastHit hit;

      if (Physics.SphereCast(shoulderPivot.position, 0.35f, direction, out hit, distance))
      {
          desiredOffset.z = (hit.distance < distanceMin) ? -distanceMin : -hit.distance;
          Debug.Log(hit.collider.name);
      }
      else
        desiredOffset.z = -distance;

      offset = Vector3.SmoothDamp(offset, desiredOffset, ref velocity, smoothTime);
      desiredCameraPos = rotation * offset + shoulderPivot.position;

      transform.position = desiredCameraPos;
      transform.rotation = rotation;
    }
  }
}
