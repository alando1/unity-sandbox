using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

  // Components
  private VehicleController vehicleControl;
  private PlayerController playerControl;
  private InputController inputControl;
  private Animator animator;

  public Transform firstPersonView;
  private LayerMask mask;

  public Transform player;
  public float angle;
  public float distance = 2.0f;
  
  public float yMinLimit = -70f;
  public float yMaxLimit = 70f;

  public float distanceMin = 1f;
  public float distanceMax = 15f;
  public float smoothTime = 0.1f;
  public float fpsDistance = 0.35f;

  [Header("Over the shoulder")]
  public Vector3 shoulderPivot;
  private Vector3 TPoffset;

  public float x = 0.0f;
  public float y = 0.0f;
  public float TPx = 0.0f;
  public float TPy = 0.0f;

  private Vector3 initial;
  private Vector3 final;

  private Vector3 velocity;
  private float colliderRadius = 0.05f;
  private PauseMenuControl pause;

  #region External Properties

  private bool FirstPersonView
  {
    get { return inputControl.FpsView; }
  }
  #endregion

  // Use this for initialization
  void Start()
  {
    vehicleControl = player.GetComponent<VehicleController>();
    playerControl = player.GetComponent<PlayerController>();
    inputControl = player.GetComponent<InputController>();
    animator = player.GetComponent<Animator>();
    pause = GameObject.FindGameObjectWithTag("Pause Menu").GetComponent<PauseMenuControl>();
    velocity = Vector3.zero;

    int layer = LayerMask.NameToLayer("Player");
    mask = ~(1 << layer);
    Debug.Log("Player Layer from NameToLayer: " + layer);
    Debug.Log("Mask Value ~(1 << layer) = " + mask.value);
  }

  private void Update()
  {
    x += player.GetComponent<InputController>().MouseHorz;
    y += player.GetComponent<InputController>().MouseVert;
    y = ClampAngle(y, yMinLimit, yMaxLimit);
  }

  void LateUpdate()
  {
    TPoffset = new Vector3(shoulderPivot.x, shoulderPivot.y, shoulderPivot.z);

    if (!pause.Paused)
    {
      switch (player.GetComponent<PlayerController>().PlayerState)
      {
        case PlayerController.ControlStates.GROUND_CONTROL: GroundCamera(); break;
        case PlayerController.ControlStates.CAR_CONTROL: DrivingCamera(); break;
        default: break;
      }
    }
  }

  public static float ClampAngle(float angle, float min, float max)
  {
    Mathf.Repeat(angle, 360F);
    if (angle < -360F)
      angle += 360F;
    else if (angle > 360F)
      angle -= 360F;
    return Mathf.Clamp(angle, min, max);
  }

  private void DrivingCamera()
  {

    if (FirstPersonView)
    {
      x = ClampAngle(x, -120.0f, 120.0f);
      Quaternion rotation = Quaternion.Euler(y, x, 0);
      transform.position = firstPersonView.position;
      transform.rotation = firstPersonView.rotation * rotation;
    }
    else
    {
      Quaternion rotation = Quaternion.Euler(y, x, 0);
      distance = Mathf.Clamp(distance - Input.mouseScrollDelta.y, distanceMin, distanceMax);

      RaycastHit hit;
      Vector3 desiredOffset = Vector3.zero;
      Vector3 playerHead = animator.GetBoneTransform(HumanBodyBones.Head).position;
      Vector3 direction = transform.position - playerHead;
      Vector3 offset = new Vector3(0F, 0F, -direction.magnitude);
      if (Physics.SphereCast(playerHead, colliderRadius, direction, out hit, distance))
      {
        desiredOffset.z = (hit.distance < distanceMin) ? -distanceMin : -hit.distance;
        //Debug.Log(hit.collider.name);
      }
      else
        desiredOffset.z = -distance;

      // smooth offset reposition
      offset = Vector3.SmoothDamp(offset, desiredOffset, ref velocity, smoothTime);
      Vector3 position = rotation * offset + playerHead;

      transform.rotation = rotation;
      transform.position = position;
    }
  }

  private void GroundCamera()
  {
    if (FirstPersonView)
    {
      Quaternion rotation = Quaternion.Euler(y, x, 0F);
      var position = player.position + new Vector3(0F, shoulderPivot.y, 0F) + rotation * new Vector3(0,0,fpsDistance);
      transform.position = position;
      transform.rotation = rotation;
    }
    else
    {
      // Update distance and clamp to min max
      //distance = Mathf.Clamp(distance - Input.mouseScrollDelta.y, distanceMin, distanceMax);
      distance = 2.0f;

      // Store global rotation and desired camera offset
      Quaternion rotation = Quaternion.Euler(y, x, 0F);
      Vector3 offset = new Vector3(0F, 0F, -distance);
      Quaternion horzRot = Quaternion.Euler(0F, x, 0F);
      Quaternion vertRot = Quaternion.Euler(y, 0F, 0F);

      // Store initial and final points to create a camera direction
      initial = player.position + horzRot * new Vector3(shoulderPivot.x, shoulderPivot.y, shoulderPivot.z);
      final = initial + rotation * offset;
      Vector3 direction = (final - initial);
      direction.Normalize();
      // Initialize collision output variables
      Vector3 desiredOffset = Vector3.zero;
      RaycastHit hit;

      var result = Physics.SphereCast(initial, colliderRadius, direction, out hit, distance, mask);
      if (result)
      {
        desiredOffset.z = -hit.distance;
        Debug.Log(hit.collider.name);
      }
      else
        desiredOffset.z = -distance;

      desiredOffset = rotation * desiredOffset + initial;

      // Update camera position and rotation
      transform.position = desiredOffset;
      transform.rotation = rotation;

    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.DrawWireCube(initial, new Vector3(0.1f,0.1f,0.1f));
    Gizmos.DrawWireCube(final, new Vector3(0.1f,0.1f,0.1f));
    Gizmos.DrawWireSphere(transform.position, colliderRadius);
    Debug.DrawRay(transform.position, transform.forward, Color.red);
  }
}
