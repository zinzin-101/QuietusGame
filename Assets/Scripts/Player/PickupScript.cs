using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [SerializeField] float pickupDistance = 0.5f;
    [SerializeField] KeyCode pickUpKey = KeyCode.F;


    private bool pickedUp;
    private string pickUpName;

    private MovementScript movementScript;

    private bool canInteract;

    private void Start()
    {
        TryGetComponent(out movementScript);
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

                StartCoroutine(InteractCooldown());
            }
        }
    }

    private void Update()
    {
        if (pickedUp && Input.GetKey(pickUpKey) && canInteract)
        {
            Transform _pickupObject = transform.Find(pickUpName);
            if (_pickupObject != null)
            {
                Vector3 _dropPosition = new Vector3();
                /*
                switch (movementScript.FacingRight)
                {
                    case true:
                        _dropPosition.x = pickupDistance;
                        break;
                    case false:
                        _dropPosition.x = -pickupDistance;
                        break;
                }
                */
                _pickupObject.transform.localPosition = _dropPosition;

                _pickupObject.transform.parent = null;
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
