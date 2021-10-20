/**
 * ItemSlotLogic.cs
 * Description: This script handles the logic of an item slot in the player character's inventory.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ItemSlotType
{
    Head,
    Chest,
    Arms,
    Legs,
    Weapon,
    Backpack
}

public class ItemSlotLogic : MonoBehaviour, IPointerClickHandler
{
    // Reference to the inventory.
    public Inventory inventory;

    // References to the icon and the name of the item.
    public Image icon;
    public Text itemName;

    // Item slot type.
    public ItemSlotType itemSlotType = ItemSlotType.Backpack;

    // If the item is equipped, the index is -1.
    // Otherwise, return the index of the item in the backpack.
    public int index = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Left Click.
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (itemSlotType == ItemSlotType.Head)
            {
                inventory.UnequipArmorHead();
            }
            else if (itemSlotType == ItemSlotType.Chest)
            {
                inventory.UnequipArmorChest();
            }
            else if (itemSlotType == ItemSlotType.Arms)
            {
                inventory.UnequipArmorArms();
            }
            else if (itemSlotType == ItemSlotType.Legs)
            {
                inventory.UnequipArmorLegs();
            }
            else if (itemSlotType == ItemSlotType.Weapon)
            {
                inventory.UnequipWeapon();
            }
            else if (index >= 0)
            {
                if (inventory.backpack[index].itemDefinition.IsOfType(ItemType.Consumable))
                {
                    inventory.Consume(index);
                }
                else
                {
                    inventory.Equip(index);
                }
            }
        }

        // Right Click.
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (itemSlotType == ItemSlotType.Head)
            {
                inventory.DropArmorHead();
            }
            else if (itemSlotType == ItemSlotType.Chest)
            {
                inventory.DropArmorChest();
            }
            else if (itemSlotType == ItemSlotType.Arms)
            {
                inventory.DropArmorArms();
            }
            else if (itemSlotType == ItemSlotType.Legs)
            {
                inventory.DropArmorLegs();
            }
            else if (itemSlotType == ItemSlotType.Weapon)
            {
                inventory.DropWeapon();
            }
            else if (index >= 0)
            {
                inventory.DropItemInPackack(index);
            }
        }
    }
}
