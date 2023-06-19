using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script for handling button input from a controller using the new input system
/// </summary>
public class InputRouter : MonoBehaviour
{
    // Assign the input action asset in the inspector that we will clone.
    [SerializeField] private InputActionAsset inputActions;
    // a copy of our action map.
    private InputActionAsset inputActionCopy;
    // so here for the drop in multiplayer to work, we need to addin the ability to switch the action map to the clone we make later.
    [SerializeField] private PlayerInput playerInput;
    // Enables debugging for this script.
    [SerializeField] private bool debugInputRouter = false;
    // Enables debugging on all buttons on start up.
    [SerializeField] private bool debugAllButtons = false;

    // List of all the actions within the game
    [SerializeField] private List<InputButton> inputs = new List<InputButton>();

    private void OnEnable()
    {
        SetUpButtons();
        EnableDebuggingOnButtons(debugAllButtons);
    }

    private void OnDisable()
    {
        TearDownButtons();
    }

    public InputButton FindAndReturnButton(InputActions actionName)
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            if (actionName == inputs[i].Button)
            {
                return inputs[i];
            }
        }

        if(debugInputRouter)
        {
            Debug.LogError("Throwing error due to no action found, returning null for safety the action searched was: " + actionName);
        }

        return null;
    }

    private void EnableDebuggingOnButtons(bool enabled)
    {
        if (enabled)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i].SetDebugging(enabled);
            }
        }
    }

    private void SetUpButtons()
    {
        // here we create a copy of the scriptable objects Action Inputs so we can have independant input for each player.
        inputActionCopy = Instantiate(inputActions);
        playerInput.actions = inputActionCopy;

        for (int i = 0; i < inputs.Count; i++)
        {
            inputs[i].SetUp(inputActionCopy);
        }
    }

    private void TearDownButtons()
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            inputs[i].TearDown();
        }

        Destroy(inputActionCopy);
    } 
}

