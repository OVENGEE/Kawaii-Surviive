using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; } // Singleton instance of the DailogueController
    public GameObject dailougePanel; // Reference to the dailouge panel prefab
    public TMP_Text dailougeText, nameText; // Reference to the text component for the name
    public Image portraitImage; // Reference to the image component for the character portrait

    public Transform choiceContainer; // Reference to the container for dialogue choices
    public GameObject choiceButtonPrefab; // Prefab for the choice buttons

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // Ensure only one instance of DailogueController exists

    }

    public void ShowDailogueUI(bool show)
    {
        dailougePanel.SetActive(show); // Show or hide the dailouge panel based on the parameter
        /* if (Show)
         {
             PauseController.SetPause(true); // Pause the game when the dailouge is shown
         }
         else
         {
             PauseController.SetPause(false); // Resume the game when the dailouge is hidden
         }*/
    }

    public void SetNPCInfo(string npcName, Sprite portrait)
    {
        nameText.text = npcName; // Set the name text in the dailouge panel
        portraitImage.sprite = portrait; // Set the character portrait if provided
    }

    public void SetDailougeText(string text)
    {
        dailougeText.text = text; // Set the dailouge text in the dailouge panel
    }

    public void ClearChoices()
    {
        foreach (Transform child in choiceContainer) Destroy(child.gameObject); // Clear all existing choice buttons
        
    }

    public GameObject CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choicebutton = Instantiate(choiceButtonPrefab, choiceContainer); // Instantiate a new choice button
        choicebutton.GetComponentInChildren<TMP_Text>().text = choiceText; // Set the text of the button
        choicebutton.GetComponent<Button>().onClick.AddListener(onClick); // Set the text of the button
        return choicebutton; // Return the created choice button
        
    }
}
