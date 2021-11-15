/**
 * ItemInstance.cs
 * Description: This script handles an instance of an item.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemInstance(ItemDefinition itemDefinition, int currentStackSize = 1)
    {
        this.itemDefinition = itemDefinition;
        this.CurrentStackSize = currentStackSize;
    }

    // What is the definition of this item instance?
    public ItemDefinition itemDefinition;

    [SerializeField] int currentStackSize = 1;
    public int CurrentStackSize
    {
        get
        {
            return currentStackSize;
        }
        set
        {
            currentStackSize = value < 0 ? 0 : (value > itemDefinition.MaxStackSize ? itemDefinition.MaxStackSize : value);
        }
    }

    // Check if this item belongs to a definition.
    public bool BelongToItemDefinition(ItemDefinition itemDefinition)
    {
        return this.itemDefinition = itemDefinition;
    }

    // Use the ! operator to check whether the item instance is valid or not.
    public static bool operator !(ItemInstance itemInstance)
    {
        return itemInstance == null || itemInstance.itemDefinition == null;
    }

    // Use the boolean implicit operator to check whether the item instance is valid or not.
    public static implicit operator bool(ItemInstance itemInstance)
    {
        return itemInstance != null && itemInstance.itemDefinition != null;
    }

    // Implicitly convert ItemInstance into ItemDefinition
    public static implicit operator ItemDefinition(ItemInstance itemInstance)
    {
        if (itemInstance == null)
            return null;

        return itemInstance.itemDefinition;
    }
}