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

    // Damage resistance (when the game object takes damage, this amount is subtracted from damage.
    [SerializeField] float damageResistance = 0.0f;
    public float DamageResistance
    {
        get
        {
            return damageResistance;
        }
        set
        {
            damageResistance = value < 0 ? 0 : value;
        }
    }

    // When health goes below 0, this game object is destroyed.
    // By default, destroy the object that contains Health component.
    [SerializeField] GameObject gameObjectToDestroy;

    // How long before objectToDestroy is destroyed, in seconds.
    [SerializeField] float destroyDelayTimeInSeconds = 0.0f;

    private void Awake()
    {
        if (!gameObjectToDestroy)
        {
            gameObjectToDestroy = gameObject;
        }
    }

    // Take damage.
    public void TakeDamage(float damageFromSource)
    {
        float finalDamage = damageFromSource - damageResistance;
        Debug.Log("Damage: " + damageFromSource + " - " + damageResistance + " = " + finalDamage);

        if (finalDamage > 0)
        {
            CurrentHealth -= finalDamage;
        }

        // Health goes below zero? Destroy root component.
        if (CurrentHealth == 0)
        {
            StartCoroutine("Die");
        }
    }

    public void Heal(float health)
    {
        CurrentHealth += health;
    }

    public virtual IEnumerator Die()
    {
        yield return new WaitForSeconds(destroyDelayTimeInSeconds);
        Destroy(gameObjectToDestroy);
    }
}
