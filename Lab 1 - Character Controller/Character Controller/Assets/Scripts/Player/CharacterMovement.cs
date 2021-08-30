/**
 * CharacterMovement.cs
 * Description: This script handles the movement of the player's mesh.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody RigidBodyComponent;
    [SerializeField] private Camera Camera;

    [SerializeField] private float BaseMoveSpeed = 30.0f; // How fast the character moves normally (without dash).
    [SerializeField] private float TurnRate = 10.0f;      // How fast the character turns.

    [SerializeField] private float DashSpeedMultiplier = 2.0f;  // DashSpeed = MoveSpeed * DashSpeedMultiplier.

    private bool bDashButtonPressed;  // Is the dash button pressed?

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDashButton();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    // Update whether the dash button is pressed or not.
    void UpdateDashButton()
    {
        if (Input.GetButtonDown("Jump"))
        {
            bDashButtonPressed = true;
        }
    }

    void HandleMovement()
    {
        // Get input.
        float Vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");
        
        // Calculate the move direction relative to the character.
        Vector3 LocalMoveDirection = new Vector3(Horizontal, 0.0f, Vertical).normalized;

        if (LocalMoveDirection.magnitude > 0.0f)
        {
            // Find the charater's new Y rotation.
            float NewRotationY = Mathf.Atan2(LocalMoveDirection.x, LocalMoveDirection.z) * Mathf.Rad2Deg + Camera.transform.eulerAngles.y;

            // Rotate the character smoothly.
            RigidBodyComponent.rotation = Quaternion.Lerp(RigidBodyComponent.rotation, Quaternion.Euler(0.0f, NewRotationY, 0.0f), Time.fixedDeltaTime * TurnRate);

            // Calculate the move direction relative to the world.
            Vector3 WorldMoveDirection = Quaternion.Euler(0.0f, NewRotationY, 0.0f) * Vector3.forward;

            // Calculate the move speed.
            float MoveSpeed = BaseMoveSpeed;

            // Use dash speed if the player presses the Dash key.
            if(bDashButtonPressed)
            {
                MoveSpeed *= DashSpeedMultiplier;
                bDashButtonPressed = false;
            }

            // Move the character.
            RigidBodyComponent.AddForce(WorldMoveDirection * MoveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        else if(bDashButtonPressed) // If the player does not press WASD but does press the Dash key, dash forward.
        {
            // Get the forward direction relative to the world.
            Vector3 WorldMoveDirection = RigidBodyComponent.rotation * Vector3.forward;

            // Dash forward.
            RigidBodyComponent.AddForce(WorldMoveDirection * BaseMoveSpeed * DashSpeedMultiplier * Time.fixedDeltaTime, ForceMode.VelocityChange);

            bDashButtonPressed = false;
        }
    }
}
