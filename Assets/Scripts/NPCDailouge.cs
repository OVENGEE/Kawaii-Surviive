//Title: Unity Manual – class ScriptableObject
//Author: Unity Technologies
//Date: 24 April 2025
//Code Version: 6000.1
//Availability: https://docs.unity3d.com/Manual/class-ScriptableObject.html

using UnityEngine;
using System.Collections; // Importing System.Collections namespace for IEnumerator
using System.Collections.Generic; // Importing System.Collections.Generic namespace for List<T>

[CreateAssetMenu(fileName = "NewNPCDailouge", menuName = "NPC Dailouge")]// Creating a new menu item in Unity's Create Asset Menu
public class NPCDailouge : ScriptableObject// Inheriting from ScriptableObject to create a custom asset type
{
    public string npcName;// Name of the NPC
    public Sprite npcPortrait; // Portrait of the NPC

    public string[] dailougeLines; // Array of dailouge lines} 

    public float typingSpeed = 0.05f; // Speed of typing effect

    public bool[] autoProgressLines;// Array of booleans for auto-progressing lines
    public bool[] endDialougeLines; // Array of booleans for end dailouge lines, Mark where dailouge ends

    public float autoProgressDelay = 1.5f; // Array of booleans for auto-progressing lines with delay

    public DialogueChoice[] choices; // Array of dialogue choices

    public int questInProgressIndex; // What does he say while quest is in progess
    public int questCompletedIndex; // What does he say when quest is completed
    public Quest quest; // Quest to give when a choice is made
}

[System.Serializable]

public class DialogueChoice
{
    public int dailougeIndex; //Dailogue line where chioces appear
    public string[] choices; // Player response options
    public int[] nextDailougeIndexes; // Where the choices lead to
    public bool[] givesQuest; // Indicates if a choice gives a quest
}