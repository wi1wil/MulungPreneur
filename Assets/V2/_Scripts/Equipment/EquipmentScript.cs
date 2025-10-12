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
        Debug.Log($"Upgrading {equipment.equipmentName[currentLevel]} for {price} currency");
        Debug.Log($"Player Currency after upgrade: {_playerCurrency.playerCurrency}");
        currentLevel++;
        SetLevel(equipment, currentLevel);

        UpgradeEffect(equipment);
        UpdateAll();
        _ui.UpdateAllUI();
    }

    private void UpgradeEffect(EquipmentsSO equipment)
    {
        switch (equipment.equipmentType)
        {
            case EquipmentType.Bag:
                // Add inventory slots;
                break;
            case EquipmentType.Footwear:
                Debug.Log($"Added {equipment.statIncrease} to current {_playerMovement.speed}");
                _playerMovement.speed += equipment.statIncrease;
                break;
            case EquipmentType.Gloves:
                Debug.Log($"Reduced the current {_interactableDetector.holdDuration} to {_interactableDetector.holdDuration - equipment.statIncrease} ");
                _interactableDetector.holdDuration -= equipment.statIncrease;
                break;
            case EquipmentType.PickingTool:
                Debug.Log($"Added the {_interactableDetector.pickUpRadius} by {equipment.statIncrease} = {_interactableDetector.pickUpRadius + equipment.statIncrease}");
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
        if (currentLevel >= equipment.maxLevel - 1)
            return "Max Level!";
        if (_playerCurrency.playerCurrency < equipment.equipmentPrices[currentLevel])
            return "Too Broke!";
        return "Upgradeable";
    }

    private void SetLevel(EquipmentsSO equipment, int newLevel)
    {
        if (equipment == bags) _bagLevel = newLevel;
        else if (equipment == footwear) _footwearLevel = newLevel;
        else if (equipment == gloves) _gloveLevel = newLevel;
        else if (equipment == tools) _toolLevel = newLevel;
    }

    public void OnBagUpgradeButton() => UpgradeEquipment(bags);
    public void OnFootwearUpgradeButton() => UpgradeEquipment(footwear);
    public void OnGlovesUpgradeButton() => UpgradeEquipment(gloves);
    public void OnToolsUpgradeButton() => UpgradeEquipment(tools);
}
