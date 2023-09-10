using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerTableScript : MonoBehaviour
{
    [SerializeField] GameObject table;
    [SerializeField] GameObject[] drawers;
    private SpriteRenderer tableSprite;
    private SpriteRenderer[] drawersSprite;

    [SerializeField] GameObject item;

    private int numOfDrawer;
    private int numOfOpenedDrawer;

    private void Awake()
    {
        table.TryGetComponent(out tableSprite);
        
        numOfDrawer = drawers.Length;
        drawersSprite = new SpriteRenderer[numOfDrawer];
        for (int i = 0; i < numOfDrawer; i++)
        {
            drawersSprite[i] = drawers[i].GetComponent<SpriteRenderer>();
        }
    }

    private void Start()
    {
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
            tableSprite.color = Color.yellow;

            foreach(var drawer in drawersSprite)
            {
                drawer.color = Color.yellow;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            tableSprite.color = Color.white;

            foreach (var drawer in drawersSprite)
            {
                drawer.color = Color.white;
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

        for (int i = 0; i < numOfDrawer; i++)
        {
            if (i <= (numOfOpenedDrawer - 1))
            {
                drawers[i].SetActive(true);
            }
            else
            {
                drawers[i].SetActive(false);
            }
        }
    }
}
