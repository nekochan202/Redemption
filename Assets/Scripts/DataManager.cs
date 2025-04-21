using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public int PlayerHealth { get; set; }
    public int MedKits { get; set; }
    public int CurrentAmmo { get; set; }
    public int TotalAmmo { get; set; }

    public int InitialMedKits { get; private set; }
    public int InitialCurrentAmmo { get; private set; }
    public int InitialTotalAmmo { get; private set; }
    private string currentSceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != currentSceneName)
        {
            currentSceneName = scene.name;
            InitialMedKits = MedKits;
            InitialCurrentAmmo = CurrentAmmo;
            InitialTotalAmmo = TotalAmmo;
        }
    }

    public void ResetToInitialValues()
    {
        MedKits = InitialMedKits;
        CurrentAmmo = InitialCurrentAmmo;
        TotalAmmo = InitialTotalAmmo;
        PlayerHealth = (int)FindObjectOfType<PlayerHealth>().MaxHealth;
    }
}