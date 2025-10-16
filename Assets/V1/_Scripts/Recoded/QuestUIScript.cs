using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUIScript : MonoBehaviour
{
    public Transform questListContent;
    public GameObject questEntryPrefab;
    public GameObject objectiveTextPrefab;

    // public Quest testQuest;
    // public int testQuestAmount;
    private List<QuestProgress> testQuests = new();

    void Start()
    {
        // for (int i = 0; i < testQuestAmount; i++)
        // {
        //     testQuests.Add(new QuestProgress(testQuest));
        // }

        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        // Remove existing entries
        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        // Create new entries for each quest
        foreach (var quest in QuestManagerScript.Instance.activateQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            TMP_Text questNameText = entry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectiveList");
            questNameText.text = quest.questName;

            foreach(var objective in quest.objectives)
            {
                GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.description} ({objective.currentAmount}/{objective.requiredAmount})";

                if (objective.isCompleted)
                {
                    objText.color = Color.green;
                }
                else
                {
                    objText.color = Color.red;
                }
            }
        }
    }
}
