using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryItem))]

public class SpecialItem : MonoBehaviour
{
    private InventoryItem inventoryItem;

    private void Awake()
    {
        TryGetComponent(out inventoryItem);
    }

    public void TransformItem()
    {
        InventoryManager.Instance.Add(SpecialItemDictionary.Instance.SpecialItemConvert(inventoryItem.Item));
        InventoryManager.Instance.Remove(inventoryItem.Item);
    }
}
