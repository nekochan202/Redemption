using UnityEngine;

public class CheckpointTrigger : MonoBehaviour {
    [SerializeField] private DialogueManager dialogueManager; 
    [SerializeField] private OldSceneDialogue oldSceneDialogue; 
    [SerializeField] private bool useOldSystem = false; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HandleTrigger();
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void HandleTrigger()
    {
        if (useOldSystem)
        {
            oldSceneDialogue.SetTriggerToDestroy(gameObject);
            oldSceneDialogue.StartDialogue();
        }
        else
        {
            dialogueManager.StartDialogue();
        }
    }
}