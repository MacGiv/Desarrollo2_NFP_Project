using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void BackToMenu()
    {
        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var flowHandler))
        {
            flowHandler.LoadSceneReplacing(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("SceneFlowHandler service not found.");
        }
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
