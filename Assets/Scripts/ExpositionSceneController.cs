using UnityEngine;

public class ExpositionSceneController : MonoBehaviour
{
    public void OnStartClick()
   {
       // Load the game scene (assuming it's named "GameScene")
       UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
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

}
