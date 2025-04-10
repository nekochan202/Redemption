using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MedKitUI : MonoBehaviour {
    [SerializeField] private Image medKitIcon;
    [SerializeField] private TMP_Text medKitCountText;

    private void Start()
    {
        PlayerHealth.OnMedKitsChanged += UpdateUI;
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        UpdateUI(playerHealth.CurrentMedKits, playerHealth.MaxMedKits);
    }

    private void OnDestroy()
    {
        PlayerHealth.OnMedKitsChanged -= UpdateUI;
    }

    private void UpdateUI(int current, int max)
    {
        medKitCountText.text = $"{current}/{max}";
    }
}
