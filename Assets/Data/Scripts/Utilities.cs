using System;
using System.Globalization;
using UnityEngine;

namespace Assets.Data.Scripts
{
  class Utilities : MonoBehaviour
  {
    private GameObject player;

    private void Start()
    {
      player = GameObject.Find("Human");
    }

    public static float Map(float value, float oldLowerBound, float oldUpperBound, float newLowerBound, float newUpperBound)
    {
      float normal = Mathf.InverseLerp(oldLowerBound, oldUpperBound, value);
      return Mathf.Lerp(newLowerBound, newUpperBound, normal);
    }

    // Calculate the initial velocity of a jump based off gravity and desired maximum height attained
    public static float CalculateJumpSpeed(float jumpHeight, float gravity)
    {
      return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    private void OnDrawGizmos()
    {
      if (player.GetComponent<Movement>().OnGround)
      {
        Gizmos.DrawSphere(player.transform.position + new Vector3(0, 0.175f, 0), 0.35f);
      }
    }


  }
}
