/**
 * GUIInput.cs
 * Description: This script handles the inputs for the UI from the player.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIInput : MonoBehaviour
{
    public GameObject inventoryPanel;

    // Start is called before the first frame update
    void Awake()
    {
        if(!inventoryPanel)
        {
            inventoryPanel = FindObjectOfType<InventoryGUILogic>().gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Open/Close the inventory.
        if(Input.GetButtonDown("Inventory"))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
}
