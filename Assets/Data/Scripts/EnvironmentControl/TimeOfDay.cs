using System;
using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
  [Header("Time Settings")]
  [Range(0.10F, 10.0F)]
  public float minPerDay;
  [Range(0F, 360F)]
  public float currentAngle;
  [Range(0, 23)]
  public int hour;
  [Range(0, 59)]
  public int min;
  [Range(0, 59)]
  public int sec;
  public string timeOfDay;
  public bool pauseTime;

  [Header("Day Settings")]
  public GameObject Sun;
  public float sunDistance;
  public Material daySkybox;

  [Header("Night Settings")]
  public GameObject Moon;
  public float moonDistance;
  public Material nightSkybox;

  private float deltaAngle;
  private GameObject pauseMenu;
  private static readonly int secondsPerDay = 86400;
  private static readonly float DuskOffset = -90.0F;

  private enum Sky{ day, night }
  private Sky currentSky;

	void Start ()
  {
    pauseMenu = GameObject.FindGameObjectWithTag("Pause Menu");
    currentAngle = 0F;
    hour = 0;
    min = 0;
    sec = 0;
    currentSky = Sky.night;

    Sun.SetActive(false);
    RenderSettings.sun = null;
  }

  void Update ()
  {
    if (!pauseMenu.GetComponent<PauseMenuControl>().Paused)
    {
      /*
      *    angle    day     min
      *    ----- * ----- * -----
      *     day     min     sec
      */

      ComputeRotations();
      UpdateTime();
      SetOrientations();
      //BlendSkyboxes();
    }
  }

  void ComputeRotations()
  {
    int currentTimeInSeconds = hour * 3600 + min * 60 + sec;
    currentAngle = (currentTimeInSeconds / (float)secondsPerDay) * 360.0F;

    if (!pauseTime)
    {
      deltaAngle = Time.deltaTime * 360.0F / minPerDay / 60.0F;
      currentAngle += deltaAngle;
    }

    if (currentAngle >= 360.0F)
    {
      currentAngle %= 360.0f;
    }
  }

  void UpdateTime()
  {
    int currentTimeInSeconds = (int)((currentAngle / 360.0F) * secondsPerDay);

    TimeSpan ts = TimeSpan.FromSeconds(currentTimeInSeconds);
    hour = ts.Hours;
    min = ts.Minutes;
    sec = ts.Seconds;
    timeOfDay = string.Format("{0}:{1}:{2}", hour, min, sec);

     // sunset around 1800 hours
    // sunrise around 0600 hours
    if (6 <= hour && hour <= 17 && currentSky == Sky.night)
    {
      currentSky = Sky.day;
      Sun.SetActive(true);
      RenderSettings.sun = Sun.GetComponent<Light>();
      RenderSettings.skybox = daySkybox;
      DynamicGI.UpdateEnvironment();
    }
    else if ((hour < 6 || hour > 17) && currentSky == Sky.day)
    {
      currentSky = Sky.night;
      Sun.SetActive(false);
      RenderSettings.sun = null;
      RenderSettings.skybox = nightSkybox;
      DynamicGI.UpdateEnvironment();
    }
  }

  void SetOrientations()
  {
    // sun orientation
    Quaternion rotation = Quaternion.Euler(currentAngle + DuskOffset, 0F, 0F);
    Vector3 offset = new Vector3(0, 0, -sunDistance);
    Vector3 position = rotation * offset;
    Sun.transform.position = position;
    Sun.transform.rotation = rotation;

    // moon orientation
    rotation = Quaternion.Euler(currentAngle + DuskOffset, 0F, 0F);
    offset = new Vector3(0, 0, moonDistance);
    position = rotation * offset;
    Moon.transform.position = position;
    Moon.transform.rotation = rotation;

    Sun.transform.LookAt(Vector3.zero);
    Moon.transform.LookAt(Vector3.zero);
  }

  void BlendSkyboxes()
  {
    switch(currentSky)
    {
      case Sky.day:
        break;
      case Sky.night:

        break;
      default: break;
    }
  }
}
