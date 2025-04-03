using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    [Header("Настройки")]
    [SerializeField] private float speed = 15f;    // Скорость пули
    [SerializeField] private float damage = 10f;  // Урон
    [SerializeField] private float lifetime = 3f; // Время жизни пули
    [SerializeField] private LayerMask collisionLayers;

    private Rigidbody2D rb;
    private Vector2 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed; // Задать скорость
        Destroy(gameObject, lifetime);   // Уничтожить через время
    }

    // Назначить направление (вызывается из EnemyAI)
    public void SetDirection(Vector2 targetDirection)
    {
        direction = targetDirection.normalized;

        // Поворот спрайта в направлении движения
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверка слоя через маску
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            // Нанести урон игроку
            if (other.CompareTag("Player"))
            {
                PlayerHealth player = other.GetComponent<PlayerHealth>();
                if (player != null) player.TakeDamage(damage);
            }

            Destroy(gameObject); // Уничтожить пулю
        }
    }
}