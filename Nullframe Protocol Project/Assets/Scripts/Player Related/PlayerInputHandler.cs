using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Capture Player's imputs and expose them to other scripts. It also handles jump and attack buffer's timers.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference lockOnAction;
    [SerializeField] private InputActionReference specialAttackAction;

    [Header("Cheats")]
    [SerializeField] private InputActionReference godModeAction;
    [SerializeField] private InputActionReference flashSpeedAction;
    [SerializeField] private InputActionReference nextLevelAction;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime = 0.15f;

    [Header("Attack Buffer")]
    [SerializeField] private float attackBufferTime = 0.3f;
    
    // Timers
    private float _jumpBufferTimer;
    private float _attackBufferTimer;

    // Events
    public event Action OnGodModeCheat;
    public event Action OnFlashSpeedCheat;
    public event Action OnNextLevelCheat;
    public event Action OnToggleLockOn;
    public event Action OnSpecialAttackPressed;

    // Properties (for public access)
    public Vector2 MovementInput { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool JumpPressed => _jumpBufferTimer > 0f;
    public bool AttackPressed { get; private set; }
    public bool BufferedAttackPressed => _attackBufferTimer > 0f;
    public bool NewAttackInput { get; private set; }
    public bool AttackBufferedManually { get; private set; }



    private void OnEnable()
    {
        // Cheat related inputs initialization
        godModeAction.action.Enable();
        flashSpeedAction.action.Enable();
        nextLevelAction.action.Enable();
        specialAttackAction.action.Enable();

        godModeAction.action.performed += ctx => OnGodModeCheat?.Invoke();
        flashSpeedAction.action.performed += ctx => OnFlashSpeedCheat?.Invoke();
        nextLevelAction.action.performed += ctx => OnNextLevelCheat?.Invoke();
        
        // Player related inputs initialization
        moveAction.action.Enable();
        jumpAction.action.Enable();
        attackAction.action.Enable();
        lockOnAction.action.Enable();

        // Lock on target
        lockOnAction.action.performed += ctx => OnToggleLockOn?.Invoke();

        // Movement
        moveAction.action.performed += ctx => MovementInput = ctx.ReadValue<Vector2>();
        moveAction.action.canceled += ctx => MovementInput = Vector2.zero;

        // Jump
        jumpAction.action.started += ctx =>
        {
            _jumpBufferTimer = jumpBufferTime;
            JumpHeld = true;
        };
        jumpAction.action.canceled += ctx => JumpHeld = false;

        // Attack
        attackAction.action.started += ctx =>
        {
            AttackPressed = true;
            _attackBufferTimer = attackBufferTime;
            NewAttackInput = true;
            AttackBufferedManually = true;
        };
        attackAction.action.canceled += ctx => AttackPressed = false;

        // Special Attack
        specialAttackAction.action.performed += ctx => OnSpecialAttackPressed?.Invoke();
    }

    private void OnDisable()
    {
        // Player actions disable
        moveAction.action.Disable();
        jumpAction.action.Disable();
        attackAction.action.Disable();
        lockOnAction.action.Disable();
        specialAttackAction.action.Disable();

        // Cheat actions disable
        godModeAction.action.Disable();
        flashSpeedAction.action.Disable();
        nextLevelAction.action.Disable();
    }

    private void Update()
    {
        // Timer of jump buffer
        if (_jumpBufferTimer > 0f)
        {
            _jumpBufferTimer -= Time.deltaTime;
        }

        // Timer of attack buffer
        if (_attackBufferTimer > 0f)
        {
            _attackBufferTimer -= Time.deltaTime;
        }

        NewAttackInput = false;  // Clean attack input
    }

    public void ResetJumpBuffer()
    {
        _jumpBufferTimer = 0f;
    }

    public void ResetAttackBuffer()
    {
        _attackBufferTimer = 0f;
    }

    public bool ConsumeAttackBuffer()
    {
        if (BufferedAttackPressed)
        {
            ResetAttackBuffer();
            return true;
        }

        return false;
    }

    public void ConsumeManualAttackBuffer()
    {
        AttackBufferedManually = false;
    }


}
