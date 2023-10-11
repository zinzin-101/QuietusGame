using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] GameObject prompt;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput) && !DialogueManager.Instance.IsRunning)
        {
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            prompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (DialogueManager.Instance.IsRunning)
        {
            prompt.SetActive(false);
        }
    }
}
