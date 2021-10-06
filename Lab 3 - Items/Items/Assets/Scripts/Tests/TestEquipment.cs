using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEquipment : MonoBehaviour
{
    public Character character;

    public Armor armorHead;
    public Armor armorChest;
    public Armor armorArms;
    public Armor armorLegs;

    // Start is called before the first frame update
    void Start()
    {
        // Create item instances.
        ItemInstance armorHeadInstance = new ItemInstance();
        armorHeadInstance.itemDefinition = armorHead;

        ItemInstance armorChestInstance = new ItemInstance();
        armorChestInstance.itemDefinition = armorChest;

        ItemInstance armorArmsInstance = new ItemInstance();
        armorArmsInstance.itemDefinition = armorArms;

        ItemInstance armorLegsInstance = new ItemInstance();
        armorLegsInstance.itemDefinition = armorLegs;

        // Add item instances to backpack.
        character.Inventory.backpack.Add(armorHeadInstance);
        character.Inventory.backpack.Add(armorChestInstance);
        character.Inventory.backpack.Add(armorArmsInstance);
        character.Inventory.backpack.Add(armorLegsInstance);

        // Equip items.
        character.Inventory.Equip(0);
        character.Inventory.Equip(1);
        character.Inventory.Equip(2);
        character.Inventory.Equip(3);
    }
}
