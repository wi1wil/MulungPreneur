using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipments", menuName = "Inventory/Equipments")]
public class EquipmentsSO : ScriptableObject
{
    public string equipmentName;
    public List<Sprite> equipmentSprites;
    public List<string> equipmentDescriptions;
    public List<int> equipmentPrices;

    public int maxLevel => equipmentSprites.Count;
}
