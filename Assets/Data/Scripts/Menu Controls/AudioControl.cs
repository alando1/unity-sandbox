using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
  public UnityEngine.UI.Slider volumeSlider;
  public UnityEngine.Audio.AudioMixer masterVolume;

  [Range(-80F, 0f)]
  public float defaultVolume;

	void Start ()
  {
    volumeSlider.minValue = -80F;
    volumeSlider.maxValue = 0F;
    volumeSlider.wholeNumbers = false;
    volumeSlider.value = defaultVolume;
    masterVolume.SetFloat("MasterVolume", volumeSlider.value);
	}
	
  public void SetVolume(float v)
  {
    masterVolume.SetFloat("MasterVolume", v);
  }
}
