/**
 * Weapon.cs
 * Description: This script is used for defining a weapon. Right now this class is only for melee weapons, ranged weapons will be supported in the future.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Create a new Weapon")]
public class Weapon : ItemDefinition
{
    // The maximum distance at which the weapon causes damage.
    [SerializeField] float range = 0.5f;
    public float Range
    {
        get
        {
            return range;
        }
        set
        {
            range = value < 0.5f ? 0.5f : value;
        }
    }

    // The normal damage of the weapon.
    [SerializeField] float baseDamage = 5.0f;
    public float BaseDamage
    {
        get
        {
            return baseDamage;
        }
        set
        {
            baseDamage = value < 0.0f ? 0.0f : value;
        }
    }

    // criticalDamage = baseDamage * criticalDamageMultiplier
    [SerializeField] float criticalDamageMultiplier = 1.0f;
    public float CriticalDamageMultiplier
    {
        get
        {
            return criticalDamageMultiplier;
        }
        set
        {
            criticalDamageMultiplier = value < 1.0f ? 1.0f : value;
        }
    }

    // How is it likely for the weapon to cause critical damage?
    [SerializeField] float criticalChance = 0.0f;
    public float CriticalChance
    {
        get
        {
            return criticalChance;
        }
        set
        {
            criticalChance = value < 0.0f ? 0.0f : (value > 100.0f ? 100.0f : value);
        }
    }

    private void Awake()
    {
        SetType(ItemType.Weapon, true);
    }

    public override void OnEquipped(Character character)
    {
        // Modify attack range.
        character.StatSystem.stats[StatType.AttackRange].PernamentBonusValue += range;

        // Modify damage.
        character.StatSystem.stats[StatType.Damage].PernamentBonusValue += baseDamage;

        // Modify critical chance.
        character.StatSystem.stats[StatType.CriticalChance].PernamentBonusValue += criticalChance;

        base.OnEquipped(character);
    }

    // Called when the item is no longer equipped by a character.
    public override void OnUnequipped(Character character)
    {
        // Modify attack range.
        character.StatSystem.stats[StatType.AttackRange].PernamentBonusValue -= range;

        // Modify damage.
        character.StatSystem.stats[StatType.Damage].PernamentBonusValue -= baseDamage;

        // Modify critical chance.
        character.StatSystem.stats[StatType.CriticalChance].PernamentBonusValue -= criticalChance;

        base.OnUnequipped(character);
    }
}
