using System;
using UnityEngine;

public class Gem : MonoBehaviour, IItem
{
    public static event Action<int> OnGemCollect; // Event to notify when a gem is collected
    public int worth = 5;
    public void Collect()
    {
        OnGemCollect.Invoke(worth); // Invoke the event with the gem's worth
        SoundEffectsManager.Play("Gem"); // Play the sound effect for collecting a gem
        Destroy(gameObject); // Destroy the gem GameObject when collected
    }


}
