/**
 * QuestSystem.cs
 * Description: This script keeps track of all active quests the player has received.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    // List of active quests.
    [SerializeField] List<QuestProgress> activeQuests = new List<QuestProgress>();

    // Accept a quest
    public void AcceptQuest(Quest quest)
    {
        // Check if the quest is already accepted.
        foreach (Quest activeQuest in activeQuests)
        {
            if (activeQuest == quest)
            {
                return;
            }
        }

        // Add the quest to list of active quests.
        activeQuests.Add(new QuestProgress(quest));
    }

    // Complete a quest.
    public void CompleteQuest(Quest completedQuest)
    {
        foreach (QuestProgress quest in activeQuests)
        {
            if (quest == completedQuest)
            {
                // Reward the player.
                Character playerCharacter = GetComponent<Character>();

                foreach (ItemInstance rewardItem in completedQuest.RewardItems)
                {
                    playerCharacter.Inventory.AddToInventory(rewardItem);
                }

                foreach (StatModifier statModifier in completedQuest.RewardStatChanges)
                {
                    playerCharacter.StatSystem.AddModifier(statModifier);
                }

                // Remove the quest from the list of active quests.
                activeQuests.Remove(quest);
                return;
            }
        }
    }

    // ---------------
    // CALLBACKS
    // ---------------

    public void OnItemPickedUp(ItemInstance item)
    {
        foreach (QuestProgress quest in activeQuests)
        {
            if (quest.Quest.QuestType == QuestType.Collection && item == ((CollectionQuest)quest).ItemToCollect)
            {
                quest.progressValue += item.CurrentStackSize;
            }
        }
    }

    public void OnItemDropped(ItemInstance item)
    {
        foreach (QuestProgress quest in activeQuests)
        {
            if (quest.Quest.QuestType == QuestType.Collection && item == ((CollectionQuest)quest).ItemToCollect)
            {
                quest.progressValue -= item.CurrentStackSize;
            }
        }
    }
}
