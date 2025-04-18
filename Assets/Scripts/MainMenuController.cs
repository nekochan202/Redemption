using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public void PlayGame()
    {
        SceneManager.LoadScene("TestLVL");
    }

    public void LoadGame()
    {
        
       
    }
 
    public void OpenSettings()
    {
        
        SceneManager.LoadScene("SettingsScene"); 
    }
  
    public void ExitGame()
    {
        Application.Quit();

        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}