using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventorySlot
{
    head,
    chest,
    arms,
    feet
}

public class Inventory : MonoBehaviour
{
    List<ItemInstance> items;
    ItemInstance head;
    ItemInstance feet;
    ItemInstance hand;
    ItemInstance chest;

    // Add an item to the inventory.
    public void AddToInventory(ItemInstance newItem)
    {
        // The inventory is empty? Add the new item to the inventory.
        if (items.Count == 0)
        {
            items.Add(newItem);
        }
        else
        {
            // Find the item in the inventory that matches the new item.
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemDefinition == newItem.itemDefinition)
                {
                    // The item in the inventory has enough empty space for the new item? 
                    // Merge that item and the new item into 1.
                    if (items[i].CurrentStackSize + newItem.CurrentStackSize <= items[i].itemDefinition.MaxStackSize)
                    {
                        items[i].CurrentStackSize += newItem.CurrentStackSize;
                    }
                    // The item in the inventory is full?
                    // Put the new item in another slot in the inventory.
                    else
                    {
                        int remainingQuantity = items[i].itemDefinition.MaxStackSize - items[i].CurrentStackSize;
                        items[i].CurrentStackSize = items[i].itemDefinition.MaxStackSize;
                        newItem.CurrentStackSize -= remainingQuantity;
                        items.Add(newItem);
                    }

                    return;
                }
            }
        }
    }

    public void AddToInventory(ItemDefinition newItem)
    {
        // Create an instance of the new item.
        ItemInstance newItemInstance = new ItemInstance();
        newItemInstance.itemDefinition = newItem;

        AddToInventory(newItemInstance);
    }
    
    // Remove an item at an index with the specified quantity from the inventory.
    // If the quantity is lower than 0, then the whole item is removed from the inventory.
    public void RemoveFromInventory(int inventoryIndex, int quantity = -1)
    {
        if (quantity < 0 || quantity >= items[inventoryIndex].CurrentStackSize)
        {
            items.RemoveAt(inventoryIndex);
        }
        else
        {
            items[inventoryIndex].CurrentStackSize -= quantity;
        }
    }
}
