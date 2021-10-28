/**
 * StartingStats.cs
 * Description: This script initializes the stats of a stat system.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatSystem))]
public class StartingStats : MonoBehaviour
{
    [SerializeField] StatSystem statSystem;

    public List<Stat> startingStats;

    // Start is called before the first frame update
    void Awake()
    {
        if(!statSystem)
        {
            statSystem = GetComponent<StatSystem>();
        }

        // Put the stats into the stat system.
        foreach(Stat stat in startingStats)
        {
            statSystem.stats[stat.statType] = stat;
        }
    }
}
