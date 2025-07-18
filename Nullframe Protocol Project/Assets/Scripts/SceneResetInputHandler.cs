using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Listens for the Restart Level input and fires an event.
/// </summary>
public class SceneResetInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference resetAction;

    public static event Action OnResetPressed;

    private void OnEnable()
    {
        if (resetAction != null)
        {
            resetAction.action.Enable();
            resetAction.action.performed += HandleReset;
        }
    }

    private void OnDisable()
    {
        if (resetAction != null)
        {
            resetAction.action.performed -= HandleReset;
            resetAction.action.Disable();
        }
    }

    private void HandleReset(InputAction.CallbackContext ctx)
    {
        OnResetPressed?.Invoke();
    }
}
