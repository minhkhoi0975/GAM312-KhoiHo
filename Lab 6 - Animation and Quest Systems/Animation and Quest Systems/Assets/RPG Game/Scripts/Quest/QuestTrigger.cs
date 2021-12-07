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
    [SerializeField] Quest triggerQuest;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.attachedRigidbody || other.attachedRigidbody.GetComponent<PlayerCharacterInput>())
            return;

        // TODO: Add quest to player's quest list.
    }
}
