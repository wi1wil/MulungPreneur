using System.IO;
using UnityEngine;
using System.Collections;

public class PlayerSaveManagerScript : MonoBehaviour
{
    private string saveLocation;
    [SerializeField] private float autosaveInterval = 30f;

    private PlayerCurrencyScript _playerCurrency;
    private DayNightCycle _dayNightCycle;
    private EquipmentScript _equipmentScript;
    private EquipmentUI _equipmentUI;
    private GameObject _player;

    void Awake()
    {
        _playerCurrency = FindAnyObjectByType<PlayerCurrencyScript>();
        _dayNightCycle = FindAnyObjectByType<DayNightCycle>();
        _equipmentScript = FindAnyObjectByType<EquipmentScript>();
        _equipmentUI = FindAnyObjectByType<EquipmentUI>();
        _player = GameObject.Find("Player");
    }

    void Start()
    {
        // saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        saveLocation = @"D:\PersonalD\UnityProjects\MulungPreneur\SaveLocation\saveData.json";
        string folderPath = Path.GetDirectoryName(saveLocation);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        LoadGame();
        StartCoroutine(AutoSaveRoutine());
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autosaveInterval);
            SaveGame();
        }
    }

    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        PlayerSaveDataScript playerSaveData = new PlayerSaveDataScript
        {
            playerPos = _player.transform.position,
            playerCurrency = _playerCurrency.playerCurrency > 0 ? _playerCurrency.playerCurrency : 0,
            currentDay = _dayNightCycle.GetCurrentDay(),
            currentTimeTicks = _dayNightCycle.GetCurrentTimeTicks(),
            bagLevel = _equipmentScript.GetLevel(_equipmentScript.bags),
            footwearLevel = _equipmentScript.GetLevel(_equipmentScript.footwear),
            gloveLevel = _equipmentScript.GetLevel(_equipmentScript.gloves),
            toolLevel = _equipmentScript.GetLevel(_equipmentScript.tools),
            questProgressData = QuestManager.Instance.activeQuests,
            handedInQuests = QuestManager.Instance.handInQuestsID
        };

        var inventory = InventoryManager.Instance.GetInventory();
        playerSaveData.inventory.Clear();
        for (int i = 0; i < inventory.Count; i++)
        {
            var stack = inventory[i];
            if (stack.item != null && stack.quantity > 0)
            {
                playerSaveData.inventory.Add(new InventorySaveData
                {
                    itemID = stack.item.itemID,
                    slotIdx = i,
                    quantity = stack.quantity
                });
            }
        }

        string json = JsonUtility.ToJson(playerSaveData, true);
        File.WriteAllText(saveLocation, json);
        Debug.Log("Game Saved: " + json);
    }

    [ContextMenu("Load Game")]
    public void LoadGame()
    {
        if (!File.Exists(saveLocation))
        {
            SaveGame(); // create default save
            Debug.Log("No save found, creating new one.");
            return;
        }

        string json = File.ReadAllText(saveLocation);
        PlayerSaveDataScript playerSaveData = JsonUtility.FromJson<PlayerSaveDataScript>(json);

        // Set player position and currency
        _player.transform.position = playerSaveData.playerPos;
        _playerCurrency.playerCurrency = playerSaveData.playerCurrency;

        // Set time
        _dayNightCycle.SetWorldTime(playerSaveData.currentDay, playerSaveData.currentTimeTicks);

        // Set equipment levels
        _equipmentScript.SetLevel(_equipmentScript.bags, playerSaveData.bagLevel);
        _equipmentScript.SetLevel(_equipmentScript.footwear, playerSaveData.footwearLevel);
        _equipmentScript.SetLevel(_equipmentScript.gloves, playerSaveData.gloveLevel);
        _equipmentScript.SetLevel(_equipmentScript.tools, playerSaveData.toolLevel);
        _equipmentScript.ApplyUpgradeEffects();
        _equipmentUI.UpdateAllUI();

        // Restore inventory
        var inventory = InventoryManager.Instance.GetInventory();
        InventoryManager.Instance.inventorySize = Mathf.Max(inventory.Count, playerSaveData.inventory.Count);
        InventoryManager.Instance.InitializeInventory(); // Ensure slots exist

        foreach (var invData in playerSaveData.inventory)
        {
            var itemSO = ItemDictionary.Instance.GetItemData(invData.itemID);
            if (itemSO != null && invData.slotIdx < inventory.Count)
            {
                inventory[invData.slotIdx].item = itemSO;
                inventory[invData.slotIdx].quantity = invData.quantity;
            }
        }
        InventoryManager.Instance.InvokeInventoryChanged();

        // Load quest progress
        QuestManager.Instance.LoadQuestProgress(playerSaveData.questProgressData);
        QuestManager.Instance.handInQuestsID = playerSaveData.handedInQuests;

        Debug.Log("Game Loaded: " + json);
    }

    [ContextMenu("Delete Save")]
    public void DeleteSave()
    {
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);
            Debug.Log("Save file deleted.");
        }
    }

    // private void OnApplicationQuit()
    // {
    //     SaveGame();
    //     Debug.Log("Game saved on exit.");
    // }
}
