using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefab : MonoBehaviour, IInteractable
{
    public ItemsSO itemData;

    public void Interact()
    {
        Debug.Log("Picked up");
        // InventoryManager.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
