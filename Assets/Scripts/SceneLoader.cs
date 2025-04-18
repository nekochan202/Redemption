using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    [SerializeField] private string Level2; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(Level2);

        }
    }

    private void SaveData()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        DataManager.Instance.PlayerHealth = (int)playerHealth.CurrentHealth;
        DataManager.Instance.MedKits = playerHealth.CurrentMedKits;
        DataManager.Instance.CurrentAmmo = Player.Instance.CurrentAmmo;
        DataManager.Instance.TotalAmmo = Player.Instance.TotalAmmo;
    }
}