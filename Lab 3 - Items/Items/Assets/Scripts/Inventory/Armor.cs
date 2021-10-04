/**
 * Armor.cs
 * Description: This script is used for defining an armor (body armor, helmet, gauntlet, shield, boots).
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Create a new Armor")]
public class Armor : ItemDefinition
{
    // How well does this armor protect from the player?
    [SerializeField] float protection;
    public float Protection
    {
        get
        {
            return protection;
        }
        set
        {
            protection = value < 0 ? 0 : value;
        }
    }

    // Character's movement speed = character's base movement speed * sum of movementSpeedMultipliers of all equipped armors.
    [SerializeField] float movementSpeedMultiplier = 1.0f;
    public float MovementSpeedMultiplier
    {
        get
        {
            return movementSpeedMultiplier;
        }
        set
        {
            movementSpeedMultiplier = value < 0.0 ? 0.0f : value; 
        }
    }

    // How is this armor equipped?
    public InventorySlot equipmentSlot;
}
