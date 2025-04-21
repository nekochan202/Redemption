using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {
    [SerializeField] private GameObject dialoguePanel; 
    [SerializeField] private TMP_Text dialogueText;    
    [SerializeField] private Image image;         
    [SerializeField] private string[] dialogueLines;   
    [SerializeField] private string nextSceneName;     

    private int currentLine = 0;
    private bool isDialogueActive = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        image.gameObject.SetActive(false);
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
        image.gameObject.SetActive(true);
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
        image.gameObject.SetActive(false);
        dialogueText.text = "";
        Time.timeScale = 1;
       
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            DataManager.Instance.PlayerHealth = (int)playerHealth.CurrentHealth;
            DataManager.Instance.MedKits = playerHealth.CurrentMedKits;
        }

        Player player = Player.Instance;
        if (player != null)
        {
            DataManager.Instance.CurrentAmmo = player.CurrentAmmo;
            DataManager.Instance.TotalAmmo = player.TotalAmmo;
        }

        SceneManager.LoadScene(nextSceneName);
    }

    public void SetTriggerToDestroy(GameObject trigger)
    {
        Destroy(trigger);
    }
}