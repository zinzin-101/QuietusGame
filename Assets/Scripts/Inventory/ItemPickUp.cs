using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class ItemPickUp : MonoBehaviour
{
    public Item Item;
    [SerializeField] Animator animator;
    [SerializeField] bool playAnimation;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite sprite, highlightSprite;

    [SerializeField] bool highlightOn = true;

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
    private void OnMouseUpAsButton()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    if (!EventSystem.current.currentSelectedGameObject.name.Equals(gameObject.name))
        //    {
        //        Pickup();
        //    }
        //}

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

    private void OnMouseEnter()
    {
        SetSprite(highlightSprite);
    }

    private void OnMouseExit()
    {
        SetSprite(sprite);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput) && highlightOn)
        {
            SetSprite(highlightSprite);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput) && highlightOn)
        {
            SetSprite(sprite);
        }
    }

    void SetSprite(Sprite sprite)
    {
        if (spriteRenderer == null) return;

        spriteRenderer.sprite = sprite;
    }
}
