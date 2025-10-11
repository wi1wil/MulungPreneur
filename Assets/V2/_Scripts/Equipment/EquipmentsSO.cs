using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipments", menuName = "Inventory/Equipments")]
public class EquipmentsSO : ScriptableObject
{
    [Header("Basic Info")]
    public List<string> equipmentName;
    public EquipmentType equipmentType;

    [Header("Visuals & Data")]
    public List<Sprite> equipmentSprites;
    public List<int> equipmentPrices;

    [Header("Upgrade Effects")]
    [Tooltip("How much stat increase per upgrade (speed, range, slots, etc)")]
    public float statIncrease;

    public int maxLevel => equipmentSprites.Count;
}

public enum EquipmentType
{
    Bag,
    Footwear,   
    Gloves,
    PickingTool
}
