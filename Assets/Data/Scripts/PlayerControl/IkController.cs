using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class IkController : MonoBehaviour
{
  protected Animator animator;
  protected Camera cam;
  protected InputController inputController;
  protected InventoryManager inventoryManager;
  protected PlayerController playerController;

  public bool ikActive = false;

  float weaponWeight = 0F;
  float aimWeight = 0F;
  float aimSpeed = 1F;
  public float bodyWeight;
  public float headWeight;
  public float eyesWeight;
  public float clampWeight;
  private Vector3 poi;
  public Transform lookAtObj;
  public Vector3 lhIkRiflePos;
  public Vector3 rhIkRiflePos;
  public Vector3 leftHandRotation;

  private enum WeaponType { NONE, RIFLE, SMG, HANDGUN, THROW };
  private WeaponType mWeapon = WeaponType.NONE;
  int prevItem;
  bool prevAim;

  void Start()
  {
    cam = Camera.main;
    animator = GetComponent<Animator>();
    inventoryManager = GetComponent<InventoryManager>();
    inputController = GetComponent<InputController>();
    playerController = GetComponent<PlayerController>();
    prevItem = inventoryManager.currentItem;
    prevAim = inputController.IsAiming;
    lhIkRiflePos = leftHandRotation = Vector3.zero;
  }

  void OnAnimatorIK()
  {
    if (ikActive)
    {
      switch(playerController.PlayerState)
      {
        case PlayerController.ControlStates.GROUND_CONTROL:
      SetLook();
      UpdateWeights();
      HandleWeapon();
          break;
        case PlayerController.ControlStates.CAR_CONTROL:

          var leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
          var rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);

          animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
          animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
          animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
          animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
          break;
      }
    }
  }

  private void OnDrawGizmos()
  {
    if(poi != null)
    {
      Gizmos.DrawSphere(poi, 1f);
    }
  }

  void UpdateWeights()
  {
    if (playerController.PlayerState == PlayerController.ControlStates.GROUND_CONTROL)
    {
      // On change, reset weapon layer weight
      if (prevItem != inventoryManager.currentItem)
      {
        weaponWeight = 0F;
      }

      SetClampedWeight(ref aimWeight, aimSpeed, inputController.IsAiming);
      SetClampedWeight(ref weaponWeight, aimSpeed, CurrentWeaponType() == WeaponType.RIFLE);
    }
    else
    {

      weaponWeight = 0F;
    }
  }

  void SetClampedWeight(ref float weight, float speed, bool increase)
  {
    var t = weight + (increase ? 1F : -1F) * speed * Time.deltaTime;
    weight = Mathf.Clamp(t, 0F, 1F);
  }

  void SetLook()
  {
    bool crouching = GetComponent<InputController>().Crouch;
    Ray ray = new Ray(cam.transform.position, cam.transform.forward);//cam.ScreenPointToRay(Input.mousePosition);
    poi = ray.GetPoint(25F);

    if (GetComponent<PlayerController>().moveMag < 0.1F)
    {
      animator.SetLookAtWeight(1F, bodyWeight, headWeight, eyesWeight, clampWeight);
      if (lookAtObj != null)
        animator.SetLookAtPosition(lookAtObj.position);
      else
        animator.SetLookAtPosition(poi);
    }
    else
    {
      animator.SetLookAtWeight(0F);
      animator.SetBoneLocalRotation(HumanBodyBones.Chest, Quaternion.identity);
    }
  }

  void HandleWeapon()
  {
    animator.SetLayerWeight(animator.GetLayerIndex("RifleLayer"), weaponWeight);

    var leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
    var rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);

    if (inventoryManager.inventory[inventoryManager.currentItem] != null)
    {
      if (inventoryManager.inventory[inventoryManager.currentItem].transform.GetChild(0).name == "LeftHandTransform")
      {
        var t = inventoryManager.inventory[inventoryManager.currentItem].transform.GetChild(0);
        leftHand.position = t.position;
        leftHand.rotation = t.rotation;

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
      }
      else
      {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
      }
    }

    prevItem = inventoryManager.currentItem;
    prevAim = inputController.IsAiming;
  }

  WeaponType CurrentWeaponType()
  {
    switch (inventoryManager.currentItem)
    {
      case 0: return WeaponType.NONE;
      case 1: return WeaponType.SMG;
      case 2: return WeaponType.RIFLE;
      case 3: return WeaponType.RIFLE;
      default: return WeaponType.NONE;
    }
  }
}