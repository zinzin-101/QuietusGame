using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PickupScript : MonoBehaviour
{
    [SerializeField] float pickupDistance = 0.5f;
    [SerializeField] KeyCode pickUpKey = KeyCode.F;

    private bool pickedUp;
    public bool PickedUp => pickedUp;
    private string pickUpName;
    public string PickUpName => pickUpName;

    private SpriteRenderer spriteRenderer;

    private bool canInteract;

    private PlayerInteractScript interactScript;

    private void Start()
    {
        TryGetComponent(out interactScript);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        canInteract = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!pickedUp && Input.GetKey(pickUpKey) && canInteract && !interactScript.IsSitting)
        {
            if (collision.gameObject.TryGetComponent(out PickableObjectScript pickableScript))
            {
                if (collision.gameObject.TryGetComponent(out BagScript bag) && !GameManager.Instance.CanPickUpBag)
                {
                    return;
                }

                if (collision.gameObject.TryGetComponent(out BonsaiScript bonsaiScript) && !GameManager.Instance.CanPickBonsai)
                {
                    return;
                }

                pickedUp = true;
                
                collision.gameObject.transform.parent = transform;
                collision.gameObject.transform.localPosition = new Vector3(0f, pickupDistance, 0f);
                pickUpName = collision.gameObject.name;

                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;

                StartCoroutine(InteractCooldown());
            }

            if (collision.gameObject.TryGetComponent(out ChairScript chairScript))
            {
                chairScript.ChangeChairState(ChairScript.ChairState.Upfront, false);
                chairScript.ToggleCollision(false);
            }
        }
    }
    private void Update()
    {
        if (pickedUp && Input.GetKey(pickUpKey) && canInteract)
        {
            DropObject();
            StartCoroutine(InteractCooldown());
        }
    }

    public Transform DropObject()
    {
        Transform _pickupTransform = transform.Find(pickUpName);

        if (_pickupTransform != null)
        {
            Vector3 _dropPosition = new Vector3();
            _pickupTransform.transform.localPosition = _dropPosition;

            _pickupTransform.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder - 1;

            if (_pickupTransform.TryGetComponent(out ChairScript chairScript))
            {
                chairScript.ToggleCollision(true);
            }
            SoundManager.PlaySound(SoundManager.Sound.Chair);

            _pickupTransform.transform.parent = null;
            pickedUp = false;
            return _pickupTransform;
        }
        return null;
    }

    public void DropChair()
    {
        Transform _pickupTransform = transform.Find(pickUpName);

        if (_pickupTransform != null)
        {
            Vector3 _dropPosition = new Vector3();
            _pickupTransform.transform.localPosition = _dropPosition;

            _pickupTransform.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder - 1;
            TryGetComponent(out ChairScript chairScript);
            chairScript.ChangeChairState(ChairScript.ChairState.Upside);

            _pickupTransform.transform.parent = null;
            pickedUp = false;
        }
    }

    IEnumerator InteractCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(0.25f);
        canInteract = true;
    }
}
