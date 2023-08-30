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
    private string pickUpName;

    private SpriteRenderer spriteRenderer;

    private bool canInteract;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        canInteract = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!pickedUp && Input.GetKey(pickUpKey) && canInteract)
        {
            if (collision.gameObject.TryGetComponent(out PickableObjectScript pickableScript))
            {
                pickedUp = true;
                collision.gameObject.transform.parent = transform;
                collision.gameObject.transform.localPosition = new Vector3(0f, pickupDistance, 0f);
                pickUpName = collision.gameObject.name;

                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;

                StartCoroutine(InteractCooldown());
            }
        }
    }
    private void Update()
    {
        if (pickedUp && Input.GetKey(pickUpKey) && canInteract)
        {
            Transform _pickupTransform = transform.Find(pickUpName);

            if (_pickupTransform != null)
            {
                Vector3 _dropPosition = new Vector3();
                _pickupTransform.transform.localPosition = _dropPosition;

                _pickupTransform.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder - 1;

                _pickupTransform.transform.parent = null;
                pickedUp = false;   
            }

            StartCoroutine(InteractCooldown());
        }
    }

    IEnumerator InteractCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(0.25f);
        canInteract = true;
    }
}
