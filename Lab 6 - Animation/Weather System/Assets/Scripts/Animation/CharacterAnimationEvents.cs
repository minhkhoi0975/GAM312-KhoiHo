using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    [SerializeField] Character character;

    public void Hit()
    {
        float attackRange = character.StatSystem.GetCurrentValue(StatType.AttackRange);
        float damage = character.StatSystem.GetCurrentValue(StatType.Damage);
        float criticalChance = character.StatSystem.GetCurrentValue(StatType.CriticalChance);
        float criticalMultiplier = character.StatSystem.GetCurrentValue(StatType.CriticalDamageMultiplier);
        character.Attack(attackRange, damage, criticalMultiplier, criticalChance);
    }

    public void FootL()
    {

    }

    public void FootR()
    {

    }
}
