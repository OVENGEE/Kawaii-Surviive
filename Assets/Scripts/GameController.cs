using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressAmount;
    public Slider progressSlider;
    public GameObject gameOverScreen;
    public TMP_Text survivedText;
    private int survivedLevelsCount;

    public static event Action OnReset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progressAmount = 0;
        progressSlider.value = 0;
        Gem.OnGemCollect += IncreaseProgressAmount;

        PlayerHealth.OnPlayerDied += GameOverScreen;
        gameOverScreen.SetActive(false);
    }

    void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        progressSlider.value = progressAmount;
        if (progressAmount >= 100)
        {
            Debug.Log("Level completed!");
        }
    }

    void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        MusicManager.PausingBackgroundMusic();
        survivedText.text = "You survived " + survivedLevelsCount + " level!";
        if (survivedLevelsCount != 1) survivedText.text += "s";
        Time.timeScale = 0f; // Pause the game
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        MusicManager.PlayBackgroundMusic(true);
        survivedLevelsCount = 0;
        OnReset.Invoke();
        Time.timeScale = 1f; // Resume the game
    }
}
