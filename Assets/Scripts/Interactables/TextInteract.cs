using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInteract : MonoBehaviour
{
    [SerializeField] GameObject text, prompt;

    private void Start()
    {
        text.SetActive(false);
        prompt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput) && !text.activeSelf)
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

    public void ToggleActiveText()
    {
        text.SetActive(!text.activeSelf);
    }

    private void Update()
    {
        if (text.activeSelf)
        {
            prompt.SetActive(false);
        }
    }
}
