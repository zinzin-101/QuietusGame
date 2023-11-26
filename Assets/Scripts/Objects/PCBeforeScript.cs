using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCBeforeScript : MonoBehaviour
{
    [SerializeField] GameObject CompletedTable;


    [SerializeField] GameObject before;
    [SerializeField] GameObject keyboard, pc, mouse;
    private Collider2D col;

    [SerializeField] Animator animator;

    [SerializeField] Item keyboardItem, pcItem, mouseItem;

    [SerializeField] Dialogue missingDialogue;

    private void Awake()
    {
        TryGetComponent(out col);

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator != null)
        {
            animator.SetBool("IsHighlight", false);
        }

        before.SetActive(true);
        keyboard.SetActive(false);
        pc.SetActive(false);
        mouse.SetActive(false);
    }

    void CheckItem()
    {
        if (!before.activeSelf)
        {
            return;
        }

        bool found = false;

        foreach (Item item in InventoryManager.Instance.Items)
        {
            if (item == keyboardItem)
            {
                InventoryManager.Instance.Remove(item);
                keyboard.SetActive(true);
                found = true;
                break;
            }

            if (item == pcItem)
            {
                InventoryManager.Instance.Remove(item);
                pc.SetActive(true);
                found = true;
                break;
            }

            if (item == mouseItem)
            {
                InventoryManager.Instance.Remove(item);
                mouse.SetActive(true);
                found = true;
                break;
            }
        }

        if (!found)
        {
            DialogueManager.Instance.StartDialogue(missingDialogue, true);
        }

        if (!keyboard.activeSelf || !pc.activeSelf || !mouse.activeSelf)
        {
            return;
        }

        before.SetActive(false);
        CompletedTable.SetActive(true);
        col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput input))
        {
            if (animator != null)
            {
                animator.SetBool("IsHighlight", true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript interactScript))
        {
            if (interactScript.InteractPressed && interactScript.CanInteract)
            {
                StartCoroutine(interactScript.InteractCooldown());
                CheckItem();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput input))
        {
            if (animator != null)
            {
                animator.SetBool("IsHighlight", false);
            }
        }
    }
}
