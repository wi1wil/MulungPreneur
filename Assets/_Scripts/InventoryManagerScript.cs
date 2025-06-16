using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InventoryManagerScript : MonoBehaviour
{
    private ItemDictionaryScript itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int inventorySize;
    public GameObject[] itemPrefabs;
    public int maxItemStack = 64; // Maximum items per stack

    public static InventoryManagerScript Instance { get; private set; }
    Dictionary<int, int> itemCountCache = new();
    public event Action onInvChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionaryScript>();
        InitializeItemCount();
    }

    public void InitializeItemCount()
    {
        itemCountCache.Clear();
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if (item != null)
                {
                    itemCountCache[item.id] = itemCountCache.GetValueOrDefault(item.id, 0) + item.Quantity;
                }
            }
        }

        onInvChanged?.Invoke();
    }

    public Dictionary<int, int> GetItemCount() => itemCountCache;

    public void addSlots(int size)
    {
        Debug.Log("Adding inventory size: " + size);
        for (int i = 0; i < size; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
            Debug.Log("Slot created: " + i);
        }
        inventorySize += size;
        Debug.Log("New inventory size: " + inventorySize);
    }

    public void setInventorySize()
    {
        Debug.Log("Setting inventory size: " + inventorySize);
        int currentSlots = inventoryPanel.transform.childCount;
        int slotsToAdd = inventorySize - currentSlots;
        for (int i = 0; i < slotsToAdd; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
            Debug.Log("Slot created: " + (currentSlots + i));
        }
    }

    public bool AddItem(GameObject itemPrefab)
    {
        Item item = itemPrefab.GetComponent<Item>();
        if (item == null) return false;

        int quantityToAdd = item.Quantity;

        // Try to stack into existing slots
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();
                if (slotItem != null && slotItem.id == item.id && slotItem.Quantity < maxItemStack)
                {
                    int spaceLeft = maxItemStack - slotItem.Quantity;
                    int addAmount = Mathf.Min(spaceLeft, quantityToAdd);
                    slotItem.AddToStack(addAmount);
                    InitializeItemCount();
                    quantityToAdd -= addAmount;
                    if (quantityToAdd <= 0)
                        return true;
                }
            }
        }

        // Add new stacks for remaining quantity
        while (quantityToAdd > 0)
        {
            // Find empty slot
            Transform emptySlot = null;
            foreach (Transform slotTransform in inventoryPanel.transform)
            {
                Slot slot = slotTransform.GetComponent<Slot>();
                if (slot != null && slot.currentItem == null)
                {
                    emptySlot = slotTransform;
                    InitializeItemCount();
                    break;
                }
            }
            if (emptySlot == null)
            {
                Debug.Log("Inventory is full");
                return false;
            }

            int addAmount = Mathf.Min(maxItemStack, quantityToAdd);
            GameObject newItem = Instantiate(itemPrefab, emptySlot);
            newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            Item newItemComponent = newItem.GetComponent<Item>();
            newItemComponent.Quantity = addAmount;
            newItemComponent.UpdateQuantity();
            emptySlot.GetComponent<Slot>().currentItem = newItem;
            quantityToAdd -= addAmount;
        }

        return true;
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
                invData.Add(new InventorySaveData
                {
                    itemID = item.id,
                    slotIndex = slot.transform.GetSiblingIndex(),
                    quantity = item.Quantity
                });
                Debug.Log("Item ID: " + item.id + ", Slot Index: " + slot.transform.GetSiblingIndex() + ", Quantity: " + item.Quantity);
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

                    Item itemComponent = item.GetComponent<Item>();
                    if (itemComponent != null)
                    {
                        itemComponent.Quantity = data.quantity;
                        itemComponent.UpdateQuantity();
                    }

                    slot.currentItem = item;
                }
            }
        }
        InitializeItemCount();
    }

    public void RemoveItemsFromInv(int itemID, int amountToRemove)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            if (amountToRemove <= 0) break;
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot?.currentItem?.GetComponent<Item>() is Item item && item.id == itemID)
            {
                int removed = Mathf.Min(amountToRemove, item.Quantity);
                item.RemoveFromStack(removed);
                amountToRemove -= removed;

                if (item.Quantity <= 0)
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null;
                }
            }
        }
        
        InitializeItemCount();
    }
}

