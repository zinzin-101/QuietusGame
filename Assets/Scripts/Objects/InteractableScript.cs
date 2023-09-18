using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    private bool playerEnter;
    public bool PlayerEnter => playerEnter;
    private void Awake()
    {
        playerEnter = false;
    }

    public void SetPlayerEnter(bool value)
    {
        playerEnter = value;
    }
}
