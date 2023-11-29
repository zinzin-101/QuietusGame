using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [SerializeField] Item unlockItem;

    private ActionProgress actionProgress;

    private void Awake()
    {
        table.TryGetComponent(out tableSpriteRenderer);
        TryGetComponent(out actionProgress);
        
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
            foreach (Item item in InventoryManager.Instance.Items)
            {
                if (item == unlockItem)
                {
                    UnlockBigDrawer();
                    numOfOpenedDrawer = 0;
                    return;
                }
            }
        }

        for (int i = 0; i < numOfDrawer; i++)
        {
            if (i == (numOfOpenedDrawer - 1))
            {
                if (i == 0 && isBigDrawerLocked)
                {
                    numOfOpenedDrawer++;
                    continue;
                }

                drawers[i].SetActive(true);

                if (drawerItems[i] != null) drawerItems[i].SetActive(true);
            }
            else
            {
                drawers[i].SetActive(false);

                if (drawerItems[i] != null) drawerItems[i].SetActive(false);
            }
        }
        SoundManager.PlaySound(SoundManager.Sound.OpenDrawer);
    }

    async void UnlockBigDrawer()
    {
        if (!isBigDrawerLocked) return;

        bool hasLockpick = false;
        Item keyItem = null;

        foreach (var item in InventoryManager.Instance.Items)
        {
            if (item.itemName == unlockItem.itemName)
            {
                hasLockpick = true;
                //InventoryManager.Instance.Remove(item);
                keyItem = item;
                break;
            }
        }

        if (!hasLockpick) return;

        actionProgress.ShowBar();
        actionProgress.SetProgressActive(true);

        while (true)
        {
            if (!actionProgress.Activate && !actionProgress.ActionFinished) return;
            if (actionProgress.ActionFinished) break;
            await Task.Delay(1);
        }

        if (keyItem != null)
        {
            InventoryManager.Instance.Remove(keyItem);
        }

        actionProgress.HideBar();

        unlockedText.SetActive(true);
        isBigDrawerLocked = false;
    }
}
