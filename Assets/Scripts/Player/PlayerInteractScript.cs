using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractScript : MonoBehaviour
{
    [SerializeField] KeyCode interactKey = KeyCode.E;

    private bool canInteract;

    private void Start()
    {
        canInteract = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(interactKey) && canInteract)
        {
            if (collision.gameObject.TryGetComponent(out TextInteract textInteract))
            {
                StartCoroutine(InteractCooldown());
                textInteract.ToggleActiveText();
            }

            if (collision.gameObject.TryGetComponent(out DialogueScript dialogueScript))
            {
                StartCoroutine(InteractCooldown());
                dialogueScript.TriggerDialogue();
            }
        }      
    }

    IEnumerator InteractCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(0.25f);
        canInteract = true;
    }
}
