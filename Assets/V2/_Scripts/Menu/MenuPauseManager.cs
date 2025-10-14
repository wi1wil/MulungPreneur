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

    public void SetPaused(bool pause)
    {
        if (pause)
            Pause();
        else
            Resume();
    }

    private void Pause()
    {
        if (gamePaused) return;
        gamePaused = true;
        Time.timeScale = 0f;
    }

    private void Resume()
    {
        if (!gamePaused) return;
        gamePaused = false;
        Time.timeScale = 1f;
    }
}
