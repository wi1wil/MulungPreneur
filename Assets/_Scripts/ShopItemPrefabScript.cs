using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPrefabScript : MonoBehaviour
{
    public Image itemIcon;
    public Text itemName;
    public Text itemPrice;
    public Text itemQuantityText;
    public Button sellButton;

    private int itemID;
    private int itemQuantity;

    private PlayerWalletScript playerWallet;

    private void Awake()
    {
        playerWallet = GameObject.Find("GameManager").GetComponent<PlayerWalletScript>();
    }

    public void Setup(Item item, int quantity)
    {
        itemID = item.id;
        this.itemQuantity = quantity;
        itemIcon.sprite = item.GetComponent<SpriteRenderer>().sprite;
        itemName.text = item.Name;
        itemPrice.text = item.price.ToString();
        itemQuantityText.text = "x" + itemQuantity.ToString();

        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => SellItemOneByOne(item.price));
    }

    void SellItemOneByOne(int itemValue)
    {
        if (itemQuantity <= 0) return;

        InventoryManagerScript.Instance.RemoveItemsFromInv(itemID, 1);
        playerWallet.AddMoneyFromItemSold(itemValue);

        itemQuantity--;
        if (itemQuantity < 1)
        {
            Destroy(gameObject);
        }
        else
        {
            itemQuantityText.text = "x" + itemQuantity.ToString();
        }
    }
}
