using System.Collections;
using System.Collections.Generic; // Importing System.Collections.Generic namespace for List<T>
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Importing Unity UI namespace for UI components

public class NPC : MonoBehaviour, Iinteractable
{
    public NPCDailouge dailougeData; // Reference to the NPCDailouge scriptable object
    public GameObject dailougePanel; // Reference to the dailouge panel prefab
    public TMP_Text nameText; // Reference to the text component for the name
    public TMP_Text dailougeText; // Reference to the text component for the dailouge text

    private int dailougeIndex ; // Index to track the current dailouge line
    private bool isTyping, isDailougeActive ; // Flag to check if the dailouge is currently being typed}

    public bool CanInteract()
    {
        return !isDailougeActive; // Check if the dailouge is not active
    }

    public void Interact()
    {
        //if no dailouge data or the game is paused and no dailouge is active, return
        if (dailougeData == null || (PauseController.IsGamePaused && !isDailougeActive))
            return;
        if(isDailougeActive) // If dailouge is active, skip to the next line
        {
            Nextline(); // Call the Nextline method to progress the dailouge
        }
        else // If dailouge is not active, start the dailouge
        {
             StartDailouge(); // Call the StartDailouge method to begin the dailouge
        }
    }
    void StartDailouge()
    {
        dailougeIndex = 0; // Reset the dailouge index
        isDailougeActive = true; // Set the dailouge as active

        nameText.SetText(dailougeData.npcName); // Set the name text to the first line's name
        
        dailougePanel.SetActive(true); // Show the dailouge panel
        PauseController.SetPause(true); // Pause the game

        //Type line
        StartCoroutine(Typeline()); // Start the typing coroutine
    }

    void Nextline()
    {
        if(isTyping)
        {
            StopAllCoroutines(); // Stop all coroutines if typing
            dailougeText.SetText(dailougeData.dailougeLines[dailougeIndex]); // Set the dailouge text to the current line
            isTyping = false; // Set typing flag to false
        }
        else if(++dailougeIndex < dailougeData.dailougeLines.Length) // Check if there are more lines
        {
            dailougeIndex++; // Move to the next line
            StartCoroutine(Typeline()); // Start typing the next line
        }
        else // If no more lines, end the dailouge
        {
            EndDailouge(); // End the dailouge
        }
    }

    IEnumerator Typeline()
    {
        isTyping = true; // Set typing flag to true
        dailougeText.SetText(""); // Clear the dailouge text

        foreach(char letter in dailougeData.dailougeLines[dailougeIndex])
        {
            dailougeText.text += letter; // Add each letter to the dailouge text
            yield return new WaitForSeconds(dailougeData.typingSpeed); // Wait for the specified typing speed
        }
        isTyping = false; // Set typing flag to false

        if(dailougeData.autoProgressLines.Length > dailougeIndex && dailougeData.autoProgressLines[dailougeIndex]) // Check if there are auto progress lines
        {
            yield return new WaitForSeconds(dailougeData.autoProgressDelay); // Wait for the specified time
            Nextline(); // Move to the next line
        }
    }

    public void EndDailouge()
    {
        StopAllCoroutines(); // Stop all coroutines
        dailougeText.SetText(""); // Clear the dailouge text
        dailougePanel.SetActive(false); // Hide the dailouge panel
        isDailougeActive = false; // Set dailouge as inactive
        PauseController.SetPause(false); // Unpause the game

    }

}
