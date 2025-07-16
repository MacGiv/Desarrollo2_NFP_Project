using UnityEngine;
using System.Collections;

/// <summary>
/// Handles narrative messages for the level.
/// Talks to the player using MessageHandlerUI service.
/// </summary>
public class NarratorManager : MonoBehaviour
{
    private MessageHandlerUI _messageHandler;

    private void Awake()
    {
        ServiceProvider.TryGetService<MessageHandlerUI>(out _messageHandler);
        if (_messageHandler == null)
            Debug.Log("Msg Handler Not Found");
    }

    public void Say(string message, float duration)
    {
        if (_messageHandler != null)
            StartCoroutine(_messageHandler.ShowMessageForDuration(message, duration));
    }

    public void SayPersistent(string message)
    {
        _messageHandler?.ShowPersistentMessage(message);
    }

    public void StopTalking()
    {
        _messageHandler?.HideMessage();
    }
}
