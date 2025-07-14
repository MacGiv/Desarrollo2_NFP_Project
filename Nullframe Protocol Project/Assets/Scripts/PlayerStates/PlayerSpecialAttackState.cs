using UnityEngine;

public class PlayerSpecialAttackState : PlayerState
{
    private Transform _target;
    private bool _hasReachedTarget;

    public PlayerSpecialAttackState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName)
        : base(core, stateMachine, data, animName) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("[SpecialAtkState] Special Attack State ENTERED");
        _target = core.SpecialAttackHandler.FindSpecialTarget();

        if (_target == null)
        {
            stateMachine.ChangeState(core.IdleState);
            return;
        }

        Vector3 directionToTarget = (_target.position - core.transform.position).normalized;
        Vector3 stopPosition = _target.position - directionToTarget * data.SpecialAttackStopDistance;

        core.Movement.LookAtPosition(directionToTarget, true);
        core.Animator.SetTrigger("specialAttack");
        core.Movement.StartSpecialAttackMovement(stopPosition, OnReachedTarget);

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_hasReachedTarget)
        {
            Debug.Log("[SpecialAtkState] Target reached!");
            core.SpecialAttackHandler.PerformDamageOnTarget(_target);
            core.SpecialAttackChargeSystem.ConsumeCharges();
            stateMachine.ChangeState(core.IdleState);
        }
    }

    private void OnReachedTarget()
    {
        _hasReachedTarget = true;
    }
}
