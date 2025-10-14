using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Settings")]
    public int maxItemStack = 64;
    public int inventorySize = 6;

    private ItemDictionary _itemDictionary;
    private InventoryUIManager _invUI;

    private List<ItemStack> _inventory = new();
    public event Action onInvChanged;

    public void InvokeInventoryChanged() => onInvChanged?.Invoke();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _itemDictionary = FindAnyObjectByType<ItemDictionary>();
        _invUI = FindAnyObjectByType<InventoryUIManager>();
        InitializeInventory();
    }

    public bool AddItem(ItemsSO itemSO, int quantity)
    {
        // Trying stacking with existing items first
        foreach (ItemStack stack in _inventory)
        {
            // Checks if stack exists & quantity is under the maxItemStack
            if (stack.item == itemSO && stack.quantity < maxItemStack)
            {
                // Count the space left in the available stack quantity
                int spaceLeft = maxItemStack - stack.quantity;
                int amountToAdd = Mathf.Min(spaceLeft, quantity);
                stack.Add(amountToAdd);
                quantity -= amountToAdd;
                if (quantity <= 0)
                {
                    onInvChanged?.Invoke();
                    return true;
                }
            }
        }

        // Add to empty slots if there are no existing items
        foreach (ItemStack stack in _inventory)
        {
            if (stack.IsEmpty())
            {
                int amountToAdd = Mathf.Min(maxItemStack, quantity);
                stack.item = itemSO;
                stack.quantity = amountToAdd;
                quantity -= amountToAdd;

                if (quantity <= 0)
                {
                    onInvChanged?.Invoke();
                    return true;
                }
            }
        }

        // There are no available slots
        Debug.Log("Inventory Full!");
        onInvChanged?.Invoke();
        return false;
    }

    public void RemoveItem(ItemsSO itemSO, int amountToRemove)
    {
        foreach (ItemStack stack in _inventory)
        {
            if (stack.item == itemSO)
            {
                int removed = stack.Remove(amountToRemove);
                amountToRemove -= removed;
                if (amountToRemove <= 0)
                    break;
            }
        }

        onInvChanged?.Invoke();
    }

    public void SwapOrStack(InventorySlot fromSlot, InventorySlot toSlot)
    {
        if (fromSlot == null || toSlot == null) return;

        int fromIndex = fromSlot.SlotIndex;
        int toIndex = toSlot.SlotIndex;

        if (fromIndex < 0 || toIndex < 0 || fromIndex >= _inventory.Count || toIndex >= _inventory.Count)
            return;

        ItemStack fromStack = _inventory[fromIndex];
        ItemStack toStack = _inventory[toIndex];

        if (fromStack == null || fromStack.IsEmpty()) return;

        // If target is empty, move
        if (toStack == null || toStack.IsEmpty())
        {
            _inventory[toIndex] = fromStack;
            _inventory[fromIndex] = new ItemStack(null, 0);
            onInvChanged?.Invoke();
            return;
        }

        // If same item type, stack
        if (fromStack.item == toStack.item)
        {
            int spaceLeft = maxItemStack - toStack.quantity;
            int moveAmount = Mathf.Min(spaceLeft, fromStack.quantity);

            toStack.quantity += moveAmount;
            fromStack.quantity -= moveAmount;

            if (fromStack.quantity <= 0)
                _inventory[fromIndex] = new ItemStack(null, 0);

            onInvChanged?.Invoke();
            return;
        }

        // Different items, swap
        _inventory[fromIndex] = toStack;
        _inventory[toIndex] = fromStack;

        onInvChanged?.Invoke();
    }

    public void InitializeInventory()
    {
        // Remove extra slots if inventory has more than inventorySize
        while (_inventory.Count > inventorySize)
        {
            _inventory.RemoveAt(_inventory.Count - 1);
        }

        // Add missing slots if inventory has less than inventorySize
        while (_inventory.Count < inventorySize)
        {
            _inventory.Add(new ItemStack(null, 0));
        }

        onInvChanged?.Invoke();
    }

    public Dictionary<ItemsSO, int> GetItemCountBySO()
    {
        Dictionary<ItemsSO, int> itemCount = new Dictionary<ItemsSO, int>();
        foreach (ItemStack stack in _inventory)
        {
            if (stack.item != null)
            {
                if (itemCount.ContainsKey(stack.item))
                    itemCount[stack.item] += stack.quantity;
                else
                    itemCount[stack.item] = stack.quantity;
            }
        }
        return itemCount;
    }

    public IReadOnlyList<ItemStack> GetInventory() => _inventory;

    public bool InventoryFull()
    {
        foreach (ItemStack stack in _inventory)
        {
            // Slot is empty, inventory is not full
            if (stack.IsEmpty())
                return false;

            // Slot still has space to stack more
            if (stack.quantity < maxItemStack)
                return false;
        }
        return true; // all slots full & maxed out
    }
}
