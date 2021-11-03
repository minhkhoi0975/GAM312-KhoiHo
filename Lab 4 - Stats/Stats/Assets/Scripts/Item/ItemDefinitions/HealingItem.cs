/**
 * HealingItem.cs
 * Description: This class defines items that can heal a character.
 */

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Item", menuName = "Items/Create a new Healing Item")]
public class HealingItem : ItemDefinition
{
    public float healthRestored;

    private void Awake()
    {
        SetType(ItemType.Consumable, true);

        StatModifier healthModifier = new StatModifier(StatType.CurrentHealth, StatModifierType.IncreaseBaseValue);
        consumeStatChanges.Add(healthModifier);
    }

    /*
    public override void OnConsumed(Character character)
    {
        character.Health.Heal(healthRestored);
        Debug.Log("Restore " + healthRestored + " health. Current health: " + character.Health.CurrentHealth + "/" + character.Health.MaxHealth);

        base.OnConsumed(character);
    }
    */
}
