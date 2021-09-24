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
    [SerializeField] Character characterComponent;

    // Movement
    float verticalAxis, horizontalAxis;
    bool dashButtonDown;

    // Object attraction
    bool attractObjectButtonDown;

    // Start is called before the first frame update
    void Awake()
    {
        if(!characterComponent)
        {
            characterComponent = GetComponent<Character>();
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
            dashButtonDown = true;
        }

        // Attract a movable object.
        if (Input.GetButton("AttractObject"))
        {
            attractObjectButtonDown = true;
        }
    }

    private void FixedUpdate()
    {
        // Movement.

        // If the player presses the dash button, dash. Otherwise, move normally.
        if(dashButtonDown)
        {
            characterComponent.Dash(new Vector3(horizontalAxis, 0.0f, verticalAxis));
            dashButtonDown = false;
        }
        else
        {
            characterComponent.Move(new Vector3(horizontalAxis, 0.0f, verticalAxis));
        }

        // Attract movable object
        if(attractObjectButtonDown)
        {
            characterComponent.AttractObject();
            attractObjectButtonDown = false;
        }
    }
}
