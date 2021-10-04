/**
 * Health.cs
 * Description: This script handles the health of characters and destructible objects.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Maximum health.
    [SerializeField] float maxHealth = 100.0f;
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = maxHealth < 0.0f ? 0.0f : value;
        }
    }

    // Current health.
    [SerializeField] float currentHealth = 100.0f;
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value < 0 ? 0 : (value > maxHealth ? maxHealth : value);
        }
    }

    // Take damage.
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    public void Heal(float health)
    {
        CurrentHealth += health;
    }
}
