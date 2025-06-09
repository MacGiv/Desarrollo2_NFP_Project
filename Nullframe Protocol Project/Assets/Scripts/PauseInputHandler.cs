using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PauseInputHandler : MonoBehaviour
{
    [Header("Pause Input")]
    [SerializeField] private InputActionReference pauseAction;

    // Every pause function related can subscribe to this one
    public static event Action OnPausePressed;

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.Enable();
            pauseAction.action.performed += HandlePause;
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= HandlePause;
            pauseAction.action.Disable();
        }
    }

    private void HandlePause(InputAction.CallbackContext ctx)
    {
        OnPausePressed?.Invoke();
    }
}
