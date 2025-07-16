using UnityEngine;

public class MessageUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;

    public void ShowMessage(string message)
    {
        messageText.text = message;
        canvasGroup.alpha = 1f;
        // TODO: efecto glitch
    }

    public void HideMessage()
    {
        canvasGroup.alpha = 0f;
    }
}
