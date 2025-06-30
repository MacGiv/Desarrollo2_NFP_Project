using UnityEngine;

public class TimeDebug : MonoBehaviour
{
    [SerializeField] float auxTime;
    [SerializeField] bool pauseBool;

    private void OnEnable()
    {
        Time.timeScale = auxTime;
        pauseBool = FindFirstObjectByType<PauseManager>().IsPaused;
    }

    private void Update()
    {
        if (!pauseBool)
        {
            Time.timeScale = auxTime;
        }
    }
}
