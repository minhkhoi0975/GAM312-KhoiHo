/**
 * InventoryGUILogic.cs
 * Description: This script handles the logic of the UI for the player character's inventory.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanelLogic : MonoBehaviour
{
    // Reference to the player character's inventory.
    public Inventory inventory;

    // References to the transforms of the item slots in the equipment panel.
    public Transform headSlotTransform, chestSlotTransform, armsSlotTransform, legsSlotTransform, weaponSlotTransform;

    // Reference to the transform of the content of the backpack scroll view.
    public Transform backpackScrollViewContentTransform;

    // Reference to all item slot buttons.
    List<GameObject> itemSlotButtons = new List<GameObject>();
    public List<GameObject> ItemSlotButtons
    {
        get
        {
            return itemSlotButtons;
        }
    }

    // Prefab for an item slot button.
    public GameObject itemSlotButtonPrefab;

    // Reference to the Equipment panel object.
    public GameObject equipmentPanel;

    // Reference to the Backpack scrollview object.
    public GameObject backpackScrollview;

    // Reference to the Drop Item panel.
    public DropItemPanelLogic dropItemPanel;

    private void Awake()
    {
        if (!dropItemPanel)
        {
            dropItemPanel = GetComponentInChildren<DropItemPanelLogic>();
        }

        if (inventory)
        {
            // When the inventory is updated, make sure that the GUI is also updated.
            inventory.inventoryUpdatedCallback += UpdateGUI;
            UpdateGUI();
        }
    }

    void UpdateGUI()
    {
        if (!inventory)
        {
            Debug.LogError("Cannot find the player's inventory.");
            return;
        }

        // Destroy old item slot buttons.
        foreach(GameObject itemSlotButton in itemSlotButtons)
        {
            Destroy(itemSlotButton);
        }
        itemSlotButtons.Clear();

        // Create new item slot buttons in the equipment panel.
        if (inventory.armorHead)
        {
            itemSlotButtons.Add(CreateItemSlotButton(inventory.armorHead, headSlotTransform, ItemSlotType.Head));
        }
        if (inventory.armorChest)
        {
            itemSlotButtons.Add(CreateItemSlotButton(inventory.armorChest, chestSlotTransform, ItemSlotType.Chest));
        }
        if (inventory.armorArms)
        {
            itemSlotButtons.Add(CreateItemSlotButton(inventory.armorArms, armsSlotTransform, ItemSlotType.Arms));
        }
        if (inventory.armorLegs)
        {
            itemSlotButtons.Add(CreateItemSlotButton(inventory.armorLegs, legsSlotTransform, ItemSlotType.Legs));
        }
        if (inventory.weapon)
        {
            itemSlotButtons.Add(CreateItemSlotButton(inventory.weapon, weaponSlotTransform, ItemSlotType.Weapon));
        }

        // Create new item slot buttons in the backpack scrollview.
        for (int i = 0; i < inventory.backpack.Count; i++)
        {
            itemSlotButtons.Add(CreateItemSlotButton(inventory.backpack[i], backpackScrollViewContentTransform, ItemSlotType.Backpack, i));
        }

        // Make the event system select the first item slot button in the inventory UI.
        if(itemSlotButtons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(itemSlotButtons[0]);
        }
    }

    // Create an item slot button.
    GameObject CreateItemSlotButton(ItemInstance itemInstance, Transform parentTransform, ItemSlotType itemSlotType, int backpackIndex = -1)
    {
        if (!itemInstance)
            return null;

        // Instantiate an item slot.
        GameObject itemSlot = Instantiate(itemSlotButtonPrefab, parentTransform);

        ItemSlotLogic itemSlotLogic = itemSlot.GetComponent<ItemSlotLogic>();

        // Make the item slot reference the Inventory panel.
        itemSlotLogic.inventoryPanel = this;

        // If the item slot type is Backpack, set the index of the item slot to match the index in the character's backpack.
        itemSlotLogic.itemSlotType = itemSlotType;
        if (itemSlotType == ItemSlotType.Backpack)
        {
            itemSlotLogic.backpackIndex = backpackIndex;
        }
        else
        {
            itemSlotLogic.backpackIndex = -1;
        }

        // Set the icon of the item slot.
        itemSlotLogic.icon.sprite = itemInstance.itemDefinition.icon;

        // Set the name of the item slot.
        // If the stack size is greater than 1, add the stack size after the name of the item.
        itemSlotLogic.itemName.text = itemInstance.itemDefinition.name;
        itemSlotLogic.itemName.text += (itemInstance.CurrentStackSize > 1 ? " (" + itemInstance.CurrentStackSize + ")" : "");

        return itemSlot;
    }

    // Make the event system select the first item slot button.
    public void MakeEventSystemSelectFirstSlot()
    {
        if(itemSlotButtons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(itemSlotButtons[0]);

            // Highlight the button to let the player know which button is selected.
            itemSlotButtons[0].GetComponent<Button>().OnSelect(null);
        }
    }
}