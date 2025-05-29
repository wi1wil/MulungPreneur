using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;
    public string Name;
    public int Quantity = 1;

    [SerializeField] private TMP_Text quantityText;
    private void Awake()
    {
        UpdateQuantity();
    }

    public void UpdateQuantity()
    {
        if (quantityText == null)
        {
            quantityText = GetComponentInChildren<TMP_Text>();
        }
        quantityText.text = Quantity > 1 ? Quantity.ToString() : "";
    }

    public void AddToStack(int amount = 1)
    {
        Quantity += amount;
        UpdateQuantity();
    }

    public int RemoveFromStack(int amount = 1)
    {
        int removed = Mathf.Min(amount, Quantity);
        Quantity -= removed;
        UpdateQuantity();
        return removed;
    }

    public GameObject CloneItem(int newQuantity)
    {
        GameObject clone = Instantiate(gameObject);
        Item cloneItem = clone.GetComponent<Item>();
        cloneItem.Quantity = newQuantity;
        cloneItem.UpdateQuantity();
        return clone;
    }

    public virtual void PickUp()
    {
        Sprite itemIcon = GetComponent<SpriteRenderer>().sprite;
        if (ItemPopUpController.Instance != null)
        {
            ItemPopUpController.Instance.ShowItemPickUp(Name, itemIcon);
        }
    }
}
