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

    // Combat
    bool attackButtonDown; // Used for Xbox One controller. Triggers are axes, not buttons.

    // Start is called before the first frame update
    void Awake()
    {
        if (!character)
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
        if (Input.GetButtonDown("Dash"))
        {
            // Cannot dash if the character is pushing an object.
            if (!character.CharacterHand.PushedGameObject)
            {
                dashButtonDown = true;
            }
        }

        // Attract a movable object.
        if (Input.GetButton("AttractObject") && !character.CharacterHand.PushedGameObject)
        {
            attractObjectButtonDown = true;
        }

        // Start or stop pushing an object.
        if (Input.GetButtonDown("Interact"))
        {
            if (!character.CharacterHand.PushedGameObject)
            {
                character.StartPushingObject();
            }
            else
            {
                character.StopPushingObject();
            }
        }

        // Drop an item.
        if(Input.GetButtonDown("DropItem"))
        {
            character.Inventory.DropItemInPackack(0, -1);
        }

        // Attack (keyboard)
        if(Input.GetButtonDown("Attack"))
        {
            character.Attack();
        }

        // Attack (Xbox One controller)
        if(Input.GetAxisRaw("AttackController") == 1.0f)
        {
            if (!attackButtonDown)
            {
                character.Attack();
                attackButtonDown = true;
                Debug.Log("Attack!");
            }
        }
        else
        {
            attackButtonDown = false;
        }
    }

    private void FixedUpdate()
    {
        // Movement.
        Vector3 relativeMoveDirection = new Vector3(horizontalAxis, 0.0f, verticalAxis);
        // If the player presses the dash button, dash. Otherwise, move normally.
        if (dashButtonDown)
        {
            // Player does not presses WASD? Dash forward.
            if (relativeMoveDirection.magnitude != 0.0f)
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
            // If the character is not pushing an object, move the character relative to the camera.
            // Otherwise, move the character and the object relative to the object.
            if (!character.CharacterHand.PushedGameObject)
            {
                character.Move(relativeMoveDirection, character.BaseMovementSpeed, character.RotationalSpeed);
            }
            else
            {
                character.CharacterHand.PushedGameObject.GetComponent<PushableObject>().Move(verticalAxis, horizontalAxis);
            }
        }

        // Attract movable object
        if (attractObjectButtonDown)
        {
            character.AttractObject();
            attractObjectButtonDown = false;
        }
    }
}