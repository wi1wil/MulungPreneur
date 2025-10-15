using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest")]

public class QuestScript : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;
    public List<QuestObjective> objectives;
    public List<QuestRewards> rewards;

    public void OnValidate()
    {
        if (string.IsNullOrEmpty(questID))
        {
            questID = questName + Guid.NewGuid().ToString();

        }
    }
}

[System.Serializable]
public class QuestObjective
{
    public string objectiveID;
    public string description;
    public ObjectiveType type;
    public int requiredAmount;
    public int currentAmount;

    public bool isCompleted => currentAmount >= requiredAmount;
}

public enum ObjectiveType { CollectItem, ReachLocation, TalkNPC, Custom }

[System.Serializable]
public class QuestProgress
{
    public string questID;
    public string questName;
    public List<QuestObjective> objectives;

    public QuestProgress(QuestScript quest)
    {
        this.questID = quest.questID;
        this.questName = quest.questName;
        objectives = new List<QuestObjective>();

        foreach (var obj in quest.objectives)
        {
            objectives.Add(new QuestObjective
            {
                objectiveID = obj.objectiveID,
                description = obj.description,
                type = obj.type,
                requiredAmount = obj.requiredAmount,
                currentAmount = 0
            });
        }
    }

    public bool isCompleted => objectives.TrueForAll(o => o.isCompleted);
}

[System.Serializable]
public class QuestRewards
{
    public RewardType rewardType;
    public int rewardID;
    public int amount = 1;

}

public enum RewardType { Item }