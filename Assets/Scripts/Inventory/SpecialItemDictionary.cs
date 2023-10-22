using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItemDictionary : MonoBehaviour
{
    [Header("Special Items")]
    [SerializeField] Item test;
    [SerializeField] Item paperclip;

    [Header("Normal Items")]
    [SerializeField] Item key;
    [SerializeField] Item lockpick;

    public static SpecialItemDictionary Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Item SpecialItemConvert(Item item)
    {
        if (item.name == paperclip.name)
        {
            return lockpick;
        }

        if (item.name == test.name)
        {
            return key;
        }

        return null;
    }
}
