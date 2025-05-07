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

    public string[] dailougeLines; // Array of dailouge lines} 

    public float typingSpeed = 0.05f; // Speed of typing effect

    public bool[] autoProgressLines;// Array of booleans for auto-progressing lines

    public float autoProgressDelay = 1.5f; // Array of booleans for auto-progressing lines with delay
}