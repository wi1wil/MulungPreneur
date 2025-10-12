using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] private Transform _shopContentPanel;
    [SerializeField] private GameObject _shopItemPrefab;

    private Dictionary<int, ShopItemPrefab> _spawnedItems = new();

    void Start()
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
            _spawnedItems.Add(stack.item.itemID, uiScript);
        }
    }

    public void UpdateItemQuantity(int itemId, int newQuantity)
    {
        if (_spawnedItems.TryGetValue(itemId, out var uiScript))
        {
            uiScript.UpdateQuantity(newQuantity);
            if (newQuantity <= 0)
            {
                Destroy(uiScript.gameObject);
                _spawnedItems.Remove(itemId);
            }
        }
    }
    
    private void RefreshShop()
    {
        PopulateShopItems(InventoryManager.Instance.GetInventory());
    }
}
