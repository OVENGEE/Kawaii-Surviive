using System.Collections.Generic;
using UnityEngine;

[System.Serializable]// This class is used to save the player's data, including their position and inventory.
// The [System.Serializable] attribute allows this class to be serialized, which means it can be converted to a format that can be saved to a file or sent over a network.
// I did a different explaination on another file
public class SaveData/// This class is used to save the player's data, including their position and inventory.
{
    public Vector3 playerPosition;// The player's position in the game world.
    public List<Inventorysavedata> inventorySaveData;// A list of items in the player's inventory.
    public List<StoryRockSaveData> storyRockSaveData;// A list of story rocks that the player has encountered.
}

[System.Serializable]// This class is used to save the data of an item in the player's inventory.
public class StoryRockSaveData
{
    public string StoryRockID;// The ID of the story rock.
    public bool isOpened;// A boolean indicating whether the story rock has been opened or not.
}