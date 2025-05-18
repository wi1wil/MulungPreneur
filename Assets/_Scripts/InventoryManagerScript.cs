using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerScript : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int inventorySize;
    public GameObject[] itemPrefabs;

    void Start()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InventorySlotScript slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<InventorySlotScript>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }
}
