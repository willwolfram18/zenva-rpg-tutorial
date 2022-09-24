using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator), typeof(PlayerInput))]
public class PlayerMovementBehavior : MonoBehaviour
{
    public float speed;

    [SerializeField]
    private Animator _animator;

    private PlayerInputActions _input;
    private Rigidbody2D _rigidbody;
    private Vector2? _userInput = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _input = new PlayerInputActions();
        _rigidbody = GetComponent<Rigidbody2D>();

        _input.Player.Move.performed += CaptureUserMovement;
    }

    public void CaptureUserMovement(InputAction.CallbackContext context) => _userInput = context.ReadValue<Vector2>();

    private void FixedUpdate()
    {
        if (_userInput == null)
        {
            return;
        }

        Vector2 newVelocity = new(0, 0);

        newVelocity.x = _userInput.Value.x switch
        {
            < 0 => -speed,
            > 0 => speed,
            _ => newVelocity.x
        };
        newVelocity.y = _userInput.Value.y switch
        {
            < 0 => -speed,
            > 0 => speed,
            _ => newVelocity.y
        };
        
        _animator.SetInteger("Horizontal", (int) (newVelocity.x / speed));
        _animator.SetInteger("Vertical", (int)(newVelocity.y / speed));

        _rigidbody.velocity = newVelocity;
    }
}