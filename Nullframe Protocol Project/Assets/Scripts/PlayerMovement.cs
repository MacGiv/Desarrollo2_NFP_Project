using UnityEngine;

/// <summary>
/// Controla el movimiento del jugador utilizando una curva de aceleración y rotación basada en la dirección de la cámara.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private AnimationCurve accelerationCurve;
    [SerializeField] private float accelerationTime = 1.0f; // Tiempo que tarda en llegar a velocidad máxima

    [Header("Rotación")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Referencias")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private PlayerInputHandler inputHandler;

    private float currentSpeed;
    private float accelerationTimer;

    private Vector3 velocity;
    private Vector3 direction;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputHandler = GetComponent<PlayerInputHandler>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// Procesa el input de movimiento, calcula la dirección relativa a la cámara,
    /// aplica aceleración y mueve al personaje.
    /// </summary>
    private void HandleMovement()
    {
        // Input del jugador
        Vector2 input = inputHandler.MovementInput;

        // Movimiento relativo a la cámara
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        direction = camForward * input.y + camRight * input.x;
        direction.Normalize();

        // Aceleración con curva
        if (direction.magnitude > 0)
        {
            accelerationTimer += Time.deltaTime / accelerationTime;
        }
        else
        {
            accelerationTimer -= Time.deltaTime / accelerationTime;
        }
        accelerationTimer = Mathf.Clamp01(accelerationTimer);

        float curveValue = accelerationCurve.Evaluate(accelerationTimer);
        currentSpeed = curveValue * moveSpeed;

        // Movimiento
        velocity = direction * currentSpeed;
        controller.Move(velocity * Time.deltaTime);

        // Rotación suave
        if (direction.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
