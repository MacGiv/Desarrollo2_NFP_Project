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
    private PlayerDeathState state_death;
    // State Variables
    private PlayerIdleState state_idle;
    private PlayerMoveState state_move;
    private PlayerJumpState state_jump;
    private PlayerInAirState state_inAir;
    private PlayerAttackState state_attack;
    private PlayerHurtState state_hurt;

    // Properties (for public access)
    public PlayerIdleState IdleState => state_idle;
    public PlayerMoveState MoveState => state_move;
    public PlayerJumpState JumpState => state_jump;
    public PlayerInAirState InAirState => state_inAir;
    public PlayerAttackState AttackState => state_attack;
    public PlayerHurtState HurtState => state_hurt;
    public PlayerDeathState DeathState => state_death;
    public PlayerData Data => data;
    public PlayerInputHandler Input => input;
    public PlayerMovement Movement => movement;
    public PlayerGroundChecker GroundChecker => groundChecker;
    public Animator Animator => animator;
    public PlayerStateMachine StateMachine => stateMachine;
    public PlayerComboHandler ComboHandler => comboHandler;

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

        // Initialize States
        state_idle = new PlayerIdleState(this, stateMachine, data, "idle");
        state_move = new PlayerMoveState(this, stateMachine, data, "isMoving");
        state_jump = new PlayerJumpState(this, stateMachine, data, "jumpingUp");
        state_inAir = new PlayerInAirState(this, stateMachine, data, "falling");
        state_attack = new PlayerAttackState(this, stateMachine, data, "attack");
        state_hurt = new PlayerHurtState(this, stateMachine, data, "hurt");
        state_death = new PlayerDeathState(this, stateMachine, data, "death");

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
}
