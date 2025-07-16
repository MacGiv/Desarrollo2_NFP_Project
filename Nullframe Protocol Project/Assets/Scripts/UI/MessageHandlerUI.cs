using UnityEngine;
using System.Collections;

/// <summary>
/// Manages how messages are displayed on screen via MessageUI.
/// </summary>
public class MessageHandlerUI : MonoBehaviour
{
    [SerializeField] private MessageUI messageUI;

    private void Awake()
    {
        ServiceProvider.SetService(this, overrideIfFound: true);
    }

    /// <summary>
    /// Show a message for a fixed duration, then hides it automatically.
    /// </summary>
    public IEnumerator ShowMessageForDuration(string text, float duration)
    {
        messageUI.ShowMessage(text);
        yield return new WaitForSeconds(duration);
        messageUI.HideMessage();
    }

    /// <summary>
    /// Shows a message and leaves it visible until hidden manually.
    /// </summary>
    public void ShowPersistentMessage(string text)
    {
        messageUI.ShowMessage(text);
    }

    /// <summary>
    /// Hides the current message immediately.
    /// </summary>
    public void HideMessage()
    {
        messageUI.HideMessage();
    }
}
