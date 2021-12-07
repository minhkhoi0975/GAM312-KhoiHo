/**
 * QuestProgress.cs
 * Description: This script stores the progress of a quest.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestProgress
{
    // The quest we need to keep track of.
    [SerializeField] Quest quest;
    public Quest Quest
    {
        get
        {
            return quest;
        }
    }

    // If the quest is Destroy, then this variable stores the number of killed creatures.
    // If the quest is Collect, then this variable stores the number of collected items.
    public int progressValue;

    public QuestProgress(Quest quest)
    {
        this.quest = quest;
    }

    public static implicit operator Quest(QuestProgress questProgress)
    {
        return questProgress.quest;
    }

    public static bool operator ==(QuestProgress left, Quest right)
    {
        return left.quest = right;
    }

    public static bool operator !=(QuestProgress left, Quest right)
    {
        return left.quest != right;
    }
}
