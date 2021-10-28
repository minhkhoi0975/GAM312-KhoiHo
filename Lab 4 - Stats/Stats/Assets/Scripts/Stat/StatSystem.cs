/**
 * StatSystem.cs
 * Description: This file contains all types of stats and stat definition.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem : MonoBehaviour
{
    // List of all stats of this game object.
    public Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();

    // Get the base value of a stat.
    public float GetBaseValue(StatType stat)
    {
        if (stats.ContainsKey(stat))
        {
            return stats[stat].BaseValue;
        }
        else
        {
            Debug.LogError("Cannot find the stat.");
            return 0;
        }
    }

    // Get the current value of a stat.
    public float GetCurrentValue(StatType stat)
    {
        if (stats.ContainsKey(stat))
        {
            return stats[stat].CurrentValue;
        }
        else
        {
            Debug.LogError("Cannot find the stat.");
            return 0;
        }
    }

    // Add a modifier to a stat.
    public void AddModifier(StatModifier modifier)
    {
        if (stats.ContainsKey(modifier.modifiedStatType))
        {
            stats[modifier.modifiedStatType].AddModifier(modifier);
        }
        else
        {
            Debug.LogError("Cannot find the stat to add the modifier.");
        }
    }

    // Remove a modifier from a stat.
    public void RemoveModifier(StatModifier modifier)
    {
        if (stats.ContainsKey(modifier.modifiedStatType))
        {
            stats[modifier.modifiedStatType].RemoveModifier(modifier);
        }
    }

    // Reset a stat.
    public void ResetStat(StatType resetStatType)
    {
        if(stats.ContainsKey(resetStatType))
        {
            stats[resetStatType].ResetStat();
        }
    }

    // Reset all stats.
    public void ResetAllStats()
    {
        foreach (KeyValuePair<StatType, Stat> stat in stats)
        {
            stat.Value.ResetStat();
        }
    }
}
