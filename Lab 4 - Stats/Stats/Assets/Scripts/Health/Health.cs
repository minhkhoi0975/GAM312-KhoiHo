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
    // We need to reference the stat system in order to get current health, maximum health and damage resistance.
    public StatSystem statSystem;
    public float CurrentHealth
    {
        get
        {
            return statSystem.stats[StatType.CurrentHealth].CurrentValue;
        }
    }

    public float MaxHealth
    {
        get
        {
            return statSystem.stats[StatType.MaxHealth].CurrentValue;
        }
    }

    public float DamageResistance
    {
        get
        {
            return statSystem.stats[StatType.DamageResistance].CurrentValue;
        }
    }

    public void IncreaseCurrentHealth(float amount)
    {
        Stat currentHealthStat = statSystem.stats[StatType.CurrentHealth];
        Stat maxHealthStat = statSystem.stats[StatType.MaxHealth];

        currentHealthStat.PernamentBonusValue += amount;

        if(currentHealthStat.CurrentValue > maxHealthStat.CurrentValue)
        {
            float redundantHealth = maxHealthStat.CurrentValue - currentHealthStat.CurrentValue;
            currentHealthStat.PernamentBonusValue -= redundantHealth;
        }
    }

    public void DecreaseCurrentHealth(float amount)
    {
        statSystem.stats[StatType.CurrentHealth].PernamentBonusValue -= amount;
    }

    // When health goes below 0, this game object is destroyed.
    // By default, destroy the object that contains Health component.
    [SerializeField] GameObject gameObjectToDestroy;

    // How long before objectToDestroy is destroyed, in seconds.
    [SerializeField] float destroyDelayTimeInSeconds = 0.0f;

    private void Awake()
    {
        if(!statSystem)
        {
            statSystem = GetComponent<StatSystem>();
        }
        if(!statSystem)
        {
            statSystem = GetComponentInChildren<StatSystem>(true);
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
            DecreaseCurrentHealth(finalDamage);
        }

        // Health goes below zero? Destroy root component.
        if (CurrentHealth == 0)
        {
            StartCoroutine("Die");
        }
    }

    public void Heal(float amount)
    {
        IncreaseCurrentHealth(amount);
    }

    public virtual IEnumerator Die()
    {
        yield return new WaitForSeconds(destroyDelayTimeInSeconds);
        Destroy(gameObjectToDestroy);
    }
}
