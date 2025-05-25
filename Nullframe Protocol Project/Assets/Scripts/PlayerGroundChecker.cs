using UnityEngine;

/// <summary>
/// Detecta si el jugador está tocando el suelo usando un sphere check.
/// </summary>
public class PlayerGroundChecker : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public bool IsGrounded { get; private set; }

    // Must be called from PlayerCore's FixedUpdate.
    public void CheckGround()
    {
        IsGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
