using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeperScript : MonoBehaviour, IInteractable
{
    private bool isPanelActive;

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
            // CloseShop();
        }
    }
}
