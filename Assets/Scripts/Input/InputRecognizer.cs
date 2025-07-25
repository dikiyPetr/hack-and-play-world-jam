using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class InputRecognizer : MonoBehaviour
{
    public GameManager gameManager;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        if (inputActions == null)
            inputActions = new PlayerInputActions();

        inputActions.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Disable();
    }

    void Update()
    {
        if (moveInput != Vector2.zero)
        {
            gameManager.tilemapPresenter.Move(moveInput).Forget();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    public void EnableKeyboardListener()
    {
        inputActions.Enable();
    }
    public void DisableKeyboardListener()
    {
        inputActions.Disable();
    }
}