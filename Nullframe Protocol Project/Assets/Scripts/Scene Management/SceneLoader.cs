using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles loading and unloading Unity scenes asynchronously.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName, bool additive = false)
    {
        StartCoroutine(LoadSceneRoutine(sceneName, additive));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, bool additive)
    {
        var loadMode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, loadMode);

        while (!op.isDone)
            yield return null;
    }

    public void UnloadScene(string sceneName)
    {
        StartCoroutine(UnloadSceneRoutine(sceneName));
    }

    private IEnumerator UnloadSceneRoutine(string sceneName)
    {
        AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName);

        while (!op.isDone)
            yield return null;
    }
}


/*

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] public enum scenesIndex {MAIN_MENU, CREDITS, GAMEPLAY_1, GAMEPLAY_2}
    
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();

// FOR DEBUG ONLY
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
// FOR DEBUG ONLY
    }
}

*/
