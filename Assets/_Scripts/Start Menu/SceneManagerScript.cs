using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Main Gameplay");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    //public void OptionsMenu()
    //{

    //}
}
