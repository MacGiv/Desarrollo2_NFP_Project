using UnityEngine;

/// <summary>
/// Listens to audio events and plays corresponding sounds.
/// </summary>
public class AudioHandler : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip playerHitClip;
    [SerializeField] private AudioClip attackHitClip;
    [SerializeField] private AudioClip specialAttackClip;
    [SerializeField] private AudioClip chargeAbsorbedClip;
    [SerializeField] private AudioClip enemyDeathClip;
    [SerializeField] private AudioClip auraActiveClip;

    [Header("Audio Settings")]
    [SerializeField] private float volume = 1.0f;

    private GameObject auraAudioSourceObj;
    private AudioSource auraAudioSource;

    private void OnEnable()
    {
        AudioEvents.OnPlayerHit += PlayPlayerHit;
        AudioEvents.OnAttackHit += PlayAttackHit;
        AudioEvents.OnSpecialAttack += PlaySpecialAttack;
        AudioEvents.OnChargeAbsorbed += PlayChargeAbsorbed;
        AudioEvents.OnEnemyDeath += PlayEnemyDeath;
        AudioEvents.OnAuraStateChanged += HandleAuraAudio;
    }

    private void OnDisable()
    {
        AudioEvents.OnPlayerHit -= PlayPlayerHit;
        AudioEvents.OnAttackHit -= PlayAttackHit;
        AudioEvents.OnSpecialAttack -= PlaySpecialAttack;
        AudioEvents.OnChargeAbsorbed -= PlayChargeAbsorbed;
        AudioEvents.OnEnemyDeath -= PlayEnemyDeath;
        AudioEvents.OnAuraStateChanged -= HandleAuraAudio;
    }

    private void PlayClip(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, pos, volume);
    }

    private void PlayPlayerHit(Vector3 pos) => PlayClip(playerHitClip, pos);
    private void PlayAttackHit(Vector3 pos) => PlayClip(attackHitClip, pos);
    private void PlaySpecialAttack(Vector3 pos) => PlayClip(specialAttackClip, pos);
    private void PlayChargeAbsorbed(Vector3 pos) => PlayClip(chargeAbsorbedClip, pos);
    private void PlayEnemyDeath(Vector3 pos) => PlayClip(enemyDeathClip, pos);

    private void HandleAuraAudio(bool active)
    {
        if (auraActiveClip == null) return;

        if (auraAudioSourceObj != null)
        {
            auraAudioSource.Stop();
            Destroy(auraAudioSourceObj);
        }
        if (active)
        {
            auraAudioSourceObj = new GameObject("AuraAudio");
            auraAudioSourceObj.transform.position = transform.position;
            auraAudioSource = auraAudioSourceObj.AddComponent<AudioSource>();
            auraAudioSource.clip = auraActiveClip;
            auraAudioSource.loop = true;
            auraAudioSource.volume = 0.25f;
            auraAudioSource.Play();
        }
    }
}
