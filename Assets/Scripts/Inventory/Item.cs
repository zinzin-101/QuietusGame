using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Normal,
    Special
}

[CreateAssetMenu(fileName ="New Item",menuName = "item/Create New Item")]

public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    [TextArea(3, 10)]
    public string itemDescription;
}