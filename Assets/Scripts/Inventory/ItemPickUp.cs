using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item Item;

    void Pickup()
    {
        InventoryManager.Instance.AddItem(Item);
        InventoryManager.Instance.ListItem();
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        Pickup();
    }
}
