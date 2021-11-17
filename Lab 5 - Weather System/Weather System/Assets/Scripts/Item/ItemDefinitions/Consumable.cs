/**
 * Consumable.cs
 * Description: This class defines items that can be consumed by a character.
 * Programmer: Khoi Ho
 */

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Items/Create a new Consumable")]
public class Consumable : ItemDefinition
{
    private void Awake()
    {
        SetType(ItemType.Consumable, true);

        if (consumeStatChanges.Count == 0)
        {
            StatModifier healthModifier = new StatModifier(StatType.CurrentHealth, StatModifierType.IncreaseBaseValue);
            consumeStatChanges.Add(healthModifier);
        }
    }
}
