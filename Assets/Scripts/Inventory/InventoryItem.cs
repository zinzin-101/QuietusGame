using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    private string itemDescription;
    public string ItemDescription => itemDescription;

    private string itemName;
    public string ItemName => itemName;

    private Item item;
    public Item Item => item;

    public void SelectItem()
    {
        InventoryManager.Instance.SetCurrentItem(gameObject, itemDescription);
    }

    public void SetItemDescription(string description)
    {
        itemDescription = description;
    }

    public void SetItemName(string name)
    {
        itemName = name;
    }

    public void SetItem(Item _item)
    {
        item = _item;
    }
}
