using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    // Reference to the navmesh agent.
    [SerializeField] NavMeshAgent navMeshAgent;

    // Reference to the enemy.
    GameObject enemy;

    // Transform of the enemy.
    Transform enemyTransform;

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
        ChaseEnemy();
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
