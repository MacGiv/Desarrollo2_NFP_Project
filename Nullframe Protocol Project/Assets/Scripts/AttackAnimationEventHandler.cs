using UnityEngine;

/// <summary>
/// Handles animation events fired from attacks
/// </summary>
public class AttackAnimationEventHandler : MonoBehaviour
{
    private PlayerCore core;
    private PlayerCombatHandler combatHandler;

    private void Awake()
    {
        core = GetComponentInParent<PlayerCore>();
        combatHandler = core.GetComponent<PlayerCombatHandler>();
    }

    public void OnAttackAnimationEnd()
    {
        core.AttackState.NotifyAttackAnimationEnded();
    }

    public void OnAttackFrame()
    {
        combatHandler.PerformAttack();
    }

    public void OnAttackInputWindowStart()
    {
        core.AttackState.NotifyAttackInputWindowStart();
    }

}