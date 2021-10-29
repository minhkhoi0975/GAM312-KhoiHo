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
    MovementSpeed = 0,
    DashSpeedMultiplier,

    // Moving objects
    PushingForce = 100,
    TelekinesisForce,
    TelekinesisDistance,

    // Health
    CurrentHealth = 200,
    MaxHealth,
    DamageResistance,

    // Combat
    Damage = 300,
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
            UpdateCurrentValue();
        }
    }

    // The pernament bonus value of the stat. Changed when a PermanentBonus modifier is applied to the stat.
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
            UpdateCurrentValue();
        }
    }

    // The minimum and maximum values of the stat.
    [Range(float.MinValue, float.MaxValue)] public float minValue = float.MinValue;
    [Range(float.MinValue, float.MaxValue)] public float maxValue = float.MaxValue;

    // List of all modifiers that affect the stat.
    [SerializeField] List<StatModifier> statModifiers = new List<StatModifier>();
    public List<StatModifier> StatModifiers
    {
        get
        {
            return statModifiers;
        }
    }

    // Has the stat modifier been recently updated?
    bool isStatUpdated = true;

    // The current value of the stat.
    float currentValue;
    public float CurrentValue
    {
        get
        {
            return GetCurrentValue();
        }
    }

    // Get the current value of the stat.
    // If including modifiers is true, the current value is baseValue + permanentBonusValue + bonuses from modifiers.
    // Otherwise, then the current value is baseValue + permanentBonusValue.
    float GetCurrentValue(bool includingModifiers = true)
    {
        if (includingModifiers)
        {
            // If the stat has been recently updated, recalculate the current value.
            if(isStatUpdated)
            {
                UpdateCurrentValue();
            }

            return currentValue;
        }
        else
        {
            return baseValue + pernamentBonusValue;
        }
    }

    // Default constructor.
    public Stat()
    {
    }

    // Deep copy constructor.
    public Stat(Stat statToCopy)
    {
        this.statType = statToCopy.statType;
        this.baseValue = statToCopy.baseValue;
        this.pernamentBonusValue = statToCopy.pernamentBonusValue;
        this.minValue = statToCopy.minValue;
        this.maxValue = statToCopy.maxValue;
        foreach(StatModifier modifier in statToCopy.statModifiers)
        {
            this.statModifiers.Add(modifier);
        }
        this.currentValue = statToCopy.currentValue;
    }

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

            UpdateCurrentValue();
        }
    }

    // Remove a modifier from the stat.
    public void RemoveModifier(StatModifier modifier)
    {
        if (statModifiers.Remove(modifier))
        {
            UpdateCurrentValue();
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

        UpdateCurrentValue();
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

        isStatUpdated = false;
    }
}