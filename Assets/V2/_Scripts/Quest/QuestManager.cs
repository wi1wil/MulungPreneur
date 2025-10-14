using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestProgression> activeQuests = new();
    public List<string> handInQuestsID = new();
    private QuestUI _questUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _questUI = FindAnyObjectByType<QuestUI>();
        InventoryManager.Instance.onInvChanged += CheckInventory;
    }

    public void AcceptQuest(QuestSO questSO)
    {
        if (isQuestActive(questSO.questID))
        {
            CheckInventory();
            return;
        }
        CheckInventory();
        activeQuests.Add(new QuestProgression(questSO));
        _questUI.UpdateQuestUI();
    }

    public bool isQuestActive(string questID) => activeQuests.Exists(q => q.QuestID == questID);

    public void CheckInventory()
    {
        var itemCounts = InventoryManager.Instance.GetItemCountBySO();

        foreach (QuestProgression quest in activeQuests)
        {
            if (quest == null || quest.objectives == null) continue;

            foreach (QuestObjectives questObjective in quest.objectives)
            {
                if (questObjective == null ||
                    questObjective.type != ObjectiveType.CollectItem ||
                    questObjective.targetItem == null)
                {
                    continue;
                }

                ItemsSO targetItem = questObjective.targetItem;
                int requiredAmount = questObjective.requiredAmount;

                itemCounts.TryGetValue(targetItem, out int currentAmount);
                int newAmount = Mathf.Min(currentAmount, requiredAmount);

                if (questObjective.currentAmount != newAmount)
                    questObjective.currentAmount = newAmount;
            }
        }

        _questUI.UpdateQuestUI();
    }

    public bool IsQuestCompleted(string questID)
    {
        QuestProgression quest = activeQuests.Find(q => q.QuestID == questID);
        return quest != null && quest.objectives.TrueForAll(obj => obj.isCompleted);
    }

    public void HandInQuest(string questID)
    {
        if (!RemoveItemsFromInventory(questID))
            return;

        QuestProgression quest = activeQuests.Find(q => q.QuestID == questID);
        if (quest != null)
        {
            handInQuestsID.Add(questID);
            activeQuests.Remove(quest);
            _questUI.UpdateQuestUI();
        }
    }

    public bool IsQuestHandedIn(string questID)
    {
        return handInQuestsID.Contains(questID);
    }

    public bool RemoveItemsFromInventory(string questID)
    {
        QuestProgression quest = activeQuests.Find(q => q.QuestID == questID);
        if (quest == null) return false;

        // Collect required items (SO + amount)
        Dictionary<ItemsSO, int> requiredItems = new();
        foreach (QuestObjectives objective in quest.objectives)
        {
            if (objective.type == ObjectiveType.CollectItem && objective.targetItem != null)
            {
                requiredItems[objective.targetItem] = objective.requiredAmount;
            }
        }

        // Get current item counts from inventory
        Dictionary<ItemsSO, int> itemCounts = InventoryManager.Instance.GetItemCountBySO();

        // Check if player has enough of each required item
        foreach (var item in requiredItems)
        {
            if (!itemCounts.TryGetValue(item.Key, out int currentAmount) || currentAmount < item.Value)
                return false;
        }

        // Remove required items from inventory
        foreach (var itemReq in requiredItems)
        {
            InventoryManager.Instance.RemoveItem(itemReq.Key, itemReq.Value);
        }

        return true;
    }

    public void LoadQuestProgress(List<QuestProgression> savedQuests)
    {
        activeQuests = savedQuests ?? new();
        CheckInventory();
        _questUI.UpdateQuestUI();
    }
}
