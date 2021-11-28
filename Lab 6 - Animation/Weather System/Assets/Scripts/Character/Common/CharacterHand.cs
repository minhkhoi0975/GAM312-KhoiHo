/**
 * CharacterHand.cs
 * Description: This script checks if the character is pushing an object.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHand : MonoBehaviour
{
    // Reference to the character.
    [SerializeField] Character character;
    public Character Character
    {
        get
        {
            return character;
        }
    }

    // The game object being pushed.
    GameObject pushedGameObject;
    public GameObject PushedGameObject
    {
        get
        {
            return pushedGameObject;
        }
    }

    private void Awake()
    {
        if (!character)
        {
            character = FindObjectOfType<Character>();
        }
    }

    // Start pushing an object.
    // gameObject is the object to be pushed.
    // initialPushingDirection is the initial pushing direction in world space.
    public void StartPushingObject(GameObject gameObject, Vector3 initialPushingDirection)
    {
        // I'm current pushing an object. I cannot push another one.
        if (pushedGameObject)
            return;

        // I cannot push this object if it has no physics.
        Rigidbody pushedObjectRigidBody = gameObject.GetComponent<Rigidbody>();
        if (!pushedObjectRigidBody)
            return;

        // I cannot push this object if it is not pushable.
        PushableObject pushable = gameObject.GetComponent<PushableObject>();
        if (!pushable)
            return;

        // The hand needs to know what game object is being pushed, and the pushed object needs to know the character that is pushing it.
        pushedGameObject = gameObject;
        pushable.pusher = character;

        // Attach the character's rigid body to the pushed object's rigid body.
        pushable.fixedJoint.connectedBody = character.RigidBodyComponent;

        // Set the initial position of the character relative to the pushed object.
        pushable.relativeAttachmentPosition = pushable.transform.InverseTransformPoint(character.transform.position);

        // Set the pushing direction relative to the pushed object.
        pushable.relativePushingDirection = pushable.transform.InverseTransformDirection(initialPushingDirection.normalized);   
    }

    public void StopPushingObject()
    {
        if (pushedGameObject)
        {
            // Detach the pushed object from the character.
            PushableObject pushable = pushedGameObject.GetComponent<PushableObject>();
            pushable.pusher = null;
            pushedGameObject = null;

            // Detach the character's rigid body to the pushed object's rigid body.
            pushable.fixedJoint.connectedBody = null;
        }
    }
}