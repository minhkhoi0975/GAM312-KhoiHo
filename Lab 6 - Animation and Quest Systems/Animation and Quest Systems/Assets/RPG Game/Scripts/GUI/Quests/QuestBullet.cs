/**
 * QuestBullet.cs
 * Description: This script updates the objective and the progress of a quest bullet.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBullet : MonoBehaviour
{
    [SerializeField] Text objectiveText;
    [SerializeField] Text progressionText;

    // Color of the text when the player has not done anything for the quest yet.
    [SerializeField] Color notStartedColor;
    // Color of the text when the player has done something for the quest.
    [SerializeField] Color inProgressColor;
    // Color of the text when the player is ready to complete the quest.
    [SerializeField] Color readyToCompleteColor;

    public void SetQuestBullet(QuestProgress quest)
    {
        if (quest.Quest == null)
            return;

        switch (quest.Quest.QuestType)
        {
            case QuestType.Collection:
                objectiveText.text = quest.Quest.QuestName + ": Collect " + ((CollectionQuest)(quest.Quest)).MinCount + " " + ((CollectionQuest)(quest.Quest)).ItemToCollect.name;
                progressionText.text = quest.progressValue + "/" + ((CollectionQuest)(quest.Quest)).MinCount;

                if(quest.progressValue == 0)
                {
                    SetTextColor(notStartedColor);
                }
                else if (quest.progressValue < ((CollectionQuest)(quest.Quest)).MinCount)
                {
                    SetTextColor(inProgressColor);
                }
                else
                {
                    SetTextColor(readyToCompleteColor);
                }

                break;

            case QuestType.Destroy:

                objectiveText.text = quest.Quest.QuestName + ": Destroy " + ((DestroyQuest)(quest.Quest)).NumEnemiesToDestroy + " " + ((DestroyQuest)(quest.Quest)).EnemyPrefab.name;
                progressionText.text = quest.progressValue + "/" + ((DestroyQuest)(quest.Quest)).NumEnemiesToDestroy;

                if (quest.progressValue == 0)
                {
                    SetTextColor(notStartedColor);
                }
                else if (quest.progressValue < ((DestroyQuest)(quest.Quest)).NumEnemiesToDestroy)
                {
                    SetTextColor(inProgressColor);
                }
                else
                {
                    SetTextColor(readyToCompleteColor);
                }

                break;
        }
    }

    public void SetTextColor(Color color)
    {
        objectiveText.color = color;
        progressionText.color = color;
    }
}
