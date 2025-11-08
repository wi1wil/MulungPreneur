using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] private Transform _shopContentPanel;
    [SerializeField] private GameObject _shopItemPrefab;

    [SerializeField] private Transform _purchaseContentPanel;
    [SerializeField] private GameObject _buyItemPrefab;

    private Dictionary<ItemsSO, ShopItemPrefab> _spawnedItems = new();
    private Dictionary<ItemsSO, ShopBuyPrefab> _buybackItems = new();

    private void Start()
    {
        InventoryManager.Instance.onInvChanged += RefreshShop;
    }

    public void PopulateShopItems(IReadOnlyList<ItemStack> inventory)
    {
        foreach (Transform child in _shopContentPanel)
            Destroy(child.gameObject);
        _spawnedItems.Clear();

        foreach (var stack in inventory)
        {
            if (stack.item == null || stack.IsEmpty()) continue;

            var uiElement = Instantiate(_shopItemPrefab, _shopContentPanel);
            var uiScript = uiElement.GetComponent<ShopItemPrefab>();
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

    public void AddToBuyback(ItemsSO itemSO)
    {
        if (_buybackItems.TryGetValue(itemSO, out var existing))
        {
            existing.IncreaseQuantity();
        }
        else
        {
            GameObject uiElement = Instantiate(_buyItemPrefab, _purchaseContentPanel);
            ShopBuyPrefab uiScript = uiElement.GetComponent<ShopBuyPrefab>();
            uiScript.Setup(itemSO, 1);
            _buybackItems.Add(itemSO, uiScript);
        }
    }

    public void RemoveFromBuyback(ItemsSO itemSO)
    {
        if (_buybackItems.ContainsKey(itemSO))
        {
            _buybackItems.Remove(itemSO);
        }
    }

    public void ClearBuyback()
    {
        foreach (Transform child in _purchaseContentPanel)
            Destroy(child.gameObject);
        _buybackItems.Clear();
    }

    private void RefreshShop()
    {
        PopulateShopItems(InventoryManager.Instance.GetInventory());
    }
}
