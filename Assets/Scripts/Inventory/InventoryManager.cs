using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();

    [SerializeField] Transform itemContent, selectedItemContent;
    public GameObject InventoryItem, SpecialItem;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] TMP_Text descriptionText;

    private void Awake()
    {
        Instance = this;
    }

    public void Add(Item item)
    {
        Items.Add(item);
        ListItems();
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
        ListItems();
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void SetCurrentItem(GameObject item, string itemDescription)
    {
        foreach (Transform child in selectedItemContent)
        {
            Destroy(child.gameObject);
        }

        GameObject obj = Instantiate(item, selectedItemContent);
        obj.TryGetComponent(out Button button);
        Destroy(button);

        descriptionText.text = itemDescription;
    }

    public void ListItems()
    {
        // Destroy all existing inventory slots to refresh the display.
        foreach (Transform child in itemContent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in selectedItemContent)
        {
            Destroy(child.gameObject);
        }

        // Create a slot for each item in the inventory.
        foreach (var item in Items)
        {
            GameObject obj;
            if (item.itemType == ItemType.Special)
            {
                obj = Instantiate(SpecialItem, itemContent);
            }
            else
            {
                obj = Instantiate(InventoryItem, itemContent);
            }

            obj.TryGetComponent(out InventoryItem inventoryItem);
            inventoryItem.SetItemDescription(item.itemDescription);

            // Find UI elements in the slot prefab.
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            // Set the text and icon using data from the item.
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }
}
