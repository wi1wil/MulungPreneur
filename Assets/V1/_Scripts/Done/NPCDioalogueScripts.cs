using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]

public class NPCDialogueScripts : ScriptableObject
{
    [Header("NPC Settings")]
    public string npcName;
    public Sprite npcPortrait;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
    public float typingSpeed = 0.05f;

    [Header("Dialogue Settings")]
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public bool[] endDialogueLines;
    public float autoProgressDelay = 1.5f;
    public DialogueChoice[] choices;

    public int questProgressIndex;
    public int questCompetedIndex;
    public QuestScript quest;
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex;
    public string[] choices;
    public int[] nextDialogueIndices;
    public bool[] giveQuest;
}
