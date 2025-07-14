using UnityEngine;

/// <summary>
/// Plays background music depending on the scene, managed externally.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicHandler : MonoBehaviour
{
    [SerializeField] private AudioClip menuTheme;
    [SerializeField] private AudioClip level1Theme;
    [SerializeField] private AudioClip level2Theme;
    [SerializeField] private AudioClip level3Theme;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
    }

    /// <summary>
    /// Call this when the scene changes to set the correct music track.
    /// </summary>
    public void PlayMusicForScene(string sceneName)
    {
        AudioClip clip = sceneName switch
        {
            "MainMenu" => menuTheme,
            "GameScene_1" => level1Theme,
            "GameScene_2" => level2Theme,
            "GameScene_3" => level3Theme,
            _ => null
        };

        if (clip != null && clip != _audioSource.clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}
