using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [SerializeField] float pickupDistance = 0.5f;

    private bool pickedUp;
    private string pickUpName;

    private MovementScript movementScript;

    private void Start()
    {
        TryGetComponent(out movementScript);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!pickedUp && Input.GetKey(KeyCode.E))
        {
            if (collision.gameObject.TryGetComponent(out PickableObjectScript pickableScript))
            {
                pickedUp = true;
                collision.gameObject.transform.parent = transform;
                collision.gameObject.transform.localPosition = new Vector3(0f, pickupDistance, 0f);
                pickUpName = collision.gameObject.name;
            }
        }
    }

    private void Update()
    {
        if (pickedUp && Input.GetKey(KeyCode.Q))
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
        }
    }
}
