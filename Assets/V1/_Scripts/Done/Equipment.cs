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
    public List<int> bagPrice;

    [Header("Footwear")]
    public int footwearLevel;
    public bool footwearUpgradeStatus;
    public List<Sprite> footwearSprites;
    public List<String> footwearNames;
    public int maxFootwearLevel;
    public List<int> footwearPrice;

    [Header("Gloves")]
    public int glovesLevel;
    public bool glovesUpgradeStatus;
    public List<Sprite> glovesSprites;
    public List<String> glovesNames;
    public int maxGlovesLevel;
    public List<int> glovesPrice;

    [Header("Picking Tool")]
    public int pickingToolLevel;
    public bool pickingToolUpgradeStatus;
    public List<Sprite> pickingToolSprites;
    public List<String> pickingToolNames;
    public int maxPickingToolLevel;
    public List<int> pickingToolPrice;

    [Header("Equipment Images")]
    public List<Image> bagImage;
    public List<Image> footwearImage;
    public List<Image> glovesImage;
    public List<Image> pickingToolImage;

    [Header("Equipment Names")]
    public List<TMP_Text> bagNameText;
    public List<TMP_Text> footwearNameText;
    public List<TMP_Text> glovesNameText;
    public List<TMP_Text> pickingToolNameText;

    [Header("Equipment Upgrade Status")]
    public List<TMP_Text> bagUpgradeStatusText;
    public List<TMP_Text> footwearUpgradeStatusText;
    public List<TMP_Text> glovesUpgradeStatusText;
    public List<TMP_Text> pickingToolUpgradeStatusText;

    [Header("Equipment Current Level")]
    public List<TMP_Text> bagLevelText;
    public List<TMP_Text> footwearLevelText;
    public List<TMP_Text> glovesLevelText;
    public List<TMP_Text> pickingToolLevelText;

    [Header("Equipment Upgrade Buttons")]
    public Button bagUpgradeButton;
    public Button footwearUpgradeButton;
    public Button glovesUpgradeButton;
    public Button pickingToolUpgradeButton;

    private PlayerWalletScript playerWalletScript;
    private InventoryManagerScripts inventoryManagerScript;
    private PlayerItemPickUpScript playerItemPickUpScript;
    private MovementScript movementScript;

    void Start()
    {
        playerWalletScript = FindObjectOfType<PlayerWalletScript>();
        inventoryManagerScript = FindObjectOfType<InventoryManagerScripts>();
        playerItemPickUpScript = FindObjectOfType<PlayerItemPickUpScript>();
        movementScript = FindObjectOfType<MovementScript>();

        if (gameObject.activeInHierarchy)
        {
            getEquipment();
            updateEquipment();
        }
    }

    void Update()
    {
        getEquipment();
        updateEquipment();
        updateStatus();
    }

    public void updateStatus()
    {
        if (bagLevel < maxBagLevel - 1 && playerWalletScript.playerMoney > bagPrice[bagLevel])
        {
            bagUpgradeStatus = true;
            bagUpgradeButton.gameObject.SetActive(true);
        }
        else
        {
            bagUpgradeStatus = false;
            bagUpgradeButton.gameObject.SetActive(false);
        }

        if (footwearLevel < maxFootwearLevel - 1 && playerWalletScript.playerMoney > footwearPrice[footwearLevel])
        {
            footwearUpgradeStatus = true;
            footwearUpgradeButton.gameObject.SetActive(true);
        }
        else
        {
            footwearUpgradeStatus = false;
            footwearUpgradeButton.gameObject.SetActive(false);
        }

        if (glovesLevel < maxGlovesLevel - 1 && playerWalletScript.playerMoney > glovesPrice[glovesLevel])
        {
            glovesUpgradeStatus = true;
            glovesUpgradeButton.gameObject.SetActive(true);
        }
        else
        {
            glovesUpgradeStatus = false;
            glovesUpgradeButton.gameObject.SetActive(false);
        }

        if (pickingToolLevel < maxPickingToolLevel - 1 && playerWalletScript.playerMoney > pickingToolPrice[pickingToolLevel])
        {
            pickingToolUpgradeStatus = true;
            pickingToolUpgradeButton.gameObject.SetActive(true);
        }
        else
        {
            pickingToolUpgradeStatus = false;  
            pickingToolUpgradeButton.gameObject.SetActive(false);
        }
    }

    public void updateEquipment()
    {
        maxBagLevel = bagSprites.Count;
        foreach (var img in bagImage)
        {
            img.sprite = bagSprites[bagLevel];
        }
        foreach (var txt in bagNameText)
        {
            txt.text = bagNames[bagLevel];
        }
        foreach (var txt in bagLevelText)
        {
            txt.text = "Level :  " + (bagLevel + 1).ToString();
        }
        foreach (var txt in bagUpgradeStatusText)
        {
            txt.text = bagUpgradeStatus ? "Upgradable" : "Not Upgradable";
            if (bagLevel == maxBagLevel - 1)
            {
                txt.text = "Max Level";
                bagUpgradeButton.gameObject.SetActive(false);
            }
        }

        maxFootwearLevel = footwearSprites.Count;
        foreach (var img in footwearImage)
        {
            img.sprite = footwearSprites[footwearLevel];
        }
        foreach (var txt in footwearNameText)
        {
            txt.text = footwearNames[footwearLevel];
        }
        foreach (var txt in footwearLevelText)
        {
            txt.text = "Level :  " + (footwearLevel + 1).ToString();
        }
        foreach (var txt in footwearUpgradeStatusText)
        {
            txt.text = footwearUpgradeStatus ? "Upgradable" : "Not Upgradable";
            if (footwearLevel == maxFootwearLevel - 1)
            {
                txt.text = "Max Level";
                footwearUpgradeButton.gameObject.SetActive(false);
            }
        }

        maxGlovesLevel = glovesSprites.Count;
        foreach (var img in glovesImage)
        {
            img.sprite = glovesSprites[glovesLevel];
        }
        foreach (var txt in glovesNameText)
        {
            txt.text = glovesNames[glovesLevel];
        }
        foreach (var txt in glovesLevelText)
        {
            txt.text = "Level :  " + (glovesLevel + 1).ToString();
        }
        foreach (var txt in glovesUpgradeStatusText)
        {
            txt.text = glovesUpgradeStatus ? "Upgradable" : "Not Upgradable";
            if (glovesLevel == maxGlovesLevel - 1)
            {
                txt.text = "Max Level";
                glovesUpgradeButton.gameObject.SetActive(false);
            }
        }

        maxPickingToolLevel = pickingToolSprites.Count;
        foreach (var img in pickingToolImage)
        {
            img.sprite = pickingToolSprites[pickingToolLevel];
        }
        foreach (var txt in pickingToolNameText)
        {
            txt.text = pickingToolNames[pickingToolLevel];
        }
        foreach (var txt in pickingToolLevelText)
        {
            txt.text = "Level :  " + (pickingToolLevel + 1).ToString();
        }
        foreach (var txt in pickingToolUpgradeStatusText)
        {
            txt.text = pickingToolUpgradeStatus ? "Upgradable" : "Not Upgradable";
            if (pickingToolLevel == maxPickingToolLevel - 1)
            {
                txt.text = "Max Level";
                pickingToolUpgradeButton.gameObject.SetActive(false);
            }
        }
    }

    public void getEquipment()
    {
        bagImage = new List<Image>(GameObject.FindObjectsOfType<Image>());
        bagImage = bagImage.FindAll(img => img.gameObject.name == "PlayerBag");

        bagNameText = new List<TMP_Text>(GameObject.FindObjectsOfType<TMP_Text>());
        bagNameText = bagNameText.FindAll(txt => txt.gameObject.name == "BagName");

        bagUpgradeStatusText = new List<TMP_Text>(GameObject.FindObjectsOfType<TMP_Text>());
        bagUpgradeStatusText = bagUpgradeStatusText.FindAll(txt => txt.gameObject.name == "BagStatus");

        bagLevelText = new List<TMP_Text>(GameObject.FindObjectsOfType<TMP_Text>());
        bagLevelText = bagLevelText.FindAll(txt => txt.gameObject.name == "BagLevel");

        footwearImage = new List<Image>(GameObject.FindObjectsOfType<Image>());
        footwearImage = footwearImage.FindAll(img => img.gameObject.name == "PlayerFootwear");

        footwearNameText = new List<TMP_Text>(GameObject.FindObjectsOfType<TMP_Text>());
        footwearNameText = footwearNameText.FindAll(txt => txt.gameObject.name == "FootwearName");

        footwearUpgradeStatusText = new List<TMP_Text>(GameObject.FindObjectsOfType<TMP_Text>());
        footwearUpgradeStatusText = footwearUpgradeStatusText.FindAll(txt => txt.gameObject.name == "FootwearStatus");

        footwearLevelText = new List<TMP_Text>(GameObject.FindObjectsOfType<TMP_Text>());
        footwearLevelText = footwearLevelText.FindAll(txt => txt.gameObject.name == "FootwearLevel");

        pickingToolImage = new List<Image>(GameObject.FindObjectsOfType<Image>());
        pickingToolImage = pickingToolImage.FindAll(img => img.gameObject.name == "PlayerTool");

        pickingToolNameText = new List<TMP_Text>(GameObject.FindObjectsOfType<TMP_Text>());
        pickingToolNameText = pickingToolNameText.FindAll(txt => txt.gameObject.name == "ToolName");

        pickingToolUpgradeStatusText = new List<TMP_Text>(GameObject.FindObjectsOfType<TMP_Text>());
        pickingToolUpgradeStatusText = pickingToolUpgradeStatusText.FindAll(txt => txt.gameObject.name == "ToolStatus");

        pickingToolLevelText = new List<TMP_Text>(GameObject.FindObjectsOfType<TMP_Text>());
        pickingToolLevelText = pickingToolLevelText.FindAll(txt => txt.gameObject.name == "ToolLevel");

        glovesImage = new List<Image>(GameObject.FindObjectsOfType<Image>());
        glovesImage = glovesImage.FindAll(img => img.gameObject.name == "PlayerGloves");

        glovesNameText = new List<TMP_Text>(GameObject.FindObjectsOfType<TMP_Text>());
        glovesNameText = glovesNameText.FindAll(txt => txt.gameObject.name == "GlovesName");

        glovesUpgradeStatusText = new List<TMP_Text>(GameObject.FindObjectsOfType<TMP_Text>());
        glovesUpgradeStatusText = glovesUpgradeStatusText.FindAll(txt => txt.gameObject.name == "GlovesStatus");

        glovesLevelText = new List<TMP_Text>(GameObject.FindObjectsOfType<TMP_Text>());
        glovesLevelText = glovesLevelText.FindAll(txt => txt.gameObject.name == "GlovesLevel");
    }

    

    public void upgradeBag()
    {
        if (!bagUpgradeStatus || bagLevel >= maxBagLevel - 1)
        {
            Debug.Log("Bag cannot be upgraded further or not enough money.");
            return;
        }
        if (playerWalletScript.playerMoney > bagPrice[bagLevel])
        {
            playerWalletScript.playerMoney -= bagPrice[bagLevel];
            Debug.Log("Bag upgrade purchased for: " + bagPrice[bagLevel] + ", Remaining Money: " + playerWalletScript.playerMoney);
            bagLevel++;
            inventoryManagerScript.addSlots(6);
            updateEquipment();
        }
    }

    public void upgradeFootwear()
    {
        if (!footwearUpgradeStatus || footwearLevel >= maxFootwearLevel - 1)
        {
            Debug.Log("Footwear cannot be upgraded further or not enough money.");
            return;
        }
        if (playerWalletScript.playerMoney > footwearPrice[footwearLevel])
        {
            playerWalletScript.playerMoney -= footwearPrice[footwearLevel];
            Debug.Log("Footwear upgrade purchased for: " + footwearPrice[footwearLevel] + ", Remaining Money: " + playerWalletScript.playerMoney);
            footwearLevel++;
            movementScript.movementSpeed += 0.25f; 
            updateEquipment();
        }
    }

    public void upgradeGloves()
    {
        if (!glovesUpgradeStatus || glovesLevel >= maxGlovesLevel - 1)
        {
            Debug.Log("Gloves cannot be upgraded further or not enough money.");
            return;
        }
        if (playerWalletScript.playerMoney > glovesPrice[glovesLevel])
        {
            playerWalletScript.playerMoney -= glovesPrice[glovesLevel];
            Debug.Log("Gloves upgrade purchased for: " + glovesPrice[glovesLevel] + ", Remaining Money: " + playerWalletScript.playerMoney);
            glovesLevel++;
            playerItemPickUpScript.updatePickUpSpeed(0.25f);
            updateEquipment();
        }
    }

    public void upgradePickingTool()
    {
        if (!pickingToolUpgradeStatus || pickingToolLevel >= maxPickingToolLevel - 1)
        {
            Debug.Log("Picking Tool cannot be upgraded further or not enough money.");
            return;
        }
        if (playerWalletScript.playerMoney > pickingToolPrice[pickingToolLevel])
        {
            playerWalletScript.playerMoney -= pickingToolPrice[pickingToolLevel];
            Debug.Log("Picking Tool upgrade purchased for: " + pickingToolPrice[pickingToolLevel] + ", Remaining Money: " + playerWalletScript.playerMoney);
            pickingToolLevel++;
            playerItemPickUpScript.updatePickUpRange(0.5f);
            updateEquipment();
        }
    }
}
