using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Plays UI sounds on click and navigation/select.
/// Attach this to UI Buttons.
/// </summary>
[RequireComponent(typeof(Button))]
public class UIButtonSoundHandler : MonoBehaviour, ISelectHandler, IMoveHandler
{
    [Header("Clips")]
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip navigateClip;
    [SerializeField] private float volume = 1f;

    public void OnSelect(BaseEventData eventData)
    {
        PlayNavigateSound();
    }

    public void OnMove(AxisEventData eventData)
    {
        PlayNavigateSound();
    }

    private void PlayNavigateSound()
    {
        if (navigateClip != null)
            AudioSource.PlayClipAtPoint(navigateClip, Camera.main.transform.position, volume/2);
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        if (clickClip != null)
            AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position, volume);
    }
}
