using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteractScript : MonoBehaviour
{
    [SerializeField] KeyCode interactKey = KeyCode.E;
    [SerializeField] KeyCode itemPickUpKey = KeyCode.F;

    private bool canInteract;
    private bool interactPressed;

    public bool CanInteract => canInteract;
    public bool InteractPressed => interactPressed;

    private bool pickupKeyPressed;
    public bool PickupKeyPressed => pickupKeyPressed;

    private void Start()
    {
        canInteract = true;
        interactPressed = false;
    }

    private void Update()
    {
        if (Input.GetKey(interactKey))
        {
            interactPressed = true;
        }
        else
        {
            interactPressed = false;
        }

        if (Input.GetKey(itemPickUpKey))
        {
            pickupKeyPressed = true;
        }
        else
        {
            pickupKeyPressed = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (interactPressed && canInteract)
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

    public IEnumerator InteractCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(0.25f);
        canInteract = true;
    }
}
