using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject optionsMenu;

    public void Awake()
    {
        if (!startMenu)
            startMenu = GameObject.Find("Start Menu");
        if (!optionsMenu)
            optionsMenu = GameObject.Find("Options Menu");

        optionsMenu.SetActive(false);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
