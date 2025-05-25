using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float deceleration = 25f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpHoldForce = 4f;
    [SerializeField] private float jumpHoldTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private int maxJumps = 2;

    public float MoveSpeed => moveSpeed;
    public float Acceleration => acceleration;
    public float Deceleration => deceleration;
    public float RotationSpeed => rotationSpeed;

    public float JumpForce => jumpForce;
    public float JumpHoldForce => jumpHoldForce;
    public float JumpHoldTime => jumpHoldTime;
    public float CoyoteTime => coyoteTime;
    public int MaxJumps => maxJumps;
}
