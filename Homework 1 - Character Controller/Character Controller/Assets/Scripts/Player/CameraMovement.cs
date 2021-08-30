/**
 * CameraMovement.cs
 * Description: This script handles the movement of the player's camera.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject CharacterMesh;

    private Vector3 DistanceVector;

    // Start is called before the first frame update
    void Start()
    {
        // Hide and lock mouse cursor.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        DistanceVector = this.transform.position - CharacterMesh.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MoveCamera(); 
    }

    void MoveCamera()
    {
        // Keep the same distance from the camera to the character.
        this.transform.position = CharacterMesh.transform.position + DistanceVector;
    }
}
