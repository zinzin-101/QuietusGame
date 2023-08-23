using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName = "item/Create New Item")]
public class Item : ScriptableObject
{

    public int id;
    public string itemName;
    public int value;
    public Sprite icon;
}
