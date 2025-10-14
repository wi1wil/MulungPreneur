using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogueManager : MonoBehaviour
{
    public static NPCDialogueManager Instance { get; private set;}

    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    public Transform choicesContainer;
    public GameObject choiceButtonPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void DisplayDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
        if(!show) MenuPauseManager.instance.SetPaused(false);
    }

    public void SetNPCInfo(string npcName, Sprite npcPortrait)
    {
        nameText.text = npcName;
        portraitImage.sprite = npcPortrait;
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void CreateChoice(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choicesContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
    }
}
