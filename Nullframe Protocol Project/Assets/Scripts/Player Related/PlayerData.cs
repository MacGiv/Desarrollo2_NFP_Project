using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float deceleration = 25f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float airMovementMultiplier = 0.7f;
    [SerializeField] private float moveInputThreshold = 0.3f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpHoldForce = 4f;
    [SerializeField] private float jumpHoldTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private int maxJumps = 2;

    [Header("Attack")]
    [SerializeField] private float attackMovementForce = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackModelHeightModifier = 0.5f;

    [Header("Combat")]
    [SerializeField] private float comboResetTime = 1.0f;
    [SerializeField] private int comboMaxLength = 3;
    [SerializeField] private float receiveHitForce = 40.0f;


    public float MoveSpeed => moveSpeed;
    public float Acceleration => acceleration;
    public float Deceleration => deceleration;
    public float RotationSpeed => rotationSpeed;
    public float AirMovementMultiplier => airMovementMultiplier;

    public float MoveInputThreshold => moveInputThreshold;

    public float JumpForce => jumpForce;
    public float JumpHoldForce => jumpHoldForce;
    public float JumpHoldTime => jumpHoldTime;
    public float CoyoteTime => coyoteTime;
    public int MaxJumps => maxJumps;

    public float AttackMovementForce => attackMovementForce;
    public float AttackRadius => attackRadius;
    public float AttackRange => attackRange;
    public int AttackDamage => attackDamage;
    public float AttackModelHeightModifier => attackModelHeightModifier;

    public float ComboResetTime => comboResetTime;
    public int ComboMaxLength => comboMaxLength;
    public float ReceiveHitForce => receiveHitForce;

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

}
