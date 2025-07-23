using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 4f;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private SpriteRenderer _sprite;
    private Animator _animator;
    private bool flipped;

    void Awake()
    {
        // Компоненты получаем сразу, безопасно для переиспользования
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();

        // Инициализируем input actions
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
        Vector3 move = new Vector3(moveInput.x, moveInput.y, 0f);

        if (move.x != 0)
        {
            flipped = move.x < 0;
            _sprite.flipX = flipped;
        }

        transform.position += move * (moveSpeed * Time.deltaTime);
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }
}