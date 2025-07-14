using UnityEngine;

/// <summary>
/// Handles spawning of visual effects based on game events.
/// </summary>
public class ParticleSystemHandler : MonoBehaviour
{
    [Header("Particle Prefabs")]
    [SerializeField] private GameObject pfxPlayerHit;
    [SerializeField] private GameObject pfxAttackHit;
    [SerializeField] private GameObject pfxSpecialAttack;
    [SerializeField] private GameObject pfxChargeAbsorbed;
    [SerializeField] private GameObject pfxEnemyDeath;

    [Header("Aura Settings")]
    [SerializeField] private GameObject specialAura; // Aura object to toggle (already in scene)

    private void OnEnable()
    {
        ParticleEvents.OnPlayerHit += PlayPlayerHit;
        ParticleEvents.OnAttackHit += PlayAttackHit;
        ParticleEvents.OnSpecialAttack += PlaySpecialAttack;
        ParticleEvents.OnChargeAbsorbed += PlayChargeAbsorbed;
        ParticleEvents.OnEnemyDeath += PlayEnemyDeath;
        ParticleEvents.OnSpecialAuraChanged += ToggleSpecialAura;
    }

    private void OnDisable()
    {
        ParticleEvents.OnPlayerHit -= PlayPlayerHit;
        ParticleEvents.OnAttackHit -= PlayAttackHit;
        ParticleEvents.OnSpecialAttack -= PlaySpecialAttack;
        ParticleEvents.OnChargeAbsorbed -= PlayChargeAbsorbed;
        ParticleEvents.OnEnemyDeath -= PlayEnemyDeath;
        ParticleEvents.OnSpecialAuraChanged -= ToggleSpecialAura;
    }

    private void PlayParticle(GameObject prefab, Vector3 position)
    {
        if (prefab == null) return;

        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        Destroy(instance, 3f);
    }

    private void PlayPlayerHit(Vector3 pos) => PlayParticle(pfxPlayerHit, pos);
    private void PlayAttackHit(Vector3 pos) => PlayParticle(pfxAttackHit, pos);
    private void PlaySpecialAttack(Vector3 pos) => PlayParticle(pfxSpecialAttack, pos);
    private void PlayChargeAbsorbed(Vector3 pos) => PlayParticle(pfxChargeAbsorbed, pos);
    private void PlayEnemyDeath(Vector3 pos) => PlayParticle(pfxEnemyDeath, pos);

    private void ToggleSpecialAura(bool isActive)
    {
        if (specialAura != null)
            specialAura.SetActive(isActive);
    }
}
