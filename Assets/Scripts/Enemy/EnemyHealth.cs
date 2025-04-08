using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private GameObject deathEffect;

    private float currentHealth;
    private EnemyAI enemyAI; 

    private void Awake()
    {
        currentHealth = maxHealth;
        enemyAI = GetComponent<EnemyAI>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Доп эффекты
        }
    }

    private void Die()
    {
        
        if (enemyAI != null) enemyAI.enabled = false;
        GetComponent<Collider2D>().enabled = false;

       
        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);

       
        Destroy(gameObject);
    }

    public bool IsDead() => currentHealth <= 0;
}