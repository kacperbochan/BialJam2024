using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    // singleton
    public static PlayerInput Instance { get; private set; }

    public event EventHandler OnPlayer1Jump;
    public event EventHandler OnPlayer2Jump;
    public event EventHandler OnPlayer2Create;
    public event EventHandler OnPlayer2GravityFlip;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new();

        playerInputActions.Player1.Enable();
        playerInputActions.Player1.Jump.performed += Player1Jump;

        playerInputActions.Player2.Enable();
        playerInputActions.Player2.Jump.performed += Player2Jump;
        playerInputActions.Player2.Create.performed += Player2Create;
        playerInputActions.Player2.GravityFlip.performed += Player2GravityFlip;
    }

    private void OnDestroy()
    {
        playerInputActions.Player1.Disable();
        playerInputActions.Player1.Jump.performed += Player1Jump;

        playerInputActions.Player2.Disable();
        playerInputActions.Player2.Jump.performed -= Player2Jump;
        playerInputActions.Player2.Create.performed -= Player2Create;
        playerInputActions.Player2.GravityFlip.performed -= Player2GravityFlip;
    }

    private void Player1Jump(InputAction.CallbackContext obj)
    {
        OnPlayer1Jump?.Invoke(this, EventArgs.Empty);
    }
    private void Player2Jump(InputAction.CallbackContext obj)
    {
        OnPlayer2Jump?.Invoke(this, EventArgs.Empty);
    }
    private void Player2Create(InputAction.CallbackContext obj)
    {
        OnPlayer2Create?.Invoke(this, EventArgs.Empty);
    }
    private void Player2GravityFlip(InputAction.CallbackContext obj)
    {
        OnPlayer2GravityFlip?.Invoke(this, EventArgs.Empty);
    }

    public float GetPlayer1MovementValue()
    {
        float movementVector = playerInputActions.Player1.Move.ReadValue<float>();
        return movementVector;
    }

    public float GetPlayer2MovementValue()
    {
        float movementVector = playerInputActions.Player2.Move.ReadValue<float>();
        return movementVector;
    }
}
