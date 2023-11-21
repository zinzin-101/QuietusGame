using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonsaiBearScript : MonoBehaviour
{
    private bool activated;
    private Transform bonsai;
    private SpriteRenderer bonsaiSpriteRenderer;

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
            bonsai = pickupScript.DropObject();
            //TryGetComponent(out BonsaiScript bonsaiScript);
            
            //if (bonsaiScript != null)
            //{
            //    bonsaiScript.SetCollider(true);
            //}

            if (bonsai == null) return;

            bonsai.transform.parent = transform;
            bonsai.transform.localPosition = new Vector3(-3.64f, -0.65f, 0f);

            if (activated) return;

            GameManager.Instance.AllowBagPickup(true);
            bonsai.TryGetComponent(out Collider2D col);
            if (col != null)
            {
                col.enabled = false;
            }
            bonsai.TryGetComponent(out bonsaiSpriteRenderer);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput input))
        {
            if (bonsaiSpriteRenderer == null) return;

            SpriteRenderer playerRenderer = collision.gameObject.GetComponentInChildren<SpriteRenderer>();
            if (collision.gameObject.transform.position.y < transform.position.y - 0.8f)
            {
                bonsaiSpriteRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
            }
            else
            {
                bonsaiSpriteRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
            }
        }
    }
}
