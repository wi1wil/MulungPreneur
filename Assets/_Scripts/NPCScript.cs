using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class NPCScript : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    private DialogueManagerScript dialogueManager;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    private enum QuestState
    {
        NotStarted,
        InProgress,
        Completed
    }

    private QuestState questState = QuestState.NotStarted;

    private void Start()
    {
        dialogueManager = DialogueManagerScript.Instance;
    }

    public bool canInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null || (PauseControllerScript.isGamePaused && !isDialogueActive))
        {
            return;
        }

        if (isDialogueActive)
        {
            nextLine();
        }
        else
        {
            startDialogue();
        }
    }

    void startDialogue()
    {
        syncQuestProgress();
        // Set Dialogue Data
        Debug.Log($"Starting dialogue with {dialogueData.npcName}");
        Debug.Log($"Quest State: {questState}");
        if (questState == QuestState.NotStarted)
        {
            dialogueIndex = 0;
        }
        else if (questState == QuestState.InProgress)
        {
            dialogueIndex = dialogueData.questProgressIndex;
        }
        else if (questState == QuestState.Completed)
        {
            dialogueIndex = dialogueData.questCompetedIndex;

        }

        isDialogueActive = true;
        // dialogueIndex = 0;

        dialogueManager.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
        dialogueManager.ShowDialogueUI(true);
        PauseControllerScript.setPaused(true);

        DisplayCurrentLine();
    }

    private void syncQuestProgress()
    {
        if (dialogueData.quest != null)
        {
            string questID = dialogueData.quest.questID;

            if (QuestManagerScript.Instance.isQuestCompleted(questID) || QuestManagerScript.Instance.isQuestHandedIn(questID))
            {
                questState = QuestState.Completed;
                dialogueIndex = dialogueData.questCompetedIndex;
            }
            else if (QuestManagerScript.Instance.isQuestActive(questID))
            {
                questState = QuestState.InProgress;
            }
            else
            {
                questState = QuestState.NotStarted;
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueManager.SetDialogueText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueManager.SetDialogueText(dialogueManager.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            nextLine();
        }
    }

    void nextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueManager.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }

        dialogueManager.ClearChoices();
        if (dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        foreach (DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if (dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }

        if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        if (questState == QuestState.Completed && !QuestManagerScript.Instance.isQuestHandedIn(dialogueData.quest.questID))
        {
            HandleQuestComplete(dialogueData.quest);
        }
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueManager.ShowDialogueUI(false);
        PauseControllerScript.setPaused(false);
        dialogueManager.SetDialogueText("");
    }

    void DisplayChoices(DialogueChoice choice)
    {
        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndices[i];
            bool giveQuest = choice.giveQuest[i];
            dialogueManager.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex, giveQuest));
            Debug.Log($"Choice: {choice.choices[i]}, Next Index: {nextIndex}");
        }
    }

    void ChooseOption(int nextIndex, bool giveQuest)
    {
        if (giveQuest)
        {
            QuestManagerScript.Instance.StartQuest(dialogueData.quest);
            questState = QuestState.InProgress;
        }
        dialogueIndex = nextIndex;
        dialogueManager.ClearChoices();
        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    void HandleQuestComplete(Quest quest)
    {
        QuestManagerScript.Instance.handInQuest(quest.questID);
    }
}
