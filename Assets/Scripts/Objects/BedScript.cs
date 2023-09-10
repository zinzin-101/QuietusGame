using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedScript : MonoBehaviour
{
    [SerializeField] GameObject blanketOn, blanketOff;
    private SpriteRenderer blanketOnSprite, blanketOffSprite;

    [SerializeField] GameObject item;

    private void Awake()
    {
        blanketOn.TryGetComponent(out blanketOnSprite);
        blanketOff.TryGetComponent(out blanketOffSprite);
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
            blanketOnSprite.color = Color.yellow;
            blanketOffSprite.color = Color.yellow;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            blanketOnSprite.color = Color.white;
            blanketOffSprite.color = Color.white;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInteractScript playerInteractScript))
        {
            if (playerInteractScript.InteractPressed && playerInteractScript.CanInteract)
            {
                StartCoroutine(playerInteractScript.InteractCooldown());

                ToggleBlanket();
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
}
