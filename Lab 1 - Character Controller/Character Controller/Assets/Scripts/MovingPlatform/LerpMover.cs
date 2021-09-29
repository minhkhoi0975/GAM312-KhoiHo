/**
 * MovingPlatform.cs
 * Description: This script makes a platform move back and forth between 2 points.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMover : MonoBehaviour
{
    // The moving platform.
    [SerializeField] GameObject movingPlatform;

    // Start point and end point.
    [SerializeField] Transform point1, point2;

    // How long does it take to move from point 1 to point 2 or vice versa?
    [SerializeField] float travelingTimeInSeconds = 3.0f;

    // The remaining time before the game object gets to destination.
    float travelingRemainingTime;

    // How long does this game object waits before it starts moving in reverse direction?
    [SerializeField] float waitTime = 3.0f;

    // True if the elevator is not waiting.
    bool canMove = true;

    // t = 0: The game object is at point 1. 
    // t = 1: The game object is at point 2.
    private float t = 0.0f;

    // direction = 1: The game object moves from position 1 to position 2. 
    // direction = -1: The game objects moves from position 2 to position 1.
    private float direction = 1.0f;

    private void Awake()
    {
        // If the game object is not at point 1, move it to point 1.
        transform.position = point1.position;

        travelingRemainingTime = travelingTimeInSeconds;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the other object is a part of the character, attach the character to the moving platform.
        if (other.gameObject.transform.parent.CompareTag("Character"))
        {
            other.gameObject.transform.parent.parent = movingPlatform.transform;
        }

        else if(other.CompareTag("MovableObject") && other.gameObject.transform.root == null)
        {
            other.gameObject.transform.parent = movingPlatform.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.transform.parent.CompareTag("Character"))
        {
            other.gameObject.transform.parent.parent = null;
        }

        else if (other.CompareTag("MovableObject"))
        {
            other.gameObject.transform.parent = null;
        }
    }

    private void Update()
    {
        if(canMove)
        {
            StartCoroutine("Move");
        }

        Debug.Log(t);
    }

    IEnumerator Move()
    {
        // Find the new position based on the remaining time.
        t = (travelingTimeInSeconds - travelingRemainingTime) / travelingTimeInSeconds;
        if (direction > 0.0f)
        {
            movingPlatform.transform.position = Vector3.Lerp(point1.position, point2.position, t);
        }
        else
        {
            movingPlatform.transform.position = Vector3.Lerp(point2.position, point1.position, t);
        }

        // Reduce the traveling remaining time.
        travelingRemainingTime -= Time.deltaTime;

        if (travelingRemainingTime < 0.0f)
        {
            // Reverse the movement direction.
            direction = -direction;

            // Reset the traveling remaing time.
            travelingRemainingTime = travelingTimeInSeconds;

            // Wait before the game object starts moving again.
            canMove = false;
            yield return new WaitForSeconds(waitTime);
            canMove = true;
        }
    }
}
