/**
 * InventoryGUILogic.cs
 * Description: This script handles the logic of the UI for the player character's inventory.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGUILogic : MonoBehaviour
{
    // Reference to the player character's inventory.
    public Inventory inventory;

    // References to the equipment slots.
    public GameObject headSlot, chestSlot, armsSlot, legsSlot, weaponSlot;

    // Reference to the content of the backpack scroll view.
    public GameObject backpackScrollViewContent;

    // Prefab for an item slot.
    public GameObject itemSlotPrefab;

    // Start is called before the first frame update
    void Start()
    {
        UpdateGUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();
    }

    void UpdateGUI()
    {
        if (!inventory)
        {
            Debug.LogError("Cannot find the player's inventory.");
            return;
        }

        // Update the equipment.
        // + Destroy the children of equipment objects.
        // + Create new childrens of equipment objects.

        foreach (Transform childTransform in headSlot.transform)
        {
            if (childTransform.gameObject.GetComponent<ItemSlotLogic>())
            {
                Destroy(childTransform.gameObject);
            }
        }

        foreach (Transform childTransform in chestSlot.transform)
        {
            if (childTransform.gameObject.GetComponent<ItemSlotLogic>())
            {
                Destroy(childTransform.gameObject);
            }
        }

        foreach (Transform childTransform in armsSlot.transform)
        {
            if (childTransform.gameObject.GetComponent<ItemSlotLogic>())
            {
                Destroy(childTransform.gameObject);
            }
        }

        foreach (Transform childTransform in legsSlot.transform)
        {
            if (childTransform.gameObject.GetComponent<ItemSlotLogic>())
            {
                Destroy(childTransform.gameObject);
            }
        }

        foreach (Transform childTransform in weaponSlot.transform)
        {
            if (childTransform.gameObject.GetComponent<ItemSlotLogic>())
            {
                Destroy(childTransform.gameObject);
            }
        }

        CreateItemSlot(inventory.armorHead, headSlot.transform, ItemSlotType.Head, -1);
        CreateItemSlot(inventory.armorChest, chestSlot.transform, ItemSlotType.Chest, -1);
        CreateItemSlot(inventory.armorArms, armsSlot.transform, ItemSlotType.Arms, -1);
        CreateItemSlot(inventory.armorLegs, legsSlot.transform, ItemSlotType.Legs, -1);
        CreateItemSlot(inventory.weapon, weaponSlot.transform, ItemSlotType.Weapon, -1);

        // Update the backpack.
        // + Destroy the children of Content in BackpackScrollView.
        // + Create new childrens of Content in BackpackScrollView.

        foreach (Transform childTransform in backpackScrollViewContent.transform)
        {
            Destroy(childTransform.gameObject);
        }

        for (int i = 0; i < inventory.backpack.Count; i++)
        {
            /*
            // Create an item slot.
            GameObject itemSlot = Instantiate(itemSlotPrefab, backpackScrollViewContent.transform);

            ItemSlotLogic itemSlotLogic = itemSlot.GetComponent<ItemSlotLogic>();

            // Set the slot type.
            itemSlotLogic.itemSlotType = ItemSlotType.Backpack;

            // Set the index of the item slot.
            itemSlotLogic.index = i;

            // Set the icon of the item slot.
            itemSlotLogic.icon.sprite = inventory.backpack[i].itemDefinition.icon;

            // Set the name of the item slot.
            // If the stack size is greater than 1, add the stack size after the name of the item.
            itemSlotLogic.itemName.text = inventory.backpack[i].itemDefinition.name;
            itemSlotLogic.itemName.text += (inventory.backpack[i].CurrentStackSize > 1 ? " (" + inventory.backpack[i].CurrentStackSize + ")" : "");
            */

            CreateItemSlot(inventory.backpack[i], backpackScrollViewContent.transform, ItemSlotType.Backpack, i);
        }
    }

    // Create an item slot.
    GameObject CreateItemSlot(ItemInstance itemInstance, Transform parentTransform, ItemSlotType itemSlotType, int index)
    {
        if (!itemInstance)
            return null;

        // Instantiate an item slot.
        GameObject itemSlot = Instantiate(itemSlotPrefab, parentTransform);

        ItemSlotLogic itemSlotLogic = itemSlot.GetComponent<ItemSlotLogic>();

        // If the item slot type is Backpack, set the index of the item slot to match the index in the character's backpack.
        itemSlotLogic.itemSlotType = itemSlotType;
        if (itemSlotType == ItemSlotType.Backpack)
        {
            itemSlotLogic.index = index;
        }
        else
        {
            itemSlotLogic.index = -1;
        }

        // Set the icon of the item slot.
        itemSlotLogic.icon.sprite = itemInstance.itemDefinition.icon;

        // Set the name of the item slot.
        // If the stack size is greater than 1, add the stack size after the name of the item.
        itemSlotLogic.itemName.text = itemInstance.itemDefinition.name;
        itemSlotLogic.itemName.text += (itemInstance.CurrentStackSize > 1 ? " (" + itemInstance.CurrentStackSize + ")" : "");

        return itemSlot;
    }
}