using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    // singleton
    public static PlayerInput Instance { get; private set; }

    public event EventHandler OnJumpAction;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump_performed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Disable();
        playerInputActions.Player.Jump.performed -= Jump_performed;
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }
}
