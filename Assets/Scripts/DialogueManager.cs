using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {
    [SerializeField] private GameObject dialoguePanel; // Панель диалога
    [SerializeField] private TMP_Text dialogueText;    // Текст для фраз
    [SerializeField] private Image Image;         // Изображение 
    [SerializeField] private string[] dialogueLines;   // Массив фраз
    [SerializeField] private string nextSceneName;

    private int currentLine = 0;
    private bool isDialogueActive = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        Image.gameObject.SetActive(false);
       
    }

    void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            ShowNextLine();
        }
    }

    public void StartDialogue()
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        Image.gameObject.SetActive(true);
        currentLine = 0;
        dialogueText.text = dialogueLines[currentLine];
        Time.timeScale = 0;
    }

    private void ShowNextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        Image.gameObject.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(nextSceneName);
    }
}