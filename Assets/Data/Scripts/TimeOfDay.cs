using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDay : MonoBehaviour {

  public Gradient groundGradient;
  public Gradient skyGradient;
  public Gradient equatorGradient;
  public float ambientIntensity;
  public float period = 15.0f;

  [Range(0.00f, 1.00f)]
  public float current;

	// Use this for initialization
	void Start ()
  {
		
	}
	
	// Update is called once per frame
	void Update ()
  {
    current = Mathf.MoveTowards(current, 1.0f, (1 / period) * Time.deltaTime);

    if (period <= 0.0f)
    {
      period = 0.001f;
    }

    if (current >= 1.0f)
    {
      current -= Mathf.Ceil(current);
    }


      RenderSettings.ambientEquatorColor = equatorGradient.Evaluate(current);
    RenderSettings.ambientGroundColor = groundGradient.Evaluate(current);
    RenderSettings.ambientSkyColor = skyGradient.Evaluate(current);
	}
}
