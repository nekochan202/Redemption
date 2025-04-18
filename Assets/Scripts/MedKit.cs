using UnityEngine;

public class MedKit : MonoBehaviour {
    [SerializeField] private int healAmount = 25;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth.AddMedKit(1))
            {
                Destroy(gameObject);
            }
        }
    }
}