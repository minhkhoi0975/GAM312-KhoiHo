/**
 * StatPanelLogic.cs
 * Description: This scripts handles the logic of the Stats panel.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPanelLogic : MonoBehaviour
{
    // Reference to the player's stats.
    [SerializeField] StatSystem playerStats;

    // Referernce to the texts displaying the values of the stats.
    public Text textMovementSpeed;
    public Text textDashSpeedMultiplier;

    public Text textPushingForce;
    public Text textTelekinesisForce;
    public Text textTelekinesisDistance;

    public Text textHealth;
    public Text textDamageResistance;

    public Text textDamage;
    public Text textCriticalChance;
    public Text textCriticalMultiplier;
    public Text textAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        if (playerStats)
        {
            playerStats.statsUpdatedCallback += UpdatePanel;
            UpdatePanel();
        }
    }

    // Update the panel.
    void UpdatePanel()
    {
        if (!playerStats)
            return;

        textMovementSpeed.text = playerStats.GetCurrentValue(StatType.MovementSpeed).ToString();
        textDashSpeedMultiplier.text = playerStats.GetCurrentValue(StatType.DashSpeedMultiplier).ToString();

        textPushingForce.text = playerStats.GetCurrentValue(StatType.PushingForce).ToString();
        textTelekinesisForce.text = playerStats.GetCurrentValue(StatType.TelekinesisForce).ToString();
        textTelekinesisDistance.text = playerStats.GetCurrentValue(StatType.TelekinesisDistance).ToString();

        textHealth.text = playerStats.GetCurrentValue(StatType.CurrentHealth).ToString() + "/" + playerStats.GetMaxValue(StatType.CurrentHealth).ToString();
        textDamageResistance.text = playerStats.GetCurrentValue(StatType.DamageResistance).ToString();

        textDamage.text = playerStats.GetCurrentValue(StatType.Damage).ToString();
        textCriticalChance.text = playerStats.GetCurrentValue(StatType.CriticalChance).ToString() + "%";
        textCriticalMultiplier.text = playerStats.GetCurrentValue(StatType.CriticalDamageMultiplier).ToString();
        textAttackRange.text = playerStats.GetCurrentValue(StatType.AttackRange).ToString();
    }
}
