//Title: Unity Script Reference – Search: Save System
//Author: Unity Technologies
//Date: 21 April 2025
//Code Version: 6000.1
//Availability: https://docs.unity3d.com/ScriptReference/30_search.html?q=save+system

//Title: .NET API Reference – System.IO.Path.Combine
//Author: Microsoft
//Date: 22 April 2025
//Code Version: .NET 9.0
//Availability: https://learn.microsoft.com/en-us/dotnet/api/system.io.path.combine?view=net-9.0

//Title: Unity Script Reference – Search: GameObject.FindGameObjectsWithTag
//Author: Unity Technologies
//Date: 21 April 2025
//Code Version: 6000.1
//Availability: https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.FindGameObjectsWithTag.html

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private Inventorycontroller inventorycontroller;// private StoryRocks storyRocks;
    private StoryRocks[] storyRocks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeComponents();//This method initializes the components of the script.
        
        LoadGame();//This method loads the game data from the save file.
    }

    private void InitializeComponents()//This method initializes the components of the script.
    {
        //Define save location
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");//Path.Combine is used to combine two strings into a single path string.
        inventorycontroller = FindAnyObjectByType<Inventorycontroller>();//FindAnyObjectByType is a method that finds any object of the specified type in the scene.
        storyRocks = FindObjectsByType<StoryRocks>(FindObjectsSortMode.None);

    }

    public void SaveGame()//This method saves the game data to a file
    {
        SaveData saveData = new SaveData//This is a class that holds the data to be saved.
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,//This line finds the player object in the scene and gets its position.
            inventorySaveData = inventorycontroller.GetInventoryItems(),//This line gets the inventory items from the inventory controller.
            storyRockSaveData = GetStoryRocksState(),
            questProgressData = QuestController.Instance.activeQuests,
            handinQuestIDs = QuestController.Instance.handinQuestIDs
        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));//This line converts the saveData object to a JSON string and writes it to the save file.
    }

    private List<StoryRockSaveData> GetStoryRocksState()
    {
        List<StoryRockSaveData> storyRocksStates = new List<StoryRockSaveData>();

        foreach(StoryRocks storyRock in storyRocks)
        {
            StoryRockSaveData storyRockSaveData = new StoryRockSaveData
            {
                StoryRockID = storyRock.StoryRocksID,
                isOpened = storyRock.IsOpened
            };
            storyRocksStates.Add(storyRockSaveData);
        }
        return storyRocksStates;
    }

    public void LoadGame()//This method loads the game data from a file
    {
        if (File.Exists(saveLocation))//This line checks if the save file exists.
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));//This line reads the save file and converts the JSON string back to a SaveData object.

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;//This line sets the player position to the saved position.

            inventorycontroller.SetInventoryItems(saveData.inventorySaveData); //This line sets the inventory items in the inventory controller.

            //Loadcheststate
            LoadChestStates(saveData.storyRockSaveData);

            QuestController.Instance.LoadQuestProgress(saveData.questProgressData);//This line loads the quest progress data from the save file.
            QuestController.Instance.handinQuestIDs = saveData.handinQuestIDs;//This line sets the handin quest IDs in the quest controller.

        }
        else
        {
            SaveGame();//This line saves the game data if the save file does not exist.
            inventorycontroller.SetInventoryItems(new List<InventorySaveData>()); //This line initializes the inventory with an empty list if the save file does not exist.
        }
    }

  private void LoadChestStates(List<StoryRockSaveData> storyRocksStates)
    {
        foreach(StoryRocks storyRock in storyRocks)
        {
            StoryRockSaveData storyRockSaveData = storyRocksStates.FirstOrDefault(c => c.StoryRockID == storyRock.StoryRocksID);
           if(storyRockSaveData != null)
           {
                storyRock.SetOpened(storyRockSaveData.isOpened);
           }
        }
    }
}
