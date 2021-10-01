/**
 * MovableObject.cs
 * Description: This script handles the movement of a pushable object.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushableObject : MonoBehaviour
{
    // The movement speed of the object when it is being pushed.
    [SerializeField] float movementSpeed = 90.0f;

    // The rotational speed of the object when it is being pushed.
    [SerializeField] float rotationalSpeed = 180.0f;

    // The rigid body of this object.
    [SerializeField] Rigidbody rigidBody;

    // The character that pushes this object.
    [HideInInspector] public Character pusher;

    // If the character pushes forward, then the object should move in this direction.
    [HideInInspector] public Vector3 relativePushingForwardDirection;

    // Used to attach the character to the pushed object.
    [HideInInspector] public Vector3 relativeAttachmentPosition;

    public Rigidbody RigidBody
    {
        get
        {
            return rigidBody;
        }
    }

    private void Awake()
    {
        if(!rigidBody)
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        // Set pushingForwardDirection to be the forward vector relative to this game object.
        relativePushingForwardDirection = transform.forward;
    }

    public void FixedUpdate()
    {
        UpdatePusherTransform();
    }

    // Move the object in a direction relative to the object itself.
    public void Move(float verticalAxis, float horizontalAxis = 0.0f)
    {
        if (!pusher)
            return;

        // Push/pull the object.
        rigidBody.AddRelativeForce(relativePushingForwardDirection * verticalAxis * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        // Rotate the object around the object itself.
        Quaternion currentRotation = rigidBody.rotation;
        Quaternion newRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y + horizontalAxis * rotationalSpeed * Time.fixedDeltaTime, currentRotation.eulerAngles.z);
        rigidBody.MoveRotation(newRotation);
    }

    // Update the pusher's transform.
    public void UpdatePusherTransform()
    {
        if (!pusher)
            return;

        // If the character is too far away from this object, detach the character from this object.
        if (Vector3.Distance(pusher.transform.position, transform.position) > 3.0f)
        {
            pusher.StopPushingObject();
            return;
        }

        Rigidbody pusherRigidBody = pusher.GetComponent<Rigidbody>();

        // Translate the pusher to match the offset.
        pusherRigidBody.position = transform.TransformPoint(relativeAttachmentPosition);

        // Make the pusher look at the pushed object.
        Vector3 lookDirection = transform.TransformDirection(relativePushingForwardDirection);
        Quaternion lookQuaternion = Quaternion.Euler(0.0f, Quaternion.LookRotation(lookDirection).eulerAngles.y, 0.0f);
        pusherRigidBody.rotation = Quaternion.Slerp(pusherRigidBody.rotation, lookQuaternion, 0.5f);
    }
}