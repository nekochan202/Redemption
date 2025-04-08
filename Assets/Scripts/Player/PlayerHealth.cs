using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invincibilityTime = 1f; // ¬рем€ неу€звимости после удара
    [SerializeField] private GameObject deathEffect;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    private float currentHealth;
    private bool isInvincible = false;

    // —обытие дл€ обновлени€ UI (опционально)
    public delegate void HealthChangedDelegate(float health);
    public static event HealthChangedDelegate OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth); 
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth); 

        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    private void Die()
    {
        // ќтключить управление
        GetComponent<Player>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Ёффект смерти
        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);

        Debug.Log("»грок умер!");
    }

    // ƒл€ лечени€
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }
}
