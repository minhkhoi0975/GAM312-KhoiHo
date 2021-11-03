/**
 * StatModifier.cs
 * Description: This script defines how a stat is modified.
 * Programmer: Khoi Ho
 * Credit(s): Kryzarel (https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// How does the modifier change the stat?
public enum StatModifierType
{
    // The value of the modifier is added to pernament bonus value of the stat.
    // This type of stat modifier is suitable for consumable items.
    PernamentBonus,

    // The modifier is added to statModifiers of the stat, and it can be removed from statModifers when the character unequips the item containing that stat modifier.
    // This type of stat modifier is suitable for equippable items.
    Attached,             
}

[System.Serializable]
public class StatModifier
{
    // The type of the stat that is modified.
    public StatType modifiedStatType;

    // How does the modifier change the stat?
    public StatModifierType statModifierType;

    // How much the stat is modified.
    public float value;

    public StatModifier()
    {
    }

    public StatModifier(StatType modifiedStatType, StatModifierType statModifierType, float value = 0.0f)
    {
        this.modifiedStatType = modifiedStatType;
        this.statModifierType = statModifierType;
        this.value = value;
    }
}
