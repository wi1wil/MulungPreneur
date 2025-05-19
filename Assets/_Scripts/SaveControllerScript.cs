using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cinemachine;

public class SaveControllerScript : MonoBehaviour
{   
    private string saveLocation;
    private InventoryManagerScript inventoryManager;

    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryManager = FindObjectOfType<InventoryManagerScript>();

        LoadGame();
    }

    public void SaveGame()
    {
        SaveDataScript saveData = new SaveDataScript
        {
            playerPosition = GameObject.Find("Player").transform.position,
            mapBoundary = FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D.name,
            inventorySaveData = inventoryManager.getInventoryItem()
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

            Debug.Log("Game Loaded");
        }
        else
        {
            SaveGame();
            Debug.Log("No save file found, creating a new one.");
        }
    }
}
