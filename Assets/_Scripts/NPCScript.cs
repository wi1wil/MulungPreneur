using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCScript : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    private DialogueManagerScript dialogueManager;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

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
        isDialogueActive = true;
        dialogueIndex = 0;

        dialogueManager.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
        dialogueManager.ShowDialogueUI(true);
        PauseControllerScript.setPaused(true);

        DisplayCurrentLine();
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
            dialogueManager.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex));
            Debug.Log($"Choice: {choice.choices[i]}, Next Index: {nextIndex}");
        }
    }

    void ChooseOption(int nextIndex)
    {
        dialogueIndex = nextIndex;
        dialogueManager.ClearChoices();
        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }
}
