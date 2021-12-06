/**
 * CharacterHand.cs
 * Description: This script checks if the character is pushing an object.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHand : MonoBehaviour
{
    // Reference to the character.
    [SerializeField] Character character;
    public Character Character
    {
        get
        {
            return character;
        }
    }

    // The object being pushed by the character.
    PushableObject pushedObject;
    public PushableObject PushedGameObject
    {
        get
        {
            return pushedObject;
        }
    }

    private void Awake()
    {
        if (!character)
        {
            character = FindObjectOfType<Character>();
        }
    }

    // Start pushing an object.
    // gameObject is the object to be pushed.
    // initialPushingDirection is the initial pushing direction in world space.
    public void StartPushingObject(PushableObject pushableObject, Vector3 initialPushingDirection)
    {
        if (!pushableObject)
            return;

        // Attach the character to the pushed object.
        pushedObject = pushableObject;
        pushedObject.Pusher = character;

        // Set the pushing direction relative to the pushed object.
        pushedObject.relativePushingDirection = pushedObject.transform.InverseTransformDirection(initialPushingDirection.normalized);

        character.Animator.SetBool("isPushingObject", true);
    }

    public void StopPushingObject()
    {
        if (pushedObject)
        {
            // Detach the pushed object from the character.
            pushedObject.Pusher = null;
            pushedObject = null;

            character.Animator.SetBool("isPushingObject", false);
        }
    }
}