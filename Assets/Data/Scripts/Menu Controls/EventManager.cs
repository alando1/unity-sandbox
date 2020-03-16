using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioClip))]
public class EventManager : MonoBehaviour {

  public AudioSource audioSource;
  public AudioClip buttonHoverClip;

	void Start ()
  {
    audioSource = GetComponent<AudioSource>();
	}
	
	void Update ()
  {
		
	}

  public void PlayHoverButtonSound()
  {
    audioSource.PlayOneShot(buttonHoverClip);
    Debug.Log("HoverButtonSound should play now...");
  }


}
