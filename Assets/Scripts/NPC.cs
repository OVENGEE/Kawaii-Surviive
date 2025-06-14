using System.Collections;
using System.Collections.Generic; // Importing System.Collections.Generic namespace for List<T>
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Importing Unity UI namespace for UI components

public class NPC : MonoBehaviour, Iinteractable
{
    public NPCDailouge dailougeData; // Reference to the NPCDailouge scriptable object
    private DialogueController dialougeUI; // Reference to the DailogueController instance
    private int dailougeIndex; // Index to track the current dailouge line
    private bool isTyping, isDailougeActive; // Flag to check if the dailouge is currently being typed}

    private enum QuestState
    {
        NotStarted,
        InProgress,
        Completed
    }

    private QuestState questState = QuestState.NotStarted; // State of the quest associated with the NPC

    public void Start()
    {
        dialougeUI = DialogueController.Instance; // Get the instance of the DailogueController
    }


    public bool CanInteract()
    {
        return !isDailougeActive; // Check if the dailouge is not active
    }

    public void Interact()
    {
        //if no dailouge data or the game is paused and no dailouge is active, return
        if (dailougeData == null || (PauseController.IsGamePaused && !isDailougeActive))
            return;
        if (isDailougeActive) // If dailouge is active, skip to the next line
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
        //sync quest state
        SyncQuestState(); // Synchronize the quest state with the dailouge data

        //Set dailougue line based on quest state
        if (questState == QuestState.NotStarted)
        {
            dailougeIndex = 0; // Set the dailouge index to the quest in progress index
        }
        else if (questState == QuestState.InProgress)
        {
            dailougeIndex = dailougeData.questInProgressIndex; // Set the dailouge index to the quest in progress index
        }
        else if (questState == QuestState.Completed)
        {
            dailougeIndex = dailougeData.questCompletedIndex; // Set the dailouge index to the quest completed index
        }

        isDailougeActive = true; // Set the dailouge as active

        dialougeUI.SetNPCInfo(dailougeData.npcName, dailougeData.npcPortrait); // Set the NPC name and portrait in the dailouge UI
        dialougeUI.ShowDailogueUI(true); // Show the dailouge UI
        PauseController.SetPause(true); // Pause the game

        //Type line
        DisplayCurrentLine();
    }

    private void SyncQuestState()
    {
        if (dailougeData.quest == null) return; // If no quest is associated, return
        string questID = dailougeData.quest.questID; // Get the quest ID from the dailouge data

        //future update add completing quest and giving quest
        if (QuestController.Instance.IsQuestCompleted(questID) || QuestController.Instance.IsQuestHandedIn(questID)) // Check if the quest is completed
        {
            questState = QuestState.Completed; // Set the quest state to Completed

        }
        else if (QuestController.Instance.IsQuestActive(questID)) // Check if the quest is already active
        {
            questState = QuestState.InProgress; // Set the quest state to InProgress    
        }
        else
        {
            questState = QuestState.NotStarted; // Set the quest state to NotStarted
        }
    }

    void Nextline()
    {
        if (isTyping)
        {
            StopAllCoroutines(); // Stop all coroutines if typing
            dialougeUI.SetDailougeText(dailougeData.dailougeLines[dailougeIndex]); // Clear the dailouge text
            isTyping = false; // Set typing flag to false
        }

        //Clear choices if any
        dialougeUI.ClearChoices(); // Clear any existing choices in the dailouge UI

        //Check end dailouge lines
        if (dailougeData.endDialougeLines.Length > dailougeIndex && dailougeData.endDialougeLines[dailougeIndex]) // Check if the current line is an end dailouge line
        {
            EndDailouge(); // End the dailouge
            return; // Exit the method
        }

        //Check for choices & display them
        foreach (DialogueChoice dialogueChoice in dailougeData.choices) // Iterate through each choice
        {
            if (dialogueChoice.dailougeIndex == dailougeIndex)
            {
                //display choices
                DisplayChoices(dialogueChoice); // Display the choices in the dailouge UI
                return; // Exit the method if choices are found
            }
        }


        if (++dailougeIndex < dailougeData.dailougeLines.Length) // Check if there are more lines
        {
            DisplayCurrentLine(); // Display the current line
        }
        else // If no more lines, end the dailouge
        {
            EndDailouge(); // End the dailouge
        }
    }

    IEnumerator Typeline()
    {
        isTyping = true; // Set typing flag to true
        dialougeUI.SetDailougeText(""); // Clear the dailouge text

        foreach (char letter in dailougeData.dailougeLines[dailougeIndex])
        {
            dialougeUI.SetDailougeText(dialougeUI.dailougeText.text += letter); // Update the dailouge text in the UI
            yield return new WaitForSeconds(dailougeData.typingSpeed); // Wait for the specified typing speed
        }
        isTyping = false; // Set typing flag to false

        if (dailougeData.autoProgressLines.Length > dailougeIndex && dailougeData.autoProgressLines[dailougeIndex]) // Check if there are auto progress lines
        {
            yield return new WaitForSeconds(dailougeData.autoProgressDelay); // Wait for the specified time
            Nextline(); // Move to the next line
        }
    }

    void DisplayChoices(DialogueChoice choice)
    {
        for (int i = 0; i < choice.choices.Length; i++) // Iterate through each choice
        {
            int nextIndex = choice.nextDailougeIndexes[i]; // Get the next dailouge index for the choice
            bool givesQuest = choice.givesQuest[i]; // Check if the choice gives a quest
            dialougeUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex, givesQuest)); // Create a button for the choice
        }
    }

    void ChooseOption(int nextIndex, bool givesQuest)
    {
        if (givesQuest) // If the choice gives a quest and the quest is not null
        {
            QuestController.Instance.AcceptQuest(dailougeData.quest); // Accept the quest
            questState = QuestState.InProgress; // Set the quest state to InProgress
        }
        dailougeIndex = nextIndex; // Set the dailouge index to the next index
        dialougeUI.ClearChoices(); // Clear any existing choices
        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines(); // Stop all coroutines
        StartCoroutine(Typeline()); // Start typing the current line
    }

    public void EndDailouge()
    {
        if (questState == QuestState.Completed && !QuestController.Instance.IsQuestHandedIn(dailougeData.quest.questID)) // If the quest is completed and not handed in
        {
            HandleQuestCompletion(dailougeData.quest); // Handle quest completion
        }

        StopAllCoroutines(); // Stop all coroutines
        isDailougeActive = false; // Set dailouge as inactive
        dialougeUI.SetDailougeText(""); // Clear the dailouge text
        dialougeUI.ShowDailogueUI(false); // Hide the dailouge panel
        PauseController.SetPause(false); // Unpause the game

    }

    void HandleQuestCompletion(Quest quest)
    {
        QuestController.Instance.HandInQuest(quest.questID); // Hand in the quest if it is completed
    }

}
