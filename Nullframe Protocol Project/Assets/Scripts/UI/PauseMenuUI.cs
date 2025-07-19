using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles UI behavior during the pause menu, including resume and main menu transitions.
/// </summary>
public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private Button resumeButton;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void OnEnable()
    {
        PauseManager.OnPauseChanged += ShowPauseMenu;
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
    }

    private void OnDisable()
    {
        PauseManager.OnPauseChanged -= ShowPauseMenu;
    }

    private void ShowPauseMenu(bool isPaused)
    {
        pauseCanvas.SetActive(isPaused);

        if (isPaused)
        {
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ResumeGame()
    {
        FindAnyObjectByType<PauseManager>()?.TogglePause();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var flow))
        {
            flow.LoadSceneReplacing(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("SceneFlowHandler not found in ServiceProvider.");
        }
    }
}
