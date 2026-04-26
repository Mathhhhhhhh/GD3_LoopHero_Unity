using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Contrôle le joueur pour le cache-cache.
/// Phase Hiding : déplacement libre + rotation caméra souris.
/// Phase Seeking : déplacement bloqué, rotation caméra souris uniquement.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class HideAndSeekPlayerController : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Caméra TPS")]
    [SerializeField] private float _mouseSensitivity = 2f;

    private CharacterController _controller;
    private InputSystem_Actions _inputActions;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private bool _movementEnabled = true;

    private const float Gravity = -9.81f;
    private float _verticalVelocity = 0f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
        _inputActions.Player.Look.performed += OnLook;
        _inputActions.Player.Look.canceled += OnLook;
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMove;
        _inputActions.Player.Look.performed -= OnLook;
        _inputActions.Player.Look.canceled -= OnLook;
        _inputActions.Player.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>Callback d'input pour le déplacement.</summary>
    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>Callback d'input pour la visée souris.</summary>
    private void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        HandleLook();

        if (_movementEnabled)
            HandleMovement();

        ApplyGravity();
    }

    private void HandleLook()
    {
        float horizontalRotation = _lookInput.x * _mouseSensitivity;
        transform.Rotate(Vector3.up * horizontalRotation);
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        _controller.Move(move * _moveSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        _verticalVelocity += Gravity * Time.deltaTime;
        _controller.Move(Vector3.up * _verticalVelocity * Time.deltaTime);
    }

    /// <summary>Active ou désactive le déplacement du joueur (la caméra reste toujours active).</summary>
    public void SetMovementEnabled(bool enabled)
    {
        _movementEnabled = enabled;
    }
}
