using System.Collections;
using System.Collections.Generic;
using Unity.Profiling.LowLevel;
using UnityEngine;

public class LockedWardrobeScript : MonoBehaviour
{
    [SerializeField] Sprite lockedSprite, unlockedSprite, openSprite;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] GameObject item;
    [SerializeField] GameObject unlockedText, prompt;

    private bool locked;
    private bool open;
    private bool canUnlock;

    private void Start()
    {
        locked = true;
        open = false;
        canUnlock = true;

        item.SetActive(false);
        spriteRenderer.sprite = lockedSprite;

        prompt.SetActive(false);
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
        spriteRenderer.sprite = unlockedSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput) && !open && !locked)
        {
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            prompt.SetActive(false);
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
                            spriteRenderer.sprite = unlockedSprite;
                            //item.SetActive(false);
                            ShowItem(item, false);
                            break;

                        case false:
                            open = true;
                            spriteRenderer.sprite = openSprite;
                            //item.SetActive(true);
                            ShowItem(item, true);
                            prompt.SetActive(false);
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
}
