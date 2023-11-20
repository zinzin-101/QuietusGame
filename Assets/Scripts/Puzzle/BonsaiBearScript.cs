using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonsaiBearScript : MonoBehaviour
{
    private bool activated;

    private void Awake()
    {
        activated = false;
    }

    private void Start()
    {
        GameManager.Instance.AllowBagPickup(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PickupScript pickupScript))
        {
            if (pickupScript.PickUpName != "Bonsai") return;
            Transform bonsai = pickupScript.DropObject();
            TryGetComponent(out BonsaiScript bonsaiScript);
            
            //if (bonsaiScript != null)
            //{
            //    bonsaiScript.SetCollider(true);
            //}

            bonsai.transform.parent = transform;
            bonsai.transform.localPosition = new Vector3(-3.64f, -0.65f, 0f);

            if (activated) return;

            GameManager.Instance.AllowBagPickup(true);
            Collider2D col = bonsai.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }
}
