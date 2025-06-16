using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

public class SaveControllerScript : MonoBehaviour
{   
    private string saveLocation;
    private InventoryManagerScript inventoryManager;
    private WorldTime worldTime;
    private VolumeSettings volumeSettings;
    private VideoSettingsScript videoSettings;
    private PlayerWalletScript playerWalletScript;
    private MovementScript playerMovementScript;
    private PlayerItemPickUpScript playerItemPickUpScript;
    private Equipment equipmentScript;

    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryManager = FindObjectOfType<InventoryManagerScript>();
        worldTime = FindObjectOfType<WorldTime>();
        volumeSettings = FindObjectOfType<VolumeSettings>();
        videoSettings = FindObjectOfType<VideoSettingsScript>();
        playerWalletScript = FindObjectOfType<PlayerWalletScript>();
        playerMovementScript = FindObjectOfType<MovementScript>();
        playerItemPickUpScript = FindObjectOfType<PlayerItemPickUpScript>();
        equipmentScript = FindObjectOfType<Equipment>();

        LoadGame();
    }

    public void SaveGame()
    {
        Debug.Log("Saving quest progress: " + JsonUtility.ToJson(QuestManagerScript.Instance.activateQuests, true));
        SaveDataScript saveData = new SaveDataScript
        {
            playerPosition = GameObject.Find("Player").transform.position,
            mapBoundary = FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D.name,
            inventorySaveData = inventoryManager.getInventoryItem(),
            currentDay = worldTime.getCurrentDay(),
            currentTimeTicks = worldTime.getCurrentTimeTicks(),
            questProgressData = QuestManagerScript.Instance.activateQuests,
            handinQuestsID = QuestManagerScript.Instance.handinQuests,
            playerMoney = playerWalletScript.playerMoney,
            bagLevel = equipmentScript.bagLevel,
            inventorySize = inventoryManager.inventorySize,
            footwearLevel = equipmentScript.footwearLevel,
            playerSpeed = playerMovementScript.movementSpeed,
            glovesLevel = equipmentScript.glovesLevel,
            pickUpSpeed = playerItemPickUpScript.holdDuration,
            toolLevel = equipmentScript.pickingToolLevel,
            pickUpRange = playerItemPickUpScript.pickUpRange
        };

        // videoSettings.SaveSettings();
        volumeSettings.LoadVolume();
        string json = JsonUtility.ToJson(saveData, true);
        Debug.Log("Saving JSON: " + json);
        File.WriteAllText(saveLocation, json);
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            string json = File.ReadAllText(saveLocation);
            Debug.Log("Loading JSON: " + json);
            SaveDataScript saveData = JsonUtility.FromJson<SaveDataScript>(json);

            GameObject.Find("Player").transform.position = saveData.playerPosition;
            GameObject.FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();
            inventoryManager.setInventoryItem(saveData.inventorySaveData);
            worldTime.setWorldTime(saveData.currentDay, saveData.currentTimeTicks);
            worldTime.StartWorldTime();
            QuestManagerScript.Instance.LoadQuestProgress(saveData.questProgressData);
            QuestManagerScript.Instance.handinQuests = saveData.handinQuestsID;
            playerWalletScript.playerMoney = saveData.playerMoney;

            equipmentScript.bagLevel = saveData.bagLevel;
            equipmentScript.glovesLevel = saveData.glovesLevel;
            equipmentScript.footwearLevel = saveData.footwearLevel;
            equipmentScript.pickingToolLevel = saveData.toolLevel;
            inventoryManager.inventorySize = saveData.inventorySize;
            inventoryManager.setInventorySize();
            playerMovementScript.movementSpeed = saveData.playerSpeed;
            playerItemPickUpScript.holdDuration = saveData.pickUpSpeed;
            if (playerItemPickUpScript.holdDuration <= 0f)
                playerItemPickUpScript.holdDuration = 1f;
            playerItemPickUpScript.pickUpRange = saveData.pickUpRange;
            playerItemPickUpScript.updatePickUpRange(saveData.pickUpRange);
            playerItemPickUpScript.updatePickUpSpeed(saveData.pickUpSpeed);

            Debug.Log("Game Loaded");
        }
        else
        {
            SaveGame();
            worldTime.StartWorldTime();
            inventoryManager.setInventorySize();
            Debug.Log("No save file found, creating a new one.");
        }
        volumeSettings.LoadVolume();
    }

    public void DeleteSave()
    {
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.Log("No save file to delete.");
        }
    }
}
