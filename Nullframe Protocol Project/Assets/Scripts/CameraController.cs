using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla la rotación orbital de la cámara alrededor del personaje.
/// Basado en input de mouse/joystick y orienta al personaje según la rotación Y.
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

    private float xRot; // vertical
    private float yRot; // horizontal

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
        Vector2 input = lookAction.action.ReadValue<Vector2>(); 
        yRot += input.x * sensitivityX;
        xRot += (invertY ? -1 : 1) * input.y * sensitivityY;

        xRot = Mathf.Clamp(xRot, xMin, xMax);
    }

    private void LateUpdate()
    {
        // Rotación completa
        Quaternion rotation = Quaternion.Euler(xRot, yRot, 0f);
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = desiredPosition;
        transform.LookAt(target);
    }

    /// <summary>
    /// Rotación Y para que PlayerMovement pueda alinear el movimiento con la cámara.
    /// </summary>
    public Quaternion YRotation => Quaternion.Euler(0, yRot, 0);
}
