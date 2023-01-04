using UnityEngine;
using UnityEngine.InputSystem;
using System;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    private CharactersActions _inputActions;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _jumPress;
    private bool _sprintPress;

    private Action _crouch;

    public Vector2 MoveInput => _moveInput;
    public Vector2 LookInput => _lookInput;
    public bool JumpPress => _jumPress;
    public bool SprintPress => _sprintPress;
  
    public Action Crouch
    {
        set => _crouch = value;
    }

    private void Awake()
    {
        _inputActions = new CharactersActions();

        _inputActions.Player.Look.performed += OnLook;
        _inputActions.Player.Move.performed += OnMove;

        _inputActions.Player.Jump.started += OnJump;
        _inputActions.Player.Jump.canceled += OnJump;

        _inputActions.Player.Sprint.performed += OnSprint;
        _inputActions.Player.Sprint.canceled += OnSprint;

        _inputActions.Player.Crouch.started += OnCrouch;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _jumPress = context.ReadValueAsButton();
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        _sprintPress = context.ReadValueAsButton();
    }

    private void OnCrouch(InputAction.CallbackContext contex)
    {
        _crouch?.Invoke();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

}
