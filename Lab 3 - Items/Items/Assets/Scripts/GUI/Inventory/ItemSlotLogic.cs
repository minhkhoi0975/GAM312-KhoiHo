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

public class ItemSlotLogic : MonoBehaviour
{
    // Reference to the Inventory panel.
    public InventoryPanelLogic inventoryPanel;

    // The icon and the name of the item.
    public Image icon;
    public Text itemName;

    // Item slot type.
    public ItemSlotType itemSlotType = ItemSlotType.Backpack;

    // If the item is equipped, the index is -1.
    // Otherwise, return the index of the item in the backpack.
    public int backpackIndex = 0;

    // The item slot is in the equipment panel? Unequip the item.
    // The item slot is in the backpack scrollview? Equip the item.
    public void OnLeftClicked()
    {
        if (itemSlotType == ItemSlotType.Head)
        {
            inventoryPanel.inventory.UnequipArmorHead();
        }
        else if (itemSlotType == ItemSlotType.Chest)
        {
            inventoryPanel.inventory.UnequipArmorChest();
        }
        else if (itemSlotType == ItemSlotType.Arms)
        {
            inventoryPanel.inventory.UnequipArmorArms();
        }
        else if (itemSlotType == ItemSlotType.Legs)
        {
            inventoryPanel.inventory.UnequipArmorLegs();
        }
        else if (itemSlotType == ItemSlotType.Weapon)
        {
            inventoryPanel.inventory.UnequipWeapon();
        }
        else if (backpackIndex >= 0)
        {
            if (inventoryPanel.inventory.backpack[backpackIndex].itemDefinition.IsOfType(ItemType.Consumable))
            {
                inventoryPanel.inventory.Consume(backpackIndex);
            }
            else
            {
                inventoryPanel.inventory.Equip(backpackIndex);
            }
        }
    }

    // Drop the item.
    public void OnRightClicked()
    {
        if (itemSlotType == ItemSlotType.Head)
        {
            inventoryPanel.inventory.DropArmorHead();
        }
        else if (itemSlotType == ItemSlotType.Chest)
        {
            inventoryPanel.inventory.DropArmorChest();
        }
        else if (itemSlotType == ItemSlotType.Arms)
        {
            inventoryPanel.inventory.DropArmorArms();
        }
        else if (itemSlotType == ItemSlotType.Legs)
        {
            inventoryPanel.inventory.DropArmorLegs();
        }
        else if (itemSlotType == ItemSlotType.Weapon)
        {
            inventoryPanel.inventory.DropWeapon();
        }
        else if (backpackIndex >= 0)
        {
            // If the stack size is only 1, drop the item immediately.
            // Otherwise, display the Drop Item panel.
            if (inventoryPanel.inventory.backpack[backpackIndex].CurrentStackSize == 1)
            {
                inventoryPanel.inventory.DropItemInBackpack(backpackIndex, 1);
            }
            else
            {
                // Let the Drop Item panel know which item in the backpack is currently selected.
                inventoryPanel.dropItemPanel.backpackIndex = backpackIndex;

                // Set options of how many items to be dropped.
                inventoryPanel.dropItemPanel.SetDropDownOptions(inventoryPanel.inventory.backpack[backpackIndex].CurrentStackSize);

                // Disable the Equipment and Backpack panel objects.
                inventoryPanel.equipmentPanel.SetActive(false);
                inventoryPanel.backpackScrollview.SetActive(false);

                // Display the Drop Item Panel.
                inventoryPanel.dropItemPanel.gameObject.SetActive(true);

                // Make the event system select the dropdown of the Drop Item panel.
                EventSystem.current.SetSelectedGameObject(inventoryPanel.dropItemPanel.dropQuantityDropDown.gameObject);
            }
        }
    }
}
