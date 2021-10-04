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
    // Items in the backpack.
    List<ItemInstance> backpack;

    // Armors being equipped by the player and not in the backpack.
    ItemInstance armorHead;
    ItemInstance armorLegs;
    ItemInstance armorArms;
    ItemInstance armorChest;

    // Weapon being equipped by the player and not in the backpack.
    ItemInstance weapon;

    // Add an item to the inventory.
    public void AddToBackPack(ItemInstance newItem)
    {
        // The inventory is empty? Add the new item to the inventory.
        if (backpack.Count == 0)
        {
            backpack.Add(newItem);
        }
        else
        {
            // Find the item in the inventory that matches the new item.
            for (int i = 0; i < backpack.Count; i++)
            {
                if (backpack[i].itemDefinition == newItem.itemDefinition)
                {
                    // The item in the inventory has enough empty space for the new item? 
                    // Merge that item and the new item into 1.
                    if (backpack[i].CurrentStackSize + newItem.CurrentStackSize <= backpack[i].itemDefinition.MaxStackSize)
                    {
                        backpack[i].CurrentStackSize += newItem.CurrentStackSize;
                    }
                    // The item in the inventory is full?
                    // Put the new item in another slot in the inventory.
                    else
                    {
                        int remainingQuantity = backpack[i].itemDefinition.MaxStackSize - backpack[i].CurrentStackSize;
                        backpack[i].CurrentStackSize = backpack[i].itemDefinition.MaxStackSize;
                        newItem.CurrentStackSize -= remainingQuantity;
                        backpack.Add(newItem);
                    }

                    return;
                }
            }
        }
    }

    public void AddToBackPack(ItemDefinition newItem)
    {
        // Create an instance of the new item.
        ItemInstance newItemInstance = new ItemInstance();
        newItemInstance.itemDefinition = newItem;

        AddToBackPack(newItemInstance);
    }
    
    // Remove an item at an index with the specified quantity from the inventory.
    // If the quantity is lower than 0, then the whole item is removed from the inventory.
    public void RemoveFromInventory(int inventoryIndex, int quantity = -1)
    {
        if (quantity < 0 || quantity >= backpack[inventoryIndex].CurrentStackSize)
        {
            backpack.RemoveAt(inventoryIndex);
        }
        else
        {
            backpack[inventoryIndex].CurrentStackSize -= quantity;
        }
    }

    // Equip an item in a slot.
    // If the slot already has another item, swap the position of the two items.
    public void Equip(int inventoryIndex)
    {
        if (inventoryIndex < 0 || inventoryIndex >= backpack.Count)
            return;

        if(backpack[inventoryIndex].itemDefinition is Weapon)
        {
            ItemInstance temp = weapon;
            weapon = backpack[inventoryIndex];
            backpack[inventoryIndex] = temp;
        }
        else if (backpack[inventoryIndex].itemDefinition is Armor)
        {
            ItemInstance temp;
            switch (((Armor)(backpack[inventoryIndex].itemDefinition)).equipmentSlot)
            {
                case InventorySlot.head:
                    temp = armorHead;
                    armorHead = backpack[inventoryIndex];
                    backpack[inventoryIndex] = temp;
                    break;
                case InventorySlot.chest:
                    temp = armorChest;
                    armorChest = backpack[inventoryIndex];
                    backpack[inventoryIndex] = temp;
                    break;
                case InventorySlot.arms:
                    temp = armorArms;
                    armorArms = backpack[inventoryIndex];
                    backpack[inventoryIndex] = temp;
                    break;
                case InventorySlot.feet:
                    temp = armorLegs;
                    armorLegs = backpack[inventoryIndex];
                    backpack[inventoryIndex] = temp;
                    break;
            }
        }
    }

    void Unequip(ref ItemInstance equipmentSlot)
    {
        if (equipmentSlot == null)
            return;

        ItemInstance item = equipmentSlot;
        AddToBackPack(item);
        equipmentSlot = null;
    }

    public void UnequipWeapon()
    {
        Unequip(ref weapon);
    }

    public void UnequipArmorHead()
    {
        Unequip(ref armorHead);
    }

    public void UnequipArmorChest()
    {
        Unequip(ref armorChest);
    }

    public void UnequipArmorArms()
    {
        Unequip(ref armorArms);
    }

    public void UnequipArmorLegs()
    {
        Unequip(ref armorLegs);
    }
}
