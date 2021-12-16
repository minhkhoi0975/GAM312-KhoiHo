/**
 * ForceFieldTrigger.cs
 * Description: This script handles the behavior of a force field trigger. A force field trigger is active if at least one object enters its collider.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ForceFieldTrigger : MonoBehaviour
{
    // Reference to the force field.
    [HideInInspector] public ForceField forceField;

    // Reference to the collider of the trigger.
    Collider collider;

    // The trigger is active if there is at least 1 object in the trigger.
    private bool isActive;
    public bool IsActive
    {
        get
        {
            return isActive;
        }
    }

    // List of objects in this trigger.
    List<GameObject> objectsInTrigger = new List<GameObject>();

    private void Awake()
    {
        if (!collider)
        {
            collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Add the object to objectsInTrigger.
        objectsInTrigger.Add(other.gameObject);

        // Make the trigger active.
        isActive = true;

        // Update the status of the force field.
        forceField.UpdateForceFieldStatus();
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the object out of the list.
        objectsInTrigger.Remove(other.gameObject);

        // No object is in the trigger? Then the trigger is inactive.
        if (objectsInTrigger.Count == 0)
        {
            isActive = false;
        }

        // Update the status of the force field.
        forceField.UpdateForceFieldStatus();
    }
}
