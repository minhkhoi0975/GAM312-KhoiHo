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
    head,
    chest,
    arms,
    feet
}

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Create a new Armor")]
public class Armor : ItemDefinition
{
    // The amount of max health added to character's health when this armor is equipped.
    [SerializeField] float healthModifier = 10.0f;
    public float HealthModifier
    {
        get
        {
            return healthModifier;
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

    // How is this armor equipped?
    public ArmorSlot armorSlot;

    private void Awake()
    {
        SetType(ItemType.Armor, true);
    }

    // Called when the item is equipped by the character.
    public override void OnEquipped(Character character) 
    {
        // Modify health.
        character.Health.MaxHealth += HealthModifier;

        // Modify movement speed.
        character.BaseMovementSpeed += movementSpeedModifier;

        base.OnEquipped(character);
    }

    // Called when the item is no longer equipped by the character.
    public override void OnUnequipped(Character character)
    {
        // Reset health.
        character.Health.MaxHealth -= HealthModifier;
        character.Health.CurrentHealth = character.Health.CurrentHealth > character.Health.MaxHealth ? character.Health.MaxHealth : character.Health.CurrentHealth;

        // Reset movement speed.
        character.BaseMovementSpeed -= movementSpeedModifier;

        base.OnUnequipped(character);
    }
}
