using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour {
    [SerializeField] private Image ammoIcon;
    [SerializeField] private TMP_Text ammoCountText;

    private void Start()
    {
        Player.OnAmmoChanged += UpdateUI;
        UpdateUI(Player.Instance.CurrentAmmo, Player.Instance.TotalAmmo);
    }

    private void OnDestroy()
    {
        Player.OnAmmoChanged -= UpdateUI;
    }

    private void UpdateUI(int current, int total)
    {
        ammoCountText.text = $"{current}/{total}";
    }
}