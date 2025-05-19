using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerScript : MonoBehaviour
{
    private ItemDictionaryScript itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int inventorySize;
    public GameObject[] itemPrefabs;

    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionaryScript>();

        // for (int i = 0; i < inventorySize; i++)
        // {
        //     Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
        //     if (i < itemPrefabs.Length)
        //     {
        //         GameObject item = Instantiate(itemPrefabs[i], slot.transform);
        //         item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //         slot.currentItem = item;
        //     }
        // }
    }

    public List<InventorySaveData> getInventoryItem()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach (Transform slotTransfrom in inventoryPanel.transform)
        {
            Slot slot = slotTransfrom.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData { itemID = item.id, slotIndex = slot.transform.GetSiblingIndex() });
                Debug.Log("Item ID: " + item.id + ", Slot Index: " + slot.transform.GetSiblingIndex());
            }   
        }
        return invData;
    }

    public void setInventoryItem(List<InventorySaveData> inventorySaveData)
    {
        // Clear inventory panel to avoid duplicates 
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create slots 
        for (int i = 0; i < inventorySize; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        // Populate slots with saved items
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < inventorySize)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}
