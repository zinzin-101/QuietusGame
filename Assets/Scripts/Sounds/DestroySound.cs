using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySound : MonoBehaviour
{
    public float delay = 10;

    private void Start()
    {
        Destroy(gameObject, delay);
    }
}
