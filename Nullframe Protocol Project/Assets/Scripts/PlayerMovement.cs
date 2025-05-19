using UnityEngine;

/// <summary>
/// Controla el movimiento del jugador utilizando una curva de aceleraci�n y rotaci�n basada en la direcci�n de la c�mara.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private AnimationCurve accelerationCurve;
    [SerializeField] private float accelerationTime = 1.0f; // Tiempo que tarda en llegar a velocidad m�xima

    [Header("Rotaci�n")]
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
    /// Procesa el input de movimiento, calcula la direcci�n relativa a la c�mara,
    /// aplica aceleraci�n y mueve al personaje.
    /// </summary>
    private void HandleMovement()
    {
        // Input del jugador
        Vector2 input = inputHandler.MovementInput;

        // Movimiento relativo a la c�mara
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        direction = camForward * input.y + camRight * input.x;
        direction.Normalize();

        // Aceleraci�n con curva
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

        // Rotaci�n suave
        if (direction.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
