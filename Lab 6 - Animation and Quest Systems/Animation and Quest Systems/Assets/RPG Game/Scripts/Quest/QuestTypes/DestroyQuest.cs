/**
 * DestroyQuest.cs
 * Description: This script contains a base class for quests that require the player to destroy X number of enemies.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Destroy Quest", menuName = "Quests/Create a new Destroy Quest")]
public class DestroyQuest : Quest
{
    // The prefab of enemies that have to be destroyed.
    [SerializeField] GameObject enemyPrefab;

    // How many enemies have to be destroyed?
    [SerializeField] int numEnemiesToDestroy;

    private void Awake()
    {
        questType = QuestType.Destroy;
    }
}
