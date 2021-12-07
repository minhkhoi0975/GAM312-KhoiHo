/**
 * CollectQuest.cs
 * Description: This script contains a base class for quests that require the player to deliver an item to a location.
 * Programmer : Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Delivery Quest", menuName = "Quests/Create a new Delivery Quest")]
public class DeliveryQuest : Quest
{
    // What item must be delivered?
    [SerializeField] ItemDefinition itemToDeliver;
    public ItemDefinition ItemToDeliver
    {
        get
        {
            return itemToDeliver;
        }
    }

    private void Awake()
    {
        questType = QuestType.Delivery;
    }
}
