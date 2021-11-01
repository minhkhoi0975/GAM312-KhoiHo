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
    // Callback when any of the stats is changed.
    public delegate void StatsUpdated();
    public StatsUpdated statsUpdatedCallback;

    // Callback when a stat modifer is applied to a stat.
    public delegate void StatModifierApplied(Stat stat, StatModifier modifier);
    public StatModifierApplied statModifierAppliedCallback;

    // Callback when a pernament bonus amount is applied to a stat.
    public delegate void PernamentBonusAmountApplied(StatType stat, float amount);
    public PernamentBonusAmountApplied pernamentBonusAmountApplied;

    // Callback when a stat modifier is removed from a stat.
    public delegate void StatModifierRemoved(Stat stat, StatModifier modifier);
    public StatModifierRemoved statModifierRemovedCallback;

    // Callback when a stat is reset.
    public delegate void StatReset(Stat stat);
    public StatReset statResetCallback;

    // List of all stats of this game object.
    public Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();

    // List of lists of starting stats.
    public List<StatList> startingStatLists;

    private void Awake()
    {
        stats = new Dictionary<StatType, Stat>();

        foreach (StatList statList in startingStatLists)
        {
            foreach(Stat stat in statList.stats)
            {
                // Copy the stat.
                // If we directly use the stat, it will be edited in the data file.
                stats[stat.statType] = new Stat(stat);
            }
        }
        Debug.Log("Total number of stats: " + stats.Count);
    }

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

    // Add a pernament bonus amount to a stat.
    public void AddPernamentBonusAmount(StatType stat, float amount)
    {
        stats[stat].PernamentBonusValue += amount;

        pernamentBonusAmountApplied?.Invoke(stat, amount);
        statsUpdatedCallback?.Invoke();
    }

    // Add a modifier to a stat.
    public void AddModifier(StatModifier modifier)
    {
        if (stats.ContainsKey(modifier.modifiedStatType))
        {
            stats[modifier.modifiedStatType].AddModifier(modifier);

            statModifierAppliedCallback?.Invoke(stats[modifier.modifiedStatType], modifier);
            statsUpdatedCallback?.Invoke();
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

            statModifierRemovedCallback?.Invoke(stats[modifier.modifiedStatType], modifier);
            statsUpdatedCallback?.Invoke();
        }
    }

    // Reset a stat.
    public void ResetStat(StatType resetStatType)
    {
        if(stats.ContainsKey(resetStatType))
        {
            stats[resetStatType].ResetStat();
        }

        statResetCallback?.Invoke(stats[resetStatType]);
        statsUpdatedCallback?.Invoke();
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
