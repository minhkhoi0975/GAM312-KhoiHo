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
    // Reference to the Character component.
    [SerializeField] Character character;

    // List of active quests.
    [SerializeField] List<QuestProgress> activeQuests = new List<QuestProgress>();

    // Callback when a quest is completed.
    public delegate void QuestCompletedCallback(Quest quest);
    public QuestCompletedCallback questCompletedCallback;

    private void Awake()
    {
        if (!character)
        {
            character = GetComponent<Character>();
        }
    }

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

        // If the quest is Collection, let the quest system know the current number of items in the inventory.
        if (quest.QuestType == QuestType.Collection)
        {
            int numItems = character.Inventory.Count(((CollectionQuest)quest).ItemToCollect);
            activeQuests[activeQuests.Count - 1].progressValue = numItems;
        }

        // Add functions to callbacks.
        character.Inventory.itemAddedToInventoryCallback += OnItemAddedToInventory;
        character.Inventory.itemRemovedFromInventoryCallback -= OnItemRemovedFromInventory;
    }

    // Complete a quest.
    public void CompleteQuest(Quest completedQuest)
    {
        foreach (QuestProgress quest in activeQuests)
        {
            if (quest == completedQuest)
            {
                // Remove functions from callbacks.
                character.Inventory.itemAddedToInventoryCallback -= OnItemAddedToInventory;
                character.Inventory.itemRemovedFromInventoryCallback -= OnItemRemovedFromInventory;

                // Remove the quest from the list of active quests.
                activeQuests.Remove(quest);

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

                questCompletedCallback?.Invoke(completedQuest);
                return;
            }
        }
    }

    // ---------------
    // CALLBACKS
    // ---------------

    public void OnItemAddedToInventory(ItemInstance item)
    {
        foreach (QuestProgress quest in activeQuests)
        {
            if (quest.Quest.QuestType == QuestType.Collection && item == ((CollectionQuest)quest).ItemToCollect)
            {
                quest.progressValue += item.CurrentStackSize;
            }
        }
    }

    public void OnItemRemovedFromInventory(ItemInstance item)
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
