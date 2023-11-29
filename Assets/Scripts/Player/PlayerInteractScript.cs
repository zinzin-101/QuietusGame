using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] GameObject interactPrompt;
    [SerializeField] GameObject pickupPrompt;

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

        interactPrompt.SetActive(false);
        pickupPrompt.SetActive(false);
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

        if (collision.TryGetComponent(out KeyMasterDetect keymasterScript))
        {
            if (isSitting && keymasterScript.CanInteract)
            {
                keymasterScript.SetCanInteract(false);
                keymasterScript.CheckItem();
            }
        }

        if (collision.TryGetComponent(out DigiClockScript digiclock))
        {
            if (!digiclock.Grouped)
            {
                if (isSitting)
                {
                    digiclock.Interact();
                }
                else if (!isSitting)
                {
                    digiclock.UnInteract();
                }
            }
        }

        if (interactPressed && canInteract)
        {
            if (collision.gameObject.TryGetComponent(out BonsaiBearScript bear))
            {
                bear.PlayBonsaiDialogue();
            }

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

            if (digiclock != null && digiclock.Grouped)
            {
                StartCoroutine(InteractCooldown());
                
                switch (digiclock.Showing)
                {
                    case true:
                        digiclock.UnInteract();
                        break;
                    case false:
                        digiclock.Interact();
                        break;
                }
            }

            if (collision.gameObject.TryGetComponent(out QuinnScript quinn))
            {
                StartCoroutine(InteractCooldown());
                quinn.SetCan(true);
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
                                //movementScript.SetToggleMove(true);
                                GameManager.Instance.AllowPlayerToMove(true);
                                //GameManager.Instance.TimerActive(true);
                            }                          
                            break;

                        case false:
                            transform.position = chairScript.SittingPos.position;
                            movementScript.SetRigidbodyVelocity(Vector3.zero);
                            chairScript.ToggleCollision(false);
                            chairScript.ChangeChairState(ChairScript.ChairState.Upside);
                            isSitting = true;
                            //movementScript.SetToggleMove(false);
                            GameManager.Instance.AllowPlayerToMove(false);
                            //GameManager.Instance.TimerActive(false);
                            break;
                    }
                }
            }

            if (digiclock != null && !digiclock.Grouped)
            {
                if (!isSitting)
                {
                    DialogueManager.Instance.StartDialogue(digiclock.TooHigh, true);
                }
            }

            //if (collision.TryGetComponent(out KeyMasterDetect keymasterScript) && keymasterScript.CanInteract)
            //{
            //    StartCoroutine(InteractCooldown());
            //    keymasterScript.CheckItem();
            //}
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DigiClockScript digiclock))
        {
            if (digiclock.Grouped)
            {
                digiclock.UnInteract();
            }
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

    public void SetActiveInteractPrompt(bool value)
    {
        interactPrompt.SetActive(value);
    }

    public void SetActivePickupPrompt(bool value)
    {
        pickupPrompt.SetActive(value);
    }

    public void SetSit(bool value)
    {
        isSitting = value;
    }
}
