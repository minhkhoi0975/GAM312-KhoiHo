using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Called when the inventory is changed (an item is added, removed, equipped, etc.)
    public delegate void InventoryUpdated();
    public InventoryUpdated inventoryUpdatedCallback;

    // Called when an item is added to the inventory.
    public delegate void ItemAddedToInventory(ItemDefinition item, int quantity);
    public ItemAddedToInventory itemAddedToInventoryCallback;

    // Called when an item is removed from the inventory.
    public delegate void ItemRemovedFromIventory(ItemDefinition removedItem, int quantity);
    public ItemRemovedFromIventory itemRemovedFromInventoryCallback;

    // Called when an item is equipped.
    public delegate void ItemEquipped(ItemDefinition equippedItem);
    public ItemEquipped itemEquippedCallback;

    // Called when an item is unequipped.
    public delegate void ItemUnequipped(ItemDefinition unequippedItem);
    public ItemEquipped itemUnequippedCallback;

    // Called when an item is consumed.
    public delegate void ItemConsumed(ItemDefinition consumedItem);
    public ItemEquipped itemConsumedCallback;

    // Called when an item is picked up.
    public delegate void ItemPickedUp(ItemDefinition pickedUpItem, int quantity);
    public ItemPickedUp itemPickedUpCallback;

    // Called when an item is dropped.
    public delegate void ItemDropped(ItemDefinition droppedItem, int quantity);
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
    public ItemInstance armorBody;
    public ItemInstance armorArms;
    public ItemInstance armorLegs;

    // Weapon being equipped by the player and not in the backpack.
    public ItemInstance weapon;

    [Header("Equipment Visuals")]
    public Transform armorHeadTransform;
    public Transform armorBodyTransform;
    public Transform armorLeftArmTransform;
    public Transform armorRightArmTransform;
    public Transform armorLeftLegTransform;
    public Transform armorRightLegTransform;

    public Transform weaponTransform;

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
        if (!character)
        {
            character = GetComponentInChildren<Character>();
        }

        // Add starting items to the backpack.
        foreach (ItemInstance item in startingItems)
        {
            AddToBackpack(item);

            // Try equipping the item.
            Equip(backpack.Count - 1, true);
        }
    }

    // Check whether a backpack index is valid.
    public bool IsBackpackIndexValid(int backpackIndex)
    {
        return backpackIndex >= 0 && backpackIndex < backpack.Count;
    }

    // Add an item to the backpack.
    public void AddToBackpack(ItemDefinition newItem, int quantity = 1)
    {
        if (!newItem || quantity <= 0)
            return;

        int remainingQuantity = quantity;

        // Find the item slots in the inventory that matches the new item in order to add up that slot's stack size.
        for (int i = 0; i < backpack.Count && remainingQuantity > 0; i++)
        {
            if (backpack[i] == newItem)
            {
                // The same item in the inventory has enough empty space for the new item? 
                // Merge that item and the new item into 1.
                if (backpack[i].CurrentStackSize + remainingQuantity <= backpack[i].itemDefinition.MaxStackSize)
                {
                    backpack[i].CurrentStackSize += remainingQuantity;
                    remainingQuantity = 0;
                }

                // The item in the inventory is full?
                // Move to another slot in the inventory.
                else
                {
                    int insertedQuantity = backpack[i].itemDefinition.MaxStackSize - backpack[i].CurrentStackSize;
                    backpack[i].CurrentStackSize = backpack[i].itemDefinition.MaxStackSize;
                    remainingQuantity -= insertedQuantity;
                }
            }
        }

        // The remaining quantity is added in the new slot.
        if (remainingQuantity > 0)
        {
            ItemInstance newItemSlot = new ItemInstance(newItem, remainingQuantity);
            backpack.Add(newItemSlot);
        }

        itemAddedToInventoryCallback?.Invoke(newItem, quantity);
        inventoryUpdatedCallback?.Invoke();
    }

    public void AddToBackpack(ItemInstance newItem)
    {
        AddToBackpack(newItem.itemDefinition, newItem.CurrentStackSize);
    }

    // Remove an item at a backpack index with the specified quantity from the inventory.
    public void RemoveFromBackpack(int backpackIndex, int quantity = 1)
    {
        if (!IsBackpackIndexValid(backpackIndex) || quantity <= 0)
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

        itemRemovedFromInventoryCallback?.Invoke(removedItem.itemDefinition, removedItem.CurrentStackSize);
        inventoryUpdatedCallback?.Invoke();
    }

    // Remove an item with the specified quantity.
    public void RemoveFromBackpack(ItemDefinition item, int quantity = 1)
    {
        if (quantity <= 0)
            return;

        // How many items left do we have to remove?
        int quantityToRemove = quantity;

        // The real number of items we removed.
        int realRemovedQuantity = 0;

        for (int i = backpack.Count - 1; i >= 0 && quantityToRemove > 0; i--)
        {
            if (backpack[i] == item)
            {
                if (quantityToRemove >= backpack[i].CurrentStackSize)
                {
                    quantityToRemove -= backpack[i].CurrentStackSize;
                    realRemovedQuantity += backpack[i].CurrentStackSize;
                    backpack.RemoveAt(i);
                }
                else
                {
                    backpack[i].CurrentStackSize -= quantityToRemove;
                    realRemovedQuantity += quantityToRemove;
                    quantityToRemove = 0;
                }
            }
        }

        itemRemovedFromInventoryCallback?.Invoke(item, realRemovedQuantity);
        inventoryUpdatedCallback?.Invoke();
    }

    // Equip an item in a slot.
    // If onlyEquipIfEmpty is true, the character only equips the item if equipmentSlot is null. Otherwise, the character unequips the current item in equipmentSlot before equipping a new item. 
    void EquipToSlot(int backpackIndex, ref ItemInstance equipmentSlot, bool onlyEquipIfEmpty = false)
    {
        if (!IsBackpackIndexValid(backpackIndex))
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
        if (!IsBackpackIndexValid(backpackIndex))
            return;

        // Equip a weapon.
        if (backpack[backpackIndex].itemDefinition.IsOfType(ItemType.Weapon))
        {
            EquipToSlot(backpackIndex, ref weapon, onlyEquipIfEmpty);
            UpdateVisual(weapon, weaponTransform);
            character.Animator.runtimeAnimatorController = ((Weapon)weapon).animatorOverrideController;
        }

        // Equip an armor.
        else if (backpack[backpackIndex].itemDefinition.IsOfType(ItemType.Armor))
        {
            switch (((Armor)(backpack[backpackIndex].itemDefinition)).armorSlot)
            {
                case ArmorSlot.Head:
                    EquipToSlot(backpackIndex, ref armorHead, onlyEquipIfEmpty);
                    UpdateVisual(armorHead, armorHeadTransform);
                    break;

                case ArmorSlot.Body:
                    EquipToSlot(backpackIndex, ref armorBody, onlyEquipIfEmpty);
                    UpdateVisual(armorBody, armorBodyTransform);
                    break;

                case ArmorSlot.Arms:
                    EquipToSlot(backpackIndex, ref armorArms, onlyEquipIfEmpty);
                    UpdateVisual(armorArms, armorLeftArmTransform);
                    UpdateVisual(armorArms, armorRightArmTransform);
                    break;

                case ArmorSlot.Legs:
                    EquipToSlot(backpackIndex, ref armorLegs, onlyEquipIfEmpty);
                    UpdateVisual(armorLegs, armorLeftLegTransform);
                    UpdateVisual(armorLegs, armorRightLegTransform);
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
        backpack.Add(item);

        equipmentSlot = null;

        itemUnequippedCallback?.Invoke(item);
        inventoryUpdatedCallback?.Invoke();
    }

    public void UnequipWeapon()
    {
        Unequip(ref weapon);
        UpdateVisual(weapon, weaponTransform);
        character.Animator.runtimeAnimatorController = character.DefaultAnimatorController;
    }

    public void UnequipArmorHead()
    {
        Unequip(ref armorHead);
        UpdateVisual(armorHead, armorHeadTransform);
    }

    public void UnequipArmorChest()
    {
        Unequip(ref armorBody);
        UpdateVisual(armorBody, armorBodyTransform);
    }

    public void UnequipArmorArms()
    {
        Unequip(ref armorArms);
        UpdateVisual(armorArms, armorLeftArmTransform);
        UpdateVisual(armorArms, armorRightArmTransform);
    }

    public void UnequipArmorLegs()
    {
        Unequip(ref armorLegs);
        UpdateVisual(armorLegs, armorLeftLegTransform);
        UpdateVisual(armorLegs, armorRightLegTransform);
    }

    // Update the visual of an equipment slot.
    void UpdateVisual(ItemInstance equipmentSlot, Transform equipmentTransform)
    {
        if (!equipmentTransform)
            return;

        // Destroy the old visual.
        foreach (Transform childTransform in equipmentTransform)
        {
            Destroy(childTransform.gameObject);
        }

        // Instantiate a new visual.
        if (equipmentSlot)
        {
            GameObject newEquipmentVisual = Instantiate(equipmentSlot.itemDefinition.mesh, equipmentTransform);
        }
    }

    // Consume an item in the backpack.
    public void Consume(int backpackIndex)
    {
        if (!IsBackpackIndexValid(backpackIndex))
            return;

        // Only characters can consume the item.
        if (!character)
            return;

        if (backpack[backpackIndex] && backpack[backpackIndex].itemDefinition.IsOfType(ItemType.Consumable))
        {
            ItemDefinition consumedItem = backpack[backpackIndex];

            consumedItem.OnConsumed(character);

            // I have used up the item. Remove it from the inventory.
            if (backpack[backpackIndex].CurrentStackSize == 1)
            {
                backpack.RemoveAt(backpackIndex);
            }
            // If the item is not used up, decrement its quantity.
            else
            {
                backpack[backpackIndex].CurrentStackSize--;
            }

            itemRemovedFromInventoryCallback?.Invoke(consumedItem, 1);
            itemConsumedCallback?.Invoke(consumedItem);
            inventoryUpdatedCallback?.Invoke();
        }
    }

    public void Consume(ItemDefinition item)
    {
        for (int i = 0; i < backpack.Count; i++)
        {
            if (backpack[i] == item && backpack[i].itemDefinition.IsOfType(ItemType.Consumable))
            {
                Consume(i);
                return;
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
        AddToBackpack(itemInstance);

        // If autoEquip is true, try equipping the item.
        if (autoEquip)
        {
            Equip(backpack.Count - 1, true);
        }

        itemPickedUpCallback?.Invoke(pickUp.ItemDefinition, pickUp.CurrentStackSize);

        // Destroy the pick-up object.
        Destroy(pickUp.gameObject);
    }

    // Drop an item in the backpack.
    public void DropItemInBackpack(int backpackIndex, int quantity = -1)
    {
        if (!IsBackpackIndexValid(backpackIndex))
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

        //Debug.Log("Dropped " + pickUpInfo.CurrentStackSize + "x" + pickUpInfo.itemDefinition.name);

        itemRemovedFromInventoryCallback?.Invoke(pickUpInfo.itemDefinition, pickUpInfo.CurrentStackSize);
        itemDroppedCallback?.Invoke(pickUpInfo.itemDefinition, pickUpInfo.CurrentStackSize);
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
        UpdateVisual(weapon, weaponTransform);
        character.Animator.runtimeAnimatorController = character.DefaultAnimatorController;
    }

    public void DropArmorHead()
    {
        DropItemInEquipmentSlot(ref armorHead);
        UpdateVisual(armorHead, armorHeadTransform);
    }

    public void DropArmorChest()
    {
        DropItemInEquipmentSlot(ref armorBody);
        UpdateVisual(armorBody, armorBodyTransform);
    }

    public void DropArmorArms()
    {
        DropItemInEquipmentSlot(ref armorArms);
        UpdateVisual(armorArms, armorLeftArmTransform);
        UpdateVisual(armorArms, armorRightArmTransform);
    }

    public void DropArmorLegs()
    {
        DropItemInEquipmentSlot(ref armorLegs);
        UpdateVisual(armorLegs, armorLeftLegTransform);
        UpdateVisual(armorLegs, armorRightLegTransform);
    }

    // Count the number of items of the same type.
    public int Count(ItemDefinition item)
    {
        int count = 0;

        count += weapon == item ? weapon.CurrentStackSize : 0;
        count += armorHead == item ? armorHead.CurrentStackSize : 0;
        count += armorBody == item ? armorBody.CurrentStackSize : 0;
        count += armorArms == item ? armorArms.CurrentStackSize : 0;
        count += armorLegs == item ? armorLegs.CurrentStackSize : 0;

        foreach (ItemInstance itemInBackpack in backpack)
        {
            count += itemInBackpack == item ? itemInBackpack.CurrentStackSize : 0;
        }

        return count;
    }
}