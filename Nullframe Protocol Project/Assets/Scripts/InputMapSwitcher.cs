using UnityEngine;
using UnityEngine.InputSystem;

public class InputMapSwitcher : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void SetGameplayMap()
    {
        playerInput.SwitchCurrentActionMap("Gameplay");
    }

    public void SetUIMap()
    {
        playerInput.SwitchCurrentActionMap("UI");
    }
}
