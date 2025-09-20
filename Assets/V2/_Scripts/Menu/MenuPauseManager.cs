using UnityEngine;

public class MenuPauseManager : MonoBehaviour
{
    public static MenuPauseManager instance;

    public bool gamePaused { get; private set; } = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void TogglePause()
    {
        gamePaused = !gamePaused;

        if (gamePaused)
            Pause();
        else
            Resume();
    }

    private void Pause()
    {
        Time.timeScale = 0f;
    }

    private void Resume()
    {
        Time.timeScale = 1f;
    }
}