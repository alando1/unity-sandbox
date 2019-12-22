using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSettings : MonoBehaviour {

  [Range(3000.0F, 100000.0F)]
  public float distance;

  [Range(1.0F, 10000.0F)]
  public float size;

  [Header("Satellites")]
  public int numberOfMoons;
  public int[] sizes;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    transform.localPosition = transform.parent.localPosition + new Vector3(0, 0, distance);
    transform.localScale = new Vector3(size, size, size);
	}
}
