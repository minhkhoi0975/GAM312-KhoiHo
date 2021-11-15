/**
 * ItemTemplate.cs
 * Description: This script defines an item.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Flags]
public enum ItemType
{
    Armor = 1 << 0,
    Weapon = 1 << 1,
    Consumable = 1 << 2
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

    public ItemType type;

    // The icon of the item in the inventory.
    public Sprite icon;

    // The visual representation of the item. Has NO effect on gameplay.
    public GameObject mesh;

    // How are the stats modified when the character consumes this item?
    public List<StatModifier> consumeStatChanges = new List<StatModifier>();

    // How are the stats modified when the character equips this item?
    public List<StatModifier> equipStatChanges = new List<StatModifier>();

    // Does this item belong to an item type?
    public bool IsOfType(ItemType itemType)
    {
        return (int)(type & itemType) != 0;
    }

    // Make the item to belong to or not belong to an item type.
    public void SetType(ItemType itemType, bool IsOfType = true)
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
            foreach(StatModifier statModifer in equipStatChanges)
            {
                character.StatSystem.AddModifier(statModifer);
            }

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
            foreach (StatModifier statModifer in equipStatChanges)
            {
                character.StatSystem.RemoveModifier(statModifer);
            }

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

    // Called when the item is consumed by the character.
    virtual public void OnConsumed(Character character)
    {
        if (character)
        {
            foreach (StatModifier statModifer in consumeStatChanges)
            {
                character.StatSystem.AddModifier(statModifer);
            }

            Debug.Log(character.gameObject.name + " has consumed " + name);
        }
    }

    virtual public void OnConsumed(GameObject characterGameObject)
    {
        if (!characterGameObject)
        {
            return;
        }

        Character character = characterGameObject.GetComponent<Character>();
        if (character)
        {
            OnConsumed(character);
        }
    }
}
