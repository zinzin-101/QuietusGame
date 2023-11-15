using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonsaiScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        TryGetComponent(out spriteRenderer);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            SpriteRenderer playerRenderer = collision.gameObject.GetComponentInChildren<SpriteRenderer>();
            if (collision.gameObject.transform.position.y < transform.position.y)
            {
                spriteRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
            }
            else
            {
                spriteRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
            }
        }
    }
}
