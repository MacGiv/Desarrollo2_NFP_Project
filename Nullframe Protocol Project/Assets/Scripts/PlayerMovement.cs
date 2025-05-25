using UnityEngine;

/// <summary>
/// Handles all player movement
/// States reference this component to handle any movement/force related stuff.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInputHandler input;
    private PlayerData data;

    private Vector3 currentVelocity;
    private float jumpHoldTimer;
    private int jumpCount;
    private float coyoteTimer;

    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInputHandler>();
    }

    public void SetData(PlayerData playerData)
    {
        data = playerData;
    }

    public void UpdateGrounded(bool grounded)
    {
        isGrounded = grounded;

        if (isGrounded)
        {
            coyoteTimer = data.CoyoteTime;
            jumpCount = 0;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }
    }

    public void Move()
    {
        Vector2 movementInput = input.MovementInput;

        Vector3 inputDir = new Vector3(movementInput.x, 0, movementInput.y);
        if (inputDir.magnitude > 1f)
            inputDir.Normalize();

        // Direction relative to camera
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        // Movement + rotation
        if (moveDir.magnitude > 0.1f)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, moveDir * data.MoveSpeed, data.Acceleration * Time.fixedDeltaTime);
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, data.RotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, data.Deceleration * Time.fixedDeltaTime);
        }

        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
    }

    public bool CanJump()
    {
        return coyoteTimer > 0f || jumpCount < data.MaxJumps;
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * data.JumpForce, ForceMode.Impulse);
        jumpHoldTimer = 0f;
        jumpCount++;
    }

    public void HoldJump(bool isHeld)
    {
        if (isHeld && jumpHoldTimer < data.JumpHoldTime)
        {
            rb.AddForce(Vector3.up * data.JumpHoldForce, ForceMode.Force);
            jumpHoldTimer += Time.fixedDeltaTime;
        }

        if (!isHeld)
        {
            jumpHoldTimer = data.JumpHoldTime;
        }
    }
}
