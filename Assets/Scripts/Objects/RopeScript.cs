using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public Item Item;
    [SerializeField] Sprite highlight, normal;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        TryGetComponent(out spriteRenderer);
        spriteRenderer.sprite = normal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.sprite = highlight;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.sprite = normal;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerInteractScript))
        {
            if (playerInteractScript.PickupKeyPressed)
            {
                SoundManager.PlaySound(SoundManager.Sound.PickupItem);
                InventoryManager.Instance.Add(Item);
                GameManager.Instance.HangManRoomButton();
                Destroy(gameObject);
            }
        }
    }
}
