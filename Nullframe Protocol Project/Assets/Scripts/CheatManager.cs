using UnityEngine;

/// <summary>
/// Activates and manages cheats like God Mode, Speed Boost, and Level Skip.
/// Subscribed to events in PlayerInputHandler.
/// </summary>
public class CheatManager : MonoBehaviour
{
    [SerializeField] private float flashSpeedMultiplier = 2.5f;

    private bool _isGodMode = false;
    private bool _isFlashSpeed = false;

    private PlayerInputHandler _input;
    private PlayerData _data;
    private float _defaultMoveSpeed;

    private void Awake()
    {
        _input = FindAnyObjectByType<PlayerInputHandler>();
        _data = FindAnyObjectByType<PlayerCore>()?.Data;

        if (_data != null)
            _defaultMoveSpeed = _data.MoveSpeed;

        if (_input != null)
        {
            _input.OnGodModeCheat += ToggleGodMode;
            _input.OnFlashSpeedCheat += ToggleFlashSpeed;
            _input.OnNextLevelCheat += SkipToNextLevel;
        }

        ServiceProvider.SetService<CheatManager>(this);
    }

    private void ToggleGodMode()
    {
        _isGodMode = !_isGodMode;
        FindAnyObjectByType<PlayerHealthSystem>()?.ToggleGodMode();
        Debug.Log("[CHEAT] GodMode: " + _isGodMode);
    }

    private void ToggleFlashSpeed()
    {
        if (_data == null) return;

        _isFlashSpeed = !_isFlashSpeed;
        _data.SetMoveSpeed(_isFlashSpeed ? _defaultMoveSpeed * flashSpeedMultiplier : _defaultMoveSpeed);
        Debug.Log("[CHEAT] FlashSpeed: " + _isFlashSpeed);
    }

    private void SkipToNextLevel()
    {
        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var sceneFlow))
        {
            string nextScene = GetNextScene(sceneFlow.CurrentScene);
            if (!string.IsNullOrEmpty(nextScene))
            {
                sceneFlow.LoadSceneReplacing(nextScene);
                Debug.Log("[CHEAT] Next level: " + nextScene);
            }
        }
    }

    private string GetNextScene(string current)
    {
        return current switch
        {
            "GameScene_1" => "GameScene_2",
            "GameScene_2" => "GameScene_3",
            "GameScene_3" => "MainMenu",
            _ => "MainMenu",
        };

        /*
        switch (current)
        {
            case "GameScene_1":
                return "GameScene_2";
            case "GameScene_2":
                return "GameScene_3";
            case "GameScene_3":
                return "MainMenu";
            default:
                return "MainMenu";
        }
        */
    }
}
