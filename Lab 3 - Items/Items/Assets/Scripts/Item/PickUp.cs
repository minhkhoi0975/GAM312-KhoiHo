/**
 * PickUp.cs
 * Description: This script handles pickups. When a character touches this object, the character picks up this object.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Item Definition
    [SerializeField] ItemDefinition itemDefinition;
    public ItemDefinition ItemDefinition
    {
        get
        {
            return itemDefinition;
        }
        set
        {
            itemDefinition = value;
            UpdatePickUpVisual();
        }
    }

    // Quantity of the pick-up.
    [SerializeField] int currentStackSize = 1;
    public int CurrentStackSize
    {
        get
        {
            return currentStackSize;
        }
        set
        {
            currentStackSize = value < 0 ? 0 : (value > itemDefinition.MaxStackSize ? itemDefinition.MaxStackSize : value);
        }
    }

    // Reference to the visual presentation of the pick-up.
    GameObject mesh;

    private void OnTriggerEnter(Collider other)
    {
        // Get the player input component of the other game object to check whether the other object is a player character.
        PlayerCharacterInput playerInputComponent = other.GetComponent<PlayerCharacterInput>();
        if (!playerInputComponent)
        {
            playerInputComponent = other.GetComponentInParent<PlayerCharacterInput>();
        }

        if (playerInputComponent)
        {
            // Get the inventory of the player character.
            Inventory inventory = playerInputComponent.gameObject.GetComponent<Inventory>();

            if (inventory)
            {
                // Add the item to the backpack.
                ItemInstance pickedUpItem = ScriptableObject.CreateInstance<ItemInstance>();
                pickedUpItem.itemDefinition = itemDefinition;
                pickedUpItem.CurrentStackSize = currentStackSize;

                inventory.AddToBackPack(pickedUpItem);

                // Try equipping the item.
                inventory.Equip(inventory.backpack.Count - 1, true);

                Debug.Log("Picked up " + currentStackSize + "x" + itemDefinition.name);

                // Destroy the pick-up object.
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        UpdatePickUpVisual();
    }

    // Update the visual representation of the pickup.
    public void UpdatePickUpVisual()
    {
        // Remove the old visual.
        if (mesh)
        {
            Destroy(mesh);
        }

        // Create a new visual.
        if (itemDefinition)
        {
            mesh = Instantiate(itemDefinition.mesh, transform);
        }
    }

    // Set the properties of the pick-up.
    public void SetPickUp(ItemInstance itemInstance)
    {
        // Set the info about the pick-up.
        this.itemDefinition = itemInstance.itemDefinition;
        this.currentStackSize = itemInstance.CurrentStackSize;

        // Update the visual presentation of the pickup.
        UpdatePickUpVisual();
    }

    public void SetPickUp(ItemDefinition itemDefinition, int quantity = 1)
    {
        ItemInstance itemInstance = ScriptableObject.CreateInstance<ItemInstance>();
        itemInstance.itemDefinition = itemDefinition;
        itemInstance.CurrentStackSize = quantity;

        SetPickUp(itemInstance);
    }
}