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
    [SerializeField] List<Quest> activeQuests = new List<Quest>();
}
