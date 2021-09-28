/**
 * PlayerCharacterInput.cs
 * Description: This script handles the inputs from the player.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerCharacterInput : MonoBehaviour
{
    [SerializeField] Character character;

    // Movement
    float verticalAxis, horizontalAxis;
    bool dashButtonDown;

    // Object attraction
    bool attractObjectButtonDown;

    // Start is called before the first frame update
    void Awake()
    {
        if(!character)
        {
            character = GetComponent<Character>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement.
        verticalAxis = Input.GetAxisRaw("Vertical");
        horizontalAxis = Input.GetAxisRaw("Horizontal");

        // Dash.
        if(Input.GetButtonDown("Dash"))
        {
            // Cannot dash if the character is pushing an object.
            if (character.CharacterHand.PushedGameObject)
                return;

            dashButtonDown = true;
        }

        // Attract a movable object.
        if (Input.GetButton("AttractObject"))
        {
            attractObjectButtonDown = true;
        }

        // Pick up or drop an object.
        if(Input.GetButtonDown("Interact"))
        {
            if(!character.CharacterHand.PushedGameObject)
            {
                character.PickUpObject();
            }
            else
            {
                character.DropObject();
            }
        }
    }

    private void FixedUpdate()
    {
        // Movement.
        Vector3 relativeMoveDirection = new Vector3(horizontalAxis, 0.0f, verticalAxis);
        // If the player presses the dash button, dash. Otherwise, move normally.
        if (dashButtonDown)
        {
            // character.Dash(new Vector3(horizontalAxis, 0.0f, verticalAxis));

            // Player does not presses WASD? Dash forward.
            if (relativeMoveDirection.magnitude == 0.0f)
            {
                character.Move(relativeMoveDirection, character.BaseMovementSpeed * character.DashSpeedMultiplier, character.RotationalSpeed);
            }
            else
            {
                character.Move(transform.forward, character.BaseMovementSpeed * character.DashSpeedMultiplier, character.RotationalSpeed);
            }
            dashButtonDown = false;
        }
        else
        {
            character.Move(relativeMoveDirection, character.BaseMovementSpeed, character.RotationalSpeed);
        }

        // Attract movable object
        if(attractObjectButtonDown)
        {
            character.AttractObject();
            attractObjectButtonDown = false;
        }
    }
}