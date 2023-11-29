using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
        can = false;

        if (activated)
        {
            DialogueManager.Instance.StartDialogue(afterAfter, true);
            GameManager.Instance.NextRoomButton(true);
            return;
        }

        if (collision.gameObject.TryGetComponent(out PickupScript pickupScript))
        {
            if (pickupScript.PickedUp && pickupScript.PickUpName == "Bag")
            {
                InventoryManager.Instance.Add(itemToGive);

                Transform bag = pickupScript.DropObject();
                bag.TryGetComponent(out BagScript bagScript);
                bagScript.DestroyObject();
                DialogueManager.Instance.StartDialogue(after, true);
                GameManager.Instance.PallorScript.NextAll();
                activated = true;
                return;
            }
        }

        DialogueManager.Instance.StartDialogue(before, true);
    }

    public void SetCan(bool value)
    {
        can = value;
    }
}
