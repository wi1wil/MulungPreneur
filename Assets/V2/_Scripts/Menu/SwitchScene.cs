using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(2);  // Load the main game scene (index 2)
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);  // Load the main menu scene (index 0)
    }
}
