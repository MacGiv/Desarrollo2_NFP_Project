using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string startLevelSceneName = "GameScene_1";
    [SerializeField] private string creditsSceneName = "Credits";
    [SerializeField] private Button ButtonfirstSelected;

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current.SetSelectedGameObject(ButtonfirstSelected.gameObject);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(ButtonfirstSelected.gameObject);
    }

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
