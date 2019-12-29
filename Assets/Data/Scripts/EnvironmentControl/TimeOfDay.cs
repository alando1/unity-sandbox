using System;
using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
  [Range(0.10F, 10.0F)]
  public float minPerDay;
  public bool pauseTime;

  public Transform Sun, Moon;
  public float sunDistance;
  public float moonDistance;

  public float deltaAngle;
  [Range(0F, 360F)]
  public float currentAngle;

  [Range(0, 23)]
  public int hour;
  [Range(0, 59)]
  public int min;
  [Range(0, 59)]
  public int sec;
  public string timeOfDay;

  private GameObject pauseMenu;
  private static readonly int secondsPerDay = 86400;

	void Start ()
  {
    pauseMenu = GameObject.Find("Pause Menu Canvas");
    currentAngle = 0F;
    hour = 0;
    min = 0;
    sec = 0;
  }

  // sunrise around 0600 hours
  //  sunset around 1800 hours

  void Update ()
  {
    if (!pauseMenu.GetComponent<PauseMenu>().Paused)
    {
      /*
      *    angle    day     min
      *    ----- * ----- * -----
      *     day     min     sec
      */

      ComputeRotations();

      if (!pauseTime)
      {

        deltaAngle = Time.deltaTime * 360.0F / minPerDay / 60.0F;
        currentAngle += deltaAngle;
      }

      UpdateTime();
      SetOrientations();
    }


  }

  void ComputeRotations()
  {
    int currentTimeInSeconds = hour * 3600 + min * 60 + sec;
    currentAngle = (currentTimeInSeconds / (float)secondsPerDay) * 360.0F;

    if (currentAngle >= 360.0F)
    {
      currentAngle %= 360.0f;
    }
  }

  void UpdateTime()
  {
    if (currentAngle >= 360.0F)
    {
      currentAngle %= 360.0f;
    }

    int currentTimeInSeconds = (int)((currentAngle / 360.0F) * secondsPerDay);

    TimeSpan ts = TimeSpan.FromSeconds(currentTimeInSeconds);
    hour = ts.Hours;
    min = ts.Minutes;
    sec = ts.Seconds;
    timeOfDay = string.Format("{0}:{1}:{2}", hour, min, sec);
  }

  void SetOrientations()
  {
    // sun orientation
    Quaternion rotation = transform.rotation * Quaternion.Euler(currentAngle, 0F, 0F);
    Vector3 position = transform.position + transform.rotation * Vector3.zero;
    Vector3 offset = new Vector3(0, 0, -sunDistance);
    position = rotation * offset + position;
    Sun.position = position;
    Sun.rotation = rotation;

    // moon orientation
    rotation = transform.rotation * Quaternion.Euler(currentAngle, 0F, 0F);
    position = transform.position + transform.rotation * Vector3.zero;
    offset = new Vector3(0, 0, moonDistance);
    position = rotation * offset + position;
    Moon.position = position;
    Moon.rotation = rotation;

    Sun.LookAt(Vector3.zero);
    Moon.LookAt(Vector3.zero);
  }
}
