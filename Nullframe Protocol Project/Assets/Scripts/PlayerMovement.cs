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
    private PlayerCore core;

    private Vector3 currentVelocity;
    private float jumpHoldTimer;
    private int jumpCount;
    private float coyoteTimer;
    private bool jumpedThisFrame = false;

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

    /// <summary>
    /// Update grounded bool
    /// </summary>
    public void UpdateGrounded(bool grounded)
    {
        isGrounded = grounded;

        if (jumpedThisFrame) 
        {
            jumpedThisFrame = false;
            return;
        }

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

    /// <summary>
    /// Move the player
    /// </summary>
    public void Move(Vector3 moveDir)
    {
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();

        currentVelocity = Vector3.Lerp(currentVelocity, moveDir * data.MoveSpeed, data.Acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
    }

    /// <summary>
    /// Rotate the player
    /// </summary>
    public void ApplyRotation(Vector3 lookDir)
    {
        if (lookDir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(lookDir);

        // Prevent "U" movement when turning 180°
        if (Vector3.Dot(transform.forward, lookDir) > -0.8f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, data.RotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            transform.rotation = targetRot;
        }
    }

    /// <summary>
    /// Return input related to camera direction
    /// </summary>
    public Vector3 GetCameraRelativeInput()
    {
        Vector2 inputVec = input.MovementInput;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        return camForward * inputVec.y + camRight * inputVec.x;
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
        // Avoid ground check resetting the jump after jumping from the ground
        jumpCount++;
        jumpedThisFrame = true;
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
