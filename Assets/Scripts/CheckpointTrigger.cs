using UnityEngine;

public class CheckpointTrigger : MonoBehaviour {
    [SerializeField] private DialogueManager dialogueManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.StartDialogue();
            GetComponent<Collider2D>().enabled = false; 
        }
    }
}
