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
    }

    // Update is called once per frame
    void Update()
    {
        // Open/Close the inventory (keyboard).
        if (Input.GetButtonDown("Inventory"))
        {
            // Show the inventory UI.
            inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeSelf);

            if (inventoryPanel.gameObject.activeSelf)
            {
                inventoryPanel.MakeEventSystemSelectFirstSlot();

                Time.timeScale = 0.0f;
            }
            else
            {
                // When closing the inventory, close the Drop Item panel as well.
                DropItemPanelLogic dropItemPanel = inventoryPanel.gameObject.GetComponentInChildren<DropItemPanelLogic>(true);
                if (dropItemPanel)
                {
                    dropItemPanel.gameObject.SetActive(false);
                }

                Time.timeScale = 1.0f;
            }
        }
    }
}
