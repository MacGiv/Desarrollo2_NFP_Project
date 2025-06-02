using UnityEngine;

/// <summary>
/// Controla todas las mec�nicas relacionadas al salto.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerJumpHandler : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpHoldForce = 4f;
    [SerializeField] private float jumpHoldTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private int maxJumps = 2;

    private Rigidbody rb;
    private PlayerInputHandler input;
    private PlayerGroundChecker groundChecker;

    private int jumpCount;
    private float coyoteTimer;
    private float jumpHoldTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInputHandler>();
        groundChecker = GetComponent<PlayerGroundChecker>();
    }

    public void Tick()
    {
        bool isGrounded = groundChecker.IsGrounded;

        // Coyote Time
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            jumpCount = 0;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }

        // Jump
        if (input.JumpPressed && (coyoteTimer > 0f || (jumpCount < maxJumps && !isGrounded)))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpHoldTimer = 0f;
            jumpCount++;
            input.ResetJumpBuffer(); // Decrease amount of jumps
        }

        // Jump Hold
        if (input.JumpHeld && jumpHoldTimer < jumpHoldTime)
        {
            rb.AddForce(Vector3.up * jumpHoldForce, ForceMode.Force);
            jumpHoldTimer += Time.fixedDeltaTime;
        }

        if (!input.JumpHeld)
        {
            jumpHoldTimer = jumpHoldTime;
        }
    }
}
