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
    [SerializeField] private GameObject character;

    private Vector3 cameraOffset;

    private void Awake()
    {
        if(!character)
        {
            character = GameObject.FindGameObjectWithTag("Character");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = this.transform.position - character.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MoveCamera(); 
    }

    void MoveCamera()
    {
        // Keep the same distance from the camera to the character.
        this.transform.position = character.transform.position + cameraOffset;
    }
}
