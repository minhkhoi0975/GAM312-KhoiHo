/**
 * HUDPanel.cs
 * Description: This script updates the stats displayed on the HUD.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanel : MonoBehaviour
{
    // Reference to the player's stats.
    [SerializeField] StatSystem playerStats;

    // Reference to texts on HUD.
    public Text textHealth;
    public Text textDamageResistance;
    public Text textDamage;

    // Start is called before the first frame update
    void Start()
    {
        if(playerStats)
        {
            playerStats.statsUpdatedCallback += UpdateHUD;
            UpdateHUD();
        }
    }

    void UpdateHUD()
    {
        if(playerStats)
        {
            textHealth.text = "Health: " + (int)playerStats.GetCurrentValue(StatType.CurrentHealth) + "/" + (int)playerStats.GetCurrentValue(StatType.MaxHealth);
            textDamageResistance.text = "Damage Resistance: " + (int)playerStats.GetCurrentValue(StatType.DamageResistance);
            textDamage.text = "Damage: " + (int)playerStats.GetCurrentValue(StatType.Damage);
        }
    }
}
