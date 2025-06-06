using UnityEngine;

public class PlayerAttackState : PlayerGroundedState
{
    protected bool hasMoved = false;
    private bool animationFinished = false;

    public PlayerAttackState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName)
        : base(core, stateMachine, data, animName) { }

    public override void Enter()
    {
        base.Enter();
        hasMoved = false;
        animationFinished = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Change state if animation is finished
        if (animationFinished)
        {
            if (core.Input.MovementInput.magnitude > 0.1f)
                stateMachine.ChangeState(core.MoveState);
            else
                stateMachine.ChangeState(core.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // Apply movement only once
        if (!hasMoved)
        {
            hasMoved = true;
            core.Movement.AttackMove(data.AttackMovementForce, core.Movement.GetCameraRelativeInput());
        }
    }

    public void NotifyAttackAnimationEnded()
    {
        animationFinished = true;
        Debug.Log("Attack State Notified!");
    }
}
