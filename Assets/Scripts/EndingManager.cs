using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class EndingManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private OldSceneDialogue targetDialogue;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject blackScreenPanel;
    [SerializeField] private TMP_Text endingText;

    [Header("Settings")]
    [SerializeField] private AudioClip firstSound;
    [SerializeField] private AudioClip secondSound;
    [SerializeField] private string endingMessage = "Конец игры";
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private float typingSpeed = 0.05f;

    private bool isEndingActive = false;
    private bool isTextComplete = false;

    private void Start()
    {
        blackScreenPanel.SetActive(false);
        endingText.text = "";
        targetDialogue.OnDialogueEnded += HandleDialogueEnded;
    }

    private void OnDestroy()
    {
        targetDialogue.OnDialogueEnded -= HandleDialogueEnded;
    }

    private void HandleDialogueEnded()
    {
        if (!isEndingActive)
        {
            isEndingActive = true;
            StartCoroutine(EndingSequence());
        }
    }

    private IEnumerator EndingSequence()
    {
        blackScreenPanel.SetActive(true);
        

        audioSource.PlayOneShot(firstSound);
        yield return new WaitForSeconds(firstSound.length);
    
        audioSource.PlayOneShot(secondSound);
        yield return StartCoroutine(TypeText(endingMessage));


        while (!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;
        }

        SceneManager.LoadScene(mainMenuScene);
    }

    private IEnumerator TypeText(string text)
    {
        endingText.text = "";
        isTextComplete = false;

        foreach (char letter in text.ToCharArray())
        {
            endingText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTextComplete = true;
    }
}
