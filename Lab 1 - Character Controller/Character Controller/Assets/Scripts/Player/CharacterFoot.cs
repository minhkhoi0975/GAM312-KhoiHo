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
    public float maxSlopeAngle = 60.0f; // The max angle of the slopes on which the character can stand.

    bool isGrounded = false;        // Is the character on the ground?
    bool isOnSlope = false;
    RaycastHit groundInfo;          // Information about the ground the character stands on.
    float slopeAngle;

    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
    }

    public bool IsOnSlope
    {
        get
        {
            return isOnSlope;
        }
    }

    public RaycastHit GroundInfo
    {
        get
        {
            return groundInfo;
        }
    }

    public float SlopeAngle
    {
        get
        {
            return slopeAngle;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, out groundInfo, 0.2f);

        if(isGrounded)
        {
            slopeAngle = Vector3.Angle(groundInfo.normal, Vector3.up);
            if(slopeAngle >= 1.0f && slopeAngle <= maxSlopeAngle)
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
