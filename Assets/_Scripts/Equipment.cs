using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Equipment : MonoBehaviour
{
    [Header("Player Character")]
    public Image playerCharacterImage;
    public Sprite playerCharacterSprite;

    [Header("Equipment Levels and Sprites")]
    [Header("Bag")]
    public int bagLevel;
    public bool bagUpgradeStatus;
    public List<Sprite> bagSprites;
    public List<String> bagNames;
    public int maxBagLevel;

    [Header("Footwear")]
    public int footwearLevel;
    public bool footwearUpgradeStatus;
    public List<Sprite> footwearSprites;
    public List<String> footwearNames;
    public int maxFootwearLevel;

    [Header("Gloves")]
    public int glovesLevel;
    public bool glovesUpgradeStatus;
    public List<Sprite> glovesSprites;
    public List<String> glovesNames;
    public int maxGlovesLevel;

    [Header("Picking Tool")]
    public int pickingToolLevel;
    public bool pickingToolUpgradeStatus;
    public List<Sprite> pickingToolSprites;
    public List<String> pickingToolNames;
    public int maxPickingToolLevel;

    [Header("Equipment Images")]
    public Image bagImage;
    public Image footwearImage;
    public Image glovesImage;
    public Image pickingToolImage;

    [Header("Equipment Names")]
    public TMP_Text bagNameText;
    public TMP_Text footwearNameText;
    public TMP_Text glovesNameText;
    public TMP_Text pickingToolNameText;

    [Header("Equipment Upgrade Status")]
    public TMP_Text bagUpgradeStatusText;
    public TMP_Text footwearUpgradeStatusText;
    public TMP_Text glovesUpgradeStatusText;
    public TMP_Text pickingToolUpgradeStatusText;

    [Header("Equipment Current Level")]
    public TMP_Text bagLevelText;
    public TMP_Text footwearLevelText;
    public TMP_Text glovesLevelText;
    public TMP_Text pickingToolLevelText;

    void Start()
    {
        playerCharacterImage = GameObject.Find("PlayerCharacter").GetComponent<Image>();
        playerCharacterImage.sprite = playerCharacterSprite;

        if (gameObject.activeInHierarchy)
        {
            getEquipment();
            updateEquipment();
        }
    }

    void Update()
    {
        if (bagImage != null && footwearImage != null && glovesImage != null && pickingToolImage != null)
        {
            updateEquipment();
        }
    }

    public void updateEquipment()
    {
        maxBagLevel = bagSprites.Count;
        bagImage.sprite = bagSprites[bagLevel];
        bagNameText.text = bagNames[bagLevel + 1];
        bagLevelText.text = "Level :  " + (bagLevel + 1).ToString();
        bagUpgradeStatusText.text = bagUpgradeStatus ? "Upgradable" : "Not Upgradable";

        maxFootwearLevel = footwearSprites.Count;
        footwearImage.sprite = footwearSprites[footwearLevel];
        footwearNameText.text = footwearNames[footwearLevel];
        footwearLevelText.text = "Level :  " + (footwearLevel + 1).ToString();
        footwearUpgradeStatusText.text = footwearUpgradeStatus ? "Upgradable" : "Not Upgradable";


        maxGlovesLevel = glovesSprites.Count;
        glovesImage.sprite = glovesSprites[glovesLevel];
        glovesNameText.text = glovesNames[glovesLevel];
        glovesLevelText.text = "Level :  " + (glovesLevel + 1).ToString();
        glovesUpgradeStatusText.text = glovesUpgradeStatus ? "Upgradable" : "Not Upgradable";

        maxPickingToolLevel = pickingToolSprites.Count;
        pickingToolImage.sprite = pickingToolSprites[pickingToolLevel];
        pickingToolNameText.text = pickingToolNames[pickingToolLevel];
        pickingToolLevelText.text = "Level :  " + (pickingToolLevel + 1).ToString();
        pickingToolUpgradeStatusText.text = pickingToolUpgradeStatus ? "Upgradable" : "Not Upgradable";
    }

    public void getEquipment()
    {
        bagImage = GameObject.Find("PlayerBag").GetComponent<Image>();
        bagNameText = GameObject.Find("BagName").GetComponent<TMP_Text>();
        bagUpgradeStatusText = GameObject.Find("BagStatus").GetComponent<TMP_Text>();
        bagLevelText = GameObject.Find("BagLevel").GetComponent<TMP_Text>();

        footwearImage = GameObject.Find("PlayerFootwear").GetComponent<Image>();
        footwearNameText = GameObject.Find("FootwearName").GetComponent<TMP_Text>();
        footwearUpgradeStatusText = GameObject.Find("FootwearStatus").GetComponent<TMP_Text>();
        footwearLevelText = GameObject.Find("FootwearLevel").GetComponent<TMP_Text>();

        pickingToolImage = GameObject.Find("PlayerTool").GetComponent<Image>();
        pickingToolNameText = GameObject.Find("ToolName").GetComponent<TMP_Text>();
        pickingToolUpgradeStatusText = GameObject.Find("ToolStatus").GetComponent<TMP_Text>();
        pickingToolLevelText = GameObject.Find("ToolLevel").GetComponent<TMP_Text>();

        glovesImage = GameObject.Find("PlayerGloves").GetComponent<Image>();
        glovesNameText = GameObject.Find("GlovesName").GetComponent<TMP_Text>();
        glovesUpgradeStatusText = GameObject.Find("GlovesStatus").GetComponent<TMP_Text>();
        glovesLevelText = GameObject.Find("GlovesLevel").GetComponent<TMP_Text>();
    }
}
