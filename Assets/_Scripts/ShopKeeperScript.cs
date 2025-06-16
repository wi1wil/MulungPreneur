using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopKeeperScript : MonoBehaviour, IInteractable
{
    private bool isPanelActive;
    [SerializeField] private GameObject shopPanel;

    public Transform shopContentPanel;
    public GameObject shopMenuPrefab;

    public bool canInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (PauseControllerScript.isGamePaused)
        {
            Debug.Log("Game is paused, cannot interact with Upgrade Keeper.");
            return;
        }
    }

    public void OpenShop()
    {
        if (isPanelActive)
        {
            CloseShop();
            return;
        }

        if (PauseControllerScript.isGamePaused)
        {
            Debug.Log("Game is paused, cannot open Upgrade Keeper shop.");
            return;
        }
        

        float maxDistance = 2.5f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null || Vector2.Distance(transform.position, player.transform.position) > maxDistance)
            return;

        isPanelActive = true;
        PopulateShopUI();
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

    public void PopulateShopUI()
    {
        // Clear existing shop entries
        foreach (Transform child in shopContentPanel)
        {
            Destroy(child.gameObject);
        }

        var inventoryData = InventoryManagerScript.Instance.getInventoryItem();
        var itemDict = FindObjectOfType<ItemDictionaryScript>();

        foreach (var data in inventoryData)
        {
            GameObject prefab = itemDict.GetItemPrefab(data.itemID);
            if (prefab == null) continue;

            Item item = prefab.GetComponent<Item>();
            if (item == null) continue;

            GameObject uiElement = Instantiate(shopMenuPrefab, shopContentPanel);
            Debug.Log("Item UI Instantiated");
            ShopItemPrefabScript uiScript = uiElement.GetComponent<ShopItemPrefabScript>();
            uiScript.Setup(item, data.quantity);
        }
    }
}
