using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Called when the inventory is changed (an item is added, removed, equipped, etc.)
    public delegate void InventoryUpdated();
    public InventoryUpdated inventoryUpdatedCallback;

    // Called when an item is equipped.
    public delegate void ItemEquipped(ItemInstance item);
    public ItemEquipped itemEquippedCallback;

    // Called when an item is consumed.
    public delegate void ItemConsumed(ItemInstance item);
    public ItemEquipped itemConsumedCallback;

    // Called when an item is unequipped.
    public delegate void ItemUnequipped(ItemInstance item);
    public ItemEquipped itemUnequippedCallback;

    // Called when an item is picked up.
    public delegate void ItemPickedUp(ItemInstance item);
    public ItemPickedUp itemPickedUpCallback;

    // Called when an item is dropped.
    public delegate void ItemDropped(ItemInstance item);
    public ItemDropped itemDroppedCallback;

    // Reference to the Character component of the game object.
    // Only characters can pick up, equip or consume item.
    public Character character;

    // Starting items.
    public List<ItemInstance> startingItems = new List<ItemInstance>();

    // Items in the backpack.
    public List<ItemInstance> backpack = new List<ItemInstance>();

    [Header("Equipment")]

    // Armors being equipped by the player and not in the backpack.
    public ItemInstance armorHead;
    public ItemInstance armorLegs;
    public ItemInstance armorArms;
    public ItemInstance armorBody;

    // Weapon being equipped by the player and not in the backpack.
    public ItemInstance weapon;

    [Header("Item Dropping")]

    // If the character drops an item, where is the item dropped?
    [SerializeField] Transform dropTransform;

    // Pick-up prefab.
    [SerializeField] GameObject pickupPrefab;

    private void Awake()
    {
        // Check if the game object has Character component.
        if (!character)
        {
            character = GetComponent<Character>();
        }
        if(!character)
        {
            character = GetComponentInChildren<Character>();
        }

        // Add starting items to the backpack.
        foreach(ItemInstance item in startingItems)
        {
            AddToBackPack(item);

            // Try equipping the item.
            Equip(backpack.Count - 1, true);
        }
    }

    // Check whether a backpack index is valid.
    public bool IsBackPackIndexValid(int backpackIndex)
    {
        return backpackIndex >= 0 && backpackIndex < backpack.Count;
    }

    // Add an item to the backpack.
    public void AddToBackPack(ItemInstance newItem)
    {
        if (!newItem || newItem.CurrentStackSize == 0)
            return;

        // Find the item in the inventory that matches the new item.
        for (int i = 0; i < backpack.Count; i++)
        {
            if (backpack[i].itemDefinition == newItem.itemDefinition)
            {
                // The same item in the inventory has enough empty space for the new item? 
                // Merge that item and the new item into 1.
                if (backpack[i].CurrentStackSize + newItem.CurrentStackSize <= backpack[i].itemDefinition.MaxStackSize)
                {
                    backpack[i].CurrentStackSize += newItem.CurrentStackSize;
                    newItem.CurrentStackSize = 0;
                    break;
                }
                // The item in the inventory is full?
                // Move to another slot in the inventory.
                else
                {
                    int insertedQuantity = backpack[i].itemDefinition.MaxStackSize - backpack[i].CurrentStackSize;
                    backpack[i].CurrentStackSize = backpack[i].itemDefinition.MaxStackSize;
                    newItem.CurrentStackSize -= insertedQuantity;
                    //Debug.Log(insertedQuantity + "x" + newItem.itemDefinition.name + " has been added to slot " + i + "." + newItem.CurrentStackSize + " remaining.");
                }
            }
        }

        // The remaining quantity is added at the end of the backpack.
        if (newItem.CurrentStackSize > 0)
        {
            backpack.Add(newItem);
            //Debug.Log(newItem.CurrentStackSize + "x" + newItem.itemDefinition.name + " has been added to slot " + (backpack.Count - 1) + ".");
        }

        inventoryUpdatedCallback?.Invoke();
    }

    public void AddToBackPack(ItemDefinition newItem)
    {
        if (!newItem)
            return;

        // Create an instance of the new item.
        ItemInstance newItemInstance = new ItemInstance(newItem);

        AddToBackPack(newItemInstance);
    }

    // Remove an item at an index with the specified quantity from the inventory.
    // If the quantity is lower than 0, then the whole item is removed from the inventory.
    public void RemoveFromBackPack(int backpackIndex, int quantity = 1)
    {
        if (!IsBackPackIndexValid(backpackIndex) || quantity < 0)
            return;

        // Get the item instance at the index.
        ItemInstance removedItem = null;

        if (quantity >= backpack[backpackIndex].CurrentStackSize)
        {
            removedItem = backpack[backpackIndex];
            backpack.RemoveAt(backpackIndex);
        }
        else
        {
            removedItem = new ItemInstance(backpack[backpackIndex].itemDefinition, quantity);
            backpack[backpackIndex].CurrentStackSize -= quantity;
        }

        inventoryUpdatedCallback?.Invoke();
    }

    // Equip an item in a slot.
    // If onlyEquipIfEmpty is true, the character only equips the item if equipmentSlot is empty. Otherwise, the character unequips the current item in equipmentSlot before equipping a new item. 
    void Equip(int backpackIndex, ref ItemInstance equipmentSlot, bool onlyEquipIfEmpty = false)
    {
        if (!IsBackPackIndexValid(backpackIndex))
            return;

        // Only characters can equip the item.
        if (!character)
            return;

        // Unequip the current item in the equipment slot if onlyEquipIfEmpty is true.
        if (equipmentSlot)
        {
            if (onlyEquipIfEmpty)
                return;
            else
                Unequip(ref equipmentSlot);
        }

        // Equip the selected item in the backpack.
        equipmentSlot = backpack[backpackIndex];
        backpack.RemoveAt(backpackIndex);

        // Make changes to the character's properties.
        equipmentSlot.itemDefinition.OnEquipped(character);

        itemEquippedCallback?.Invoke(equipmentSlot);
        inventoryUpdatedCallback?.Invoke();
    }

    public void Equip(int backpackIndex, bool onlyEquipIfEmpty = false)
    {
        if (!IsBackPackIndexValid(backpackIndex))
            return;

        // Equip a weapon.
        if (backpack[backpackIndex].itemDefinition.IsOfType(ItemType.Weapon))
        {
            Equip(backpackIndex, ref weapon, onlyEquipIfEmpty);
        }

        // Equip an armor.
        else if (backpack[backpackIndex].itemDefinition.IsOfType(ItemType.Armor))
        {
            switch (((Armor)(backpack[backpackIndex].itemDefinition)).armorSlot)
            {
                case ArmorSlot.Head:
                    Equip(backpackIndex, ref armorHead, onlyEquipIfEmpty);
                    break;

                case ArmorSlot.Body:
                    Equip(backpackIndex, ref armorBody, onlyEquipIfEmpty);
                    break;

                case ArmorSlot.Arms:
                    Equip(backpackIndex, ref armorArms, onlyEquipIfEmpty);
                    break;

                case ArmorSlot.Legs:
                    Equip(backpackIndex, ref armorLegs, onlyEquipIfEmpty);
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

        // Put the item back to the backpack.
        ItemInstance item = equipmentSlot;
        AddToBackPack(item);

        equipmentSlot = null;

        itemUnequippedCallback?.Invoke(item);
        inventoryUpdatedCallback?.Invoke();
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
        Unequip(ref armorBody);
    }

    public void UnequipArmorArms()
    {
        Unequip(ref armorArms);
    }

    public void UnequipArmorLegs()
    {
        Unequip(ref armorLegs);
    }

    // Consume an item in the backpack.
    public void Consume(int backpackIndex)
    {
        if (!IsBackPackIndexValid(backpackIndex))
            return;

        // Only characters can consume the item.
        if (!character)
            return;

        ItemInstance item = backpack[backpackIndex];
        if (item)
        {
            if (item.itemDefinition.IsOfType(ItemType.Consumable))
            {
                item.itemDefinition.OnConsumed(character);

                // I have used up the item. Remove it from the inventory.
                if (item.CurrentStackSize == 1)
                {
                    backpack.RemoveAt(backpackIndex);
                }
                // If the item is not used up, decrement its quantity.
                else
                {
                    item.CurrentStackSize--;
                }

                ItemInstance consumedItem = new ItemInstance(item.itemDefinition, 1);
                itemConsumedCallback?.Invoke(consumedItem);
                inventoryUpdatedCallback?.Invoke();
            }
        }
    }

    // Pick up an item
    public void PickUpItem(PickUp pickUp, bool autoEquip = true)
    {
        if (!pickUp)
            return;

        // Only characters can pick up items.
        if (!character)
            return;

        // Create an item instance.
        ItemInstance itemInstance = new ItemInstance(pickUp.ItemDefinition, pickUp.CurrentStackSize);

        // Add the item instance to the backpack.
        AddToBackPack(itemInstance);

        // If autoEquip is true, try equipping the item.
        if (autoEquip)
        {
            Equip(backpack.Count - 1, true);
        }

        // Create another copy of item instance for itemPickedUpCallback since currentStackSize of itemInstance may have been changed.
        ItemInstance callbackItemInstance = new ItemInstance(pickUp.ItemDefinition, pickUp.CurrentStackSize);
        itemPickedUpCallback?.Invoke(callbackItemInstance);

        // Destroy the pick-up object.
        Destroy(pickUp.gameObject);   
    }

    // Drop an item in the backpack.
    public void DropItemInBackpack(int backpackIndex, int quantity = -1)
    {
        if (!IsBackPackIndexValid(backpackIndex))
            return;

        // Ray cast to check if there is an obstacle in front of this object.
        // If there is, drop the item at the hit position. Otherwise, drop the item at dropTransform.
        Vector3 dropPosition;
        RaycastHit hitInfo;
        Vector3 rayCastStartPosition = transform.position + new Vector3(0.0f, 1.2f, 0.0f);
        bool rayCastHit = Physics.Raycast(rayCastStartPosition, dropTransform.position - rayCastStartPosition, out hitInfo, Vector3.Distance(transform.position, dropTransform.position));

        if (rayCastHit)
        {
            dropPosition = hitInfo.point;
        }
        else
        {
            dropPosition = dropTransform.position;
        }

        // Create an item instance for the pick-up.
        ItemInstance pickUpInfo = new ItemInstance(backpack[backpackIndex].itemDefinition);
        if (quantity < 0 || quantity >= backpack[backpackIndex].CurrentStackSize)
        {
            pickUpInfo.CurrentStackSize = backpack[backpackIndex].CurrentStackSize;
            backpack.RemoveAt(backpackIndex);
        }
        else
        {
            pickUpInfo.CurrentStackSize = quantity;
            backpack[backpackIndex].CurrentStackSize -= quantity;
        }

        // Create a pick-up object.
        GameObject pickUpObject = Instantiate(pickupPrefab, dropPosition, Quaternion.identity);
        pickUpObject.GetComponent<PickUp>().SetPickUp(pickUpInfo);

        Debug.Log("Dropped " + pickUpInfo.CurrentStackSize + "x" + pickUpInfo.itemDefinition.name);

        itemDroppedCallback?.Invoke(pickUpInfo);
        inventoryUpdatedCallback?.Invoke();
    }

    // Drop an item in an equipment slot.
    void DropItemInEquipmentSlot(ref ItemInstance equipmentSlot)
    {
        if (!equipmentSlot)
            return;

        // Unequip the item.
        Unequip(ref equipmentSlot);

        // Drop the item.
        DropItemInBackpack(backpack.Count - 1);
    }

    public void DropWeapon()
    {
        DropItemInEquipmentSlot(ref weapon);
    }

    public void DropArmorHead()
    {
        DropItemInEquipmentSlot(ref armorHead);
    }

    public void DropArmorChest()
    {
        DropItemInEquipmentSlot(ref armorBody);
    }

    public void DropArmorArms()
    {
        DropItemInEquipmentSlot(ref armorArms);
    }

    public void DropArmorLegs()
    {
        DropItemInEquipmentSlot(ref armorLegs);
    }
}