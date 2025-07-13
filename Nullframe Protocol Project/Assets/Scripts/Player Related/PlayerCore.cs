using UnityEngine;

public class PlayerCore : MonoBehaviour
{

    // Cached Components
    [SerializeField] private PlayerData data;
    private PlayerInputHandler input;
    private PlayerGroundChecker groundChecker;
    private PlayerMovement movement;
    private PlayerStateMachine stateMachine;
    private Animator animator;
    private PlayerComboHandler comboHandler;
    private AttackDirectionResolver directionResolver;
    private SpecialAttackHandler specialAttackHandler;
    private SpecialAttackChargeSystem specialAttackChargeSystem;

    // State Variables
    private PlayerIdleState state_idle;
    private PlayerMoveState state_move;
    private PlayerJumpState state_jump;
    private PlayerInAirState state_inAir;
    private PlayerAttackState state_attack;
    private PlayerHurtState state_hurt;
    private PlayerDeathState state_death;
    private PlayerSpecialAttackState state_specialAttack;

    // State Properties 
    public PlayerIdleState IdleState => state_idle;
    public PlayerMoveState MoveState => state_move;
    public PlayerJumpState JumpState => state_jump;
    public PlayerInAirState InAirState => state_inAir;
    public PlayerAttackState AttackState => state_attack;
    public PlayerHurtState HurtState => state_hurt;
    public PlayerDeathState DeathState => state_death;
    public PlayerSpecialAttackState SpecialAttackState => state_specialAttack;
    
    // Cached Components Properties
    public PlayerData Data => data;
    public PlayerInputHandler Input => input;
    public PlayerMovement Movement => movement;
    public PlayerGroundChecker GroundChecker => groundChecker;
    public Animator Animator => animator;
    public PlayerStateMachine StateMachine => stateMachine;
    public PlayerComboHandler ComboHandler => comboHandler;
    public AttackDirectionResolver DirectionResolver => directionResolver;
    public LockOnHandler LockOnHandler { get; private set; }
    public SpecialAttackHandler SpecialAttackHandler => specialAttackHandler;
    public SpecialAttackChargeSystem SpecialAttackChargeSystem => specialAttackChargeSystem;

    private void OnEnable()
    {
        Input.OnToggleLockOn += LockOnHandler.ToggleLockOn;
        Input.OnSpecialAttackPressed += HandleSpecialAttackInput;
    }

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        input = GetComponent<PlayerInputHandler>();
        groundChecker = GetComponent<PlayerGroundChecker>();
        movement = GetComponent<PlayerMovement>();
        movement.SetData(data);
        stateMachine = new PlayerStateMachine();
        animator = GetComponentInChildren<Animator>();
        comboHandler = GetComponent<PlayerComboHandler>();
        directionResolver = GetComponent<AttackDirectionResolver>();
        LockOnHandler = GetComponent<LockOnHandler>();
        specialAttackHandler = GetComponent<SpecialAttackHandler>();
        specialAttackChargeSystem = GetComponent<SpecialAttackChargeSystem>();

        // Initialize States
        state_idle = new PlayerIdleState(this, stateMachine, data, "idle");
        state_move = new PlayerMoveState(this, stateMachine, data, "isMoving");
        state_jump = new PlayerJumpState(this, stateMachine, data, "jumpingUp");
        state_inAir = new PlayerInAirState(this, stateMachine, data, "falling");
        state_attack = new PlayerAttackState(this, stateMachine, data, "attack");
        state_hurt = new PlayerHurtState(this, stateMachine, data, "hurt");
        state_death = new PlayerDeathState(this, stateMachine, data, "death");
        state_specialAttack = new PlayerSpecialAttackState(this, stateMachine, data, "special");

        stateMachine.Initialize(state_idle);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void OnDisable()
    {
        Input.OnToggleLockOn -= LockOnHandler.ToggleLockOn;
        Input.OnSpecialAttackPressed -= HandleSpecialAttackInput;
    }

    private void HandleSpecialAttackInput()
    {
        if (stateMachine.CurrentState == AttackState)
            return;

        if (!specialAttackHandler.HasValidTarget())
            return;

        if (specialAttackChargeSystem.CanUseSpecial)
        {
            stateMachine.ChangeState(SpecialAttackState);
        }
    }


}
