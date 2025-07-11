using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DefeatScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject defeatCanvas;
    [SerializeField] private GameObject firstSelectedButton;

    private void OnEnable()
    {
        FindFirstObjectByType<PlayerHealthSystem>().OnDeath += ShowDefeatScreen;
    }

    private void OnDisable()
    {
        FindFirstObjectByType<PlayerHealthSystem>().OnDeath -= ShowDefeatScreen;
    }

    private void ShowDefeatScreen()
    {
        GetComponent<PauseManager>().enabled = false;
        defeatCanvas.SetActive(true);
        FindFirstObjectByType<EventSystem>().SetSelectedGameObject(firstSelectedButton);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Retry()
    {
        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var loader))
        {
            OnDisable();
            loader.LoadSceneReplacing(loader.CurrentScene);
        }
        else
        {
            Debug.LogError("SceneFlowHandler service not found.");
        }
    }

    public void BackToMenu()
    {
        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var loader))
        {
            OnDisable();
            loader.LoadSceneReplacing("MainMenu");
        }
        else
        {
            Debug.LogError("SceneFlowHandler service not found.");
        }
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

