using UnityEngine;

[CreateAssetMenu(fileName = "NPC Dialogue", menuName = "ScriptableObjects/NPCDialogue")]
public class NPCDialogue : ScriptableObject
{
    [Header("NPC Info")]
    public string npcName;
    public Sprite npcPotrait;

    [Header("Dialogue Settings")]
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public bool[] endDialogueLines;
    public DialogueChoices[] choices;

    [Header("Text Settings")]
    public float textSpeed = 0.05f;
    public float autoProgressDelay = 2f;

    [Header("Quest Settings")]
    public int questInProgressIdx;
    public int questCompletedIdx;
    public QuestSO quest;
}

[System.Serializable]
public class DialogueChoices
{
    public int dialogueIndex;
    public string[] choices;
    public int[] nextDialogueIndices;
    public bool[] givesQuest;
}
