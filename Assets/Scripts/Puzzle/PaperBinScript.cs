using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBinScript : MonoBehaviour
{
    [SerializeField] Dialogue hint;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerInteractScript))
        {
            if (playerInteractScript.InteractPressed && playerInteractScript.CanInteract)
            {
                StartCoroutine(playerInteractScript.InteractCooldown());
                DialogueManager.Instance.StartDialogue(hint, true);
            }
        }
    }
}

