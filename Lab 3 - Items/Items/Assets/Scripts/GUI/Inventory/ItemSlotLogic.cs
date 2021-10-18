/**
 * ItemSlotLogic.cs
 * Description: This script handles the logic of an item slot in the player character's inventory.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemSlotType
{
    Head,
    Chest,
    Arms,
    Legs,
    Weapon,
    Backpack
}

public class ItemSlotLogic : MonoBehaviour
{
    // References to the icon and the name of the item.
    public Image icon;
    public Text itemName;

    // Item slot type.
    public ItemSlotType itemSlotType = ItemSlotType.Backpack;

    // If the item is equipped, the index is -1.
    // Otherwise, return the index of the item in the backpack.
    public int index = 0;
}
