using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Items in the backpack.
    public List<ItemInstance> backpack;

    [Header("Equipment")]

    // Armors being equipped by the player and not in the backpack.
    public ItemInstance armorHead;
    public ItemInstance armorLegs;
    public ItemInstance armorArms;
    public ItemInstance armorChest;

    // Weapon being equipped by the player and not in the backpack.
    public ItemInstance weapon;

    [Header("Item Dropping")]

    // If the character drops an item, where is the item dropped?
    [SerializeField] Transform dropTransform;

    // Pick-up prefab.
    [SerializeField] GameObject pickupPrefab;

    private void Awake()
    {
        if(!pickupPrefab)
        {
            pickupPrefab = (GameObject)Resources.Load("Prefabs/PickUp/PickUp", typeof(GameObject));
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
                // The item in the inventory has enough empty space for the new item? 
                // Merge that item and the new item into 1.
                if (backpack[i].CurrentStackSize + newItem.CurrentStackSize <= backpack[i].itemDefinition.MaxStackSize)
                {
                    backpack[i].CurrentStackSize += newItem.CurrentStackSize;
                    break;
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
        if (newItem.CurrentStackSize > 0)
        {
            backpack.Add(newItem);
        }

        Debug.Log(newItem.CurrentStackSize + "x" + newItem.itemDefinition.name + " has been added to backpack");
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
    public void RemoveFromBackPack(int backpackIndex, int quantity = 1)
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
        if (!IsBackPackIndexValid(backpackIndex))
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
        if (!IsBackPackIndexValid(backpackIndex))
            return;

        // Equip a weapon.
        if (backpack[backpackIndex].itemDefinition.IsOfType(ItemType.Weapon))
        {
            Equip(backpackIndex, ref weapon);
        }

        // Equip an armor.
        else if (backpack[backpackIndex].itemDefinition.IsOfType(ItemType.Armor))
        {
            switch (((Armor)(backpack[backpackIndex].itemDefinition)).armorSlot)
            {
                case ArmorSlot.head:
                    Equip(backpackIndex, ref armorHead);
                    break;

                case ArmorSlot.chest:
                    Equip(backpackIndex, ref armorChest);
                    break;

                case ArmorSlot.arms:
                    Equip(backpackIndex, ref armorArms);
                    break;

                case ArmorSlot.feet:
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

    // Consume an item in the backpack.
    public void Consume(int backpackIndex)
    {
        if (!IsBackPackIndexValid(backpackIndex))
            return;

        ItemInstance item = backpack[backpackIndex];
        if(item)
        {
            if (item.itemDefinition.IsOfType(ItemType.Consumable))
            {
                item.itemDefinition.OnConsumed(GetComponent<Character>());

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
            }
            else
            {
                Debug.Log("Cannot consume the item since it is not Consumable.");
            }
        }
    }

    // Drop an item in the backpack.
    public void DropItemInPackack(int backpackIndex, int quantity = -1)
    {
        if (!IsBackPackIndexValid(backpackIndex))
            return;

        // Create an item instance for the pick-up.
        ItemInstance pickUpInfo = ScriptableObject.CreateInstance<ItemInstance>();
        pickUpInfo.itemDefinition = backpack[backpackIndex].itemDefinition;
        if(quantity < 0 || quantity >= backpack[backpackIndex].CurrentStackSize)
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
        GameObject pickUpObject = Instantiate(pickupPrefab, dropTransform.position, Quaternion.identity);
        pickUpObject.GetComponent<PickUp>().SetPickUp(pickUpInfo);

        Debug.Log("Dropped " + pickUpInfo.CurrentStackSize + "x" + pickUpInfo.itemDefinition.name);
    }

    // Drop an item in an equipment slot.
    void DropItemInEquipmentSlot(ref ItemInstance equipmentSlot)
    {
        if (!equipmentSlot)
            return;

        // Unequip the item.
        Unequip(ref equipmentSlot);

        // Drop the item.
        DropItemInPackack(backpack.Count - 1);
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
        DropItemInEquipmentSlot(ref armorChest);
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