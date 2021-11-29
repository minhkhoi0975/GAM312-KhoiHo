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
    [Header("Movement")]
    [SerializeField] Camera cameraComponent;          // Camera to look at player. Make sure that the camera points down (rotationX is close to -90).

    [SerializeField] Rigidbody rigidBodyComponent;    // Reference to character's rigid body.
    public Rigidbody RigidBodyComponent
    {
        get
        {
            return rigidBodyComponent;
        }
    }

    [SerializeField] CapsuleCollider capsuleColliderComponent; // Reference to character's capsule collider.
    public CapsuleCollider CapsuleColliderComponent
    {
        get
        {
            return capsuleColliderComponent;
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

    [SerializeField] CharacterHand characterHand;     // Reference to character's "invisible" hand that pushes an object.
    public CharacterHand CharacterHand
    {
        get
        {
            return characterHand;
        }
    }

    [Header("Animation")]
    [SerializeField] Animator animatorController;
    public Animator AnimatorController
    {
        get
        {
            return animatorController;
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

        if(!capsuleColliderComponent)
        {
            capsuleColliderComponent = GetComponentInChildren<CapsuleCollider>();
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

    private void FixedUpdate()
    {
        UpdateFallingAnimation();
    }

    void UpdateFallingAnimation()
    {
        if (characterFoot.IsGrounded)
        {
            animatorController.SetBool("isFalling", false);
        }
        else if (characterFoot.FallingTime > 0.1f && rigidBodyComponent.velocity.y < -0.1f)
        {
            animatorController.SetBool("isFalling", true);
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
            rigidBodyComponent.rotation = Quaternion.Slerp(rigidBodyComponent.rotation, Quaternion.Euler(0.0f, newWorldRotationY, 0.0f), Time.fixedDeltaTime * 20.0f);

            // Calculate the world movement direction.
            Vector3 worldMovementDirection = Quaternion.Euler(0.0f, newWorldRotationY, 0.0f) * Vector3.forward;

            // If the character is on a slope, project worldMoveDirection on slope surface.
            if (characterFoot.IsOnSlope)
            {
                worldMovementDirection = Vector3.ProjectOnPlane(worldMovementDirection, characterFoot.GroundInfo.normal).normalized;
            }

            // Move the character.
            rigidBodyComponent.AddForce(worldMovementDirection * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    // Perform telekinesis.
    public void PerformTelekinesis(float maxAttractionDistance, float attractiveForce)
    {
        // Ray cast at the character's forward direction.
        // The start position of the ray is at the middle of the character's mesh.
        RaycastHit hitInfo;
        bool rayCastHit = Physics.Raycast(characterHand.transform.position, transform.forward, out hitInfo, maxAttractionDistance);
        Debug.DrawLine(characterHand.transform.position, characterHand.transform.position + transform.forward * maxAttractionDistance, Color.white);

        // Does the ray hit an object?
        if (rayCastHit && hitInfo.rigidbody)
        {
            // Check if the game object is movable.
            if (hitInfo.rigidbody.gameObject.CompareTag("MovableObject"))
            {
                Vector3 attractionDirection = -transform.forward;
                attractionDirection = new Vector3(attractionDirection.x, 0.0f, attractionDirection.z).normalized;
                hitInfo.rigidbody.AddForce(attractionDirection * attractiveForce, ForceMode.Force);

                // Display the hit game object.
                Debug.Log("Attracting " + hitInfo.rigidbody.name + " with force = " + attractiveForce);
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
        bool rayCastHit = Physics.Raycast(transform.TransformPoint(capsuleColliderComponent.center), transform.forward, out hitInfo, capsuleColliderComponent.radius + 1.0f);
        Debug.DrawLine(transform.TransformPoint(capsuleColliderComponent.center), transform.TransformPoint(capsuleColliderComponent.center) + transform.forward * (capsuleColliderComponent.radius + 1.0f), Color.green, 1.0f);

        // A movable object is in front of the character. Try pushing it.
        if (rayCastHit)
        {
            // Reposition the character.
            rigidBodyComponent.position = hitInfo.point + hitInfo.normal * (capsuleColliderComponent.radius + 1.41f);

            characterHand.StartPushingObject(hitInfo.collider.gameObject.GetComponent<PushableObject>(), -hitInfo.normal);
            animatorController.SetBool("isPushingObject", true);
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
        animatorController.SetBool("isPushingObject", false);
    }

    // Attack
    public void Attack(float attackRange, float damage, float criticalDamageMultiplier = 0.0f, float criticalChance = 0.0f)
    {
        // animatorController.SetTrigger("isAttacking");

        // Ray cast forward to "melee attack" the enemy.
        RaycastHit hitInfo;
        bool rayCastHit = Physics.Raycast(transform.TransformPoint(capsuleColliderComponent.center), transform.forward, out hitInfo, attackRange);
        Debug.DrawLine(transform.TransformPoint(capsuleColliderComponent.center), transform.TransformPoint(capsuleColliderComponent.center) + transform.forward * attackRange, Color.white);

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