using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LockpickContainerScript : MonoBehaviour
{
    [SerializeField] Sprite closedSprite, openedSprite;
    [SerializeField] Sprite highlightClosed;
    private SpriteRenderer spriteRenderer;

    [SerializeField] GameObject lockpickObject;
    private Collider2D _collider;

    private bool isOpened;

    private void Awake()
    {
        TryGetComponent(out spriteRenderer);
        TryGetComponent(out _collider);
    }

    private void Start()
    {
        lockpickObject.SetActive(false);
        spriteRenderer.sprite = closedSprite;
        isOpened = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerInteractScript))
        {
            if (!isOpened)
            {
                spriteRenderer.sprite = highlightClosed;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerInteractScript))
        {
            if (!isOpened)
            {
                spriteRenderer.sprite = closedSprite;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerInteractScript))
        {
            if (playerInteractScript.InteractPressed && !isOpened)
            {
                StartCoroutine(playerInteractScript.InteractCooldown());
                isOpened = true;
                spriteRenderer.sprite = openedSprite;
                if (lockpickObject != null) lockpickObject.SetActive(true);
                _collider.enabled = false;
            }
        }
    }
}
