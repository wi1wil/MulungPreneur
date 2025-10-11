using UnityEngine;

public class EquipmentScript : MonoBehaviour
{
    [Header("Equipment References")]
    public EquipmentsSO bags;
    public EquipmentsSO footwear;
    public EquipmentsSO gloves;
    public EquipmentsSO tools;

    [Header("Current Levels")]
    private int _bagLevel, _footwearLevel, _gloveLevel, _toolLevel;

    private PlayerCurrencyScript _playerCurrency;
    private PlayerMovementScript _playerMovement;
    private InteractableDetector _interactableDetector;
    private EquipmentUI _ui;

    void Awake()
    {
        _ui = FindAnyObjectByType<EquipmentUI>();
        _playerCurrency = FindAnyObjectByType<PlayerCurrencyScript>();
        _playerMovement = FindAnyObjectByType<PlayerMovementScript>();
        _interactableDetector = FindAnyObjectByType<InteractableDetector>();
        UpdateAll();
    }

    public void UpdateAll()
    {
        UpdateEquipment(bags, _bagLevel);
        UpdateEquipment(footwear, _footwearLevel);
        UpdateEquipment(gloves, _gloveLevel);
        UpdateEquipment(tools, _toolLevel);
    }

    public void UpdateEquipment(EquipmentsSO equipment, int level)
    {
        if (equipment == null) return;
        // Checks the level, if its above or under 0 - maxlevel - 1 if it is then return max/min; 
        level = Mathf.Clamp(level, 0, equipment.maxLevel - 1);

        Debug.Log($"{equipment.equipmentName[level]} updated to Level {level + 1}");
    }

    public void UpgradeEquipment(EquipmentsSO equipment)
    {
        if (equipment == null || _playerCurrency == null) return;
        int currentLevel = GetLevel(equipment);
        if (currentLevel >= equipment.maxLevel - 1)
        {
            Debug.Log($"{equipment.equipmentName[currentLevel]} is maxed");
            return;
        }

        int price = equipment.equipmentPrices[currentLevel];
        if (_playerCurrency.playerCurrency < price)
        {
            Debug.Log($"Not enough money to upgrade {equipment.equipmentName[currentLevel]}");
            return;
        }

        _playerCurrency.DecreaseCurrency(equipment.equipmentPrices[currentLevel]);
        currentLevel++;
        SetLevel(equipment, currentLevel);

        UpgradeEffect(equipment);
        UpdateAll();
        _ui.UpdateAllUI();
        Debug.Log($"Upgaded {equipment.equipmentName[currentLevel]} to level {currentLevel}!");
    }

    private void UpgradeEffect(EquipmentsSO equipment)
    {
        switch (equipment.equipmentType)
        {
            case EquipmentType.Bag:
                // Add inventory slots;
                break;
            case EquipmentType.Footwear:
                _playerMovement.speed += equipment.statIncrease;
                break;
            case EquipmentType.Gloves:
                _interactableDetector.holdDuration -= equipment.statIncrease;
                break;
            case EquipmentType.PickingTool:
                _interactableDetector.pickUpRadius += equipment.statIncrease;
                break;
        }
    }

    public int GetLevel(EquipmentsSO equipment)
    {
        if (equipment == bags) return _bagLevel;
        if (equipment == footwear) return _footwearLevel;
        if (equipment == gloves) return _gloveLevel;
        if (equipment == tools) return _toolLevel;
        return 0;
    }

    public string GetStatus(EquipmentsSO equipment, int currentLevel)
    {
        if (equipment.equipmentPrices[currentLevel] > _playerCurrency.playerCurrency)
        {
            return "Too Broke!";
        }
        else if (GetLevel(equipment) == equipment.maxLevel)
        {
            return "Max Level!";
        }
        else
        {
            return "Upgradeable";
        }
    }

    private void SetLevel(EquipmentsSO equipment, int newLevel)
    {
        if (equipment == bags) _bagLevel = newLevel;
        else if (equipment == footwear) _footwearLevel = newLevel;
        else if (equipment == gloves) _gloveLevel = newLevel;
        else if (equipment == tools) _toolLevel = newLevel;
    }
}
