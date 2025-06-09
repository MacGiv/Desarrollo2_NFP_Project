using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private Button resumeButton; // First selected
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void OnEnable()
    {
        PauseManager.OnPauseChanged += ShowPauseMenu;
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
            // Gamepad focus
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
        //TODO: Scene loader async
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
