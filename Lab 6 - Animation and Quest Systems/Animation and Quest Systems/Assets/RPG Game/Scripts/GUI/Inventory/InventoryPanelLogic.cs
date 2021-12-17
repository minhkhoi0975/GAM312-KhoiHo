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

    // Prefab for an item slot.
    public GameObject itemSlotPrefab;

    // Reference to all item slots in the equipment panel.
    GameObject headItemSlot;
    public GameObject HeadItemSlot
    {
        get
        {
            return headItemSlot;
        }
    }

    GameObject bodyItemSlot;
    public GameObject BodyItemSlot
    {
        get
        {
            return bodyItemSlot;
        }
    }

    GameObject armsItemSlot;
    public GameObject ArmsItemSlot
    {
        get
        {
            return armsItemSlot;
        }
    }

    GameObject legsItemSlot;
    public GameObject LegsItemSlot
    {
        get
        {
            return legsItemSlot;
        }
    }

    GameObject weaponItemSlot;
    public GameObject WeaponItemSlot
    {
        get
        {
            return weaponItemSlot;
        }
    }

    // Reference to all item slots in the backpack scrollview.
    List<GameObject> backpackItemSlots = new List<GameObject>();
    public List<GameObject> BackpackItemSlots
    {
        get
        {
            return backpackItemSlots;
        }
    }

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

    private void OnEnable()
    {
        // Enable the equipment and backpack panels.
        equipmentPanel.gameObject.SetActive(true);
        backpackScrollview.gameObject.SetActive(true);

        // Disable the drop item panel.
        dropItemPanel.gameObject.SetActive(false);

        // Make the event system select the first button.
        MakeEventSystemSelectFirstSlot();
    }

    void UpdateGUI()
    {
        if (!inventory)
        {
            Debug.LogError("Cannot find the player's inventory.");
            return;
        }

        // Destroy old item slots.
        DestroyItemSlots();

        // Create new item slots in the equipment panel.
        CreateItemSlots();

        // If the Inventory panel is active, make the event system select the first item slot button.
        if (gameObject.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(null);
            MakeEventSystemSelectFirstSlot();
        }
    }

    void DestroyItemSlots()
    {
        // Equipment
        if (headItemSlot)
        {
            Destroy(headItemSlot);
            headItemSlot = null;
        }
        if (bodyItemSlot)
        {
            Destroy(bodyItemSlot);
            bodyItemSlot = null;
        }
        if (armsItemSlot)
        {
            Destroy(armsItemSlot);
            armsItemSlot = null;
        }
        if (legsItemSlot)
        {
            Destroy(legsItemSlot);
            legsItemSlot = null;
        }
        if (weaponItemSlot)
        {
            Destroy(weaponItemSlot);
            weaponItemSlot = null;
        }

        // Backpack
        foreach (GameObject itemSlot in backpackItemSlots)
        {
            Destroy(itemSlot);
        }
        backpackItemSlots.Clear();
    }

    void CreateItemSlots()
    {
        if (inventory.armorHead)
        {
            headItemSlot = CreateItemSlotButton(inventory.armorHead, headSlotTransform, ItemSlotType.Head);
        }
        if (inventory.armorBody)
        {
            bodyItemSlot = CreateItemSlotButton(inventory.armorBody, chestSlotTransform, ItemSlotType.Chest);
        }
        if (inventory.armorArms)
        {
            armsItemSlot = CreateItemSlotButton(inventory.armorArms, armsSlotTransform, ItemSlotType.Arms);
        }
        if (inventory.armorLegs)
        {
            legsItemSlot = CreateItemSlotButton(inventory.armorLegs, legsSlotTransform, ItemSlotType.Legs);
        }
        if (inventory.weapon)
        {
            weaponItemSlot = CreateItemSlotButton(inventory.weapon, weaponSlotTransform, ItemSlotType.Weapon);
        }

        // Create new item slots in the backpack scrollview.
        for (int i = 0; i < inventory.backpack.Count; i++)
        {
            backpackItemSlots.Add(CreateItemSlotButton(inventory.backpack[i], backpackScrollViewContentTransform, ItemSlotType.Backpack, i));
        }
    }

    // Create an item slot button.
    GameObject CreateItemSlotButton(ItemInstance itemInstance, Transform parentTransform, ItemSlotType itemSlotType, int backpackIndex = -1)
    {
        if (!itemInstance)
            return null;

        // Instantiate an item slot.
        GameObject itemSlot = Instantiate(itemSlotPrefab, parentTransform);

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
        if (headItemSlot)
        {
            EventSystem.current.SetSelectedGameObject(headItemSlot);
            headItemSlot.GetComponent<Button>().OnSelect(null);
        }
        else if (bodyItemSlot)
        {
            EventSystem.current.SetSelectedGameObject(bodyItemSlot);
            bodyItemSlot.GetComponent<Button>().OnSelect(null);
        }
        else if (armsItemSlot)
        {
            EventSystem.current.SetSelectedGameObject(armsItemSlot);
            armsItemSlot.GetComponent<Button>().OnSelect(null);
        }
        else if (legsItemSlot)
        {
            EventSystem.current.SetSelectedGameObject(legsItemSlot);
            legsItemSlot.GetComponent<Button>().OnSelect(null);
        }
        else if (weaponItemSlot)
        {
            EventSystem.current.SetSelectedGameObject(weaponItemSlot);
            weaponItemSlot.GetComponent<Button>().OnSelect(null);
        }
        else
        {
            MakeEventSystemSelectBackpackItemSlot(0);
        }
    }

    // Make the event system select a particular item slot button.
    public void MakeEventSystemSelectBackpackItemSlot(int itemSlotIndex)
    {
        if (backpackItemSlots.Count > 0)
        {
            int index;

            if (itemSlotIndex < 0)
                index = 0;
            else if (itemSlotIndex >= backpackItemSlots.Count)
                index = backpackItemSlots.Count - 1;
            else
                index = itemSlotIndex;

            EventSystem.current.SetSelectedGameObject(backpackItemSlots[index]);

            // Highlight the button to let the player know which button is selected.
            backpackItemSlots[0].GetComponent<Button>().OnSelect(null);
        }
    }
}