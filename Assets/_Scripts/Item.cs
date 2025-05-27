using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;
    public string Name;
    public virtual void PickUp()
    {
        Sprite itemIcon = GetComponent<SpriteRenderer>().sprite;
        if(ItemPopUpController.Instance != null)
        {
            ItemPopUpController.Instance.ShowItemPickUp(Name, itemIcon);
        }
    }
}
