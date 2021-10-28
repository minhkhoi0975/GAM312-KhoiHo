/**
 * TestEquipment.cs
 * Description: This script is used for testing the inventory system. Do not use this script when building the game.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class TestEquipment : MonoBehaviour
{
    // Reference to the inventory.
    public Inventory inventory;

    // List of items to be added to the inventory.
    public List<ItemDefinition> items;

    // Start is called before the first frame update
    void Start()
    {
        if (!inventory)
        {
            inventory = GetComponent<Inventory>();
        }

        if (inventory)
        {
            TestEquippingArmorAndWeapon();
        }
    }

    public void TestEquippingArmorAndWeapon()
    {
        foreach (ItemDefinition item in items)
        {
            // Create an item instance.
            ItemInstance itemInstance = new ItemInstance(item);

            // Add the item to the backpack.
            inventory.AddToBackPack(itemInstance);

            // Try equipping the item.
            inventory.Equip(inventory.backpack.Count - 1);
        }
    }
}
