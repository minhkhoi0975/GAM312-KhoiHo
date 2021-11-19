/**
 * GUIInput.cs
 * Description: This script handles the inputs for the UI from the player.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIInput : MonoBehaviour
{
    // Reference to the inventory panel.
    public InventoryPanelLogic inventoryPanel;

    // Reference to the stats panel.
    public StatPanelLogic statsPanel;

    // Is stats button down (Xbox One controller)?
    bool statsButtonDown = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!inventoryPanel)
        {
            inventoryPanel = GetComponent<InventoryPanelLogic>();
        }
        if (!inventoryPanel)
        {
            inventoryPanel = GetComponentInChildren<InventoryPanelLogic>(true);
        }

        if(!statsPanel)
        {
            statsPanel = GetComponent<StatPanelLogic>();
        }
        if(!statsPanel)
        {
            statsPanel = GetComponentInChildren<StatPanelLogic>(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Open/Close the inventory.
        if (Input.GetButtonDown("Inventory"))
        {
            // Show the inventory UI.
            inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeSelf);

            if (inventoryPanel.gameObject.activeSelf)
            {
                inventoryPanel.MakeEventSystemSelectFirstSlot();
            }
            else
            {
                // When closing the inventory, close the Drop Item panel as well.
                DropItemPanelLogic dropItemPanel = inventoryPanel.gameObject.GetComponentInChildren<DropItemPanelLogic>(true);
                if (dropItemPanel)
                {
                    dropItemPanel.gameObject.SetActive(false);
                }
            }
        }

        // Open/Close the stats (keyboard).
        if(Input.GetButtonDown("Stats"))
        {
            // Show the stats UI.
            statsPanel.gameObject.SetActive(!statsPanel.gameObject.activeSelf);
        }

        // Open/Close the stats (Xbox One controller).
        if(Input.GetAxis("StatsController") == 1.0f)
        {
            if(!statsButtonDown)
            {
                // Show the stats UI.
                statsPanel.gameObject.SetActive(!statsPanel.gameObject.activeSelf);

                statsButtonDown = true;
            }
        }
        else
        {
            statsButtonDown = false;
        }


        // If at least one of the panels are active, keep the game paused.
        if (AreAllPanelsInactive())
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }
    }

    // Check if all the panels are inactive.
    bool AreAllPanelsInactive()
    {
        return !inventoryPanel.gameObject.activeSelf && !statsPanel.gameObject.activeSelf;
    }
}
