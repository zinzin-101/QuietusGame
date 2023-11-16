using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMasterDetect : MonoBehaviour
{
    [SerializeField] Item[] itemList = new Item[3];

    private bool canInteract;
    public bool CanInteract => canInteract;

    private int currentItemIndex;
    private bool hasItem;
    public bool HasItem => hasItem;

    private void Awake()
    {
        canInteract = false;
        currentItemIndex = 0;
    }

    private void Update()
    {
        print(canInteract);
    }

    public void CheckItem()
    {
        canInteract = false;
        hasItem = false;

        foreach (Item item in InventoryManager.Instance.Items)
        {
            if (currentItemIndex > itemList.Length - 1) return;
            if (item == null) continue;
            if (itemList[currentItemIndex].name == item.name)
            {
                hasItem = true;
                InventoryManager.Instance.Remove(item);
                currentItemIndex++;
                break;
            }
        }
        
        if (!hasItem)
        {
            canInteract = true;
        }
    }

    public void SetCanInteract(bool value)
    {
        canInteract = value;
    }

    public void SetHasItem(bool value)
    {
        hasItem = value;
    }

    public void SetRequiredItem(Item item, int index)
    {
        itemList[index] = item;
    }
}
