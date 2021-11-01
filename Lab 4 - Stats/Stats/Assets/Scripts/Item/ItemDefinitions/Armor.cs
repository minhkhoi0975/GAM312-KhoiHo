/**
 * Armor.cs
 * Description: This script is used for defining an armor (body armor, helmet, gauntlet, shield, boots).
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorSlot
{
    Head,
    Body,
    Arms,
    Legs
}

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Create a new Armor")]
public class Armor : ItemDefinition
{
    // The amount of damage resistance added to character's damage resistance.
    [SerializeField] float damageResistance = 2.0f;
    public float DamageResistance
    {
        get
        {
            return damageResistance;
        }
    }

    // The amount of movement speed added to character's movement speed when this armor is equipped.
    [SerializeField] float movementSpeedModifier = 10.0f;
    public float MovementSpeedModifier
    {
        get
        {
            return movementSpeedModifier;
        }
    }

    // The amount added to character's dashSpeedMultiplier when this armor is equipped.
    [SerializeField] float bonusDashSpeedMultiplier;
    public float BonusDashSpeedMultiplier
    {
        get
        {
            return bonusDashSpeedMultiplier;
        }
    }

    // How is this armor equipped?
    public ArmorSlot armorSlot;

    private void Awake()
    {
        SetType(ItemType.Armor, true);
    }

    // Called when the item is equipped by a character.
    public override void OnEquipped(Character character)
    {
        // Modify damage resistance.
        //character.Health.DamageResistance += DamageResistance;
        character.StatSystem.stats[StatType.DamageResistance].PernamentBonusValue += DamageResistance;

        // Modify movement speed.
        // character.BaseMovementSpeed += movementSpeedModifier;

        // Modify dash speed multiplier.
        // character.DashSpeedMultiplier += bonusDashSpeedMultiplier;

        base.OnEquipped(character);
    }

    // Called when the item is no longer equipped by a character.
    public override void OnUnequipped(Character character)
    {
        // Restore damage resistance.
        // character.Health.DamageResistance -= DamageResistance;
        character.StatSystem.stats[StatType.DamageResistance].PernamentBonusValue -= DamageResistance;

        // Restore movement speed.
        // character.BaseMovementSpeed -= movementSpeedModifier;

        // Restore dash speed multiplier.
        // character.DashSpeedMultiplier -= bonusDashSpeedMultiplier;

        base.OnUnequipped(character);
    }
}
