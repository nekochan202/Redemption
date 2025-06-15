using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TriggerLight2D : MonoBehaviour {
    [Header("Настройки света")]
    public Light2D targetLight;

    [Header("Настройки триггера")]
    public string triggerTag = "Player";

    private void Start()
    {
        if (targetLight != null)
        {
            targetLight.enabled = false;
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            if (targetLight != null)
            {
                targetLight.enabled = true;
            }

        }
    }
}