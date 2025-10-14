using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    private PlayerCurrencyScript _playerCurrency;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        _playerCurrency = FindAnyObjectByType<PlayerCurrencyScript>();
    }

    public void GiveReward(QuestSO questSO)
    {
        foreach (var reward in questSO.rewards)
        {
            switch (reward.rewardType)
            {
                case RewardTypes.Item:
                    InventoryManager.Instance.AddItem(reward.itemReward, reward.amount);
                    for(int i = 0; i < reward.amount; i++)
                    {
                        ItemPopUps.Instance.DisplayPopUp(reward.itemReward.itemName, reward.itemReward.itemIcon);
                    }
                    Debug.Log($"Given {reward.amount} x {reward.itemReward.itemName}");
                    break;
                case RewardTypes.Currency:
                    _playerCurrency.AddCurrency(reward.amount);
                    Debug.Log($"Given {reward.amount} currency");
                    break;
                default:
                    Debug.LogWarning("Unknown reward type");
                    break;
            }
        }
    }
}
