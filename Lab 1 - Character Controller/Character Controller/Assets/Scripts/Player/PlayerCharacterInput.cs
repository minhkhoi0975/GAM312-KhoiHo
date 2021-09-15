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

    float verticalAxis, horizontalAxis;
    bool dashButtonDown;

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
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");

        // Dash.
        if(Input.GetButtonDown("Dash"))
        {
            dashButtonDown = true;
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
    }
}
