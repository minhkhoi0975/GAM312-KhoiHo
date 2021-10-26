using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    // Movement
    MovementSpeed,
    DashSpeedMultiplier,

    // Moving objects
    PushingForce,
    TelekinesisForce,
    TelekinesisDistance,

    // Health
    MaxHealth,
    DamageResistance,

    // Combat
    Damage,
    CriticalChance,
    CriticalDamageMultiplier,
    AttackRange
}

[System.Serializable]
[CreateAssetMenu(fileName = "New Stat", menuName = "Stats/Create a new Stat")]
public class Stat : ScriptableObject
{
    // The type of the stat.
    public StatType statType;

    // The initial value of the stat without any modifiers.
    public float initialValue;

    // The current value of the stat.
    public float currentValue;
}
