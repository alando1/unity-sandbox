using UnityEngine;

public class PauseMenu : MonoBehaviour
{
  private static bool mPaused = false;
  public bool Paused { get { return mPaused; } }
  public GameObject PauseMenuUI;
  public AudioSource onButtonSound;
  public AudioClip click;

  private void Start()
  {
    PauseMenuUI.SetActive(false);
    //onButtonSound = GetComponent<AudioSource>();
    //onButtonSound.clip = click;
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
    PauseMenuUI.SetActive(false);

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
    Debug.Log("Control Options");
  }

  public void Video()
  {
    Debug.Log("Display Options.");
  }

  public void Audio()
  {
    Debug.Log("Audio Options.");
  }

  public void Pause()
  {
    PauseMenuUI.SetActive(true);
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
