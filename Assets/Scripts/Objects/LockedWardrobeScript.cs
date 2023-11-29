using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
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

    private Collider2D col;

    private ActionProgress actionProgress;

    private void Awake()
    {
        currentSpriteArray = new Sprite[2];
        TryGetComponent(out col);
        TryGetComponent(out actionProgress);
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

    public async void UnlockWardrobe()
    {
        if (!locked || !canUnlock) return;

        bool hasKey = false;
        Item keyItem = null;

        foreach (var item in InventoryManager.Instance.Items)
        {
            if (item.itemName == "Key")
            {
                hasKey = true;
                SoundManager.PlaySound(SoundManager.Sound.UseKey);
                //InventoryManager.Instance.Remove(item);
                keyItem = item;
                break;
            }
        }

        if (!hasKey) return;

        actionProgress.ShowBar();
        actionProgress.SetProgressActive(true);

        while (true)
        {
            if (!actionProgress.Activate && !actionProgress.ActionFinished) return;
            if (actionProgress.ActionFinished) break;
            await Task.Delay(1);
        }

        actionProgress.HideBar();

        if (keyItem != null)
        {
            InventoryManager.Instance.Remove(keyItem);
        }

        canUnlock = false;
        locked = false;
        unlockedText.SetActive(true);
        SetSprite(unlockedSprites);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput _playerInput) && !open)
        {
            highlighted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            highlighted = false;
            actionProgress.ResetProgressBar();
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
                    //switch (open)
                    //{
                    //    case true:
                    //        //open = false;
                    //        //SetSprite(unlockedSprites);
                    //        //ShowItem(item, false);

                    //        //disable interaction after first open
                    //        break;

                    //    case false:
                    //        highlighted = false;
                    //        open = true;
                    //        SetSprite(openSprites);
                    //        ShowItem(item, true);
                    //        break;
                    //}

                    highlighted = false;
                    open = true;
                    SetSprite(openSprites);
                    ShowItem(item, true);
                    col.enabled = false;
                    SoundManager.PlaySound(SoundManager.Sound.WardropeOpen);
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
