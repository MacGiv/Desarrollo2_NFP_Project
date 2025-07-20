using UnityEngine;

/// <summary>
/// Handles visibility of the lock-on crosshair UI.
/// </summary>
public class LockOnUI : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;

    private void Awake()
    {
        // Optional: hide on start
        crosshair.SetActive(false);
    }

    private void OnEnable()
    {
        LockOnEvents.OnLockOnEnabled += ShowCrosshair;
        LockOnEvents.OnLockOnDisabled += HideCrosshair;
    }

    private void OnDisable()
    {
        LockOnEvents.OnLockOnEnabled -= ShowCrosshair;
        LockOnEvents.OnLockOnDisabled -= HideCrosshair;
    }

    private void ShowCrosshair()
    {
        crosshair.SetActive(true);
    }

    private void HideCrosshair()
    {
        crosshair.SetActive(false);
    }
}
