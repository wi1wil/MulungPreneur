using System.Collections;
using System.Collections.Generic;
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
        PopulateShopUI();
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
