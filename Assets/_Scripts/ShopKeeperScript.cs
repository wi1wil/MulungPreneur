using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeperScript : MonoBehaviour, IInteractable
{
    private bool isPanelActive;
    [SerializeField] private GameObject shopPanel;

    public bool canInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (PauseControllerScript.isGamePaused)
        {
            return;
        }

        if (isPanelActive)
        {
            CloseShop();
        }
    }

    public void ToggleShop()
    {
        if (isPanelActive)
        {
            CloseShop();
        }
        else
        {
            OpenShop();
        }
    }

    public void OpenShop()
    {
        if (PauseControllerScript.isGamePaused)
            return;

        float maxDistance = 2.5f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null || Vector2.Distance(transform.position, player.transform.position) > maxDistance)
            return;

        isPanelActive = true;
        shopPanel.SetActive(true);
        PauseControllerScript.setPaused(true);
    }

    public void CloseShop()
    {
        isPanelActive = false;
        shopPanel.SetActive(false);
        PauseControllerScript.setPaused(false);
    }

    void Update()
    {
        if (isPanelActive && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }
}
