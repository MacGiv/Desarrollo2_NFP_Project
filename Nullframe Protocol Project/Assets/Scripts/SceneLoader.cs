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
