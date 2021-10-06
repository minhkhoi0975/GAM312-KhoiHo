/**
 * Character.cs
 * Description: This script contains the behaviors of a character.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Inventory))]
public class Character : MonoBehaviour
{
    // Components
    [SerializeField] Rigidbody rigidBodyComponent;
    [SerializeField] Camera cameraComponent;          // Camera to look at player. Make sure that the camera points down (rotationX = -90).

    [SerializeField] Health health;                   // Reference to character's health component.
    public Health Health
    {
        get
        {
            return health;
        }
    }

    [SerializeField] Inventory inventory;            // Reference to character's inventory.
    public Inventory Inventory
    {
        get
        {
            return inventory;
        }
    }

    [SerializeField] CharacterFoot characterFoot;     // Referfence to character's foot.
    public CharacterFoot CharacterFoot
    {
        get
        {
            return characterFoot;
        }
    }

    [SerializeField] CharacterHand characterHand;     // Reference to character's hand.
    public CharacterHand CharacterHand
    {
        get
        {
            return characterHand;
        }
    }

    // Movement
    [SerializeField] float baseMovementSpeed = 60.0f; // How fast the character moves without dashing or pushing an object.
    public float BaseMovementSpeed
    {
        get
        {
            return baseMovementSpeed;
        }
        set
        {
            baseMovementSpeed = value < 0 ? 0 : value;
        }
    }

    [SerializeField] float rotationalSpeed = 10.0f;   // How fast the character turns.
    public float RotationalSpeed
    {
        get
        {
            return rotationalSpeed;
        }
    }

    [SerializeField] float dashSpeedMultiplier = 15.0f;  // dashSpeed = baseMoveSpeed * dashSpeedMultiplier.
    public float DashSpeedMultiplier
    {
        get
        {
            return dashSpeedMultiplier;
        }
    }

    // Object attraction
    [Tooltip("How far away can a MoveableObject be attracted by this character?")]
    [SerializeField] float maxAttractionDistance = 10.0f;
    [SerializeField] float attractiveForce = 60.0f;

    // Start is called before the first frame update
    void Awake()
    {
        if (!rigidBodyComponent)
        {
            rigidBodyComponent = GetComponent<Rigidbody>();
        }

        if (!cameraComponent)
        {
            cameraComponent = FindObjectOfType<Camera>();
        }

        if(!health)
        {
            health = GetComponent<Health>();
        }

        if(!inventory)
        {
            inventory = GetComponent<Inventory>();
        }

        if (!characterFoot)
        {
            characterFoot = GetComponentInChildren<CharacterFoot>();
        }

        if (!characterHand)
        {
            characterHand = GetComponentInChildren<CharacterHand>();
        }
    }

    private void Update()
    {
        if (characterFoot.IsGrounded)
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
            if (!characterHand.PushedGameObject)
            {
                rigidBodyComponent.rotation = Quaternion.Lerp(rigidBodyComponent.rotation, Quaternion.Euler(0.0f, newWorldRotationY, 0.0f), Time.fixedDeltaTime * rotationalSpeed);
            }

            // Calculate the world movement direction.
            Vector3 worldMoveDirection = Quaternion.Euler(0.0f, newWorldRotationY, 0.0f) * Vector3.forward;

            // If the character is on a slope, project worldMoveDirection on slope surface.
            if (characterFoot.IsOnSlope)
            {
                worldMoveDirection = Vector3.ProjectOnPlane(worldMoveDirection, characterFoot.GroundInfo.normal).normalized;
            }

            // Move the character.
            rigidBodyComponent.AddForce(worldMoveDirection * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    // Attract a moveable object.
    public void AttractObject()
    {
        // Ray cast at the character's forward direction.
        // The start position of the ray is at the middle of the character's mesh.
        RaycastHit hitInfo;
        bool rayCastHit = Physics.Raycast(transform.position + new Vector3(0.0f, 1.2f, 0.0f), transform.forward, out hitInfo, maxAttractionDistance);

        // Does the ray hit an object?
        if (rayCastHit)
        {
            // Get the hit game object.
            GameObject hitGameObject = hitInfo.transform.gameObject;

            // Display the hit game object.
            Debug.Log(hitGameObject);

            // Check if the game object is movable.
            if (hitGameObject.CompareTag("MovableObject"))
            {
                Rigidbody rigidBodyComponent = hitGameObject.GetComponent<Rigidbody>();
                if (rigidBodyComponent)
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

    // Start pushing a movable object.
    public void StartPushingObject()
    {
        // Ray cast to check if there is an object in front of the character.
        RaycastHit hitInfo;
        bool rayCastHit = Physics.Raycast(transform.position + new Vector3(0.0f, 1.2f, 0.0f), transform.forward, out hitInfo, 0.8f);

        // A movable object is in front of the character. Try pushing it.
        if (rayCastHit)
        {
            characterHand.StartPushingObject(hitInfo.collider.gameObject, -hitInfo.normal);
        }
        else
        {
            Debug.Log("Object not found.");
        }
    }

    // Stop pushing a movable object.
    public void StopPushingObject()
    {
        characterHand.StopPushingObject();
    }

    // Attack
    public void Attack()
    {

    }
}