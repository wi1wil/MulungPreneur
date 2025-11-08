using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefab : MonoBehaviour, IInteractable
{
    private SpriteRenderer _icon;
    public ItemsSO itemData;
    public int quantity = 1;

    public bool RequiresHold => true;

    public bool IsBeingInteractedWith { get; private set; }

    public void SetInteracting(bool state)
    {
        IsBeingInteractedWith = state;
    }

    private void Awake()
    {
        _icon = GetComponent<SpriteRenderer>();
        Initialize(itemData, quantity);
    }

    public void Initialize(ItemsSO data, int qty = 1)
    {
        itemData = data;
        quantity = qty;
        if (_icon != null && itemData != null)
            _icon.sprite = itemData.itemIcon;
    }

    public void Interact()
    {
        bool added = InventoryManager.Instance.AddItem(itemData, quantity);

        if (added)
        {
            Debug.Log("Picked up");
            ItemPopUps.Instance.DisplayPopUp(itemData.itemName, itemData.itemIcon);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory full — cannot pick up item!");
        }
    }
}
