using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemPrefab : MonoBehaviour
{
    public Image itemIcon;
    public Text itemNameText;
    public Text itemPriceText;
    public Text itemQuantityText;
    public Button sellButton;

    private int _itemId;
    private int _itemQuantity;
    private int _itemPrice;

    private PlayerCurrencyScript _playerCurrency;
    private ShopUIManager _shopUIManager;

    private void Awake()
    {
        _playerCurrency = FindAnyObjectByType<PlayerCurrencyScript>();
        _shopUIManager = FindAnyObjectByType<ShopUIManager>();
    }

    public void Setup(ItemsSO item, int quantity)
    {
        if (item == null) return;

        _itemId = item.itemID;
        _itemQuantity = quantity;
        _itemPrice = item.itemPrice;

        itemIcon.sprite = item.itemIcon;
        itemNameText.text = item.name;
        itemPriceText.text = $"Price: {_itemPrice}";
        itemQuantityText.text = $"x{_itemQuantity}";

        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(OnSellButtonClicked);
    }

    public void UpdateQuantity(int quantity)
    {
        _itemQuantity = quantity;
        itemQuantityText.text = $"x{_itemQuantity}";
        if (_itemQuantity <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnSellButtonClicked()
    {
        if (_itemQuantity <= 0) return;

        InventoryManager.Instance.RemoveItem(_itemId, 1);
        _playerCurrency.AddCurrency(_itemPrice);

        _itemQuantity--;

        UpdateQuantity(_itemQuantity);
        _shopUIManager.UpdateItemQuantity(_itemId, _itemQuantity);
    }
}
