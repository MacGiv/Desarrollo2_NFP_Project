using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Capture Player's imputs and expose them to other scripts
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime = 0.15f;

    public Vector2 MovementInput { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool JumpPressed => jumpBufferTimer > 0f;

    private float jumpBufferTimer;

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();

        moveAction.action.performed += ctx => MovementInput = ctx.ReadValue<Vector2>();
        moveAction.action.canceled += ctx => MovementInput = Vector2.zero;

        jumpAction.action.started += ctx =>
        {
            jumpBufferTimer = jumpBufferTime;
            JumpHeld = true;
        };

        jumpAction.action.canceled += ctx => JumpHeld = false;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Update()
    {
        // Timer of jump buffer
        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Consume Jump
    /// </summary>
    public void UseJump()
    {
        jumpBufferTimer = 0f;
    }
}
