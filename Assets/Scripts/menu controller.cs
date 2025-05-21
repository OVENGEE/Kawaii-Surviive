using UnityEngine;

public class menucontroller : MonoBehaviour
{
    public GameObject menuCanvas;// Reference to the menu canvas
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);// Hide the menu canvas at the start
    }
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))// Check if the Escape key is pressed
        {
            audioManager.PlaySFX(audioManager.MenuPopup);
            if (!menuCanvas.activeSelf && PauseController.IsGamePaused)// Check if the menu is not active and the game is paused
            {
                return;
            }
            menuCanvas.SetActive(!menuCanvas.activeSelf);// Toggle the menu canvas visibility
            PauseController.SetPause(menuCanvas.activeSelf);// Set the game pause state based on the menu visibility
        }
    }
}
