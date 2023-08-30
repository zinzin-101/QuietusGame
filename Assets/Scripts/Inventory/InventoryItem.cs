using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public void SelectItem()
    {
        InventoryManager.Instance.SetCurrentItem(gameObject);
    }
}
