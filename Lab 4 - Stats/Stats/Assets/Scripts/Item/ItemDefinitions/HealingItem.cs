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
    }

    public override void OnConsumed(Character character)
    {
        character.Health.Heal(healthRestored);

        base.OnConsumed(character);
    }
}
