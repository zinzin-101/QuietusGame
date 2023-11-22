using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuinnScript : MonoBehaviour
{
    [SerializeField] Item itemToGive;
    private bool activated;

    private void Awake()
    {
        activated = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (activated) return;
        if (collision.gameObject.TryGetComponent(out PickupScript pickupScript))
        {
            if (pickupScript.PickedUp && pickupScript.PickUpName == "Bag")
            {
                activated = true;

                InventoryManager.Instance.Add(itemToGive);

                Transform bag = pickupScript.DropObject();
                bag.TryGetComponent(out BagScript bagScript);
                bagScript.DestroyObject();
            }
        }
    }
}
