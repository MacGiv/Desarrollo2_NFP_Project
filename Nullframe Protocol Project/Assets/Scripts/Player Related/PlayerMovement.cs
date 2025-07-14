using UnityEngine;
using System;
using System.Collections;


/// <summary>
/// Handles all player movement and rotation
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
    private float coyoteTimer;
    private bool jumpedThisFrame = false;
    private int jumpCount;

    public int JumpCount { get => jumpCount; private set => jumpCount = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInputHandler>();
    }

    public void SetData(PlayerData playerData)
    {
        data = playerData;
    }

    public float GetYVelocity()
    {
        return rb.linearVelocity.y;
    }

    /// <summary>
    /// Update grounded bool and handles Coyote's timer.
    /// </summary>
    public void UpdateGrounded(bool grounded)
    {
        if (jumpedThisFrame) 
        {
            jumpedThisFrame = false;
            return;
        }

        if (grounded)
        {
            coyoteTimer = data.CoyoteTime;
            JumpCount = 0;
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
    /// Rotate the player to look at desired direction
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
    /// Rotate the player instantly to look at desired direction
    /// </summary>
    public void ApplyRotationInstant(Vector3 lookDir)
    {
        if (lookDir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        transform.rotation = targetRot;
    }

    /// <summary>
    /// Rotate the player to face a world position
    /// </summary>
    public void LookAtPosition(Vector3 worldPosition, bool instant = false)
    {
        Vector3 direction = worldPosition - transform.position;
        direction.y = 0f;

        if (instant)
            ApplyRotationInstant(direction);
        else
            ApplyRotation(direction);
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
        return coyoteTimer > 0f || JumpCount < data.MaxJumps;
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * data.JumpForce, ForceMode.Impulse);
        jumpHoldTimer = 0f;
        JumpCount++;
        // Avoid ground check resetting the jump after jumping from the ground
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

    public void StopMovement()
    {
        rb.linearVelocity = Vector3.zero;
    }

    public void AttackMove(float force, Vector3 lookDir)
    {
        StopMovement();
        Vector3 attackDir = lookDir;
        rb.AddForce(attackDir * force, ForceMode.Impulse);
    }

    public void HurtMove(float force, Vector3 lookDir)
    {
        StopMovement();
        Vector3 forceDir = -lookDir;
        rb.AddForce(forceDir * force, ForceMode.Impulse);
    }

    public void StartSpecialAttackMovement(Vector3 targetPosition, Action onComplete)
    {
        ApplyRotationInstant(targetPosition);
        StartCoroutine(SpecialAttackRoutine(targetPosition, onComplete));
    }

    private IEnumerator SpecialAttackRoutine(Vector3 targetPos, Action onComplete)
    {
        float duration = data.SpecialAttackDuration;
        float timer = 0f;

        Vector3 start = transform.position;
        Vector3 end = targetPos;

        bool isAirborne = Mathf.Abs(end.y - start.y) > 1f;

        while (timer < duration)
        {
            float t = timer / duration;

            // Concavidad para curva aérea (si hay altura)
            Vector3 pos;
            if (isAirborne)
            {
                float arcHeight = 2.5f;
                float yOffset = Mathf.Sin(t * Mathf.PI) * arcHeight;
                pos = Vector3.Lerp(start, end, t) + Vector3.up * yOffset;
            }
            else
            {
                pos = Vector3.Lerp(start, end, t);
            }

            rb.MovePosition(pos); // Interpola
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(end);
        onComplete?.Invoke();
    }



}
