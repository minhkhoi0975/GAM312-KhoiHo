/**
 * Character.cs
 * Description: This script contains the behaviors of a character.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBodyComponent;
    [SerializeField] private Camera cameraComponent;

    // Movement
    [SerializeField] private float baseMoveSpeed = 30.0f; // How fast the character moves normally (without dash).
    [SerializeField] private float turnRate = 10.0f;      // How fast the character turns.

    
    [SerializeField] private float dashSpeedMultiplier = 15.0f;

    // Object attraction
    [Tooltip("How far away can a MoveableObject be attracted by this character?")]
    [SerializeField]float maxAttractionDistance = 10.0f;
    [SerializeField]float attractionForce = 30.0f;

    // Start is called before the first frame update
    void Awake()
    {
        if(!rigidBodyComponent)
        {
            rigidBodyComponent = GetComponent<Rigidbody>();
        }

        if(!cameraComponent)
        {
            cameraComponent = FindObjectOfType<Camera>();
        }
    }

    // Move the character in direction relative to the player.
    public void Move(Vector3 relativeMoveDirection)
    {
        // Normalize the move direction to prevent fast diagonal movement.
        relativeMoveDirection = relativeMoveDirection.normalized;

        if (relativeMoveDirection.magnitude > 0.0f)
        {
            // Find the charater's new world Y rotation.
            float newWorldRotationY = Mathf.Atan2(relativeMoveDirection.x, relativeMoveDirection.z) * Mathf.Rad2Deg + cameraComponent.transform.eulerAngles.y;

            // Rotate the character smoothly.
            rigidBodyComponent.rotation = Quaternion.Lerp(rigidBodyComponent.rotation, Quaternion.Euler(0.0f, newWorldRotationY, 0.0f), Time.fixedDeltaTime * turnRate);

            // Calculate the move direction relative to the world.
            Vector3 worldMoveDirection = Quaternion.Euler(0.0f, newWorldRotationY, 0.0f) * Vector3.forward;

            // Calculate the move speed.
            float MoveSpeed = baseMoveSpeed;

            // Move the character.
            rigidBodyComponent.AddForce(worldMoveDirection * MoveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    // Dash the character in direction relative to the player.
    // If the direction is 0, dash forward.
    public void Dash(Vector3 relativeMoveDirection)
    {
        // Normalize the move direction to prevent fast diagonal movement.
        relativeMoveDirection = relativeMoveDirection.normalized;

        if (relativeMoveDirection.magnitude > 0.0f)
        {
            // Find the charater's new Y rotation.
            float newWorldRotationY = Mathf.Atan2(relativeMoveDirection.x, relativeMoveDirection.z) * Mathf.Rad2Deg + cameraComponent.transform.eulerAngles.y;

            // Rotate the character smoothly.
            rigidBodyComponent.rotation = Quaternion.Lerp(rigidBodyComponent.rotation, Quaternion.Euler(0.0f, newWorldRotationY, 0.0f), Time.fixedDeltaTime * turnRate);

            // Calculate the move direction relative to the world.
            Vector3 WorldMoveDirection = Quaternion.Euler(0.0f, newWorldRotationY, 0.0f) * Vector3.forward;

            // Calculate the dash speed.
            float DashSpeed = baseMoveSpeed * dashSpeedMultiplier;

            // Dash.
            rigidBodyComponent.AddForce(WorldMoveDirection * DashSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        // If the player does not press any move buttons, make the character dash forward.
        else
        {
            Dash(rigidBodyComponent.transform.forward);
        }
    }

    // Attract a moveable object.
    public void AttractObject()
    {
        // Ray cast at the character's forward direction.
        RaycastHit hitInfo;
        bool rayCastHit = Physics.Raycast(transform.position, transform.forward, out hitInfo, maxAttractionDistance);
        
        // Does the ray hit an object?
        if(rayCastHit)
        {
            // Get the hit game object.
            GameObject hitGameObject = hitInfo.transform.gameObject;

            // Display the hit game object.
            Debug.Log(hitGameObject);

            // Check if the game object is movable.
            if(hitGameObject.CompareTag("MovableObject"))
            {
                Rigidbody rigidBodyComponent = hitGameObject.GetComponent<Rigidbody>();
                if(rigidBodyComponent)
                {
                    Vector3 attractionDirection = -transform.forward;
                    rigidBodyComponent.AddForce(attractionDirection * attractionForce, ForceMode.Force);
                }
            }
        }
        else
        {
            Debug.Log("No object is found.");
        }
    }
}
