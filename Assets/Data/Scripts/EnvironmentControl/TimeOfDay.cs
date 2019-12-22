using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDay : MonoBehaviour
{  
  private GameObject pauseMenu;

  [Range(0.10F, 10.0F)]
  public float minPerDay;

	void Start ()
  {
    pauseMenu = GameObject.Find("Pause Menu Canvas");
    
  }
	
	// Update is called once per frame
	void Update ()
  {
    if (!pauseMenu.GetComponent<PauseMenu>().Paused)
    {
      float deltaAngle = Time.deltaTime * 360.0F / 60.0F / minPerDay;
      Vector3 deltaVector = new Vector3(deltaAngle, 0F, 0F);
      transform.Rotate(deltaVector, Space.Self);
    }
  }
}
