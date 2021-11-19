/**
 * AIController.cs
 * Description: This script deals with the behavior of an NPC.
 * Programmer: Khoi Ho
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,           // The NPC stands still and does nothing.
    Alerted         // The enemy is detected. Attack the enemy.
}

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    // Reference to the navmesh agent.
    [SerializeField] NavMeshAgent navMeshAgent;

    // Reference to the character component.
    [SerializeField] Character character;

    // Reference to the stat system.
    [SerializeField] StatSystem statSystem;

    // Reference to the inventory.
    [SerializeField] Inventory inventory;

    // Current state of the enemy.
    AIState currentState = AIState.Idle;

    // Reference to the enemy.
    GameObject enemy;

    // Transform of the enemy.
    Transform enemyTransform;

    // How long the NPC has to way before next attack?
    public float attackDelayInSeconds = 3.0f;

    // Returns false when the NPC is under attack delay.
    bool canAttack = true;

    private void Awake()
    {
        if (!navMeshAgent)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        if(!character)
        {
            character = GetComponent<Character>();
        }

        if (!statSystem)
        {
            statSystem = GetComponent<StatSystem>();
        }

        if (!inventory)
        {
            inventory = GetComponent<Inventory>();
        }
    }

    private void Start()
    {
        if (enemy == null)
        {
            PlayerCharacterInput player = FindObjectOfType<PlayerCharacterInput>();
            if (player)
            {
                enemy = player.gameObject;
                enemyTransform = enemy.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0.0f)
            return;

        UpdateAIBehavior();
    }

    // Update the behavior of the AI.
    void UpdateAIBehavior()
    {
        switch (currentState)
        {
            case AIState.Idle:

                // NPC see the enemy? Start chasing the enemy.
                if (enemy && Vector3.Distance(enemy.transform.position, transform.position) <= statSystem.GetCurrentValue(StatType.DetectionRadius))
                {
                    currentState = AIState.Alerted;
                }

                break;

            case AIState.Alerted:

                ChaseEnemy();
                AttackEnemy();

                // The enemy is too far away? Stop chasing the enemy.
                if (!enemy || Vector3.Distance(enemy.transform.position, transform.position) >= statSystem.GetCurrentValue(StatType.EvasionRadius))
                {
                    currentState = AIState.Idle;
                }

                break;
        }
    }

    void ChaseEnemy()
    {
        if (!enemy)
            return;

        // If the enemy moves, update the transform.
        if (enemyTransform.position != enemy.transform.position)
        {
            enemyTransform = enemy.transform;
        }

        // Set the destination for the NPC.
        navMeshAgent.SetDestination(enemyTransform.position);
    }

    void AttackEnemy()
    {
        if (!enemy || !character || !statSystem || !inventory)
            return;

        // Cannot attack if the NPC is under attack delay.
        if (!canAttack)
            return;

        // Check if the enemy is close enough to the NPC.
        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
        if (distanceToEnemy > statSystem.GetCurrentValue(StatType.AttackRange))
            return;

        // Check if the NPC looks at the enemy.
        float lookDotProduct = Vector3.Dot(transform.forward, (enemy.transform.position - transform.position).normalized);
        if (lookDotProduct >= 0.9f)
        {
            character.Attack(statSystem.GetCurrentValue(StatType.AttackRange), statSystem.GetCurrentValue(StatType.Damage), 
                statSystem.GetCurrentValue(StatType.CriticalDamageMultiplier), statSystem.GetCurrentValue(StatType.CriticalChance));

            StartCoroutine(WaitBeforeNextAttack());
        }
    }

    IEnumerator WaitBeforeNextAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackDelayInSeconds);
        canAttack = true;
    }
}
