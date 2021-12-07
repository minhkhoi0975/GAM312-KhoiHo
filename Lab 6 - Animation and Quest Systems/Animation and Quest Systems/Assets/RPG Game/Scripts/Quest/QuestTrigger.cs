/**
 * QuestTrigger.cs
 * Description: When the player enters this trigger, the player receives a quest.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] Quest triggeredQuest;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.attachedRigidbody || !other.attachedRigidbody.GetComponent<PlayerCharacterInput>())
            return;

        if (!triggeredQuest)
        {
            Debug.LogError("Error: the trigger does not have a quest attached to it.");
            return;
        }

        // TODO: Add quest to player's quest list.
        other.attachedRigidbody.GetComponent<QuestSystem>().AcceptQuest(triggeredQuest);
    }
}
