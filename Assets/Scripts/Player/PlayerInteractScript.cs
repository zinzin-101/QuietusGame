using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteractScript : MonoBehaviour
{
    public enum InteractType
    {
        Interact,
        Pickup
    }

    [SerializeField] KeyCode interactKey = KeyCode.E;
    [SerializeField] KeyCode itemPickUpKey = KeyCode.F;

    private bool canInteract;
    private bool interactPressed;

    public bool CanInteract => canInteract;
    public bool InteractPressed => interactPressed;

    private bool pickupKeyPressed;
    public bool PickupKeyPressed => pickupKeyPressed;

    private MovementScript movementScript;
    private bool isSitting = false;
    public bool IsSitting => isSitting;

    private void Awake()
    {
        TryGetComponent(out movementScript);
    }

    private void Start()
    {
        canInteract = true;
        interactPressed = false;
        isSitting = false;
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
        if (collision.gameObject.TryGetComponent(out PallorMortisScript pallorScript))
        {
            if (isSitting)
            {
                pallorScript.TriggerFirstDialogue();
            }
        }

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

                if (collision.gameObject.TryGetComponent(out ChairScript chairScript))
            {
                if (chairScript.CurrentState != ChairScript.ChairState.Down)
                {
                    StartCoroutine(InteractCooldown());
                    switch (isSitting)
                    {
                        case true:
                            if (GameManager.Instance.PlayerCanSit)
                            {
                                transform.position = chairScript.transform.position;
                                chairScript.ToggleCollision(true);
                                chairScript.ChangeChairState(ChairScript.ChairState.Upfront);
                                isSitting = false;
                                movementScript.SetToggleMove(true);
                                GameManager.Instance.TimerActive(true);
                            }                          
                            break;

                        case false:
                            transform.position = chairScript.SittingPos.position;
                            movementScript.SetRigidbodyVelocity(Vector3.zero);
                            chairScript.ToggleCollision(false);
                            chairScript.ChangeChairState(ChairScript.ChairState.Upside);
                            isSitting = true;
                            movementScript.SetToggleMove(false);
                            GameManager.Instance.TimerActive(false);
                            break;
                    }
                }
            }
                
            //if (pallorScript != null)
            //{
            //    //pallorScript.PlayPhaseDialogue();
            //}           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out KeyMasterDetect keymasterScript) && keymasterScript.CanSelect)
        {
            InventoryManager.Instance.SetActiveKeyMasterItem(!InventoryManager.Instance.IsKeyMasterItemActive());
            InventoryManager.Instance.OpenKeymasterItem();
            keymasterScript.StartCoroutine(keymasterScript.SelectItem());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out KeyMasterDetect keymasterScript))
        {
            InventoryManager.Instance.SetActiveKeyMasterItem(false);
            keymasterScript.StopCoroutine(keymasterScript.SelectItem());
        }
    }

    public IEnumerator InteractCooldown()
    {
        canInteract = false;

        yield return new WaitForSeconds(0.15f); // for now use this

        // doesn't work as debounce
        //switch (type) 
        //{
        //    case InteractType.Interact:
        //        yield return new WaitUntil(() => Input.GetKeyUp(interactKey));
        //        break;

        //    case InteractType.Pickup:
        //        yield return new WaitUntil(() => Input.GetKeyUp(itemPickUpKey));
        //        break;
        //}

        canInteract = true;
    }
}
