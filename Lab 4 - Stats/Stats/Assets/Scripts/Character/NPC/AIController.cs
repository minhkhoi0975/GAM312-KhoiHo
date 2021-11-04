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

    // Current state of the enemy.
    AIState currentState = AIState.Idle;

    // Reference to the enemy.
    GameObject enemy;

    // Transform of the enemy.
    Transform enemyTransform;

    // If the enemy is within this radius, the NPC will be alerted.
    public float detectionRadius = 10.0f;

    // If the enemy is out of this radius, the NPC will become idle again.
    public float evasionRadius = 20.0f;

    private void Awake()
    {
        if (!navMeshAgent)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }

    private void Start()
    {
        if (enemy == null)
        {
            PlayerCharacterInput player = FindObjectOfType<PlayerCharacterInput>();
            if(player)
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
        switch(currentState)
        {
            case AIState.Idle:
                // NPC see the enemy? Start chasing the enemy.
                if(Vector3.Distance(enemy.transform.position, transform.position) <= detectionRadius)
                {
                    currentState = AIState.Alerted;
                }
                break;

            case AIState.Alerted:
                ChaseEnemy();
                // The enemy is too far away? Stop chasing the enemy.
                if (Vector3.Distance(enemy.transform.position, transform.position) >= evasionRadius)
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
        if(enemyTransform.position != enemy.transform.position)
        {
            enemyTransform = enemy.transform;
        }

        // Set the destination for the NPC.
        navMeshAgent.SetDestination(enemyTransform.position);
    }
}
