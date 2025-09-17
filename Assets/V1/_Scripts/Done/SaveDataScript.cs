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
    public List<string> handinQuestsID;
    public int playerMoney;

    public int bagLevel;
    public int inventorySize;

    public int footwearLevel;
    public float playerSpeed;

    public int glovesLevel;
    public float pickUpSpeed;

    public int toolLevel;
    public float pickUpRange;
}
