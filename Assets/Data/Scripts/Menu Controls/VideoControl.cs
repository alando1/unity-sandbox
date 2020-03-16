using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VideoControl : MonoBehaviour
{
  Resolution[] resolutions;
  public TMP_Dropdown resolutionsDropdown;
  public UnityEngine.UI.Toggle fullscreenToggle;
  int currentResolutionIndex;

  void Start ()
  {
    resolutionsDropdown.ClearOptions();
    Vector2 current = new Vector2(Screen.width, Screen.height);
    Vector2 option;
    resolutions = Screen.resolutions;
    List<string> options = new List<string>();

    for (int i = 0; i < resolutions.Length; i++)
    {
      option = new Vector2(resolutions[i].width, resolutions[i].height);
      options.Add(option.x + " X " + option.y + " @ " + resolutions[i].refreshRate + " Hz");
      if (option == current)
      {
        currentResolutionIndex = i;
      }
    }

    resolutionsDropdown.AddOptions(options);
    resolutionsDropdown.value = currentResolutionIndex;
    resolutionsDropdown.RefreshShownValue();
    Screen.fullScreen = fullscreenToggle.isOn;
	}

  public void SetQuality(int i)
  {
    QualitySettings.SetQualityLevel(i);
  }

  public void ToggleFullscreen(bool b)
  {
    Screen.fullScreen = b;
    Debug.Log("Screen.fullscreen : " + b.ToString() + "\nfullScreenToggle.IsOn : " + fullscreenToggle.isOn.ToString());
  }
  
  public void SetResolution(int i)
  {
    resolutionsDropdown.value = i;
    Screen.SetResolution(resolutions[i].width, resolutions[i].height, fullscreenToggle.isOn);
  }
}
