using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    // Cached Components
    private PlayerInputHandler input;
    private PlayerGroundChecker groundChecker;
    private PlayerMovement movement;
    private PlayerStateMachine stateMachine;
    private Animator animator;

    // State Variables
    private PlayerIdleState state_idle;
    private PlayerMoveState state_move;
    private PlayerJumpState state_jump;
    private PlayerInAirState state_inAir;
    private PlayerAttackState state_attack;

    // Properties (for public access)
    public PlayerIdleState IdleState => state_idle;
    public PlayerMoveState MoveState => state_move;
    public PlayerJumpState JumpState => state_jump;
    public PlayerInAirState InAirState => state_inAir;
    public PlayerAttackState AttackState => state_attack;

    public PlayerInputHandler Input => input;
    public PlayerMovement Movement => movement;
    public PlayerGroundChecker GroundChecker => groundChecker;
    public Animator Animator => animator;
    public PlayerStateMachine StateMachine => stateMachine;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        groundChecker = GetComponent<PlayerGroundChecker>();
        movement = GetComponent<PlayerMovement>();
        movement.SetData(data);
        stateMachine = new PlayerStateMachine();
        animator = GetComponentInChildren<Animator>();

        // Initialize States
        state_idle = new PlayerIdleState(this, stateMachine, data, "idle");
        state_move = new PlayerMoveState(this, stateMachine, data, "isMoving");
        state_jump = new PlayerJumpState(this, stateMachine, data, "jump");
        state_inAir = new PlayerInAirState(this, stateMachine, data, "inAir");
        state_attack = new PlayerAttackState(this, stateMachine, data, "attack");

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
