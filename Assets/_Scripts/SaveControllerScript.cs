using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cinemachine;

public class SaveControllerScript : MonoBehaviour
{   
    private string saveLocation;
    private InventoryManagerScript inventoryManager;
    private WorldTime worldTime;

    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryManager = FindObjectOfType<InventoryManagerScript>();
        worldTime = FindObjectOfType<WorldTime>();

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
            questProgressData = QuestManagerScript.Instance.activateQuests
        };

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
            Debug.Log("Loaded quest progress: " + JsonUtility.ToJson(QuestManagerScript.Instance.activateQuests, true));

            Debug.Log("Game Loaded");
        }
        else
        {
            SaveGame();
            worldTime.StartWorldTime();
            inventoryManager.setInventorySize();
            Debug.Log("No save file found, creating a new one.");
        }
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
