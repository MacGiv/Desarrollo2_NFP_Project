using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Se encarga de capturar y exponer los inputs del jugador.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    // Movimiento expuesto como propiedad pública
    public Vector2 MovementInput { get; private set; }

    // Salto
    public bool JumpPressed { get; private set; }          // Se presionó este frame
    public bool JumpHeld { get; private set; }             // Se mantiene presionado

    private void OnEnable()
    {
        // Movimiento
        moveAction.action.started += HandleMoveInput;
        moveAction.action.performed += HandleMoveInput;
        moveAction.action.canceled += HandleMoveInput;

        // Salto
        jumpAction.action.started += OnJumpStarted;
        jumpAction.action.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        moveAction.action.started -= HandleMoveInput;
        moveAction.action.performed -= HandleMoveInput;
        moveAction.action.canceled -= HandleMoveInput;

        jumpAction.action.started -= OnJumpStarted;
        jumpAction.action.canceled -= OnJumpCanceled;
    }

    /// <summary>
    /// Lee y almacena el input direccional.
    /// </summary>
    private void HandleMoveInput(InputAction.CallbackContext ctx)
    {
        MovementInput = ctx.ReadValue<Vector2>();
    }

    /// <summary>
    /// Marca el inicio de un salto.
    /// </summary>
    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        JumpPressed = true;
        JumpHeld = true;
    }

    /// <summary>
    /// Marca el fin del salto.
    /// </summary>
    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        JumpHeld = false;
    }

    /// <summary>
    /// Llamar desde el script de movimiento luego de consumir el input.
    /// </summary>
    public void UseJump() => JumpPressed = false;
}
