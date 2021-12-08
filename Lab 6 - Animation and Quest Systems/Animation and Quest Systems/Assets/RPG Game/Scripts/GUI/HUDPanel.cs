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
    // Reference to the player's inventory.
    [SerializeField] Inventory playerInventory;

    // Reference to the player's stats.
    [SerializeField] StatSystem playerStats;

    // Reference to texts on HUD.
    public Text textMessage;
    public Text textHealth;
    public Text textDamageResistance;
    public Text textWeaponName;
    public Text textDamage;

    // The number of massages to be displayed on HUD.
    int messagesToDisplay = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (playerInventory)
        {
            playerInventory.inventoryUpdatedCallback += UpdateStats;
            playerInventory.itemEquippedCallback += OnItemEquipped;
            playerInventory.itemUnequippedCallback += OnItemUnequipped;
            playerInventory.itemConsumedCallback += OnItemConsumed;
            playerInventory.itemPickedUpCallback += OnItemPickedUp;
            playerInventory.itemDroppedCallback += OnItemDropped;
        }

        if (playerStats)
        {
            playerStats.statsUpdatedCallback += UpdateStats;
        }

        UpdateStats();
    }

    void UpdateStats()
    {
        if (playerStats)
        {
            textHealth.text = "HP: " + playerStats.GetCurrentValue(StatType.CurrentHealth) + "/" + playerStats.GetMaxValue(StatType.CurrentHealth);
            textDamageResistance.text = "DR: " + playerStats.GetCurrentValue(StatType.DamageResistance);

            if (playerInventory && playerInventory.weapon)
            {
                textWeaponName.text = playerInventory.weapon.itemDefinition.name;
            }
            else
            {
                textWeaponName.text = "Unarmed";
            }
            textDamage.text = "DM: " + playerStats.GetCurrentValue(StatType.Damage);
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
