using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class EquipmentUI : MonoBehaviour
{
    [Header("References")]
    private EquipmentScript _equipmentScript;
    private PlayerCurrencyScript _playerCurrency;

    [Header("Bag UI")]
    public List<Image> bagImage;
    public List<TMP_Text> bagName;
    public List<TMP_Text> bagLevel;
    public List<TMP_Text> bagStatus;

    [Header("Gloves UI")]
    public List<Image> glovesImage;
    public List<TMP_Text> glovesName;
    public List<TMP_Text> glovesLevel;
    public List<TMP_Text> glovesStatus;

    [Header("Footwear UI")]
    public List<Image> footwearImage;
    public List<TMP_Text> footwearName;
    public List<TMP_Text> footwearLevel;
    public List<TMP_Text> footwearStatus;

    [Header("Tool UI")]
    public List<Image> toolImage;
    public List<TMP_Text> toolName;
    public List<TMP_Text> toolLevel;
    public List<TMP_Text> toolStatus;

    [Header("Upgrade Buttons")]
    public Button upgradeBagButton;
    public Button upgradeGlovesButton;
    public Button upgradeFootwearButton;
    public Button upgradeToolsButton;

    void Awake()
    {
        _equipmentScript = FindAnyObjectByType<EquipmentScript>();
        _playerCurrency = FindAnyObjectByType<PlayerCurrencyScript>();
    }

    void Start()
    {
        InventoryManager.Instance.onInvChanged += UpdateAllUI;
        UpdateAllUI();
    }

    public void UpdateAllUI()
    {
        _playerCurrency.UpdateText();

        UpdateUpgradeButtons();
        UpdateUI(_equipmentScript.bags, _equipmentScript, bagImage, bagName, bagLevel, bagStatus);
        UpdateUI(_equipmentScript.gloves, _equipmentScript, glovesImage, glovesName, glovesLevel, glovesStatus);
        UpdateUI(_equipmentScript.footwear, _equipmentScript, footwearImage, footwearName, footwearLevel, footwearStatus);
        UpdateUI(_equipmentScript.tools, _equipmentScript, toolImage, toolName, toolLevel, toolStatus);

        Debug.Log("Updated all equipments");
    }

    public void UpdateUpgradeButtons()
    {
        UpdateButtonState(_equipmentScript.bags, _equipmentScript, upgradeBagButton);
        UpdateButtonState(_equipmentScript.gloves, _equipmentScript, upgradeGlovesButton);
        UpdateButtonState(_equipmentScript.footwear, _equipmentScript, upgradeFootwearButton);
        UpdateButtonState(_equipmentScript.tools, _equipmentScript, upgradeToolsButton);
    }

    private void UpdateButtonState(
        EquipmentsSO equipmentsSO,
        EquipmentScript equipmentScript,
        Button upgradeButton
        )
    {
        if (equipmentsSO == null) return;
        int level = equipmentScript.GetLevel(equipmentsSO);
        bool isMaxed = level >= equipmentsSO.maxLevel - 1;
        bool canAfford = !isMaxed && _playerCurrency.playerCurrency >= equipmentsSO.equipmentPrices[level];
        upgradeButton.gameObject.SetActive(canAfford);
    }

    private void UpdateUI(
        EquipmentsSO equipmentSO,
        EquipmentScript equipment,
        List<Image> icon,
        List<TMP_Text> nameText,
        List<TMP_Text> levelText,
        List<TMP_Text> statusText 
        )
    {
        if (equipmentSO == null) return;
        int currentLevel = equipment.GetLevel(equipmentSO);
        for(int i = 0; i < icon.Count; i++)
        {
            icon[i].sprite = equipmentSO.equipmentSprites[currentLevel];
            nameText[i].text = equipmentSO.equipmentName[currentLevel];
            levelText[i].text = $"Level: {currentLevel + 1}";
            statusText[i].text = equipment.GetStatus(equipmentSO, currentLevel);
        }
    }
}
