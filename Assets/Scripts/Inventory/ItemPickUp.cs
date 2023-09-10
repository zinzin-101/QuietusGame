using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ItemPickUp : MonoBehaviour
{
    public Item Item;
    [SerializeField] Animator animator;
    [SerializeField] bool playAnimation;
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (animator != null)
        {
            if (playAnimation)
            {
                animator.enabled = true;
            }
            else
            {
                animator.enabled = false;
            }
        }     
    }

    void Pickup()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
    }
    private void OnMouseDown()
    {
        Pickup();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerInteractScript))
        {
            if (playerInteractScript.PickupKeyPressed)
            {
                Pickup();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            SetColor(Color.white);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            SetColor(Color.white);
        }
    }

    void SetColor(Color color)
    {
        if (spriteRenderer == null) return;

        spriteRenderer.color = color;
    }
}
