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
    bool attackButtonDown; // Used for Xbox One controller.

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
        // Don't receive input if the game is paused.
        if (Time.timeScale == 0.0f)
            return;

        // Don't receive input if the character is dead.
        if (character.Health.CurrentHealth <= 0.0f)
            return;

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

        // Perform telekinesis (attracting a movable object).
        if (Input.GetButton("Telekinesis") && !character.CharacterHand.PushedGameObject)
        {
            attractObjectButtonDown = true;
        }
        else
        {
            character.Animator.SetBool("isPerformingTelekinesis", false);
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

        // Drop an equipped item in this priorty order: weapon, armorHead, armorArms, armorLegs, armorChest, first item in backpack.
        if (Input.GetButtonDown("DropItem"))
        {
            if (character.Inventory.weapon)
            {
                character.Inventory.DropWeapon();
            }
            else if (character.Inventory.armorHead)
            {
                character.Inventory.DropArmorHead();
            }
            else if (character.Inventory.armorArms)
            {
                character.Inventory.DropArmorArms();
            }
            else if (character.Inventory.armorLegs)
            {
                character.Inventory.DropArmorLegs();
            }
            else if (character.Inventory.armorBody)
            {
                character.Inventory.DropArmorChest();
            }
            else
            {
                character.Inventory.DropItemInBackpack(0, -1);
            }
        }

        // Attack (keyboard)
        if (Input.GetButtonDown("Attack"))
        {
            InputAttack();
        }

        // Attack (Xbox One controller)
        if (Input.GetAxis("AttackController") == 1.0f)
        {
            if (!attackButtonDown)
            {
                InputAttack();
                attackButtonDown = true;
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
        float movementSpeed = character.StatSystem.GetCurrentValue(StatType.MovementSpeed);
        float dashSpeedMultiplier = character.StatSystem.GetCurrentValue(StatType.DashSpeedMultiplier);

        // If the player presses the dash button, dash. Otherwise, move normally.
        if (dashButtonDown)
        {
            // Only dash if dash speed multiplier is greater than 1.0.
            if (dashSpeedMultiplier > 1.0f)
            {
                // Player does not presses WASD? Dash forward.
                if (relativeMoveDirection.magnitude != 0.0f)
                {
                    character.Move(relativeMoveDirection, movementSpeed * dashSpeedMultiplier);
                }
                else
                {
                    character.Move(transform.forward, movementSpeed * dashSpeedMultiplier);
                }

                // Play dash animation.
                character.Animator.SetTrigger("isDashing");
            }

            dashButtonDown = false;
        }
        else
        {
            // If the character is not pushing an object, move the character relative to the camera.
            // Otherwise, move the character and the object relative to the object.
            if (!character.CharacterHand.PushedGameObject)
            {
                character.Move(relativeMoveDirection, movementSpeed);
            }
            else
            {
                character.CharacterHand.PushedGameObject.GetComponent<PushableObject>().Move(verticalAxis, horizontalAxis);
            }
        }

        if (relativeMoveDirection.magnitude > 0.0f)
        {
            character.Animator.SetFloat("movementSpeed", character.StatSystem.GetCurrentValue(StatType.MovementSpeed));
        }
        else
        {
            character.Animator.SetFloat("movementSpeed", 0.0f);
        }

        // Perform telekinesis.
        if (attractObjectButtonDown)
        {
            float telekinesisDistance = character.StatSystem.stats[StatType.TelekinesisDistance].CurrentValue;
            float telekinesisForce = character.StatSystem.stats[StatType.TelekinesisForce].CurrentValue;
            character.PerformTelekinesis(telekinesisDistance, telekinesisForce);
            attractObjectButtonDown = false;
        }
    }

    void InputAttack()
    {
        character.Animator.SetTrigger("isAttacking");
    }
}