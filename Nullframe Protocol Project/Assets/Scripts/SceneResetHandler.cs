using UnityEngine;

/// <summary>
/// Listens for the restart event and reloads the current scene via SceneFlowHandler.
/// </summary>
public class SceneResetHandler : MonoBehaviour
{
    private void OnEnable()
    {
        SceneResetInputHandler.OnResetPressed += ResetScene;
    }

    private void OnDisable()
    {
        SceneResetInputHandler.OnResetPressed -= ResetScene;
    }

    private void ResetScene()
    {
        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var sceneFlow))
        {
            if (!string.IsNullOrEmpty(sceneFlow.CurrentScene))
            {
                sceneFlow.LoadSceneReplacing(sceneFlow.CurrentScene);
                Debug.Log("[SceneResetHandler] Scene reset: " + sceneFlow.CurrentScene);
            }
            else
            {
                Debug.LogWarning("[SceneResetHandler] No current scene to reload.");
            }
        }
        else
        {
            Debug.LogError("SceneFlowHandler not found in ServiceProvider.");
        }
    }
}
