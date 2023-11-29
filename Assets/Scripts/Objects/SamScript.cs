using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamScript : MonoBehaviour
{
    [SerializeField] Dialogue dialogueBefore, dialogueAfter, dialogueAfterAfter;  
    [SerializeField] Item requiredItem;

    private bool finished;
    private void Start()
    {
        GameManager.Instance.AllowBonsaiPickup(false);
        finished = false;
    }

    private void TriggerDialogue(Dialogue dialogue)
    {
        DialogueManager.Instance.StartDialogue(dialogue, true);
    }

    private bool CheckItem()
    {
        foreach (Item item in InventoryManager.Instance.Items)
        {
            if (item == requiredItem)
            {
                InventoryManager.Instance.Remove(item);
                return true;
            }
        }
        return false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerInteractScript))
        {
            if (playerInteractScript.InteractPressed && playerInteractScript.CanInteract && !playerInteractScript.IsSitting)
            {
                StartCoroutine(playerInteractScript.InteractCooldown());

                switch (finished)
                {
                    case true:
                        TriggerDialogue(dialogueAfterAfter);
                        GameManager.Instance.NextRoomButton(true);
                        break;

                    case false:
                        if (CheckItem())
                        {
                            finished = true;
                            GameManager.Instance.AllowBonsaiPickup(true);
                            //GameManager.Instance.EnableSkip(true);
                            TriggerDialogue(dialogueAfter);
                            GameManager.Instance.PallorScript.NextAll();
                        }
                        else
                        {
                            TriggerDialogue(dialogueBefore);
                        }
                        break;
                }
            }
        }
    }
}