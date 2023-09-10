using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

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
    [SerializeField] Collider2D chairFrontColl, chairSideColl, chairDownColl;

    private void Awake()
    {
        TryGetComponent(out spriteRenderer);
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
    }

    public void ChangeChairState(ChairState state)
    {
        chairFrontColl.enabled = false;
        chairSideColl.enabled = false;
        chairDownColl.enabled = false;

        switch (state)
        {
            case ChairState.Upfront:
                spriteRenderer.sprite = chairUprightFront;
                chairFrontColl.enabled = true;
                break;

            case ChairState.Upside:
                spriteRenderer.sprite = chairUprightSide;
                chairSideColl.enabled = true;
                break;

            case ChairState.Down:
                spriteRenderer.sprite = chairNonUpright;
                chairDownColl.enabled = true;
                break;

        }
    }

    public void ToggleCollision(bool value)
    {
        chairFrontColl.enabled = value;
        chairSideColl.enabled = value;
        chairDownColl.enabled = value;
    }
}
