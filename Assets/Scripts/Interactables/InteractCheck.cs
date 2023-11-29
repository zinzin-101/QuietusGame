using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class InteractCheck : MonoBehaviour
{
    public enum InteractType
    {
        Interact,
        Pickup,
        Both,
        None
    }

    [SerializeField] InteractType interactType;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerScript))
    //    {
    //        switch (interactType)
    //        {
    //            case InteractType.Interact:
    //                playerScript.SetActiveInteractPrompt(true);
    //                break;

    //            case InteractType.Pickup:
    //                playerScript.SetActivePickupPrompt(true);
    //                break;
    //        }
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerScript))
        {
            switch (interactType)
            {
                case InteractType.Interact:
                    playerScript.SetActiveInteractPrompt(true);
                    if (playerScript.InteractPressed)
                    {
                        playerScript.SetActiveInteractPrompt(false);
                    }
                    break;

                case InteractType.Pickup:
                    playerScript.SetActivePickupPrompt(true);
                    if (playerScript.PickupKeyPressed)
                    {
                        playerScript.SetActivePickupPrompt(false);
                    }
                    break;

                case InteractType.Both:
                    playerScript.SetActiveInteractPrompt(true);
                    playerScript.SetActivePickupPrompt(true);
                    break;

                case InteractType.None:
                    playerScript.SetActiveInteractPrompt(false);
                    playerScript.SetActivePickupPrompt(false);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerScript))
        {
            switch (interactType)
            {
                case InteractType.Interact:
                    playerScript.SetActiveInteractPrompt(false);
                    playerScript.SetActivePickupPrompt(false);
                    break;

                case InteractType.Pickup:
                    playerScript.SetActivePickupPrompt(false);
                    playerScript.SetActiveInteractPrompt(false);
                    break;

                case InteractType.Both:
                    playerScript.SetActiveInteractPrompt(false);
                    playerScript.SetActivePickupPrompt(false);
                    break;

                case InteractType.None:
                    playerScript.SetActiveInteractPrompt(false);
                    playerScript.SetActivePickupPrompt(false);
                    break;
            }
        }
    }

    public void ChangeInteractType(InteractType type)
    {
        interactType = type;
    }
}
