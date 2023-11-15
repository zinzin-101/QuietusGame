using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightScript : MonoBehaviour
{
    [SerializeField] Sprite normalSprite, highlightedSprite;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        TryGetComponent(out spriteRenderer);

        spriteRenderer.sprite = normalSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput player))
        {
            spriteRenderer.sprite = highlightedSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput player))
        {
            spriteRenderer.sprite = normalSprite;
        }
    }
}
