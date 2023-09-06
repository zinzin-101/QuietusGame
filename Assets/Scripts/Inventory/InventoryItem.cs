using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    private string itemDescription;
    public string ItemDescription => itemDescription;

    public void SelectItem()
    {
        InventoryManager.Instance.SetCurrentItem(gameObject, itemDescription);
    }

    public void SetItemDescription(string description)
    {
        itemDescription = description;
    }
}
