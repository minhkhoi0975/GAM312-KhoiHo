/**
 * ForceField.cs
 * Description: This script handles the logic of force field. Force field is disabled when all the triggers are active.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    // These triggers must be active so that the force field is disabled.
    [SerializeField] List<ForceFieldTrigger> triggers;

    private void Start()
    {
        // Make all triggers reference to this force field.
        foreach (ForceFieldTrigger trigger in triggers)
        {
            trigger.forceField = this;
        }
    }

    public void UpdateForceFieldStatus()
    {
        // All triggers are active? Disable the force field.
        if (AreAllTriggersActive())
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
        }
    }

    // Check if all the triggers are active.
    public bool AreAllTriggersActive()
    {
        foreach (ForceFieldTrigger trigger in triggers)
        {
            if (!trigger.IsActive)
            {
                return false;
            }
        }

        return true;
    }
}
