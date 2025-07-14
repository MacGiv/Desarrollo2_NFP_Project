using System;
using UnityEngine;

/// <summary>
/// Static events to request visual feedback (particles, VFX, etc).
/// </summary>
public static class ParticleEvents
{
    public static event Action<Vector3> OnPlayerHit;
    public static event Action<Vector3> OnAttackHit;
    public static event Action<Vector3> OnSpecialAttack;
    public static event Action<Vector3> OnChargeAbsorbed;
    public static event Action<Vector3> OnEnemyDeath;

    public static event Action<bool> OnSpecialAuraChanged;

    public static void RaisePlayerHit(Vector3 pos) => OnPlayerHit?.Invoke(pos);
    public static void RaiseAttackHit(Vector3 pos) => OnAttackHit?.Invoke(pos);
    public static void RaiseSpecialAttack(Vector3 pos) => OnSpecialAttack?.Invoke(pos);
    public static void RaiseChargeAbsorbed(Vector3 pos) => OnChargeAbsorbed?.Invoke(pos);
    public static void RaiseEnemyDeath(Vector3 pos) => OnEnemyDeath?.Invoke(pos);

    public static void RaiseSpecialAuraChanged(bool isActive) => OnSpecialAuraChanged?.Invoke(isActive);
}
