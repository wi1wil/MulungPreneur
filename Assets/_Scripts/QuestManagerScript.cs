using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManagerScript : MonoBehaviour
{
    public static QuestManagerScript Instance { get; private set; }

    public List<QuestProgress> activateQuests = new();
    private QuestUIScript questUI;

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

        questUI = FindObjectOfType<QuestUIScript>();
        InventoryManagerScript.Instance.onInvChanged += checkInvForQuests;
    }

    public void StartQuest(Quest quest)
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
        Dictionary<int, int> itemCount = InventoryManagerScript.Instance.GetItemCount();
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

    public void LoadQuestProgress(List<QuestProgress> savedQuests)
    {
        activateQuests = savedQuests ?? new();
        checkInvForQuests();
        questUI.UpdateQuestUI();
    }
}
