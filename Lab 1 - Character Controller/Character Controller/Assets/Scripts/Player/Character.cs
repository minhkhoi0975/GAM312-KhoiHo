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
    // Components
    [SerializeField] private Rigidbody rigidBodyComponent;
    [SerializeField] private Camera cameraComponent;          // Camera to look at player. Make sure that the camera points down (rotationX = -90).

    [SerializeField] private CharacterFoot characterFoot;     // Referfence to character's foot.
    public CharacterFoot CharacterFoot
    {
        get
        {
            return characterFoot;
        }
    }

    [SerializeField] private CharacterHand characterHand;     // Reference to character's hand.
    public CharacterHand CharacterHand
    {
        get
        {
            return characterHand;
        }
    }

    // Movement
    [SerializeField] private float baseMovementSpeed = 60.0f; // How fast the character moves normally (without dash).
    public float BaseMovementSpeed
    {
        get
        {
            return baseMovementSpeed;
        }
    }

    [SerializeField] private float rotationalSpeed = 10.0f;   // How fast the character turns.
    public float RotationalSpeed
    {
        get
        {
            return rotationalSpeed;
        }
    }

    [SerializeField] private float dashSpeedMultiplier = 15.0f;  // dashSpeed = baseMoveSpeed * dashSpeedMultiplier.
    public float DashSpeedMultiplier
    {
        get
        {
            return dashSpeedMultiplier;
        }
    }

    // Object attraction
    [Tooltip("How far away can a MoveableObject be attracted by this character?")]
    [SerializeField]float maxAttractionDistance = 10.0f;
    [SerializeField]float attractiveForce = 60.0f;

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

        if(!characterFoot)
        {
            characterFoot = GetComponentInChildren<CharacterFoot>();
        }

        if(!characterHand)
        {
            characterHand = GetComponentInChildren<CharacterHand>();
        }
    }

    private void Update()
    {
        if(characterFoot.IsGrounded)
        {
            rigidBodyComponent.useGravity = false;
        }
        else
        {
            rigidBodyComponent.useGravity = true;
        }
    }
    
    // Move the character in direction relative to the player.
    public void Move(Vector3 relativeMovementDirection, float movementSpeed, float rotationalSpeed)
    {
        // Normalize the move direction to prevent fast diagonal movement.
        relativeMovementDirection = relativeMovementDirection.normalized;

        if (relativeMovementDirection.magnitude > 0.0f)
        {    
            // Find the charater's new world Y rotation.
            float newWorldRotationY = Mathf.Atan2(relativeMovementDirection.x, relativeMovementDirection.z) * Mathf.Rad2Deg + cameraComponent.transform.eulerAngles.y;

            // Rotate the character smoothly.
            rigidBodyComponent.rotation = Quaternion.Lerp(rigidBodyComponent.rotation, Quaternion.Euler(0.0f, newWorldRotationY, 0.0f), Time.fixedDeltaTime * rotationalSpeed);

            // Calculate the move direction relative to the world.
            Vector3 worldMoveDirection = Quaternion.Euler(0.0f, newWorldRotationY, 0.0f) * Vector3.forward;

            // Check if there is a wall in front of the player.
            // If there is, move along the wall.
            RaycastHit wallRayCastHitInfo;
            bool wallRayCastHit = Physics.Raycast(transform.position, transform.forward, out wallRayCastHitInfo, 0.1f);

            if(wallRayCastHit)
            {
                worldMoveDirection = Vector3.ProjectOnPlane(worldMoveDirection, wallRayCastHitInfo.normal).normalized;
            }

            // Move the character.
            rigidBodyComponent.AddForce(worldMoveDirection * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
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
            rigidBodyComponent.rotation = Quaternion.Lerp(rigidBodyComponent.rotation, Quaternion.Euler(0.0f, newWorldRotationY, 0.0f), Time.fixedDeltaTime * rotationalSpeed);

            // Calculate the move direction relative to the world.
            Vector3 WorldMoveDirection = Quaternion.Euler(0.0f, newWorldRotationY, 0.0f) * Vector3.forward;

            // Calculate the dash speed.
            float DashSpeed = baseMovementSpeed * dashSpeedMultiplier;

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
        // The start position of the ray is at the middle of the character's mesh.
        RaycastHit hitInfo;
        bool rayCastHit = Physics.Raycast(transform.position + transform.up, transform.forward, out hitInfo, maxAttractionDistance);
        
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
                    Vector3 attractionDirection = hitInfo.normal;
                    rigidBodyComponent.AddForce(attractionDirection * attractiveForce, ForceMode.Force);
                }
            }
        }
        else
        {
            Debug.Log("No object is found.");
        }
    }

    public void PickUpObject()
    {
        // Ray cast to check if there is an object in front of the character.
        RaycastHit hitInfo;
        bool rayCastHit = Physics.Raycast(transform.position + transform.up, transform.forward, out hitInfo, 0.8f);

        if(rayCastHit && hitInfo.collider.CompareTag("MovableObject") && hitInfo.collider.GetComponent<Rigidbody>())
        {
            characterHand.PickUpObject(hitInfo.collider.gameObject);
        }
        else
        {
            Debug.Log("Object not found.");
        }
    }

    public void DropObject()
    {
        characterHand.DropObject();
    }
}