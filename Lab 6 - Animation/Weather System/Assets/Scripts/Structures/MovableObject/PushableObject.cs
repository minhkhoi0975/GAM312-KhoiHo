/**
 * MovableObject.cs
 * Description: This script handles the movement of a pushable object.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
public class PushableObject : MonoBehaviour
{
    // Reference to the fixed joint component.
    [SerializeField] FixedJoint fixedJoint;
    public FixedJoint FixedJoint
    {
        get
        {
            return fixedJoint;
        }
    }

    // The rotational speed of the object when it is being pushed.
    [SerializeField] float rotationalSpeed = 80.0f;

    // The rigid body of this object.
    [SerializeField] Rigidbody rigidBody;

    // The character that pushes this object.
    [HideInInspector] public Character pusher;

    // If the character pushes forward, then the object should move in this direction.
    [HideInInspector] public Vector3 relativePushingDirection;

    public Rigidbody RigidBody
    {
        get
        {
            return rigidBody;
        }
    }

    private void Awake()
    {
        if(!fixedJoint)
        {
            fixedJoint = GetComponent<FixedJoint>();
        }

        if (!rigidBody)
        {
            rigidBody = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        // If the character is too far away from this object, detach the character from this object.
        if (pusher && Vector3.Distance(pusher.transform.position, transform.position) > 3.0f)
        {
            pusher.StopPushingObject();
        }
    }

    // Move the object in a direction relative to the object itself.
    public void Move(float verticalAxis, float horizontalAxis = 0.0f)
    {
        if (!pusher)
            return;

        // Push/pull the object.
        if (verticalAxis != 0.0f)
        {
            // Don't make the pushed object move too fast.
            if (rigidBody.velocity.magnitude < 3.0f)
            {
                // Get the pushing force from player's stats.
                float pushingForce = pusher.StatSystem.GetCurrentValue(StatType.PushingForce);

                // Push.
                rigidBody.AddRelativeForce(relativePushingDirection * verticalAxis * pushingForce, ForceMode.Force);
            }

            // Fix the pushed object not moving sometimes.
            RigidBody.AddForce(new Vector3(0.0F, 0.5f, 0.0f), ForceMode.VelocityChange);

            if (verticalAxis > 0.0f)
            {
                pusher.AnimatorController.SetFloat("pushingDirection", 1.0f);
            }
            else if(verticalAxis < 0.0f)
            {
                pusher.AnimatorController.SetFloat("pushingDirection", -1.0f);
            }
            else
            {
                pusher.AnimatorController.SetFloat("pushingDirection", 0.0f);
            }
        }

        // Rotate the object around its pivot.
        if (horizontalAxis != 0.0f)
        {
            Quaternion currentRotation = rigidBody.rotation;
            Quaternion newRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y + horizontalAxis * rotationalSpeed * Time.fixedDeltaTime, currentRotation.eulerAngles.z);
            rigidBody.MoveRotation(newRotation);
        }

        // Make the pusher look at the object.
        Vector3 lookDirection = transform.TransformDirection(relativePushingDirection);
        Quaternion lookQuaternion = Quaternion.Euler(0.0f, Quaternion.LookRotation(lookDirection).eulerAngles.y, 0.0f);
        pusher.RigidBodyComponent.rotation = Quaternion.Slerp(pusher.RigidBodyComponent.rotation, lookQuaternion, 0.5f);
    }
}