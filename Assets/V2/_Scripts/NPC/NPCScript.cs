using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPCScript : MonoBehaviour, IInteractable
{
    public bool RequiresHold => false;
    public NPCDialogue dialogueData;
    
    private int _dialogueIdx;
    private bool _isTyping, _isDialogueActive;
    private NPCDialogueManager _dialogueManager;

    private enum QuestState
    {
        NotStarted,
        InProgress,
        Completed
    }

    private QuestState _questState = QuestState.NotStarted;

    void Start()
    {
        _dialogueManager = NPCDialogueManager.Instance;
    }

    public void Interact()
    {
        // If no dialogue data or game is paused and dialogue is not active, do nothing
        if (dialogueData == null || (MenuPauseManager.instance.gamePaused && !_isDialogueActive)) return;
        Debug.Log("Interacting with NPC");

        if (_isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        // Sync with quest data
        SyncQuestState();
        // Set dialogue line based on questSTate
        if (_questState == QuestState.NotStarted)
        {
            _dialogueIdx = 0;
        }
        else if (_questState == QuestState.InProgress)
        {
            _dialogueIdx = dialogueData.questInProgressIdx;
        }
        else if (_questState == QuestState.Completed)
        {
            _dialogueIdx = dialogueData.questCompletedIdx;
        }
        

        _isDialogueActive = true;

        _dialogueManager.SetNPCInfo(dialogueData.npcName, dialogueData.npcPotrait);
        _dialogueManager.DisplayDialogueUI(true);
        DisplayCurrentLine();
    }

    private void SyncQuestState()
    {
        if (QuestManager.Instance == null) return;
        if (dialogueData.quest == null) return;

        string questID = dialogueData.quest.questID;
        if (QuestManager.Instance.IsQuestCompleted(questID) || QuestManager.Instance.IsQuestHandedIn(questID))
        {
            _questState = QuestState.Completed;
            return;
        }
        else if (QuestManager.Instance.isQuestActive(questID))
        {
            _questState = QuestState.InProgress;
        }
        else
        {
            _questState = QuestState.NotStarted;
        }
    }

    public void NextLine()
    {
        if (_isTyping)
        {
            StopAllCoroutines();
            _dialogueManager.SetDialogueText(dialogueData.dialogueLines[_dialogueIdx]);
            _isTyping = false;
        }
        // Clear Choices
        _dialogueManager.ClearChoices();
        // Check EndDialogue    
        if (dialogueData.endDialogueLines.Length > _dialogueIdx && dialogueData.endDialogueLines[_dialogueIdx])
        {
            EndDialogue();
            return;
        }

        // Check Choices  
        foreach (DialogueChoices choice in dialogueData.choices)
        {
            if (choice.dialogueIndex == _dialogueIdx)
            {
                DisplayChoice(choice);
                return; 
            }
        }
        
        if (++_dialogueIdx < dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();  
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        _isTyping = true;
        _dialogueManager.SetDialogueText("");
        foreach (char letter in dialogueData.dialogueLines[_dialogueIdx].ToCharArray())
        {
            _dialogueManager.SetDialogueText(_dialogueManager.dialogueText.text += letter);
            yield return new WaitForSecondsRealtime(dialogueData.textSpeed);
        }
        _isTyping = false;

        if (dialogueData.autoProgressLines.Length > _dialogueIdx && dialogueData.autoProgressLines[_dialogueIdx])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void DisplayChoice(DialogueChoices choice)
    {
        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIdx = choice.nextDialogueIndices[i];
            bool givesQuest = choice.givesQuest[i];
            _dialogueManager.CreateChoice(choice.choices[i], () =>
            {
                ChooseOption(nextIdx, givesQuest);
            });
        }
    }

    public void ChooseOption(int nextDialogueIdx, bool givesQuest)
    {
        AudioManager.instance.PlayUIClick();
        if(givesQuest)
        {
            QuestManager.Instance.AcceptQuest(dialogueData.quest);
            AudioManager.instance.PlayQuestCompleted();
            _questState = QuestState.InProgress;
        }
        _dialogueIdx = nextDialogueIdx;
        _dialogueManager.ClearChoices();
        DisplayCurrentLine();  
    }
    
    public void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        if (_questState == QuestState.Completed && !QuestManager.Instance.IsQuestHandedIn(dialogueData.quest.questID))
        {
            HandleQuestCompletion(dialogueData.quest);
        }

        StopAllCoroutines();
        _isDialogueActive = false;
        _isTyping = false;
        _dialogueManager.SetDialogueText("");
        _dialogueManager.DisplayDialogueUI(false);
        MenuPauseManager.instance.SetPaused(false);
    }
    
    public void HandleQuestCompletion(QuestSO questSO)
    {
        AudioManager.instance.PlayQuestCompleted();
        RewardManager.Instance.GiveReward(questSO);
        QuestManager.Instance.HandInQuest(questSO.questID);
    }
}
