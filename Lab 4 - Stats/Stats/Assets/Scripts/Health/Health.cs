/**
 * Health.cs
 * Description: This script handles the health of characters and destructible objects.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatSystem))]
public class Health : MonoBehaviour
{
    // We need to reference the stat system in order to get current health, maximum health and damage resistance.
    [SerializeField] StatSystem statSystem;
    public float CurrentHealth
    {
        get
        {
            return statSystem.GetCurrentValue(StatType.CurrentHealth);
        }
    }

    public float DamageResistance
    {
        get
        {
            return statSystem.GetCurrentValue(StatType.DamageResistance);
        }
    }

    // When health goes below 0, this game object is destroyed.
    // By default, destroy the object that contains Health component.
    [SerializeField] GameObject gameObjectToDestroy;

    // How long before objectToDestroy is destroyed, in seconds.
    [SerializeField] float destroyDelayTimeInSeconds = 0.0f;

    private void Awake()
    {
        if (!statSystem)
        {
            statSystem = GetComponent<StatSystem>();
        }

        if (!gameObjectToDestroy)
        {
            gameObjectToDestroy = gameObject;
        }
    }

    // Take damage.
    public void TakeDamage(float damageFromSource)
    {
        float finalDamage = damageFromSource - DamageResistance;
        Debug.Log("Damage: " + damageFromSource + " - " + DamageResistance + " = " + finalDamage);

        if (finalDamage > 0)
        {
            statSystem.stats[StatType.CurrentHealth].BaseValue -= finalDamage;
        }

        // Health goes below zero? Die.
        if (CurrentHealth == 0)
        {
            StartCoroutine("Die");
        }

        statSystem.statsUpdatedCallback?.Invoke();
    }

    // Heal.
    public void Heal(float amount)
    {
        statSystem.stats[StatType.CurrentHealth].BaseValue += amount;

        statSystem.statsUpdatedCallback?.Invoke();
    }

    public virtual IEnumerator Die()
    {
        yield return new WaitForSeconds(destroyDelayTimeInSeconds);
        Destroy(gameObjectToDestroy);
    }
}
