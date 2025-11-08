using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemPrefab : MonoBehaviour
{
    [Header("UI Elements")]
    public Image itemIcon;
    public Text itemNameText;
    public Text itemPriceText;
    public Text itemQuantityText;
    public Button sellButton;

    private ItemsSO _itemSO;
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

        _itemSO = item;                 
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
        AudioManager.instance.PlaySellingItem();

        // Remove 1 item using SO reference
        InventoryManager.Instance.RemoveItem(_itemSO, 1);
        _playerCurrency.AddCurrency(_itemPrice);

        _itemQuantity--;
        UpdateQuantity(_itemQuantity);

        // Update UI in ShopUIManager (also use SO)
        _shopUIManager.UpdateItemQuantity(_itemSO, _itemQuantity);
        _shopUIManager.AddToBuyback(_itemSO);
    }
}
