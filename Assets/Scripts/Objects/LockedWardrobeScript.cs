using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class LockedWardrobeScript : MonoBehaviour
{
    [SerializeField] Sprite[] lockedSprites, unlockedSprites, openSprites;
    private Sprite[] currentSpriteArray;
    private Sprite currentSprite;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] GameObject item;
    [SerializeField] GameObject unlockedText;

    private bool locked;
    private bool open;
    private bool canUnlock;
    private bool highlighted;

    private void Awake()
    {
        currentSpriteArray = new Sprite[2];
    }

    private void Start()
    {
        locked = true;
        open = false;
        canUnlock = true;

        if (item != null) item.SetActive(false);

        //spriteRenderer.sprite = lockedSprite[0];

        highlighted = false;

        SetSprite(lockedSprites);
    }

    private void Update()
    {
        switch (highlighted)
        {
            case true:
                currentSprite = currentSpriteArray[1];
                break;
            case false:
                currentSprite = currentSpriteArray[0];
                break;
        }

        spriteRenderer.sprite = currentSprite;
    }

    public void UnlockWardrobe()
    {
        if (!locked || !canUnlock) return;

        bool hasKey = false;

        foreach (var item in InventoryManager.Instance.Items)
        {
            if (item.itemName == "Key")
            {
                hasKey = true;
                InventoryManager.Instance.Remove(item);
                break;
            }
        }

        if (!hasKey) return;

        canUnlock = false;
        locked = false;
        unlockedText.SetActive(true);
        SetSprite(unlockedSprites);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput _playerInput))
        {
            highlighted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            highlighted = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerInteractScript))
        {
            if (playerInteractScript.InteractPressed && playerInteractScript.CanInteract)
            {
                StartCoroutine(playerInteractScript.InteractCooldown());

                if (!locked)
                {
                    switch (open)
                    {
                        case true:
                            open = false;
                            SetSprite(unlockedSprites);
                            ShowItem(item, false);
                            break;

                        case false:
                            open = true;
                            SetSprite(openSprites);
                            ShowItem(item, true);
                            break;
                    }
                }
                else
                {
                    UnlockWardrobe();
                }
            }
        }
    }

    void ShowItem(GameObject obj, bool value)
    {
        if (obj != null)
        {
            obj.SetActive(value);
        }
    }

    void SetSprite(Sprite[] sprites)
    {
        currentSpriteArray = sprites;
        switch (highlighted)
        {
            case true:
                currentSprite = currentSpriteArray[1];
                break;
            case false:
                currentSprite = currentSpriteArray[0];
                break;
        }
        spriteRenderer.sprite = currentSprite;
    }
}
