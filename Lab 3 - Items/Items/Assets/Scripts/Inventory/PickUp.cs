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
            UpdateMesh();
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

    // Reference to the visual of the pick-up.
    GameObject mesh; 

    private void OnTriggerEnter(Collider other)
    {
        // Get the Character component of the other game object.
        Character characterComponent = other.GetComponent<Character>();
        if(!characterComponent)
        {
            characterComponent = other.GetComponentInParent<Character>();
        }

        if(characterComponent)
        {
            // Get the inventory of the character.
            Inventory characterInventory = characterComponent.gameObject.GetComponent<Inventory>();

            
            if(characterInventory)
            {
                // Add the item to the backpack.
                ItemInstance pickedUpItem = ScriptableObject.CreateInstance<ItemInstance>();
                pickedUpItem.itemDefinition = itemDefinition;
                pickedUpItem.CurrentStackSize = currentStackSize;

                characterInventory.AddToBackPack(pickedUpItem);

                // Try equipping the item.
                characterInventory.Equip(characterInventory.backpack.Count - 1, true);

                // Destroy the pick-up.
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        UpdateMesh();
    }

    // Update the visual representation of the pickup.
    public void UpdateMesh()
    {
        // Remove the old visual.
        if(mesh)
        {
            Destroy(mesh);
        }

        // Create a new visual.
        if(itemDefinition)
        {
            mesh = Instantiate(itemDefinition.mesh, transform);
        }
    }

    public void SetPickUp(ItemInstance itemInstance)
    {
        this.itemDefinition = itemInstance.itemDefinition;
        this.currentStackSize = itemInstance.CurrentStackSize;

        UpdateMesh();
    }

    public void SetPickUp(ItemDefinition itemDefinition, int quantity = 1)
    {
        ItemInstance itemInstance = ScriptableObject.CreateInstance<ItemInstance>();
        itemInstance.itemDefinition = itemDefinition;
        itemInstance.CurrentStackSize = quantity;

        SetPickUp(itemInstance);
    }
}
