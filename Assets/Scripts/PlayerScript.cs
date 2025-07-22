using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 1f;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private SpriteRenderer _sprite;
    private Animator _animator;
    private bool flipped;

    void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        inputActions = new PlayerInputActions();
        // todo эту штуку нужно грохать
        inputActions.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, moveInput.y, transform.position.z);
        if (move.x != 0)
        {
            flipped = move.x < 0;
        }

        _sprite.flipX = flipped;
        transform.position += move * (moveSpeed * Time.deltaTime);
    }
}