/**
 * StatModifier.cs
 * Description: This script defines how a stat is modified.
 * Programmer: Khoi Ho
 * Credit(s): Kryzarel (https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// How does the modifier change the stat?
public enum StatModifierType
{
    // The value of the modifier is added to the base value of the stat.
    IncreaseBaseValue,

    // The modifier is added to statModifers.
    // Suitable for temporary changes to the stat (e.g. weapons and armors).
    Attached,
    
    // The value of the modifier is added to the maximum base value of the stat.
    IncreaseMaxBaseValue,

    // The value of the modifier is added to the minimum base value of the stat.
    IncreaseMinBaseValue
}

[System.Serializable]
public class StatModifier
{
    // The type of the stat that is modified.
    public StatType statType;

    // How does the modifier change the stat?
    public StatModifierType statModifierType;

    // How much the stat is modified.
    public float value;

    public StatModifier()
    {
    }

    public StatModifier(StatType modifiedStatType, StatModifierType statModifierType, float value = 0.0f)
    {
        this.statType = modifiedStatType;
        this.statModifierType = statModifierType;
        this.value = value;
    }
}
