using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControllerScript : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if(!menuCanvas.activeSelf && PauseControllerScript.isGamePaused)
        {
            return; // Do not toggle if the menu is already open and the game is paused
        }
        menuCanvas.SetActive(!menuCanvas.activeSelf);
        PauseControllerScript.setPaused(menuCanvas.activeSelf);
    }
}
