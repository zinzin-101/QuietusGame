using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairScript : MonoBehaviour
{
    public enum ChairState
    {
        Upfront,
        Upside,
        Down
    }

    [SerializeField] bool startUpright = false;

    private SpriteRenderer spriteRenderer;

    [SerializeField] Sprite chairUprightFront, chairUprightSide, chairNonUpright;
    [SerializeField] Sprite highlightUp, highlightSide, highlightNonUpright;
    [SerializeField] SpriteRenderer backRenderer;
    [SerializeField] Sprite backUprightFront, backUprightSide, backNonUpright;
    [SerializeField] Sprite highlightBackUp, highlightBackSide, highlightBackNonUpright;
    [SerializeField] GameObject chairBackDownObject, chairBackObject;
    private SpriteRenderer chairBackDownRenderer;

    [SerializeField] Collider2D chairFrontColl, chairSideColl, chairDownColl;

    private ChairState currentState;
    public ChairState CurrentState => currentState;
    private bool collisionOn;

    [SerializeField] Transform sittingPos;
    public Transform SittingPos => sittingPos;

    private InteractCheck interactCheck;

    private void Awake()
    {
        TryGetComponent(out spriteRenderer);
        TryGetComponent(out interactCheck);
        chairBackDownRenderer = chairBackDownObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (startUpright)
        {
            ChangeChairState(ChairState.Upfront);
        }
        else
        {
            ChangeChairState(ChairState.Down);
            
        }

        collisionOn = true;
    }

    private void Update()
    {
        if (currentState == ChairState.Down)
        {
            interactCheck.ChangeInteractType(InteractCheck.InteractType.Pickup);
            
        }
        else
        {
            interactCheck.ChangeInteractType(InteractCheck.InteractType.Both);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            ChangeChairState(currentState, true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            SpriteRenderer playerRenderer = collision.gameObject.GetComponentInChildren<SpriteRenderer>();
            if (collision.gameObject.transform.position.y < transform.position.y)
            {
                backRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
            }
            else
            {
                backRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerInput playerInput))
        {
            ChangeChairState(currentState, false);
            
        }
    }

    public void ChangeChairState(ChairState state)
    {
        switch (state)
        {
            case ChairState.Upfront:
                spriteRenderer.sprite = chairUprightFront;
                backRenderer.sprite = backUprightFront;
                currentState = ChairState.Upfront;

                break;

            case ChairState.Upside:
                spriteRenderer.sprite = chairUprightSide;
                backRenderer.sprite = backUprightSide;
                currentState = ChairState.Upside;
                SoundManager.PlaySound(SoundManager.Sound.StandOnChair);

                break;

            case ChairState.Down:
                spriteRenderer.sprite = chairNonUpright;
                backRenderer.sprite = backNonUpright;
                currentState = ChairState.Down;

                break;
        }
        ToggleCollision(collisionOn);
    }

    public void ChangeChairState(ChairState state, bool setHighlight)
    {
        chairBackObject.SetActive(true);
        chairBackDownObject.SetActive(false);

        switch (state)
        {
            case ChairState.Upfront:
                if (setHighlight)
                {
                    spriteRenderer.sprite = highlightUp;
                    backRenderer.sprite = highlightBackUp;
                }
                else
                {
                    spriteRenderer.sprite = chairUprightFront;
                    backRenderer.sprite = backUprightFront;
                }
                currentState = ChairState.Upfront;
                break;

            case ChairState.Upside:
                if (setHighlight)
                {
                    spriteRenderer.sprite = highlightSide;
                    backRenderer.sprite = highlightBackSide;
                }
                else
                {
                    spriteRenderer.sprite = chairUprightSide;
                    backRenderer.sprite = backUprightSide;
                }
                currentState = ChairState.Upside;
                break;

            case ChairState.Down:
                chairBackDownObject.SetActive(true);
                chairBackObject.SetActive(false);
                if (setHighlight)
                {
                    spriteRenderer.sprite = highlightNonUpright;
                    chairBackDownRenderer.sprite = highlightBackNonUpright;
                }
                else
                {
                    spriteRenderer.sprite = chairNonUpright;
                    chairBackDownRenderer.sprite = backNonUpright;
                }
                currentState = ChairState.Down;
                break;

        }
        ToggleCollision(collisionOn);
    }

    public void ToggleCollision(bool value)
    {
        chairFrontColl.enabled = false;
        chairSideColl.enabled = false;
        chairDownColl.enabled = false;

        collisionOn = value;

        if (!value) return;

        switch (currentState)
        {
            case ChairState.Upfront:
                chairFrontColl.enabled = value;
                break;
            case ChairState.Upside:
                chairSideColl.enabled = value;
                break;
            case ChairState.Down:
                chairDownColl.enabled = value;
                break;
        }
    }
}
