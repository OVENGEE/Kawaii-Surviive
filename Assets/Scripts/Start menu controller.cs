using UnityEngine;

public class Startmenucontroller : MonoBehaviour
{   
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

   public void OnStartClick()
    {
        // Load the game scene (assuming it's named "GameScene")


        UnityEngine.SceneManagement.SceneManager.LoadScene("sampleScene");
        Debug.Log("Game is starting"); // Log message for debugging
    }
   public void OnQuitClick()
   {
        // Quit the application
       Application.Quit();
       #if UNITY_EDITOR
       UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
       #endif
       Application.Quit(); // Quit the application
       Debug.Log("Game is quitting"); // Log message for debugging
   }
     public void OnContinuedClick()
    {
        // Load the options scene (assuming it's named "OptionsScene")

         UnityEngine.SceneManagement.SceneManager.LoadScene("Exposition scene");
         Debug.Log("Options menu is opening"); // Log message for debugging
    }
}
