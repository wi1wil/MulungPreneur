using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManagerScript : MonoBehaviour
{
    public static QuestManagerScript Instance { get; private set; }

    public List<QuestProgress> activateQuests = new();
    private QuestUIScript questUI;

    public List<string> handinQuests = new();   

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

        questUI = FindFirstObjectByType<QuestUIScript>();
        InventoryManagerScripts.Instance.onInvChanged += checkInvForQuests;
    }

    public void StartQuest(QuestScript quest)
    {
        if (isQuestActive(quest.questID))
        {
            Debug.LogWarning($"Quest {quest.questName} is already active.");
            return;
        }
        activateQuests.Add(new QuestProgress(quest));
        checkInvForQuests();
        questUI.UpdateQuestUI();
    }

    public bool isQuestActive(string questID) => activateQuests.Exists(q => q.questID == questID);

    public void checkInvForQuests()
    {
        Dictionary<int, int> itemCount = InventoryManagerScripts.Instance.GetItemCount();
        foreach (QuestProgress quest in activateQuests)
        {
            foreach (QuestObjective questObjective in quest.objectives)
            {
                if (questObjective.type != ObjectiveType.CollectItem)
                    continue;

                if (!int.TryParse(questObjective.objectiveID, out int itemId)) continue;
                int newAmount = itemCount.TryGetValue(itemId, out int count) ? Mathf.Min(count, questObjective.requiredAmount) : 0;
                if (questObjective.currentAmount != newAmount)
                {
                    questObjective.currentAmount = newAmount;
                }
            }
        }
        questUI.UpdateQuestUI();
    }

    public bool isQuestCompleted(string questID)
    {
        QuestProgress quest = activateQuests.Find(q => q.questID == questID);
        return quest != null && quest.objectives.TrueForAll(o => o.isCompleted);
    }

    public void handInQuest(string questID)
    {
        if (!removeItemsFromInv(questID))
        {
            return;
        }

        QuestProgress quest = activateQuests.Find(q => q.questID == questID);
        if (quest != null)
        {
            handinQuests.Add(questID);
            activateQuests.Remove(quest);
            questUI.UpdateQuestUI();
        }
       
    }

    public bool isQuestHandedIn(string questID)
    {
        return handinQuests.Contains(questID);
    }

    public bool removeItemsFromInv(string questID)
    {
        QuestProgress quest = activateQuests.Find(q => q.questID == questID);
        if (quest == null) return false;

        Dictionary<int, int> requiredItems = new();
        foreach (QuestObjective objective in quest.objectives)
        {
            if (objective.type == ObjectiveType.CollectItem && int.TryParse(objective.objectiveID, out int itemId))
            {
                requiredItems[itemId] = objective.requiredAmount;
            }
        }
        Dictionary<int, int> itemCount = InventoryManagerScripts.Instance.GetItemCount();
        foreach (var item in requiredItems)
        {
            if (itemCount.GetValueOrDefault(item.Key) < item.Value)
            {
                return false;
            }
        }

        foreach (var itemReq in requiredItems)
        {
            InventoryManagerScripts.Instance.RemoveItemsFromInv(itemReq.Key, itemReq.Value);
        }
        return true;
    }

    public void LoadQuestProgress(List<QuestProgress> savedQuests)
    {
        activateQuests = savedQuests ?? new();
        checkInvForQuests();
        questUI.UpdateQuestUI();
    }
}
