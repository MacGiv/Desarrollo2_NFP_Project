using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string startLevelSceneName = "GameScene_1";
    [SerializeField] private string creditsSceneName = "Credits";

    public void StartGame()
    {
        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var flow))
        {
            flow.LoadSceneReplacing(startLevelSceneName);
        }
    }

    public void ShowCredits()
    {
        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var flow))
        {
            flow.LoadSceneReplacing(creditsSceneName);
        }
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
