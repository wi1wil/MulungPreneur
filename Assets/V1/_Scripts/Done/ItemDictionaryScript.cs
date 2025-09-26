using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionaryScript : MonoBehaviour
{
    public List<Item> itemPrefabs;
    private Dictionary<int, GameObject> itemDictionary;

    public static ItemDictionaryScript Instance { get; private set; }
    
    void Awake()
    {
        Instance = this;
        itemDictionary = new Dictionary<int, GameObject>();
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].id = i + 1;
            }
        }

        foreach (Item item in itemPrefabs)
        {
            itemDictionary[item.id] = item.gameObject;
        }
    }

    public GameObject GetItemPrefab(int itemID)
    {
        itemDictionary.TryGetValue(itemID, out GameObject prefab);
        if (prefab == null)
        {
            Debug.LogWarning("Item ID not found: " + itemID);
        }
        return prefab;
    }
}
