/**
 * Stat.cs
 * Description: This class contains the definition of a stat.
 * Programmer: Khoi Ho
 * Credit(s): Kryzarel (https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    // Movement
    MovementSpeed,
    DashSpeedMultiplier,

    // Moving objects
    PushingForce,
    TelekinesisForce,
    TelekinesisDistance,

    // Health
    MaxHealth,
    DamageResistance,

    // Combat
    Damage,
    CriticalChance,
    CriticalDamageMultiplier,
    AttackRange
}

[System.Serializable]
public class Stat
{
    // The type of the stat.
    public StatType statType;

    // The initial value of the stat without any modifiers.
    [SerializeField] float baseValue;
    public float BaseValue
    {
        get
        {
            return baseValue;
        }
        set
        {
            baseValue = value;
            isStatModified = true;
        }
    }

    // The pernament bonus value of the stat.
    [SerializeField] float pernamentBonusValue = 0.0f;
    public float PernamentBonusValue
    {
        get
        {
            return pernamentBonusValue;
        }
        set
        {
            pernamentBonusValue = value;
            isStatModified = true;
        }
    }

    // The minimum and maximum values of the stat.
    [SerializeField] [Range(float.MinValue, float.MaxValue)] public float minValue = float.MinValue;
    [SerializeField] [Range(float.MinValue, float.MaxValue)] public float maxValue = float.MaxValue;

    // List of all modifiers that affect the stat.
    [SerializeField] List<StatModifier> statModifiers = new List<StatModifier>();
    public List<StatModifier> StatModifiers
    {
        get
        {
            return statModifiers;
        }
    }

    // The current value of the stat.
    [SerializeField] float currentValue;
    public float CurrentValue
    {
        get
        {
            if(isStatModified)
            {
                UpdateCurrentValue();
            }

            return currentValue;
        }
    }

    // Has the stat been recently modified?
    bool isStatModified = true;

    // Add a modifier to the stat.
    public void AddModifier(StatModifier modifier)
    {
        if (modifier.modifiedStatType == statType)
        {
            if (modifier.statModifierType == StatModifierType.PernamentBonus)
            {
                pernamentBonusValue += modifier.value;
            }
            else if (modifier.statModifierType == StatModifierType.Attached)
            {
                statModifiers.Add(modifier);
            }

            Debug.Log("Stat modified");
            isStatModified = true;
        }
    }

    // Remove a modifier from the stat.
    public void RemoveModifier(StatModifier modifier)
    {
        if (statModifiers.Remove(modifier))
        {
            isStatModified = true;
        }
    }

    // Reset the stat to the base value.
    public void ResetStat()
    {
        // Remove the pernament bonus value.
        pernamentBonusValue = 0;

        // Get rid of all the modifiers.
        foreach (StatModifier modifier in statModifiers)
        {
            statModifiers.Remove(modifier);
        }

        isStatModified = true;
    }

    // Update the current value of the stat.
    void UpdateCurrentValue()
    {
        float newCurrentValue = baseValue + pernamentBonusValue;

        foreach(StatModifier modifier in statModifiers)
        {
            newCurrentValue += modifier.value;
        }

        newCurrentValue = Mathf.Clamp(newCurrentValue, minValue, maxValue);

        currentValue = newCurrentValue;

        isStatModified = false;
    }
}
