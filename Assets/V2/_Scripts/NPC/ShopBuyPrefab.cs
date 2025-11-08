using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopBuyPrefab : MonoBehaviour
{
    [Header("UI Elements")]
    public Image itemIcon;
    public TMP_Text itemNameText;
    public TMP_Text itemQuantityText;
    public Button buybackButton;

    private ItemsSO _item;
    private int _quantity;
    private PlayerCurrencyScript _playerCurrency;
    private InventoryManager _inventory;
    private ShopUIManager _shopUIManager;

    private void Awake()
    {
        _playerCurrency = FindAnyObjectByType<PlayerCurrencyScript>();
        _inventory = InventoryManager.Instance;
        _shopUIManager = FindAnyObjectByType<ShopUIManager>();
    }

    public void Setup(ItemsSO item, int quantity)
    {
        _item = item;
        _quantity = quantity;

        itemIcon.sprite = item.itemIcon;
        itemNameText.text = item.itemName;
        itemQuantityText.text = $"x{_quantity}";

        buybackButton.onClick.RemoveAllListeners();
        buybackButton.onClick.AddListener(OnBuyBackClicked);
    }

    private void OnBuyBackClicked()
    {
        if (_quantity <= 0) return;
        int price = _item.itemPrice;

        // Check if player can afford
       
        if (_playerCurrency.playerCurrency < price)
        {
            Debug.Log("Not enough currency to buy back the item.");
            return;
        }

        // Deduct money, add item back to inventory
        _playerCurrency.DecreaseCurrency(price);
        _inventory.AddItem(_item, 1);

        _quantity--;
        itemQuantityText.text = $"x{_quantity}";

        if (_quantity <= 0)
        {
            _shopUIManager.RemoveFromBuyback(_item);
            Destroy(gameObject);
        }
    }

    public void IncreaseQuantity()
    {
        _quantity++;
        itemQuantityText.text = $"x{_quantity}";
    }
}