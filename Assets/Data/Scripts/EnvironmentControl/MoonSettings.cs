using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSettings : MonoBehaviour {

  [Range(3000.0F, 100000.0F)]
  public float distance;

  [Range(1.0F, 10000.0F)]
  public float size;

  //private Vector3 axisOfRotation;

	void Start ()
  {
    //axisOfRotation = new Vector3(1F, 0F, 0F);
	}
	
	void Update ()
  {
    //transform.localPosition = transform.parent.localPosition + new Vector3(0, 0, distance);
    //transform.localScale = new Vector3(size, size, size);
	}
}
