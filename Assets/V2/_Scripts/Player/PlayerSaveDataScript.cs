using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveDataScript
{
    public Vector3 playerPos;
    public int playerCurrency;
    public int currentDay;
    public long currentTimeTicks;

    // Equipment levels
    public int bagLevel;
    public int footwearLevel;
    public int gloveLevel;
    public int toolLevel;

    // Inventory
    public List<InventorySaveData> inventory = new List<InventorySaveData>();
    public List<QuestProgression> questProgressData;
    public List<string> handedInQuests;
}
