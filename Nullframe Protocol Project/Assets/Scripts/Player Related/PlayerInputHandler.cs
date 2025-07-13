using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Capture Player's imputs and expose them to other scripts
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference lockOnAction;

    [Header("Cheats")]
    [SerializeField] private InputActionReference godModeAction;
    [SerializeField] private InputActionReference flashSpeedAction;
    [SerializeField] private InputActionReference nextLevelAction;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime = 0.15f;

    [Header("Attack Buffer")]
    [SerializeField] private float attackBufferTime = 0.3f;
    
    private float jumpBufferTimer;
    private float attackBufferTimer;

    public event Action OnGodModeCheat;
    public event Action OnFlashSpeedCheat;
    public event Action OnNextLevelCheat;
    public event Action OnToggleLockOn;

    public Vector2 MovementInput { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool JumpPressed => jumpBufferTimer > 0f;
    public bool AttackPressed { get; private set; }
    public bool BufferedAttackPressed => attackBufferTimer > 0f;
    public bool NewAttackInput { get; private set; }
    public bool AttackBufferedManually { get; private set; }



    private void OnEnable()
    {
        // Cheat related inputs initialization
        godModeAction.action.Enable();
        flashSpeedAction.action.Enable();
        nextLevelAction.action.Enable();

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
            jumpBufferTimer = jumpBufferTime;
            JumpHeld = true;
        };
        jumpAction.action.canceled += ctx => JumpHeld = false;

        // Attack
        attackAction.action.started += ctx =>
        {
            AttackPressed = true;
            attackBufferTimer = attackBufferTime;
            NewAttackInput = true;
            AttackBufferedManually = true;
        };
        attackAction.action.canceled += ctx => AttackPressed = false;

    }

    private void OnDisable()
    {
        // Player actions disable
        moveAction.action.Disable();
        jumpAction.action.Disable();
        attackAction.action.Disable();
        lockOnAction.action.Disable();

        // Cheat actions disable
        godModeAction.action.Disable();
        flashSpeedAction.action.Disable();
        nextLevelAction.action.Disable();
    }

    private void Update()
    {
        // Timer of jump buffer
        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // Timer of attack buffer
        if (attackBufferTimer > 0f)
        {
            attackBufferTimer -= Time.deltaTime;
        }

        NewAttackInput = false;  // Clean attack input
    }

    public void ResetJumpBuffer()
    {
        jumpBufferTimer = 0f;
    }

    public void ResetAttackBuffer()
    {
        attackBufferTimer = 0f;
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
