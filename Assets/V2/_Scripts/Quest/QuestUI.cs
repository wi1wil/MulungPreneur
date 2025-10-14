using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public Transform questListContainer;
    public GameObject questEntryPrefab;
    public GameObject objectiveTextPrefab;

    void Start()
    {
        UpdateQuestUI();
    }
    
    public void UpdateQuestUI()
    {
        foreach (Transform child in questListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var quest in QuestManager.Instance.activeQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContainer);
            TMP_Text questNameText = entry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectiveList");
            questNameText.text = quest.questSO.questName;

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
