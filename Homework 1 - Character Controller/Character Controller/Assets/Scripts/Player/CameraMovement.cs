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
    [SerializeField] private float SensitivityX = 100.0f;
    [SerializeField] private float SensitivityY = 100.0f;

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
        RotateCamera();
        MoveCamera();   
    }

    void MoveCamera()
    {
        // Keep the same distance from the camera to the character.
        this.transform.position = CharacterMesh.transform.position + DistanceVector;

        // Make sure that the camera looks at the character mesh.
        transform.LookAt(CharacterMesh.transform);
    }

    void RotateCamera()
    {
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");

        Quaternion CameraRotation = Quaternion.AngleAxis(MouseX * SensitivityX * Time.deltaTime, CharacterMesh.transform.up);
        DistanceVector = CameraRotation * DistanceVector;
    }
}
