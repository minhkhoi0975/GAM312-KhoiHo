/**
 * CollectQuest.cs
 * Description: This script contains a base class for quests that require the player to collect X number of items.
 * Programmer : Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collection Quest", menuName = "Quests/Create a new Collection Quest")]
public class CollectionQuest : Quest
{
    [SerializeField] ItemDefinition itemToCollect;

    [SerializeField] int minCount;

    private void Awake()
    {
        questType = QuestType.Collection;
    }
}
