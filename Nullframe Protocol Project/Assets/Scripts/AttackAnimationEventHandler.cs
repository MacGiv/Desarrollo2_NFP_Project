using UnityEngine;


/// <summary>
/// Handles animation evets fired from attacks
/// </summary>
public class AttackAnimationEventHandler : MonoBehaviour
{
    private PlayerCore core;

    private void Awake()
    {
        core = GetComponentInParent<PlayerCore>();
    }

    public void OnAttackAnimationEnd()
    {
        Debug.Log("Animation Ended");
        core.AttackState.NotifyAttackAnimationEnded();
    }
}
