using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuinnScript : MonoBehaviour
{
    [SerializeField] Item itemToGive;
    private bool activated;

    [SerializeField] Dialogue before;
    [SerializeField] Dialogue after;
    [SerializeField] Dialogue afterAfter;

    private bool can;

    private void Awake()
    {
        activated = false;
        can = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!can) return;
        if (activated)
        {
            DialogueManager.Instance.StartDialogue(afterAfter, true);
            GameManager.Instance.NextRoomButton();
            return;
        }

        can = false;

        if (collision.gameObject.TryGetComponent(out PickupScript pickupScript))
        {
            if (pickupScript.PickedUp && pickupScript.PickUpName == "Bag")
            {
                activated = true;

                InventoryManager.Instance.Add(itemToGive);

                Transform bag = pickupScript.DropObject();
                bag.TryGetComponent(out BagScript bagScript);
                bagScript.DestroyObject();
                DialogueManager.Instance.StartDialogue(after, true);
                return;
            }

            DialogueManager.Instance.StartDialogue(before, true);
        }
    }

    public void SetCan(bool value)
    {
        can = value;
    }
}
