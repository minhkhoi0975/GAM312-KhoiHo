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
    public List<ItemInstance> backpack;

    // Armors being equipped by the player and not in the backpack.
    public ItemInstance armorHead;
    public ItemInstance armorLegs;
    public ItemInstance armorArms;
    public ItemInstance armorChest;

    // Weapon being equipped by the player and not in the backpack.
    public ItemInstance weapon;

    // Add an item to the backpack.
    public void AddToBackPack(ItemInstance newItem)
    {
        if (!newItem)
            return;

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
                    return;
                }
                // The item in the inventory is full?
                // Put the new item in another slot in the inventory.
                else
                {
                    int remainingQuantity = backpack[i].itemDefinition.MaxStackSize - backpack[i].CurrentStackSize;
                    backpack[i].CurrentStackSize = backpack[i].itemDefinition.MaxStackSize;
                    newItem.CurrentStackSize -= remainingQuantity;
                }
            }
        }

        // The remaining quantity is added at the end of the backpack.
        backpack.Add(newItem);
    }

    public void AddToBackPack(ItemDefinition newItem)
    {
        if (!newItem)
            return;

        // Create an instance of the new item.
        ItemInstance newItemInstance = new ItemInstance();
        newItemInstance.itemDefinition = newItem;

        AddToBackPack(newItemInstance);
    }

    // Remove an item at an index with the specified quantity from the inventory.
    // If the quantity is lower than 0, then the whole item is removed from the inventory.
    public void RemoveFromBackPack(int backpackIndex, int quantity = -1)
    {
        if (quantity < 0 || quantity >= backpack[backpackIndex].CurrentStackSize)
        {
            backpack.RemoveAt(backpackIndex);
        }
        else
        {
            backpack[backpackIndex].CurrentStackSize -= quantity;
        }
    }

    // Equip an item in a slot.
    void Equip(int backpackIndex, ref ItemInstance equipmentSlot)
    {
        if (backpackIndex < 0 || backpackIndex >= backpack.Count)
            return;

        // Unequip the current item in the equipment slot.
        if(equipmentSlot)
        {
            Unequip(ref equipmentSlot);
        }

        // Equip the selected item in the backpack.
        equipmentSlot = backpack[backpackIndex];
        backpack.RemoveAt(backpackIndex);

        // Make changes to character's properties.
        equipmentSlot.itemDefinition.OnEquipped(GetComponent<Character>());
    }

    public void Equip(int backpackIndex)
    {
        if (backpackIndex < 0 || backpackIndex >= backpack.Count)
            return;

        // Equip a weapon.
        if (backpack[backpackIndex].itemDefinition is Weapon)
        {
            Equip(backpackIndex, ref weapon);
        }

        // Equip an armor.
        else if (backpack[backpackIndex].itemDefinition is Armor)
        {
            switch (((Armor)(backpack[backpackIndex].itemDefinition)).equipmentSlot)
            {
                case InventorySlot.head:
                    Equip(backpackIndex, ref armorHead);
                    break;

                case InventorySlot.chest:
                    Equip(backpackIndex, ref armorChest);
                    break;

                case InventorySlot.arms:
                    Equip(backpackIndex, ref armorArms);
                    break;

                case InventorySlot.feet:
                    Equip(backpackIndex, ref armorLegs);
                    break;
            }
        }
    }

    // Unequip an item.
    void Unequip(ref ItemInstance equipmentSlot)
    {
        if (equipmentSlot == null)
            return;

        // Remove attribute bonuses from the item.
        equipmentSlot.itemDefinition.OnUnequipped(GetComponent<Character>());

        // But the item back to the backpack.
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