using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestSO", menuName = "ScriptableObjects/QuestSO")]
public class QuestSO : ScriptableObject
{
    public string questID;
    public string questName;
    public List<QuestObjectives> objectives;
    public List<QuestReward> rewards;

    public void OnValidate()
    {
        if (string.IsNullOrEmpty(questID))
        {
            questID = questName + Guid.NewGuid().ToString();
        }
    }
}

[System.Serializable]
public class QuestObjectives
{
    public ItemsSO targetItem;
    public string description;
    public ObjectiveType type;
    public int requiredAmount;
    public int currentAmount;

    public bool isCompleted => currentAmount >= requiredAmount;
}

public enum ObjectiveTypes { CollectItem, ReachLocation, TalkNPC, Custom }

[System.Serializable]
public class QuestProgression
{
    public QuestSO questSO;
    public List<QuestObjectives> objectives;

    public QuestProgression(QuestSO quest)
    {
        questSO = quest;
        objectives = new List<QuestObjectives>();

        foreach (var obj in quest.objectives)
        {
            objectives.Add(new QuestObjectives
            {
                targetItem = obj.targetItem,
                description = obj.description,
                type = obj.type,
                requiredAmount = obj.requiredAmount,
                currentAmount = 0
            });
        }
    }

    public bool isCompleted => objectives.TrueForAll(o => o.isCompleted);

    public string QuestID => questSO.questID;
}

[System.Serializable]
public class QuestReward
{
    public RewardTypes rewardType;
    public ItemsSO itemReward;
    public int amount = 1;
}

public enum RewardTypes {Item, Currency}