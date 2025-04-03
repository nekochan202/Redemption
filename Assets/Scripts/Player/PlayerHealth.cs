using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invincibilityTime = 1f; // ����� ������������ ����� �����
    [SerializeField] private GameObject deathEffect;

    private float currentHealth;
    private bool isInvincible = false;

    // ������� ��� ���������� UI (�����������)
    public delegate void HealthChangedDelegate(float health);
    public static event HealthChangedDelegate OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth); // �������� UI ��� ������
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth); // �������� UI

        // �������� �� ������
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
            // �������������� ������� (��������, �������)
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
        // ��������� ����������
        GetComponent<Player>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // ������ ������
        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);

        // ������������ ����� ��� ����� Game Over
        Debug.Log("����� ����!");
    }

    // ��� ������� (�����������)
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }
}
