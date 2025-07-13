using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Control rotation around the player and lock on target
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Cached")]
    [SerializeField] private Transform target;

    [Header("Offset")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5);

    [Header("Rotation")]
    [SerializeField] private float xMin = -40f;
    [SerializeField] private float xMax = 70f;
    [SerializeField] private float sensitivityX = 1f;
    [SerializeField] private float sensitivityY = 1f;
    [SerializeField] private bool invertY = false;

    [Header("Input")]
    [SerializeField] private InputActionReference lookAction;

    [Header("Lock On Parameters")]
    [SerializeField] private LockOnHandler lockOnHandler;

    private float xRot; // Vertical
    private float yRot; // Horizontal

    private void OnEnable()
    {
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        lookAction.action.Disable();
    }

    private void Update()
    {
        if (lockOnHandler != null && lockOnHandler.IsLockedOn)
        {
            // No aceptar input en lock-on
            return;
        }

        Vector2 input = lookAction.action.ReadValue<Vector2>(); 
        yRot += input.x * sensitivityX;
        xRot += (invertY ? -1 : 1) * input.y * sensitivityY;

        xRot = Mathf.Clamp(xRot, xMin, xMax);
    }

    private void LateUpdate()
    {
        if (lockOnHandler != null && lockOnHandler.IsLockedOn)
        {
            // if (lockOnHandler.CurrentTarget == null)
            // {
            //     // Target Destroyed
            //     lockOnHandler.ForceUnlock();
            //     return;
            // }

            Vector3 playerPos = target.position;
            Vector3 targetPos = lockOnHandler.CurrentTarget.position;

            // Direction towards taget horizontally
            Vector3 toTarget = targetPos - playerPos;
            toTarget.y = -lockOnHandler.LockOnYOffset; // Set Y camera pos
            toTarget.Normalize();

            // Place camera behind the player facing enemy
            Quaternion lookRotation = Quaternion.LookRotation(toTarget);
            Vector3 desiredPosition = playerPos - (lookRotation * offset);

            transform.position = desiredPosition;
            transform.LookAt(targetPos); // Always looking at target
        }
        else
        {
            // Modo normal (libre)
            Quaternion rotation = Quaternion.Euler(xRot, yRot, 0f);
            Vector3 desiredPosition = target.position + rotation * offset;

            transform.position = desiredPosition;
            transform.LookAt(target);
        }
    }

    /// <summary>
    /// Rotation Y to align PlayerMovement with the camera.
    /// </summary>
    public Quaternion YRotation => Quaternion.Euler(0, yRot, 0);
}
