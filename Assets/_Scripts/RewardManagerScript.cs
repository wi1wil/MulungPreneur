using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManagerScript : MonoBehaviour
{
    public static RewardManagerScript Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GiveRewards(Quest quest)
    {
        if (quest?.rewards == null) return;
        foreach (var reward in quest.rewards)
        {
            switch (reward.rewardType)
            {
                case RewardType.Item:
                    giveItemReward(reward.rewardID, reward.amount);
                    break;
            }
        }
    }

    public void giveItemReward(int itemID, int amount)
    {
        var itemPrefab = FindAnyObjectByType<ItemDictionaryScript>().GetItemPrefab(itemID);
        if (itemPrefab == null)
        {
            Debug.LogWarning($"Item with ID {itemID} not found.");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            if (!InventoryManagerScript.Instance.AddItem(itemPrefab))
            {
                GameObject dropItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                itemPrefab.GetComponent<Item>().ShowPopup();
            }
        }
    }
}
