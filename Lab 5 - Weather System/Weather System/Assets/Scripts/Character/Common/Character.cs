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
[RequireComponent(typeof(StatSystem))]
public class Character : MonoBehaviour
{
    // Camera
    [Header("Camera")]
    [SerializeField] Camera cameraComponent;          // Camera to look at player. Make sure that the camera points down (rotationX is close to -90).

    // Physics
    [Header("Physics")]
    [SerializeField] Rigidbody rigidBodyComponent;    // Reference to character's rigid body.

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

    [Header("Health")]
    [SerializeField] Health health;                   // Reference to character's health component.
    public Health Health
    {
        get
        {
            return health;
        }
    }

    [Header("Inventory")]
    [SerializeField] Inventory inventory;            // Reference to character's inventory.
    public Inventory Inventory
    {
        get
        {
            return inventory;
        }
    }

    [Header("Stats")]
    [SerializeField] StatSystem statSystem;           // Reference to the stat system component.
    public StatSystem StatSystem
    {
        get
        {
            return statSystem;
        }
    }

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

        if (!health)
        {
            health = GetComponent<Health>();
        }

        if (!inventory)
        {
            inventory = GetComponent<Inventory>();
        }

        if(!statSystem)
        {
            statSystem = GetComponent<StatSystem>();
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
    public void Move(Vector3 relativeMovementDirection, float movementSpeed)
    {
        // Normalize the move direction to prevent fast diagonal movement.
        relativeMovementDirection = relativeMovementDirection.normalized;

        if (relativeMovementDirection.magnitude > 0.0f && movementSpeed > 0.0f)
        {
            // Find the charater's new world Y rotation.
            float newWorldRotationY = Mathf.Atan2(relativeMovementDirection.x, relativeMovementDirection.z) * Mathf.Rad2Deg + cameraComponent.transform.eulerAngles.y;

            // Rotate the character smoothly.
            if (!characterHand.PushedGameObject)
            {
                rigidBodyComponent.rotation = Quaternion.Lerp(rigidBodyComponent.rotation, Quaternion.Euler(0.0f, newWorldRotationY, 0.0f), Time.fixedDeltaTime * 15.0f);
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

    // Perform telekinesis.
    public void PerformTelekinesis(float maxAttractionDistance, float attractiveForce)
    {
        // Ray cast at the character's forward direction.
        // The start position of the ray is at the middle of the character's mesh.
        RaycastHit hitInfo;
        bool rayCastHit = Physics.Raycast(characterHand.transform.position, transform.forward, out hitInfo, maxAttractionDistance);

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
        bool rayCastHit = Physics.Raycast(characterHand.transform.position, transform.forward, out hitInfo, 0.8f);

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
    public void Attack(float attackRange, float damage, float criticalDamageMultiplier = 0.0f, float criticalChance = 0.0f)
    {
        // Ray cast forward to "melee attack" the enemy.
        RaycastHit hitInfo;
        bool rayCastHit = Physics.Raycast(characterHand.transform.position, transform.forward, out hitInfo, attackRange);

        // If ray cast hit, cause damage to the hit object if it has Health component.
        if (rayCastHit)
        {
            Health health = hitInfo.collider.GetComponent<Health>();
            if (!health)
            {
                health = hitInfo.collider.GetComponentInParent<Health>();
            }

            if (health)
            {
                // Calculate the final damage.
                float finalDamage = GetFinalDamage(damage, criticalDamageMultiplier, criticalChance);

                // Hit object takes damage.
                health.TakeDamage(finalDamage);
                Debug.Log("Hit target's remaining health: " + health.CurrentHealth);

                // Push the hit object backward.
                Rigidbody hitTargetRigidBody = hitInfo.rigidbody;
                if(hitTargetRigidBody)
                {
                    hitTargetRigidBody.AddForce(transform.forward * 1200.0f);
                }
            }
        }
        else
        {
            Debug.Log("The attack did not hit anything.");
        }
    }

    float GetFinalDamage(float damage, float criticalDamageMultiplier = 0.0f, float criticalChance = 0.0f)
    {
        float percentage = Random.Range(0.0f, 100.0f);

        if (percentage <= criticalChance)
        {
            return damage * criticalDamageMultiplier;
        }
        else
        {
            return damage;
        }
    }
}