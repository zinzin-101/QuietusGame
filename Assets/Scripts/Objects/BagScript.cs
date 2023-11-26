using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    [SerializeField] Dialogue noBonsai;
    public Dialogue NoBonsai => noBonsai;

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
