/**
 * HUDPanel.cs
 * Description: This script updates the stats displayed on the HUD.
 * Programmer: Khoi Ho
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanel : MonoBehaviour
{
    // Reference to the player character.
    [SerializeField] Character playerCharacter;

    // Reference to texts on HUD.
    public Text textMessage;
    public Text textHealth;
    public Text textDamageResistance;
    public Text textWeaponName;
    public Text textDamage;

    // Reference to the quest panel.
    public RectTransform questPanel;

    // Quest bullet prefab.
    public GameObject questBulletPrefab;

    // The number of massages to be displayed on HUD.
    int messagesToDisplay = 0;

    // Start is called before the first frame update
    void Start()
    {
        if( !playerCharacter)
        {
            playerCharacter = FindObjectOfType<PlayerCharacterInput>().GetComponent<Character>();
        }

        if (playerCharacter.Inventory)
        {
            playerCharacter.Inventory.inventoryUpdatedCallback += UpdateStats;
            playerCharacter.Inventory.itemEquippedCallback += OnItemEquipped;
            playerCharacter.Inventory.itemUnequippedCallback += OnItemUnequipped;
            playerCharacter.Inventory.itemConsumedCallback += OnItemConsumed;
            playerCharacter.Inventory.itemPickedUpCallback += OnItemPickedUp;
            playerCharacter.Inventory.itemDroppedCallback += OnItemDropped;
        }

        if (playerCharacter.StatSystem)
        {
            playerCharacter.StatSystem.statsUpdatedCallback += UpdateStats;
        }

        if (playerCharacter.QuestSystem)
        {
            playerCharacter.QuestSystem.questStartedCallback += OnQuestStarted;
            playerCharacter.QuestSystem.questProgressionUpdatedCallback += OnQuestProgressionUpdated;
            playerCharacter.QuestSystem.questCompletedCallback += OnQuestCompleted;
        }

        UpdateStats();
        UpdateQuests();
    }

    void UpdateStats()
    {
        if (playerCharacter.StatSystem)
        {
            textHealth.text = "HP: " + playerCharacter.StatSystem.GetCurrentValue(StatType.CurrentHealth) + "/" + playerCharacter.StatSystem.GetMaxValue(StatType.CurrentHealth);
            textDamageResistance.text = "DR: " + playerCharacter.StatSystem.GetCurrentValue(StatType.DamageResistance);

            if (playerCharacter.Inventory && playerCharacter.Inventory.weapon)
            {
                textWeaponName.text = playerCharacter.Inventory.weapon.itemDefinition.name;
            }
            else
            {
                textWeaponName.text = "Unarmed";
            }
            textDamage.text = "DM: " + playerCharacter.StatSystem.GetCurrentValue(StatType.Damage);
        }
    }

    void UpdateQuests()
    {
        // Remove all the contents in queestPanel.
        foreach (Transform childTransform in questPanel)
        {
            Destroy(childTransform.gameObject);
        }

        // Add new contents to contentTransform.
        foreach (QuestProgress questProgress in playerCharacter.QuestSystem.ActiveQuests)
        {
            GameObject questBulletGameObject = Instantiate(questBulletPrefab, questPanel);
            questBulletGameObject.GetComponent<QuestBullet>().SetQuestBullet(questProgress);
        }
    }

    void OnItemEquipped(ItemDefinition equippedItem)
    {
        DisplayMessage("You have equipped " + equippedItem + ".");
    }

    void OnItemUnequipped(ItemDefinition unequippedItem)
    {
        DisplayMessage("You have unequipped " + unequippedItem + ".");
    }

    void OnItemConsumed(ItemDefinition consumedItem)
    {
        DisplayMessage("You have consumed " + consumedItem.name + ".");
    }

    void OnItemPickedUp(ItemDefinition pickedUpItem, int quantity)
    {
        DisplayMessage("You have picked up " + quantity + " x " + pickedUpItem.name + ".");
    }

    void OnItemDropped(ItemDefinition droppedItem, int quantity)
    {
        DisplayMessage("You have dropped " + quantity + " x " + droppedItem.name + ".");
    }

    void OnQuestStarted(Quest quest)
    {
        DisplayMessage("Quest started: " + quest.QuestName);
        UpdateQuests();
    }

    void OnQuestProgressionUpdated(Quest quest, int progressValue)
    {
        UpdateQuests();
    }

    void OnQuestCompleted(Quest quest)
    {
        DisplayMessage("Quest completed: " + quest.QuestName);
        UpdateQuests();
    }

    // Display a message on screen.
    public void DisplayMessage(string message)
    {
        textMessage.text = message;
        StartCoroutine(ClearMessage());
    }

    // Wait for 5 seconds before removing the message.
    IEnumerator ClearMessage()
    {
        messagesToDisplay++;
        yield return new WaitForSeconds(5.0f);
        messagesToDisplay--;

        if (messagesToDisplay == 0)
        {
            textMessage.text = "";
        }
    }
}
