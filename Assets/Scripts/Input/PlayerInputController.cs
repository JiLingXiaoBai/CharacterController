using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputActionMap _playerNormalMap;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private void Start()
    {
        _playerInput = transform.GetComponent<PlayerInput>();
        _playerNormalMap = _playerInput.actions.FindActionMap("PlayerNormal", throwIfNotFound: true);
        _moveAction = _playerNormalMap.FindAction("Move", throwIfNotFound: true);
        _jumpAction = _playerNormalMap.FindAction("Jump", throwIfNotFound: true);

        SetCallBack(_moveAction, Move);
        SetCallBack(_jumpAction, Jump);
    }

    private static void SetCallBack(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started += callback;
        action.performed += callback;
        action.canceled += callback;
    }

    private static void Jump(InputAction.CallbackContext ctx)
    {
        
    }

    private static void Move(InputAction.CallbackContext ctx)
    {

    }
}
