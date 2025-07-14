using UnityEngine;

/// <summary>
/// Bootstraps the scene system and registers services.
/// </summary>
public class Bootstrap : MonoBehaviour
{
    [SerializeField] private string firstScene = "MainMenu";

    private void Awake()
    {
        if (FindObjectsByType<Bootstrap>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        var loader = GetComponent<SceneLoader>();
        if (loader != null)
            ServiceProvider.SetService(loader);

        var flowHandler = GetComponent<SceneFlowHandler>();
        if (flowHandler != null)
            ServiceProvider.SetService(flowHandler);

        var music = GetComponentInChildren<MusicHandler>();
        if (music != null)
            ServiceProvider.SetService(music);

        flowHandler.LoadSceneReplacing(firstScene);
    }
}
