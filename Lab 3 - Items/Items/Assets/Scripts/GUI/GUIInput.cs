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
    public GameObject inventoryPanel;

    // Start is called before the first frame update
    void Awake()
    {
        if(!inventoryPanel)
        {
            inventoryPanel = FindObjectOfType<InventoryPanelLogic>().gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Open/Close the inventory (keyboard).
        if(Input.GetButtonDown("Inventory"))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);

            if(inventoryPanel.activeSelf)
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
