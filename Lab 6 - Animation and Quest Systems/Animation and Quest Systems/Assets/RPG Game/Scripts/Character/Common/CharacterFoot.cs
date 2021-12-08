/**
 * CharacterFoot.cs
 * Description: This script checks if the character stands on something.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFoot : MonoBehaviour
{
    // Reference to the rigid body.
    [SerializeField] Rigidbody rigidBody;

    // The max angle of the slope on which the character can stand.
    public float maxSlopeAngle = 60.0f;

    // Is the character on the ground?
    bool isGrounded = false;
    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
    }

    // Is this character on a slope?
    bool isOnSlope = false;
    public bool IsOnSlope
    {
        get
        {
            return isOnSlope;
        }
    }

    // Information about the ground the character stands on.
    RaycastHit groundInfo;
    public RaycastHit GroundInfo
    {
        get
        {
            return groundInfo;
        }
    }

    // The slope angle of the ground the character stands on.
    float slopeAngle;
    public float SlopeAngle
    {
        get
        {
            return slopeAngle;
        }
    }

    // How long has the character been falling?
    float fallingTime;
    public float FallingTime
    {
        get
        {
            return fallingTime;
        }
    }

    // If the character is not grounded, then gravity should be applied to the character.
    public float gravity = 300.0f;

    private void Awake()
    {
        if (!rigidBody)
        {
            rigidBody = GetComponentInParent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        UpdateIsGrounded();

        if (!isGrounded)
        {
            rigidBody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }
    }

    // Update whether the character is on the ground.
    void UpdateIsGrounded()
    {
        // Perform a ray cast downward.
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out groundInfo, 0.25f);

        if (isGrounded)
        {
            fallingTime = 0.0f;

            // Calculate the slope angle and determine whether the character is on a slope or not.
            slopeAngle = Vector3.Angle(groundInfo.normal, Vector3.up);
            if (slopeAngle >= 1.0f && slopeAngle <= maxSlopeAngle)
            {
                isOnSlope = true;
            }
            else
            {
                slopeAngle = 0.0f;
                isOnSlope = false;
            }
        }
        else
        {
            fallingTime += Time.deltaTime;

            slopeAngle = 0.0f;
            isOnSlope = false;
        }
    }
}