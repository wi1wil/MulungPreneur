using UnityEngine;

public class EquipmentScript : MonoBehaviour
{
    [Header("Equipment References")]
    public EquipmentsSO bags;
    public EquipmentsSO footwear;
    public EquipmentsSO gloves;
    public EquipmentsSO tools;

    [Header("Current Levels")]
    private int _bagLevel = 0;
    private int _footwearLevel = 0;
    private int _gloveLevel = 0;
    private int _toolLevel = 0;

    private PlayerCurrencyScript _playerCurrency;
    private PlayerMovementScript _playerMovement;
    private InteractableDetector _interactableDetector;
    private EquipmentUI _ui;

    // Base stats for resetting
    private int _baseInventorySize;
    private float _basePlayerSpeed;
    private float _baseHoldDuration;
    private float _basePickUpRadius;

    void Awake()
    {
        _ui = FindAnyObjectByType<EquipmentUI>();
        _playerCurrency = FindAnyObjectByType<PlayerCurrencyScript>();
        _playerMovement = FindAnyObjectByType<PlayerMovementScript>();
        _interactableDetector = FindAnyObjectByType<InteractableDetector>();
    }

    void Start()
    {
        // Ensure base stats are captured after everything is initialized
        _baseInventorySize = InventoryManager.Instance.inventorySize;
        _basePlayerSpeed = _playerMovement.speed;
        _baseHoldDuration = _interactableDetector.holdDuration;
        _basePickUpRadius = _interactableDetector.pickUpRadius;

        // Apply upgrades from saved levels
        ApplyUpgradeEffects();

        // Update UI after applying stats
        _ui.UpdateAllUI();
    }

    // Upgrade from button
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

        _playerCurrency.DecreaseCurrency(price);
        currentLevel++;
        SetLevel(equipment, currentLevel);

        ApplyUpgradeEffects();
        _ui.UpdateAllUI();
    }

    // Apply all upgrade effects based on saved/current levels
    public void ApplyUpgradeEffects()
    {
        // Reset stats
        InventoryManager.Instance.inventorySize = _baseInventorySize;
        _playerMovement.speed = _basePlayerSpeed;
        _interactableDetector.holdDuration = _baseHoldDuration;
        _interactableDetector.pickUpRadius = _basePickUpRadius;

        // Apply accumulated effects for all equipment
        ApplyEquipmentEffect(bags, _bagLevel);
        ApplyEquipmentEffect(footwear, _footwearLevel);
        ApplyEquipmentEffect(gloves, _gloveLevel);
        ApplyEquipmentEffect(tools, _toolLevel);
    }

    private void ApplyEquipmentEffect(EquipmentsSO equipment, int level)
    {
        if (equipment == null || level <= 0) return;

        for (int i = 0; i < level; i++)
        {
            switch (equipment.equipmentType)
            {
                case EquipmentType.Bag:
                    InventoryManager.Instance.inventorySize += (int)equipment.statIncrease;
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

        if (equipment.equipmentType == EquipmentType.Bag)
            InventoryManager.Instance.InitializeInventory();
        else if (equipment.equipmentType == EquipmentType.PickingTool)
            _interactableDetector.UpdateCircleRange();
    }

    // Getters/Setters
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

    public void SetLevel(EquipmentsSO equipment, int newLevel)
    {
        if (equipment == bags) _bagLevel = newLevel;
        else if (equipment == footwear) _footwearLevel = newLevel;
        else if (equipment == gloves) _gloveLevel = newLevel;
        else if (equipment == tools) _toolLevel = newLevel;
    }

    // Button helpers
    public void OnBagUpgradeButton() => UpgradeEquipment(bags);
    public void OnFootwearUpgradeButton() => UpgradeEquipment(footwear);
    public void OnGlovesUpgradeButton() => UpgradeEquipment(gloves);
    public void OnToolsUpgradeButton() => UpgradeEquipment(tools);
}
