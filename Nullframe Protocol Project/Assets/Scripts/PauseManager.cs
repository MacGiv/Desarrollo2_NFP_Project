using UnityEngine;
using System;
using TMPro.Examples;

public class PauseManager : MonoBehaviour
{
    public static event Action<bool> OnPauseChanged;

    public bool IsPaused { get; private set; }

    private PlayerInputHandler playerInput;
    private CameraController cameraController;

    private void OnEnable()
    {
        // Subrscribe PlayerInputHandler to Pause Action to prevent Moving while on Pause
        playerInput = FindFirstObjectByType<PlayerInputHandler>();
        cameraController = FindFirstObjectByType<CameraController>();
        PauseInputHandler.OnPausePressed += TogglePause;
    }

    private void OnDisable()
    {
        PauseInputHandler.OnPausePressed -= TogglePause;
    }

    public void TogglePause()
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }

    private void Pause()
    {
        cameraController.enabled = false;
        playerInput.enabled = false;
        IsPaused = true;
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        OnPauseChanged?.Invoke(true);
        Debug.Log("Game paused");
    }

    private void Resume()
    {
        cameraController.enabled = true;
        playerInput.enabled = true;
        IsPaused = false;
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        OnPauseChanged?.Invoke(false);
        Debug.Log("Game resumed");
    }
}
