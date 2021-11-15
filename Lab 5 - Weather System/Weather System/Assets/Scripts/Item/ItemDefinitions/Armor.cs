/**
 * Armor.cs
 * Description: This script is used for defining an armor (body armor, helmet, gauntlet, shield, boots).
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorSlot
{
    Head,
    Body,
    Arms,
    Legs
}

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Create a new Armor")]
public class Armor : ItemDefinition
{
    // How is this armor equipped?
    public ArmorSlot armorSlot;

    private void Awake()
    {
        SetType(ItemType.Armor, true);

        StatModifier damageResistanceModifier = new StatModifier(StatType.DamageResistance, StatModifierType.Attached);
        equipStatChanges.Add(damageResistanceModifier);
    }
}
