using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterDeparture : MonoBehaviour
{
    public Transform destination;  // If an object enters the departure, where does the object teleport to?

    private void OnTriggerEnter(Collider other)
    {
        if (!destination)
            return;

        GameObject otherGameObject = other.gameObject;
        Rigidbody otherGameObjectRigidBody = otherGameObject.GetComponent<Rigidbody>();

        if(otherGameObjectRigidBody)
        {
            otherGameObjectRigidBody.position = destination.position;
            otherGameObjectRigidBody.rotation = destination.rotation;
        }
        else
        {
            otherGameObject.transform.position = destination.position;
            otherGameObject.transform.rotation = destination.rotation;
        }
    }
}
