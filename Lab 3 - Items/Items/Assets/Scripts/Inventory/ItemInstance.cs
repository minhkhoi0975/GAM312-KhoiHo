/**
 * ItemInstance.cs
 * Description: This script handles an instance of an item.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance
{
    // What is the definition of this item instance?
    public ItemDefinition itemDefinition;

    private int currentStackSize = 1;
    public int CurrentStackSize
    {
        get
        {
            return currentStackSize;
        }
        set
        {
            currentStackSize = value < 1 ? 1 : (value > itemDefinition.MaxStackSize ? itemDefinition.MaxStackSize : value);  
        }
    }

    // Check if this item belongs to a definition.
    public bool BelongToItemDefinition(ItemDefinition itemDefinition)
    {
        return this.itemDefinition = itemDefinition;
    }
}
