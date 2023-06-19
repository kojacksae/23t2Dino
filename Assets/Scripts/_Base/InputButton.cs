using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class InputButton
{
    [SerializeField] private string inputName;

    [SerializeField] private InputActions inputType;

    // Reference to the input action
    private InputAction inputAction;

    // Delegate for the button press event
    public VoidEventNoPram onButtonPress;

    // Delegate for the button hold event
    public VoidEventNoPram onButtonHold;

    // Delegate for the button release event
    public VoidEventNoPram onButtonRelease;

    // Optional debugging toggle
    [SerializeField] bool debug;

    private Vector2 axisInputValue = Vector2.zero;

    public void SetUp(InputActionAsset inputActions)
    {

        // Get the input action from the input action asset
        inputAction = inputActions.FindAction(inputType.ToString());

        if (inputAction == null)
        {
            Debug.LogError("No action Found in InputAction Asset called: " + inputType.ToString() + " returning null to prevent errors");
            return;
        }

        // Enable the input action
        inputAction.Enable();

        if (inputAction.type == InputActionType.Value)
        {
            inputAction.performed += OnAxisInput;
            inputAction.canceled += OnAxisCanceled;
        }
        else if (inputAction.type == InputActionType.Button)
        {
            // Add callbacks for button press, button hold, and button release events c
            inputAction.performed += context => OnButtonPress();
            inputAction.started += context => OnButtonHold();
            inputAction.canceled += context => OnButtonRelease();
        }
    }

    public void TearDown()
    {
        // Disable the input action when the script is destroyed
        if (inputAction.type == InputActionType.Value)
        {
            inputAction.performed -= OnAxisInput;
            inputAction.canceled -= OnAxisCanceled;
        }
        else if (inputAction.type == InputActionType.Button)
        {
            // Add callbacks for button press, button hold, and button release events c
            inputAction.performed -= context => OnButtonPress();
            inputAction.started -= context => OnButtonHold();
            inputAction.canceled -= context => OnButtonRelease();
        }
        inputAction.Disable();
    }

    public void SetDebugging(bool enabled)
    {
        debug = enabled;
    }

    public InputActions Button
    {
        get
        {
            return inputType;
        }
    }

    public Vector2 AxisVectorValue
    {
        get
        {
            return axisInputValue;
        }
    }

    private void OnAxisInput(InputAction.CallbackContext context)
    {
        axisInputValue = context.ReadValue<Vector2>();
        axisInputValue.x = Mathf.Clamp(axisInputValue.x, -1, 1);
        axisInputValue.y = Mathf.Clamp(axisInputValue.y, -1, 1);
    }

    private void OnAxisCanceled(InputAction.CallbackContext context)
    {
        axisInputValue = Vector2.zero;
    }

    private void OnButtonPress()
    {
        // Handle the button press event here
        if (debug) Debug.Log("Button pressed!");
        onButtonPress?.Invoke();
    }

    private void OnButtonHold()
    {
        // Handle the button hold event here
        if (debug) Debug.Log("Button held!");
        onButtonHold?.Invoke();
    }

    private void OnButtonRelease()
    {
        // Handle the button release event here
        if (debug) Debug.Log("Button released!");
        onButtonRelease?.Invoke();
    }
}
