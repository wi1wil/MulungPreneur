using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ItemsSO")]

public class ItemsSO : ScriptableObject
{
    [Header("Item Info")]
    public string itemID;
    public string itemName;
    public string itemDesc;
    public int itemPrice;

    [Header("Visuals")]
    public Sprite itemIcon;
}

[System.Serializable]
public class ItemStack
{
    public ItemsSO item;
    public int quantity;

    public ItemStack(ItemsSO item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public bool IsEmpty() => quantity <= 0;
    public void Add(int amount)
    {
        quantity += amount;
    }

    public int Remove(int amount)
    {
        int removed = Mathf.Min(amount, quantity);
        quantity -= removed;
        return removed;
    }
}