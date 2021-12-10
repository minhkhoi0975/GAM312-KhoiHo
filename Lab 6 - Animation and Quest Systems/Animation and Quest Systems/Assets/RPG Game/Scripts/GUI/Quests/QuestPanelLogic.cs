/**
 * QuestPanelLogic.cs
 * Description: This script handles the logic of the Quest panel.
 * Programmer: Khoi Ho
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanelLogic : MonoBehaviour
{
    // Reference to the quest system
    [SerializeField] QuestSystem questSystem;

    // Reference to the transform of the content of quest bullets.
    [SerializeField] Transform contentTransform;

    // The prefab for each quest bullet.
    [SerializeField] GameObject questBulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (!questSystem)
        {
            questSystem = FindObjectOfType<PlayerCharacterInput>()?.GetComponent<QuestSystem>();
        }

        questSystem.questStartedCallback += OnQuestStarted;
        questSystem.questProgressionUpdatedCallback += OnQuestProgressionUpdated;
        questSystem.questCompletedCallback += OnQuestCompletedUpdated;

        UpdateQuestBullets();
    }

    public void OnQuestStarted(Quest quest)
    {
        UpdateQuestBullets();
    }

    public void OnQuestProgressionUpdated(Quest quest, int progressValue)
    {
        UpdateQuestBullets();
    }

    private void OnQuestCompletedUpdated(Quest quest)
    {
        UpdateQuestBullets();
    }

    void UpdateQuestBullets()
    {
        // Remove all the contents in contentTransform.
        foreach (Transform childTransform in contentTransform)
        {
            Destroy(childTransform.gameObject);
        }

        // Add new contents to contentTransform.
        foreach (QuestProgress questProgress in questSystem.ActiveQuests)
        {
            GameObject questBulletGameObject = Instantiate(questBulletPrefab, contentTransform);
            questBulletGameObject.GetComponent<QuestBullet>().SetQuestBullet(questProgress);
        }
    }
}
