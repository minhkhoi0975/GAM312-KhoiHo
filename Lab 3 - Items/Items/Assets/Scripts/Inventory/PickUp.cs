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
    // Info about the pick-up.
    [SerializeField] ItemInstance itemInstance;
    public ItemInstance ItemInstance
    {
        get
        {
            return itemInstance;
        }
        set
        {
            itemInstance = value;
            UpdateMesh();
        }
    }

    // Reference to the visual of the pick-up.
    GameObject mesh; 

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Character"))
        {
            Destroy(gameObject);
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
        if(itemInstance)
        {
            mesh = Instantiate(itemInstance.itemDefinition.mesh, transform);
        }
    }
}
