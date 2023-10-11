using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DiaryScript : MonoBehaviour
{
    [SerializeField] Sprite normalSprite, highlightedSprite;
    private SpriteRenderer spriteRenderer;

    [SerializeField] GameObject diaryPanel;
    [SerializeField] Image panelSpriteRenderer;
    [SerializeField] Sprite[] diaryPages;
    private int pageIndex;
    private int totalPages;

    public GameObject item;
    public int itemPageIndex;


    private void Awake()
    {
        TryGetComponent(out spriteRenderer);
    }

    private void Start()
    {
        pageIndex = 0;
        totalPages = diaryPages.Length;
        panelSpriteRenderer.sprite = diaryPages[pageIndex];
        diaryPanel.SetActive(false);
        item.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerInteractScript))
        {
            if (playerInteractScript.PickupKeyPressed && playerInteractScript.CanInteract)
            {
                StartCoroutine(playerInteractScript.InteractCooldown());

                PlayerInteracted();
            }
        }
    }

    private void OnMouseDown()
    {
        PlayerInteracted();
    }

    private void OnMouseEnter()
    {
        spriteRenderer.sprite = highlightedSprite;
    }

    private void OnMouseExit()
    {
        spriteRenderer.sprite = normalSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            spriteRenderer.sprite = highlightedSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            spriteRenderer.sprite = normalSprite;
            diaryPanel.SetActive(false);

            if (item != null) item.SetActive(false);
        }
    }

    void PlayerInteracted()
    {
        diaryPanel.SetActive(!diaryPanel.activeSelf);

        if (item != null)
        {
            if (pageIndex == itemPageIndex)
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    public void FlipDiaryPage()
    {
        if (!diaryPanel.activeSelf) return;

        panelSpriteRenderer.sprite = diaryPages[++pageIndex % totalPages];

        if (item != null)
        {
            if (pageIndex == itemPageIndex)
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }

        if (pageIndex % totalPages == 0)
        {
            pageIndex = 0;
            diaryPanel.SetActive(false);
        }
    }
}
