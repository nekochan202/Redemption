using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OldSceneDialogue : MonoBehaviour {
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image image;
    [SerializeField] private string[] dialogueLines;

    private int currentLine = 0;
    private bool isDialogueActive = false;
    private GameObject _triggerToDestroy;

    void Start()
    {
        dialoguePanel.SetActive(false);
        image.gameObject.SetActive(false);
    }

    void Update()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.IsPaused)
            return;

        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            ShowNextLine();
        }
    }


    public void SetTriggerToDestroy(GameObject trigger)
    {
        _triggerToDestroy = trigger;
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

        if (_triggerToDestroy != null)
        {
            Destroy(_triggerToDestroy);
        }
    }

}