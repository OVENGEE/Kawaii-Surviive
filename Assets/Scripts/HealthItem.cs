using System;
using UnityEngine;

public class HealthItem : MonoBehaviour, IItem
{
    public int healAmount = 1; // Amount of health this item restores
    public static event Action<int> OnHealthCollect;
    public void Collect()
    {
        OnHealthCollect.Invoke(healAmount); // Notify subscribers that health has been collected
        Destroy(gameObject); // Destroy the health item GameObject when collected
    }

}
