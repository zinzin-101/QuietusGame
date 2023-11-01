using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerTableScript : MonoBehaviour
{
    [SerializeField] GameObject table;
    [SerializeField] GameObject[] drawers;
    private SpriteRenderer tableSpriteRenderer;
    private SpriteRenderer[] drawersSpriteRenderer;
    [SerializeField] GameObject[] drawerItems = new GameObject[4];
    [SerializeField] Sprite bigDrawerSprite, highlightBigdrawerSprite, drawerSprite, highlightDrawerSprite, tableSprite, highlightTableSprite;

    [SerializeField] GameObject item;

    private int numOfDrawer;
    private int numOfOpenedDrawer;

    private bool isBigDrawerLocked;
    [SerializeField] GameObject unlockedText;

    private void Awake()
    {
        table.TryGetComponent(out tableSpriteRenderer);
        
        numOfDrawer = drawers.Length;
        drawersSpriteRenderer = new SpriteRenderer[numOfDrawer];

        for (int i = 0; i < numOfDrawer; i++)
        {
            drawersSpriteRenderer[i] = drawers[i].GetComponent<SpriteRenderer>();

            if (drawerItems[i] != null)
            {
                drawerItems[i].TryGetComponent(out SpriteRenderer spriteRenderer);
                spriteRenderer.sortingOrder = drawersSpriteRenderer[i].sortingOrder + 1;
                drawerItems[i].SetActive(false);
            }
        }
    }

    private void Start()
    {
        isBigDrawerLocked = true;

        table.SetActive(true);

        foreach (var drawer in drawers)
        {
            drawer.SetActive(false);
        }

        if (item != null) item.SetActive(false);

        numOfOpenedDrawer = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            tableSpriteRenderer.sprite = highlightTableSprite;

            //foreach(var drawer in drawersSpriteRenderer)
            //{
            //    drawer.sprite = highlightDrawerSprite;
            //}

            for (int i = 0; i < numOfDrawer; i++)
            {
                var drawer = drawersSpriteRenderer[i];

                if (i == 0)
                {
                    drawer.sprite = highlightBigdrawerSprite;
                }
                else
                {
                    drawer.sprite = highlightDrawerSprite;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            tableSpriteRenderer.sprite = tableSprite;

            //foreach (var drawer in drawersSpriteRenderer)
            //{
            //    drawer.sprite = drawerSprite;
            //}

            for (int i = 0; i < numOfDrawer; i++)
            {
                var drawer = drawersSpriteRenderer[i];

                if (i == 0)
                {
                    drawer.sprite = bigDrawerSprite;
                }
                else
                {
                    drawer.sprite = drawerSprite;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerInteractScript))
        {
            if (playerInteractScript.InteractPressed && playerInteractScript.CanInteract)
            {
                StartCoroutine(playerInteractScript.InteractCooldown());

                numOfOpenedDrawer++;
                OpenDrawer();
            }
        }
    }

    void OpenDrawer()
    {
        if (numOfOpenedDrawer > numOfDrawer) numOfOpenedDrawer = 0;

        if (isBigDrawerLocked)
        {
            UnlockBigDrawer();
        }

        for (int i = 0; i < numOfDrawer; i++)
        {
            if (i == (numOfOpenedDrawer - 1))
            {
                if (i == 0 && isBigDrawerLocked)
                {
                    i++;
                }

                drawers[i].SetActive(true);

                if (drawerItems[i] != null) drawerItems[i].SetActive(true);
                SoundManager.PlaySound(SoundManager.Sound.Desk);
            }
            else
            {
                drawers[i].SetActive(false);

                if (drawerItems[i] != null) drawerItems[i].SetActive(false);
            }
        }
    }

    void UnlockBigDrawer()
    {
        if (!isBigDrawerLocked) return;

        bool hasLockpick = false;

        foreach (var item in InventoryManager.Instance.Items)
        {
            if (item.itemName == "Lockpick")
            {
                hasLockpick = true;
                InventoryManager.Instance.Remove(item);
                break;
            }
        }

        if (!hasLockpick) return;

        unlockedText.SetActive(true);
        isBigDrawerLocked = false;
        SoundManager.PlaySound(SoundManager.Sound.BedSheet);
    }
}
