using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<ItemsSO> itemDataList;
    public static ItemDictionary Instance { get; private set; }

    private Dictionary<int, ItemsSO> _itemDictionary;

    void Awake()
    {
        Instance = this;
        _itemDictionary = new Dictionary<int, ItemsSO>();

        for (int i = 0; i < itemDataList.Count; i++)
        {
            if (itemDataList[i] != null)
            {
                itemDataList[i].itemID = i + 1;
                _itemDictionary[itemDataList[i].itemID] = itemDataList[i];
            }
        }
    }
    
    public ItemsSO GetItemData(int itemID)
    {
        if (_itemDictionary.TryGetValue(itemID, out ItemsSO item))
        {
            return item;
        }

        Debug.LogWarning("Item ID not found: " + itemID);
        return null;
    }
}
