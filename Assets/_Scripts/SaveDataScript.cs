using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataScript
{
    public Vector3 playerPosition;
    public string mapBoundary;
    public List<InventorySaveData> inventorySaveData;
    public int currentDay;
    public long currentTimeTicks;
    public List<QuestProgress> questProgressData;
}
