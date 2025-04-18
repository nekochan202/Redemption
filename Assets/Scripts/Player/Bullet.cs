using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private LayerMask collisionLayers;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверка слоя через маску
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            //урон врагу
            if (other.CompareTag("Enemy"))
            {
                EnemyHealth enemy = other.GetComponent<EnemyHealth>();
                if (enemy != null) enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}