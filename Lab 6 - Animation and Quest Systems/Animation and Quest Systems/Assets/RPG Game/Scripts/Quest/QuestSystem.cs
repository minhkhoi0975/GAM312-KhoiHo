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
    public List<QuestProgress> ActiveQuests
    {
        get
        {
            return activeQuests;
        }
    }

    // Callback when a quest starts.
    public delegate void QuestStartedCallback(Quest quest);
    public QuestStartedCallback questStartedCallback;

    // Callback when a quest's progression changes.
    public delegate void QuestProgressionUpdated(Quest quest, int progressValue);
    public QuestProgressionUpdated questProgressionUpdatedCallback;

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

    private void Start()
    {
        // Add functions to callbacks.
        character.Inventory.itemAddedToInventoryCallback += OnItemAddedToInventory;
        character.Inventory.itemRemovedFromInventoryCallback += OnItemRemovedFromInventory;
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

        questStartedCallback?.Invoke(quest);
        Debug.Log("Quest started: " + quest.QuestName);
    }

    // Complete a quest.
    public void CompleteQuest(Quest completedQuest)
    {
        foreach (QuestProgress quest in activeQuests)
        {
            if (quest.Quest == completedQuest)
            {
                // If the quest is Colect quest, removed all collected items.
                if (quest.Quest.QuestType == QuestType.Collection)
                {
                    character.Inventory.RemoveFromBackpack(((CollectionQuest)quest).ItemToCollect, ((CollectionQuest)quest).MinCount);
                }

                // Remove the quest from the list of active quests.
                activeQuests.Remove(quest);

                // Reward the player.
                Character playerCharacter = GetComponent<Character>();

                foreach (ItemInstance rewardItem in completedQuest.RewardItems)
                {
                    playerCharacter.Inventory.AddToBackpack(rewardItem);
                }

                foreach (StatModifier statModifier in completedQuest.RewardStatChanges)
                {
                    playerCharacter.StatSystem.AddModifier(statModifier);
                }

                questCompletedCallback?.Invoke(completedQuest);
                Debug.Log("Quest completed: " + completedQuest.QuestName);
                return;
            }
        }
    }

    // ---------------
    // CALLBACKS
    // ---------------

    // Called when an item is added to the inventory.
    public void OnItemAddedToInventory(ItemDefinition addedItem, int quantity)
    {
        foreach (QuestProgress quest in activeQuests)
        {
            if (quest.Quest.QuestType == QuestType.Collection)
            {
                if (addedItem == ((CollectionQuest)quest).ItemToCollect)
                {
                    quest.progressValue += quantity;
                    questProgressionUpdatedCallback?.Invoke(quest, quest.progressValue);
                }
            }
        }
    }

    // Called when an item is removed from the inventory.
    public void OnItemRemovedFromInventory(ItemDefinition removedItem, int quantity)
    {
        foreach (QuestProgress quest in activeQuests)
        {
            if (quest.Quest.QuestType == QuestType.Collection)
            {
                if (removedItem == ((CollectionQuest)quest).ItemToCollect)
                {
                    quest.progressValue -= quantity;
                    questProgressionUpdatedCallback?.Invoke(quest, quest.progressValue);
                }
            }
        }
    }

    // Called when a game object is destroyed.
    public void OnGameObjectDestroyed(GameObject destroyedGameObject)
    {
        foreach (QuestProgress quest in activeQuests)
        {
            if (quest.Quest.QuestType == QuestType.Destroy)
            {
                // Need to find a better way to find the prefab from the gameobject.
                if (destroyedGameObject.name.StartsWith(((DestroyQuest)quest).EnemyPrefab.name))
                {
                    quest.progressValue++;
                    questProgressionUpdatedCallback?.Invoke(quest, quest.progressValue);
                }
            }
        }
    }
}
