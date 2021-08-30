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

    [SerializeField] private float MoveSpeed = 3.0f; // How fast the character moves.
    [SerializeField] private float TurnRate = 4.0f;           // How fast the character turns.


    private bool bIsGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        bIsGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        bIsGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Get input.
        float Vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");
        
        // Calculate the move direction relative to the character.
        Vector3 LocalMoveDirection = new Vector3(Horizontal, 0.0f, Vertical).normalized;

        if (LocalMoveDirection.magnitude >= 0.1f) // Prevent the character from turning when the player does not press any keys.
        {
            // Rotate the character.
            float RotationAngleInDegrees = Mathf.Atan2(LocalMoveDirection.x, LocalMoveDirection.z) * Mathf.Rad2Deg + Camera.transform.eulerAngles.y;
            RigidBodyComponent.rotation = Quaternion.Lerp(RigidBodyComponent.rotation, Quaternion.Euler(0.0f, RotationAngleInDegrees, 0.0f), Time.fixedDeltaTime * TurnRate);

            // Move the character.
            Vector3 WorldMoveDirection = Quaternion.Euler(0.0f, RotationAngleInDegrees, 0.0f) * Vector3.forward;
            RigidBodyComponent.AddForce(WorldMoveDirection * MoveSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
}
