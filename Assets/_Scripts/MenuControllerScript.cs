using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControllerScript : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;

    void Start()
    {
        //menuCanvas.SetActive(false); //ini gw komen soalny w dh terlalu deep pas bagian settings trs trlalu males ganti ke opsi yg lebi efisien tp ydhlh gpp ni first time
                                       //bikin settings menu jga, w dh set false di script lain oke makasih semoga mata g sakit liat spaghetti code. -r
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
