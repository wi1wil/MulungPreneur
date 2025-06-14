using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeperScript : MonoBehaviour, IInteractable
{
    private bool isPanelActive;
    [SerializeField] private GameObject shopPanel;

    public bool canInteract()
    {
        return !isPanelActive;
    }

    public void Interact()
    {
        if (PauseControllerScript.isGamePaused)
        {
            return;
        }

        if (isPanelActive)
        {
            return;
        }

    }

    public void OpenShop()
    {
        if (PauseControllerScript.isGamePaused && !isPanelActive)
        {
            return;
        }
        if (isPanelActive)
        {
            CloseShop();
            return;
        }
        isPanelActive = true;
        shopPanel.SetActive(true);
        PauseControllerScript.setPaused(true);
        Debug.Log("Opening shop panel...");
    }

    public void CloseShop()
    {
        isPanelActive = false;
        shopPanel.SetActive(false);
        PauseControllerScript.setPaused(false);
        Debug.Log("Closing shop panel...");
    }
}
