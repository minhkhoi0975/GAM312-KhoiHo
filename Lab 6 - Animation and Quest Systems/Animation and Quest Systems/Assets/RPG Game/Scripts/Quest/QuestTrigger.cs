/**
 * QuestTrigger.cs
 * Description: This script defines a quest trigger. A quest trigger can either start or finish a quest.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestTriggerType
{
    StartQuest = 0,
    EndQuest = 1
}

public class QuestTrigger : MonoBehaviour
{
    // Reference to the quest name.
    [SerializeField] TextMesh questNameText;

    [SerializeField] QuestTriggerType triggerType;

    // The quest to start/end.
    [SerializeField] Quest quest;

    // When the quest starts or ends, these triggers are unlocked.
    [SerializeField] List<QuestTrigger> lockedTriggers;

    private void Awake()
    {
        // Set the quest name.
        if (!questNameText)
        {
            questNameText = GetComponentInChildren<TextMesh>();
        }
        questNameText.text = quest.QuestName;

        // Lock the triggers.
        foreach(QuestTrigger questTrigger in lockedTriggers)
        {
            questTrigger.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.attachedRigidbody || !other.attachedRigidbody.GetComponent<PlayerCharacterInput>())
            return;

        if (!quest)
        {
            Debug.LogError("Error: the trigger does not have a quest attached to it.");
            return;
        }

        if (triggerType == QuestTriggerType.StartQuest)
        {
            // TODO: Add quest to player's quest list.
            other.attachedRigidbody.GetComponent<QuestSystem>().AcceptQuest(quest);

            // Disable the start trigger.
            gameObject.SetActive(false);

            // Enable the end triggers.
            UnlockTriggers();
        }
        else
        {
            Character character = other.attachedRigidbody.GetComponent<Character>();

            for (int i = 0; i < character.QuestSystem.ActiveQuests.Count; i++)
            {
                QuestProgress activeQuest = character.QuestSystem.ActiveQuests[i];
                if (activeQuest == quest)
                {
                    bool isQuestCompleted = false;

                    if (quest.QuestType == QuestType.Collection)
                    {
                        int itemCount = activeQuest.progressValue;
                        int minItemCount = ((CollectionQuest)quest).MinCount;

                        if (itemCount >= minItemCount)
                        {
                            isQuestCompleted = true;
                        }
                    }
                    else if (quest.QuestType == QuestType.Destroy)
                    {
                        int killCount = activeQuest.progressValue;
                        int minKillCount = ((DestroyQuest)quest).NumEnemiesToDestroy;

                        if (killCount >= minKillCount)
                        {
                            isQuestCompleted = true;
                        }
                    }

                    if (isQuestCompleted)
                    {
                        character.QuestSystem.CompleteQuest(quest);
                        gameObject.SetActive(false);
                        UnlockTriggers();
                    }

                    return;
                }
            }
        }
    }

    // When a quest is completed, unlock the start triggers.
    void UnlockTriggers()
    {
        foreach(QuestTrigger startTrigger in lockedTriggers)
        {
            startTrigger.gameObject.SetActive(true);
        }
    }
}
