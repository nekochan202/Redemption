using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    // Метод для кнопки "Играть"
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // Метод для кнопки "Загрузить"
    public void LoadGame()
    {
        
       
    }

    // Метод для кнопки "Настройки"
    public void OpenSettings()
    {
        
        SceneManager.LoadScene("SettingsScene"); 
    }

    // Метод для кнопки "Выход"
    public void ExitGame()
    {
        Application.Quit();

        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}