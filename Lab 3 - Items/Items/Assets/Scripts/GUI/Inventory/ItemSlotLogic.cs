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
    // Reference to the Drop Item panel.
    public DropItemPanelLogic dropItemPanel;

    // Reference to the inventory.
    public Inventory inventory;

    // The icon and the name of the item.
    public Image icon;
    public Text itemName;

    // Item slot type.
    public ItemSlotType itemSlotType = ItemSlotType.Backpack;

    // If the item is equipped, the index is -1.
    // Otherwise, return the index of the item in the backpack.
    public int backpackIndex = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Left Click.
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClicked();
        }

        // Right Click.
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClicked();
        }
    }

    // The item slot is in the equipment panel? Unequip the item.
    // The item slot is in the backpack scrollview? Equip the item.
    public void OnLeftClicked()
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
        else if (backpackIndex >= 0)
        {
            if (inventory.backpack[backpackIndex].itemDefinition.IsOfType(ItemType.Consumable))
            {
                inventory.Consume(backpackIndex);
            }
            else
            {
                inventory.Equip(backpackIndex);
            }
        }
    }

    // Drop the item.
    public void OnRightClicked()
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
        else if (backpackIndex >= 0)
        {
            // If the stack size is only 1, drop the item immediately.
            // Otherwise, display the Drop Item panel.
            if (inventory.backpack[backpackIndex].CurrentStackSize == 1)
            {
                inventory.DropItemInBackpack(backpackIndex, 1);
            }
            else
            {
                // Let the Drop Item panel know the player's inventory.
                if (!dropItemPanel.inventory)
                {
                    dropItemPanel.inventory = inventory;
                }

                // Let the Drop Item panel know which item in the backpack is currently selected.
                dropItemPanel.backpackIndex = backpackIndex;

                // Set options of how many items to be dropped.
                dropItemPanel.SetDropDownOptions(inventory.backpack[backpackIndex].CurrentStackSize);

                // Display the Drop Item Panel.
                dropItemPanel.gameObject.SetActive(true);

                // Make the event system select the dropdown of the Drop Item panel.
                dropItemPanel.gameObject.GetComponentInChildren<Dropdown>().Select();
            }
        }
    }
}
