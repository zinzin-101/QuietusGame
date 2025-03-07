using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedScript : MonoBehaviour
{
    [SerializeField] GameObject blanketOn, blanketOff;
    [SerializeField] Sprite blanketOnSprite, blanketOffSprite;
    [SerializeField] Sprite blanketOnHighlight, blanketOffHighlight;
    private SpriteRenderer blanketOnRenderer, blanketOffRenderer;

    [SerializeField] GameObject item;

    private Collider2D col;

    private void Awake()
    {
        blanketOn.TryGetComponent(out blanketOnRenderer);
        blanketOff.TryGetComponent(out blanketOffRenderer);
        TryGetComponent(out col);
    }

    private void Start()
    {
        blanketOn.SetActive(true);
        blanketOff.SetActive(false);

        if (item != null) item.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            blanketOnRenderer.sprite = blanketOnHighlight;
            blanketOffRenderer.sprite = blanketOffHighlight;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            blanketOnRenderer.sprite = blanketOnSprite;
            blanketOffRenderer.sprite = blanketOffSprite;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerInteractScript))
        {
            if (playerInteractScript.InteractPressed && playerInteractScript.CanInteract)
            {
                StartCoroutine(playerInteractScript.InteractCooldown());

                //ToggleBlanket();
                OpenBlanket();
                SoundManager.PlaySound(SoundManager.Sound.Blanked);
            }
        }
    }

    void ToggleBlanket()
    {
        blanketOn.SetActive(!blanketOn.activeSelf);
        blanketOff.SetActive(!blanketOff.activeSelf);

        if (item == null) return;

        if (blanketOff.activeSelf)
        {
            item.SetActive(true);
        }
        else
        {
            item.SetActive(false);
        }
        
    }

    void OpenBlanket()
    {
        blanketOn.SetActive(false);
        blanketOff.SetActive(true);

        col.enabled = false;

        if (item == null)
        {
            
            return;
        }

        item.SetActive(true);
    }
}
