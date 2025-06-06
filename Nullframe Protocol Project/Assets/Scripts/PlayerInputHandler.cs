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
    [SerializeField] private InputActionReference attackAction;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime = 0.15f;

    public Vector2 MovementInput { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool JumpPressed => jumpBufferTimer > 0f;
    public bool AttackPressed { get; private set; }

    private float jumpBufferTimer;

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        attackAction.action.Enable();

        // Movement
        moveAction.action.performed += ctx => MovementInput = ctx.ReadValue<Vector2>();
        moveAction.action.canceled += ctx => MovementInput = Vector2.zero;

        // Jump
        jumpAction.action.started += ctx =>
        {
            jumpBufferTimer = jumpBufferTime;
            JumpHeld = true;
        };

        jumpAction.action.canceled += ctx => JumpHeld = false;

        // Attack
        attackAction.action.started += ctx => AttackPressed = true;
        attackAction.action.canceled += ctx => AttackPressed = false;

    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        attackAction.action.Disable();
    }

    private void Update()
    {
        // Timer of jump buffer
        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
        }
    }

    public void ResetJumpBuffer()
    {
        jumpBufferTimer = 0f;
    }
}
