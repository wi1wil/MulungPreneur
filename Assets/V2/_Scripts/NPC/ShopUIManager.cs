using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] private Transform _shopContentPanel;
    [SerializeField] private GameObject _shopItemPrefab;

    // Use ItemsSO as the key instead of int
    private Dictionary<ItemsSO, ShopItemPrefab> _spawnedItems = new();

    private void Start()
    {
        InventoryManager.Instance.onInvChanged += RefreshShop;
    }

    public void PopulateShopItems(IReadOnlyList<ItemStack> inventory)
    {
        foreach (Transform child in _shopContentPanel)
        {
            Destroy(child.gameObject);
        }
        _spawnedItems.Clear();

        foreach (var stack in inventory)
        {
            if (stack.item == null || stack.IsEmpty()) continue;

            GameObject uiElement = Instantiate(_shopItemPrefab, _shopContentPanel);
            ShopItemPrefab uiScript = uiElement.GetComponent<ShopItemPrefab>();
            uiScript.Setup(stack.item, stack.quantity);
            _spawnedItems.Add(stack.item, uiScript); 
        }
    }

    public void UpdateItemQuantity(ItemsSO itemSO, int newQuantity)
    {
        if (_spawnedItems.TryGetValue(itemSO, out var uiScript))
        {
            uiScript.UpdateQuantity(newQuantity);
            if (newQuantity <= 0)
            {
                Destroy(uiScript.gameObject);
                _spawnedItems.Remove(itemSO);
            }
        }
    }

    private void RefreshShop()
    {
        PopulateShopItems(InventoryManager.Instance.GetInventory());
    }
}
