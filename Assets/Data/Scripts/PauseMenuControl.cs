using UnityEngine;

public class PauseMenuControl : MonoBehaviour
{
  private static bool mPaused = false;
  public bool Paused { get { return mPaused; } }
  public GameObject PauseMenu;
  public GameObject ControlMenu;
  public GameObject VideoMenu;
  public GameObject AudioMenu;
  public AudioSource onButtonSound;
  public AudioClip click;

  private void Start()
  {
    PauseMenu.SetActive(false);
    VideoMenu.SetActive(false);
    AudioMenu.SetActive(false);
    ControlMenu.SetActive(false);
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (mPaused)
        Resume();
      else
        Pause();

      mPaused = !mPaused;
    }
  }

  public void Resume()
  {
    PauseMenu.SetActive(false);
    ControlMenu.SetActive(false);
    VideoMenu.SetActive(false);
    AudioMenu.SetActive(false);

    // hide mouse and lock position to center
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
    Time.timeScale = 1.0f;
    AudioListener.pause = false;
    Debug.Log("Resuming ...");
  }

  public void Controls()
  {
    // gui with current mapped inputs for different states
    // Make controls mappable/check for conflicts
    Debug.Log("Control Options.");
  }

  public void Video()
  {
    Debug.Log("Video Options.");
  }

  public void Audio()
  {
    Debug.Log("Audio Options.");
  }

  public void Pause()
  {
    PauseMenu.SetActive(true);
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
    Time.timeScale = 0.0f;
    AudioListener.pause = true;
    Debug.Log("Paused ...");
  }

  public void Exit()
  {
    Debug.Log("Exiting ...");
    Application.Quit();
  }

  private void playMenuSound()
  {
    onButtonSound.Play();
  }
}
