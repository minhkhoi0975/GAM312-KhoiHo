/**
 * ItemTemplate.cs
 * Description: This script defines an item.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Armor = 1 << 0,
    Weapon = 1 << 1
}

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Create a new Item")]
public class ItemDefinition : ScriptableObject
{
    public string name;

    [SerializeField] int maxStackSize = 1;
    public int MaxStackSize
    {
        get
        {
            return maxStackSize;
        }
        set
        {
            maxStackSize = value < 1 ? 1 : value;
        }
    }

    ItemType type;

    // Does this item belong to an item type?
    bool IsOfType(ItemType itemType)
    {
        return (int)(type & itemType) != 0;
    }

    // Make the item to belong to or not belong to an item type.
    void SetType(ItemType itemType, bool IsOfType = true)
    {
        if (IsOfType)
        {
            type = type | itemType;
        }
        else
        {
            type = type & ~itemType;
        }
    }

    // Called when the item is equipped by the character.
    virtual public void OnEquipped(Character character)
    {
        if (character)
        {
            Debug.Log(character.gameObject.name + " has equipped " + name);
        }
    }

    virtual public void OnEquipped(GameObject characterGameObject)
    {
        if (!characterGameObject)
        {
            return;
        }

        Character character = characterGameObject.GetComponent<Character>();
        if (character)
        {
            OnEquipped(character);
        }
    }

    // Called when the item is no longer equipped by the character.
    virtual public void OnUnequipped(Character character)
    {
        if (character)
        {
            Debug.Log(character.gameObject.name + " has unequipped " + name);
        }
    }

    virtual public void OnUnequipped(GameObject characterGameObject)
    {
        if (!characterGameObject)
        {
            return;
        }

        Character character = characterGameObject.GetComponent<Character>();
        if (character)
        {
            OnUnequipped(character);
        }
    }
}
