using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
  public Transform Target;

	void Start ()
  {
	}
	
	void Update ()
  {
		if (Target != null)
    {
      transform.LookAt(Target);
    }
	}
}
