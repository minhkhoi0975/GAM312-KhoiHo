using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEquipment : MonoBehaviour
{
    public Character character;

    public ItemDefinition armorHead;
    public ItemDefinition armorChest;
    public ItemDefinition armorArms;
    public ItemDefinition armorLegs;

    public Weapon weapon;

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
            ItemInstance helmet = ScriptableObject.CreateInstance<ItemInstance>();
            helmet.itemDefinition = armorHead;

            ItemInstance armor = ScriptableObject.CreateInstance<ItemInstance>();
            armor.itemDefinition = armorChest;

            ItemInstance gauntlets = ScriptableObject.CreateInstance<ItemInstance>();
            gauntlets.itemDefinition = armorArms;

            ItemInstance boots = ScriptableObject.CreateInstance<ItemInstance>();
            boots.itemDefinition = armorLegs;

            ItemInstance sword = ScriptableObject.CreateInstance<ItemInstance>();
            sword.itemDefinition = weapon;

            // Add item instances to backpack.
            character.Inventory.backpack.Add(helmet);
            character.Inventory.backpack.Add(armor);
            character.Inventory.backpack.Add(gauntlets);
            character.Inventory.backpack.Add(boots);
            character.Inventory.backpack.Add(sword);

            // Equip items.
            character.Inventory.Equip(0);
            character.Inventory.Equip(0);
            character.Inventory.Equip(0);
            character.Inventory.Equip(0);
            character.Inventory.Equip(0);

            // Print character's new max health and movement speed.
            Debug.Log("Character's new max health: " + character.Health.MaxHealth);
            Debug.Log("Character's new movement speed: " + character.BaseMovementSpeed);
            if (character.Inventory.weapon)
            {
                Debug.Log("Character's carrying " + character.Inventory.weapon.itemDefinition.name);
            }

            // Unequip items.
            character.Inventory.UnequipArmorArms();
            character.Inventory.UnequipArmorChest();
            character.Inventory.UnequipArmorHead();
            character.Inventory.UnequipArmorLegs();
            //character.Inventory.UnequipWeapon();

            // Print character's new max health and movement speed.
            Debug.Log("Character's new max health: " + character.Health.MaxHealth);
            Debug.Log("Character's new movement speed: " + character.BaseMovementSpeed);
            if (!character.Inventory.weapon)
            {
                Debug.Log("Character is unarmed");
            }
        }
        else
        {
            Debug.LogError("Character component is not found");
        }
    }
}
