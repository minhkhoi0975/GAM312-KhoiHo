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
        if(!inventoryPanel)
        {
            inventoryPanel = GetComponentInChildren<InventoryPanelLogic>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Open/Close the inventory (keyboard).
        if(Input.GetButtonDown("Inventory"))
        {
            // Show the inventory UI.
            inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeSelf);

            // Make the event system select the first item slot in the inventory UI.
            ItemSlotLogic firstItemSlot = inventoryPanel.gameObject.GetComponentInChildren<ItemSlotLogic>();
            if (firstItemSlot)
            {
                firstItemSlot.gameObject.GetComponent<Button>().Select();
                firstItemSlot.gameObject.GetComponent<Button>().OnSelect(null);
            }

            if(inventoryPanel.gameObject.activeSelf)
            {
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }
    }
}
