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
    DamageResistance = 202,

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

    // When the stat is reset, set baseValue to this value.
    [SerializeField] float initialBaseValue;
    public float InitialBaseValue
    {
        get
        {
            return initialBaseValue;
        }
        set
        {
            initialBaseValue = value;
            isStatChanged = true;
        }
    }

    // The current value of the stat without any modifiers.
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
            baseValue = Mathf.Clamp(baseValue, minValue, maxValue);
            isStatChanged = true;
        }
    }

    // The minimum and maximum values of baseValue.
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

    // Get the total value of the modifiers.
    public float ModifierValue
    {
        get
        {
            float totalValue = 0;
            foreach(StatModifier modifier in statModifiers)
            {
                totalValue += modifier.value;
            }
            return totalValue;
        }
    }

    // Has the stat modifier been recently changed?
    bool isStatChanged = true;

    // The current value of the stat, including both the base value and the modifier value.
    float currentValue;
    public float CurrentValue
    {
        get
        {
            UpdateCurrentValue();
            return currentValue;
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
        this.initialBaseValue = statToCopy.initialBaseValue;
        this.baseValue = statToCopy.baseValue;
        this.minValue = statToCopy.minValue;
        this.maxValue = statToCopy.maxValue;
        foreach(StatModifier modifier in statToCopy.statModifiers)
        {
            this.statModifiers.Add(modifier);
        }
        this.currentValue = statToCopy.currentValue;
    }

    // Add a bonus amount to baseValue.
    public void AddBonusAmount(float amount)
    {
        BaseValue += amount;
        isStatChanged = true;
    }

    // Add a modifier to the stat.
    public void AddModifier(StatModifier modifier)
    {
        if (modifier.modifiedStatType == statType)
        {
            if (modifier.statModifierType == StatModifierType.BaseValue)
            {
                BaseValue += modifier.value;
            }
            else if (modifier.statModifierType == StatModifierType.Attached)
            {
                statModifiers.Add(modifier);
            }

            isStatChanged = true;
        }
    }

    // Remove a modifier from the stat.
    public void RemoveModifier(StatModifier modifier)
    {
        if (statModifiers.Remove(modifier))
        {
            isStatChanged = true;
        }
    }

    // Reset the stat.
    public void ResetStat()
    {
        // Reset baseValue.
        baseValue = initialBaseValue;

        // Get rid of all the modifiers.
        statModifiers.Clear();

        isStatChanged = true;
    }

    // Update the current value of the stat.
    void UpdateCurrentValue()
    {
        // No need to update the current value of the stat if it is not changed.
        if (!isStatChanged)
            return;

        // Recalculate the current value.
        currentValue = baseValue + ModifierValue;

        isStatChanged = false;
    }
}