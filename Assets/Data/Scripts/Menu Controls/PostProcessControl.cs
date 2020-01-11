using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessControl : MonoBehaviour
{
  public PostProcessVolume volume;
  MotionBlur motionBlur;

  void Start ()
  {
    volume = GetComponent<PostProcessVolume>();
    bool motionBlurPresent = volume.profile.TryGetSettings<MotionBlur>(out motionBlur);
    print("Motion Blur is " + (motionBlurPresent ? "Enabled" : "Disabled"));
	}
	
	void Update ()
  {
	}

  public void ToggleMotionBlur()
  {
    if(motionBlur != null)
    {
      motionBlur.enabled.Override(!motionBlur.enabled);
      print("Motion Blur is " + (motionBlur.enabled ? "Enabled" : "Disabled"));
    }
  }
}
