/**
 * DropItemPanelLogic.cs
 * Description: This script handles the logic of the drop item panel. This panel allows the player to select how many items to be dropped from the inventory.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropItemPanelLogic : MonoBehaviour
{
    // Reference to the Inventory panel.
    public InventoryPanelLogic inventoryPanel;

    // Reference to the drop quantity dropdown.
    public Dropdown dropQuantityDropDown;

    // The index of the selected item in the backpack.
    public int backpackIndex = -1;

    private void OnEnable()
    {
        // Make the event system select the dropdown.
        EventSystem.current.SetSelectedGameObject(dropQuantityDropDown.gameObject);
    }

    // Set the options for the dropdown.
    public void SetDropDownOptions(int maxDropQuantity)
    {
        // Remove the old options.
        dropQuantityDropDown.ClearOptions();

        // Create options.
        for (int i = 0; i < maxDropQuantity; i++)
        {
            dropQuantityDropDown.options.Add(new Dropdown.OptionData((i + 1).ToString()));
            dropQuantityDropDown.value = i;
        }
    }

    // Drop item.
    public void DropItem()
    {
        // Get the drop quantity from the dropdown.
        int dropQuantity = dropQuantityDropDown.value + 1;

        // Drop item.
        inventoryPanel.inventory.DropItemInBackpack(backpackIndex, dropQuantity);

        // Close the panel.
        gameObject.SetActive(false);

        // Enable the equipment panel and backpack scrollview.
        inventoryPanel.equipmentPanel.SetActive(true);
        inventoryPanel.backpackScrollview.SetActive(true);

        inventoryPanel.MakeEventSystemSelectFirstSlot();
    }

    // Cancel dropping item.
    public void Cancel()
    {
        // Close the panel.
        gameObject.SetActive(false);

        // Enable the equipment panel and backpack scrollview.
        inventoryPanel.equipmentPanel.SetActive(true);
        inventoryPanel.backpackScrollview.SetActive(true);

        inventoryPanel.MakeEventSystemSelectFirstSlot();
    }
}
