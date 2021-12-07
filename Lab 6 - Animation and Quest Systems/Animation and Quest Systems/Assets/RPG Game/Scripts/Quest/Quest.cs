/**
 * Quest.cs
 * Description: This script contains the base class for quests.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum QuestType
{
    Collection = 0,
    Destroy = 1,
    Delivery = 2
}

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Create a new Quest")]
public class Quest : ScriptableObject
{
    [SerializeField] protected QuestType questType;

    [SerializeField] string questName = "New Quest";

    [SerializeField] string questDescription = "No Description";

    [SerializeField] List<ItemInstance> rewardItems;

    [SerializeField] List<StatModifier> rewardStatChanges;
}
