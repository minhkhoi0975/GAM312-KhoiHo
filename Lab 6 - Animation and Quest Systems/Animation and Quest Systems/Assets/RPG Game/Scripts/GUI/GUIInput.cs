/**
 * GUIInput.cs
 * Description: This script handles the inputs for the UI from the player.
 * Programmer: Khoi Ho
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class InputPanelPair
{
    public string input;
    public bool isJoystickAxis;  // true = input is a joystick axis, false = input is a button
    public GameObject panel;
}

public class GUIInput : MonoBehaviour
{
    // List of all inputs and respective panels.
    public List<InputPanelPair> inputPanelPairs;

    // The panel the player has recently opened.
    GameObject lastOpenedPanel;
    public GameObject ActivePanel
    {
        get
        {
            return lastOpenedPanel;
        }
    }

    // Is the joystick button down (used for some Xbox One buttons)?
    bool joystickButtonDown = false;

    // Start is called before the first frame update
    void Start()
    {
        // Remove all invalid input-panel pairs.
        inputPanelPairs.RemoveAll(inputPanelPair => inputPanelPair.input.Trim() == "" || !inputPanelPair.panel);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (InputPanelPair inputPanelPair in inputPanelPairs)
        {
            // Input from joystick.
            if (inputPanelPair.isJoystickAxis)
            {
                if (Input.GetAxis(inputPanelPair.input) == 1.0f)
                {
                    if (!joystickButtonDown)
                    {
                        ProcessInput(inputPanelPair);
                        joystickButtonDown = true;
                        break;
                    }
                }
                else
                {
                    joystickButtonDown = false;
                }
            }

            // Input from button.
            else
            {
                if (Input.GetButtonDown(inputPanelPair.input))
                {
                    ProcessInput(inputPanelPair);
                    break;
                }
            }
        }
    }

    void ProcessInput(InputPanelPair inputPanelPair)
    {
        if (inputPanelPair.panel != lastOpenedPanel)
        {
            // Close the previous active panel.
            if (lastOpenedPanel)
            {
                lastOpenedPanel.SetActive(false);
            }

            // Set and open the new active panel.
            lastOpenedPanel = inputPanelPair.panel;
            lastOpenedPanel.SetActive(true);
        }
        else
        {
            lastOpenedPanel.SetActive(!lastOpenedPanel.activeInHierarchy);
        }

        // If lastOpenedPanel is active, pause the game.
        if (lastOpenedPanel && lastOpenedPanel.activeInHierarchy)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
