using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonsaiScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] GameObject col;

    private void Awake()
    {
        TryGetComponent(out spriteRenderer);
        col.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PickupScript pickupScript))
        {
            SpriteRenderer playerRenderer = collision.gameObject.GetComponentInChildren<SpriteRenderer>();
            if (collision.gameObject.transform.position.y < transform.position.y - 0.6f && !pickupScript.PickedUp)
            {
                spriteRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
            }
            else
            {
                spriteRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
            }
        }
    }

    public void SetCollider(bool value)
    {
        col.SetActive(value);
    }
}
