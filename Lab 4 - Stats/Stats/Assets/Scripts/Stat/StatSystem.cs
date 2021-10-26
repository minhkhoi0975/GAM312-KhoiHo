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

    // Get a value of a stat/
    public float GetValue(StatType stat)
    {
        if(stats.ContainsKey(stat))
        {
            return stats[stat].currentValue;
        }
        else
        {
            Debug.LogError("Cannot find the stat.");
            return 0;
        }
    }

    // Modifer a stat by adding the value of statModifier to the current value of the stat.
    public void ChangeStat(Stat statModifier)
    {
        ChangeStat(statModifier.statType, statModifier.currentValue);
    }

    public void ChangeStat(StatType stat, float value)
    {
        if(stats.ContainsKey(stat))
        {
            stats[stat].currentValue += value;
        }
    }

    // Reset the value of a stat.
    public void ResetValue(StatType stat)
    {
        if (stats.ContainsKey(stat))
        {
            stats[stat].currentValue = stats[stat].initialValue;
        }
    }
}
