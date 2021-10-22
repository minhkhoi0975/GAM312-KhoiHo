/**
 * ItemSlotLogic.cs
 * Description: This script the input for an item slot button.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemSlotLogic))]
public class ItemSlotInput : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    // Reference to the item slot logic.
    public ItemSlotLogic itemSlotLogic;

    void Awake()
    {
        if(!itemSlotLogic)
        {
            itemSlotLogic = GetComponent<ItemSlotLogic>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Is this button currently selected by the event system?
        if (EventSystem.current.currentSelectedGameObject != gameObject)
            return;

        // If the player presses Enter, treat the input as clicking the left mouse button.
        if(Input.GetButtonDown("Submit"))
        {
            itemSlotLogic.OnLeftClicked();
        }

        // If the player presses Drop Item key, treat the input as clicking the right mouse button.
        if (Input.GetButtonDown("DropItem"))
        {
            itemSlotLogic.OnRightClicked();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Left Click.
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            itemSlotLogic.OnLeftClicked();
        }

        // Right Click.
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            itemSlotLogic.OnRightClicked();
        }
    }

    // If the mouse is over this button, make the event system select this button.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!itemSlotLogic.inventoryPanel.dropItemPanel.isActiveAndEnabled)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}