using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPrefabScript : MonoBehaviour
{
    public Image itemIcon;
    public Text itemName;
    public Text itemPrice;
    public Button sellButton;

    private int itemID;
    private int itemQuantity;

    public void Setup(Item item, int quantity)
    {
        itemID = item.id;
        this.itemQuantity = quantity;
        itemIcon.sprite = item.GetComponent<SpriteRenderer>().sprite;
        itemName.text = item.Name;
        itemPrice.text = item.price.ToString();

        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() => SellItemOneByOne());
    }

    void SellItemOneByOne()
    {
        if (itemQuantity <= 0) return;

        InventoryManagerScript.Instance.RemoveItemsFromInv(itemID, 1);

        itemQuantity--;
        if (itemQuantity < 1)
        {
            Destroy(gameObject); // Remove from UI when none left
        }
    }
}
