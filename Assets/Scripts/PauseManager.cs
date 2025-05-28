using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
    public static PauseManager Instance { get; private set; }

    [SerializeField] private GameObject pauseMenuUI;
    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Гарантируем, что меню паузы выключено при старте
        pauseMenuUI.SetActive(false);
        IsPaused = false;
        Time.timeScale = 1f;

;
    }

    private void Update()
    {
        if (GameInput.Instance.IsPausePressed())
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(IsPaused);
        Time.timeScale = IsPaused ? 0f : 1f;


    }

    public void ResumeGame()
    {
        TogglePause(); // Используем существующую логику
    }

    public void LoadGame()
    {
        Time.timeScale = 1f;
        // Здесь будет логика загрузки
        Debug.Log("Load game logic here");
    }

    public void OpenSettings()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SettingsScene");
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}