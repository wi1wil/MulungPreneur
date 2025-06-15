using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject optionsMenu;
    public VolumeSettings volumeSettings;

    public void Awake()
    {
        if (!startMenu)
            startMenu = GameObject.Find("Start Menu");
        if (!optionsMenu)
            optionsMenu = GameObject.Find("Options Menu");

        optionsMenu.SetActive(false);
        volumeSettings = FindObjectOfType<VolumeSettings>();
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
        volumeSettings.LoadVolume();
    }

    public void CloseSettings()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
