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
        if(character == null)
        {
            character = GetComponent<Character>();
        }

        if (character)
        {
            // Create item instances.
            ItemInstance armorHeadInstance = ScriptableObject.CreateInstance<ItemInstance>();
            armorHeadInstance.itemDefinition = armorHead;

            ItemInstance armorChestInstance = ScriptableObject.CreateInstance<ItemInstance>();
            armorChestInstance.itemDefinition = armorChest;

            ItemInstance armorArmsInstance = ScriptableObject.CreateInstance<ItemInstance>();
            armorArmsInstance.itemDefinition = armorArms;

            ItemInstance armorLegsInstance = ScriptableObject.CreateInstance<ItemInstance>();
            armorLegsInstance.itemDefinition = armorLegs;

            // Add item instances to backpack.
            character.Inventory.backpack.Add(armorHeadInstance);
            character.Inventory.backpack.Add(armorChestInstance);
            character.Inventory.backpack.Add(armorArmsInstance);
            character.Inventory.backpack.Add(armorLegsInstance);

            // Equip items.
            character.Inventory.Equip(0);
            character.Inventory.Equip(0);
            character.Inventory.Equip(0);
            character.Inventory.Equip(0);
            
            // Print character's new max health and movement speed.
            Debug.Log("Character's new max health: " + character.Health.MaxHealth);
            Debug.Log("Character's new movement speed: " + character.BaseMovementSpeed);

            // Unequip items.
            character.Inventory.UnequipArmorArms();
            character.Inventory.UnequipArmorChest();
            character.Inventory.UnequipArmorHead();
            character.Inventory.UnequipArmorLegs();

            // Print character's new max health and movement speed.
            Debug.Log("Character's new max health: " + character.Health.MaxHealth);
            Debug.Log("Character's new movement speed: " + character.BaseMovementSpeed);
        }
        else
        {
            Debug.LogError("Character component is not found");
        }
    }
}
