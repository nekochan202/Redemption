using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invincibilityTime = 1f; // Время неуязвимости после удара
    [SerializeField] private GameObject deathEffect;

    private float currentHealth;
    private bool isInvincible = false;

    // Событие для обновления UI (опционально)
    public delegate void HealthChangedDelegate(float health);
    public static event HealthChangedDelegate OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth); // Обновить UI при старте
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth); // Обновить UI

        // Проверка на смерть
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
            // Дополнительные эффекты (например, мигание)
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
        // Отключить управление
        GetComponent<Player>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Эффект смерти
        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Перезагрузка сцены или вывод Game Over
        Debug.Log("Игрок умер!");
    }

    // Для лечения (опционально)
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }
}
