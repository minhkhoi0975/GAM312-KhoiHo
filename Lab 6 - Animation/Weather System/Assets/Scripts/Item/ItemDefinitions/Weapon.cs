/**
 * Weapon.cs
 * Description: This script is used for defining a weapon.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Create a new Weapon")]
public class Weapon : ItemDefinition
{
    [Header("Animations")]
    [SerializeField] AnimationClip idleAnimation;
    [SerializeField] AnimationClip walkAnimation;
    [SerializeField] AnimationClip runAnimation;
    [SerializeField] AnimationClip attackAnimaion;

    private void Awake()
    {
        SetType(ItemType.Weapon, true);

        if (equipStatChanges.Count == 0)
        {
            StatModifier attackRangeModifier = new StatModifier(StatType.AttackRange, StatModifierType.Attached);
            StatModifier damageModifier = new StatModifier(StatType.Damage, StatModifierType.Attached);
            StatModifier criticalChanceModifier = new StatModifier(StatType.CriticalChance, StatModifierType.Attached);
            StatModifier criticalDamageMultiplierModifier = new StatModifier(StatType.CriticalDamageMultiplier, StatModifierType.Attached);

            equipStatChanges.Add(attackRangeModifier);
            equipStatChanges.Add(damageModifier);
            equipStatChanges.Add(criticalChanceModifier);
            equipStatChanges.Add(criticalDamageMultiplierModifier);
        }
    }
}
