using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invincibilityTime = 1f; 
    [SerializeField] private GameObject deathEffect;

    [Header("Medkits")]
    [SerializeField] private int maxMedKits = 3;
    [SerializeField] private float healAmount = 25f;
    private int currentMedKits;

    [Header("Death Screen")]
    [SerializeField] private GameObject deathScreen; 
    private bool isDead = false;

    public int CurrentMedKits => currentMedKits;
    public int MaxMedKits => maxMedKits;
    public static event System.Action<int, int> OnMedKitsChanged;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    private float currentHealth;
    private bool isInvincible = false;

    public delegate void HealthChangedDelegate(float health);
    public static event HealthChangedDelegate OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentMedKits = 0; 
        OnMedKitsChanged?.Invoke(currentMedKits, maxMedKits); 
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void Start()
    {
        if (DataManager.Instance.PlayerHealth > 0)
        {
            currentHealth = DataManager.Instance.PlayerHealth;
        }
        if (DataManager.Instance.MedKits > 0)
        {
            currentMedKits = DataManager.Instance.MedKits;
        }


        OnHealthChanged?.Invoke(currentHealth);
        OnMedKitsChanged?.Invoke(currentMedKits, maxMedKits);

        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible || isDead) return;

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
        if (isDead) return;
        isDead = true;

        Transform visualTransform = transform.Find("PlayerVisual");
        if (visualTransform != null)
        {
            visualTransform.gameObject.SetActive(false);
        }

        GetComponent<Player>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);

        if (deathScreen != null) deathScreen.SetActive(true);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    public bool AddMedKit(int amount)
    {
        if (currentMedKits >= maxMedKits) return false;

        currentMedKits = Mathf.Min(currentMedKits + amount, maxMedKits);
        OnMedKitsChanged?.Invoke(currentMedKits, maxMedKits); 
        return true;
    }

    public void UseMedKit()
    {
        if (currentMedKits > 0 && currentHealth < maxHealth)
        {
            Heal(healAmount);
            currentMedKits--;
            OnMedKitsChanged?.Invoke(currentMedKits, maxMedKits);
            DataManager.Instance.PlayerHealth = (int)currentHealth;
        }
    }
    private void Update()
    {
        // Проверка на рестарт после смерти
        if (isDead && Input.GetKeyDown(KeyCode.F))
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        DataManager.Instance.ResetToInitialValues();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}