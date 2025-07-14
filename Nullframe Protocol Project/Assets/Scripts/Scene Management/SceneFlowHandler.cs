using UnityEngine;

/// <summary>
/// Manages which scenes are currently loaded and handles transitions.
/// </summary>
public class SceneFlowHandler : MonoBehaviour
{
    private string _currentScene;
    private string _lastScene;

    public string CurrentScene => _currentScene;
    public string LastScene => _lastScene;

    public void LoadSceneReplacing(string sceneToLoad)
    {
        if (ServiceProvider.TryGetService<SceneLoader>(out var loader))
        {
            if (!string.IsNullOrEmpty(_currentScene))
            {
                loader.UnloadScene(_currentScene);
                _lastScene = _currentScene;
            }

            loader.LoadScene(sceneToLoad, additive: true);
            _currentScene = sceneToLoad;

            if (ServiceProvider.TryGetService<MusicHandler>(out var music))
            {
                music.PlayMusicForScene(sceneToLoad);
            }

        }
        else
        {
            Debug.LogError("SceneLoader service not found.");
        }
    }

    public void LoadAdditionalScene(string additiveScene)
    {
        if (ServiceProvider.TryGetService<SceneLoader>(out var loader))
        {
            loader.LoadScene(additiveScene, additive: true);
        }
    }

    public void UnloadScene(string sceneToUnload)
    {
        if (ServiceProvider.TryGetService<SceneLoader>(out var loader))
        {
            loader.UnloadScene(sceneToUnload);
        }
    }
}