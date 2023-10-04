using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GametapeScript : MonoBehaviour
{
    [SerializeField] Sprite highlight, normal;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        TryGetComponent(out spriteRenderer);
        spriteRenderer.sprite = normal;
    }

    private void OnMouseEnter()
    {
        spriteRenderer.sprite = highlight;
    }

    private void OnMouseExit()
    {
        spriteRenderer.sprite = normal;
    }
}
