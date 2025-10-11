using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class EquipmentUI : MonoBehaviour
{
    [Header("References")]
    private EquipmentScript _equipmentScript;
    private PlayerCurrencyScript _playerCurrency;

    [Header("Bag UI")]
    public Image bagImage;
    public TMP_Text bagName;
    public TMP_Text bagLevel;
    public TMP_Text bagStatus;

    [Header("Gloves UI")]
    public Image glovesImage;
    public TMP_Text glovesName;
    public TMP_Text glovesLevel;
    public TMP_Text glovesStatus;

    [Header("Footwear UI")]
    public Image footwearImage;
    public TMP_Text footwearName;
    public TMP_Text footwearLevel;
    public TMP_Text footwearStatus;

    [Header("Tool UI")]
    public Image toolImage;
    public TMP_Text toolName;
    public TMP_Text toolLevel;
    public TMP_Text toolStatus;

    void Awake()
    {
        _equipmentScript = FindAnyObjectByType<EquipmentScript>();
        _playerCurrency = FindAnyObjectByType<PlayerCurrencyScript>();
    }

    void Start()
    {
        UpdateAllUI();
    }

    public void UpdateAllUI()
    {
        _playerCurrency.UpdateText();
        UpdateUI(_equipmentScript.bags, _equipmentScript, bagImage, bagName, bagLevel, bagStatus);
        UpdateUI(_equipmentScript.gloves, _equipmentScript, glovesImage, glovesName, glovesLevel, glovesStatus);
        UpdateUI(_equipmentScript.footwear, _equipmentScript, footwearImage, footwearName, footwearLevel, footwearStatus);
        UpdateUI(_equipmentScript.tools, _equipmentScript, toolImage, toolName, toolLevel, toolStatus);
        Debug.Log("Updated all equipments");
    }

    private void UpdateUI(EquipmentsSO equipmentSO, EquipmentScript equipment, Image icon, TMP_Text nameText, TMP_Text levelText, TMP_Text statusText)
    {
        if (equipmentSO == null) return;
        int currentLevel = equipment.GetLevel(equipmentSO);
        icon.sprite = equipmentSO.equipmentSprites[currentLevel];
        nameText.text = equipmentSO.equipmentName[currentLevel];
        levelText.text = $"Level: {currentLevel + 1}";
        statusText.text = equipment.GetStatus(equipmentSO, currentLevel);
    }
}
