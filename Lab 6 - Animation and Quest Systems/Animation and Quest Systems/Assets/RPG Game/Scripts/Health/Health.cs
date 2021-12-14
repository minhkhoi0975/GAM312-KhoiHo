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
    // Callback when the game object takes damage.
    public delegate void OnGameObjectTakeDamage(float damage);
    public OnGameObjectTakeDamage onGameObjectTakesDamageCallback;

    // Callback when the game object is destroyed.
    public delegate void OnGameObjectDestroyed(GameObject gameObject);
    public OnGameObjectDestroyed onGameObjectDestroyedCallback;

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

    private void Start()
    {
        PlayerCharacterInput player = FindObjectOfType<PlayerCharacterInput>();
        if(player)
        {
            onGameObjectDestroyedCallback += player.gameObject.GetComponent<QuestSystem>().OnGameObjectDestroyed;
        }
    }

    // Take damage.
    public void TakeDamage(float damageFromSource)
    {
        // Don't take damage if the character is already dead.
        if (statSystem.GetCurrentValue(StatType.CurrentHealth) <= 0)
            return;

        float finalDamage = damageFromSource - DamageResistance;
        Debug.Log("Damage: " + damageFromSource + " - " + DamageResistance + " = " + finalDamage);

        if (finalDamage > 0)
        {
            StatModifier healthModifier = new StatModifier(StatType.CurrentHealth, StatModifierType.IncreaseBaseValue, -finalDamage);
            statSystem.AddModifier(healthModifier);

            // Health goes below zero? Die.
            if (CurrentHealth == 0)
            {
                StartCoroutine("Die");
            }

            onGameObjectTakesDamageCallback?.Invoke(finalDamage);
        }
    }

    // Heal.
    public void Heal(float amount)
    {
        StatModifier healthModifier = new StatModifier(StatType.CurrentHealth, StatModifierType.IncreaseBaseValue, amount);
        statSystem.AddModifier(healthModifier);
    }

    public virtual IEnumerator Die()
    {
        onGameObjectDestroyedCallback?.Invoke(gameObject);

        yield return new WaitForSeconds(destroyDelayTimeInSeconds);
        Destroy(gameObjectToDestroy);
    }
}
