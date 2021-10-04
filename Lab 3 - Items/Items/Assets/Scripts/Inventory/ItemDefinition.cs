/**
 * ItemTemplate.cs
 * Description: This script defines an item.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Armor = 1 << 0,
    Weapon = 1 << 1
}

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Create New Item")]
public class ItemDefinition : ScriptableObject
{
    public string name;

    private int maxStackSize = 1;
    public int MaxStackSize
    {
        get
        {
            return maxStackSize;
        }
        set
        {
            maxStackSize = value < 1 ? 1 : value;
        }
    }

    ItemType type;

    // Does this item belong to an item type?
    bool IsOfType(ItemType itemType)
    {
        return (int)(type & itemType) != 0;
    }

    // Make the item to belong to or not belong to an item type.
    void SetType(ItemType itemType, bool IsOfType = true)
    {
        if (IsOfType)
        {
            type = type | itemType;
        }
        else
        {
            type = type & ~itemType;
        }
    }
}
