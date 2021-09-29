/**
 * CharacterFoot.cs
 * Description: This script check if the character stands on something.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFoot : MonoBehaviour
{
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

    private void FixedUpdate()
    {
        UpdateIsGrounded();
    }

    // Update whether the character is on the ground.
    void UpdateIsGrounded()
    {
        // Perform a ray cast downward.
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, out groundInfo, 0.2f);

        if (isGrounded)
        {
            // Calculate the slope angle and determine whether the character is on a slope or not.
            slopeAngle = Vector3.Angle(groundInfo.normal, Vector3.up);
            if (slopeAngle >= 1.0f && slopeAngle <= maxSlopeAngle)
            {
                isOnSlope = true;
            }
            else
            {
                isOnSlope = false;
            }

            //Debug.Log("The character is on ground: " + groundInfo.transform.gameObject + ". Slope angle: " + slopeAngle);
        }
        else
        {
            slopeAngle = 0.0f;
            isOnSlope = false;

            //Debug.Log("This character is in air.");
        }
    }
}