using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public Vector3 aimOffset = new Vector3(-3.0f, -1.0f, 1.0f); 
    //bool aiming = false;
	// Use this for initialization
	void Start ()
    {
        Movement.ADS += Aim;
        Movement.StopADS += StopAim;
		
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void Aim(object o)
    {
        //var player = o as Movement;
        
        //if(player != null)
        //{
        //    player.Camera...
        //}
    }

    public void StopAim(object o)
    {

    }
}
