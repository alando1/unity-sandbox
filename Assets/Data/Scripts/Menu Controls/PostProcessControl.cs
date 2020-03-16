using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessControl : MonoBehaviour
{
  PostProcessVolume volume;
  MotionBlur motionBlur;
  bool motionBlurEnabled;

  [Header("Menu Components")]
  public UnityEngine.UI.Slider shutterAngleSlider;
  public UnityEngine.UI.Slider sampleCountSlider;
  public TMP_InputField shutterAngleInput; 
  public TMP_InputField sampleCountInput;

  public int sampleCount = 4;
  public int shutterAngle = 45;

  void Start ()
  {
    // initialize slider ranges and defaults
    shutterAngleSlider.minValue = 0;
    shutterAngleSlider.maxValue = 360;
    shutterAngleSlider.wholeNumbers = true;
    shutterAngleSlider.value = shutterAngle;
    shutterAngleInput.text = shutterAngle.ToString();
    sampleCountSlider.minValue = 4;
    sampleCountSlider.maxValue = 32;
    sampleCountSlider.wholeNumbers = true;
    sampleCountSlider.value = sampleCount;
    sampleCountInput.text = sampleCount.ToString();

    volume = GetComponent<PostProcessVolume>();
    bool motionBlurPresent = volume.profile.TryGetSettings(out motionBlur);
    if(motionBlurPresent)
    {
      print("Motion Blur is Available.");
      motionBlurEnabled = motionBlur.enabled;
      print("Motion Blur is " + (motionBlurEnabled ? "ENABLED." : "DISABLED"));
    }
    else
      print("Motion Blur is Not Available.");
  }

  public void ToggleMotionBlur()
  {
    if(motionBlur != null)
    {
      motionBlurEnabled = !motionBlurEnabled;
      motionBlur.enabled.Override(motionBlurEnabled);
      print("Motion Blur is " + (motionBlurEnabled ? "ENABLED." : "DISABLED"));
    }
  }

  public void SetShutterAngle(float i)
  {
    if (motionBlur != null)
    {
      shutterAngle = Mathf.Clamp((int)i, 0, 360);
      motionBlur.shutterAngle.value = shutterAngle;
      shutterAngleSlider.value = shutterAngle;
      shutterAngleInput.text = shutterAngle.ToString();
      Debug.Log(shutterAngle);
    }
  }

  public void SetShutterAngle(string s)
  {
    if (motionBlur != null)
    {
      int i = -1;
      bool result = int.TryParse(s, out i);

      if (result && i != -1)
      {
        SetShutterAngle(i);
      }
    }
  }

  public void SetSampleCount(float i)
  {
    if (motionBlur != null)
    {
      sampleCount = Mathf.Clamp((int)i, 4, 32);
      motionBlur.sampleCount.value = sampleCount;
      sampleCountSlider.value = sampleCount;
      sampleCountInput.text = sampleCount.ToString();
      Debug.Log(sampleCount);
    }
  }

  public void SetSampleCountInput(string s)
  {
    if (motionBlur != null)
    {
      int i = -1;
      bool result = int.TryParse(s, out i);

      if (result && i != -1)
      {
        SetSampleCount(i);
      }
    }
  }
}
