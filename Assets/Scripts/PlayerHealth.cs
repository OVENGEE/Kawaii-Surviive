using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public HealthUI healthUI;
    public static event Action OnPlayerDied;
    private SpriteRenderer spriteRenderer;

    private float damageCooldown = 3f;
    private float lastDamageTime = -Mathf.Infinity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetHealth();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameController.OnReset += ResetHealth;

        HealthItem.OnHealthCollect += Heal; // Subscribe to the health item collection event
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                TakeDamage(enemy.damage);
                lastDamageTime = Time.time;
            }
        }

    }

    void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Ensure health does not exceed maxHealth
        }
        healthUI.UpdateHearts(currentHealth);
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);

        StartCoroutine(FlashRed());
        if (currentHealth <= 0)
        {
            //player dead! --- call game over, animation, etc.
            OnPlayerDied.Invoke();
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;

    }
}
