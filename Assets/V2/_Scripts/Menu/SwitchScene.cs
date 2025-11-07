using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void LoadGame()
    {
        AudioManager.instance.PlayUIClick();
        SceneManager.LoadScene(1);  // Load the main game scene (index 2)
        MenuPauseManager.instance.SetPaused(false);
    }

    public void BackToMenu()
    {
        AudioManager.instance.PlayUIClick();
        SceneManager.LoadScene(0);  // Load the main menu scene (index 0)
        AudioManager.instance.PlaySunnyBgm();
        MenuPauseManager.instance.SetPaused(false);
    }
}
