using UnityEngine;

public class PlayerAttackState : PlayerGroundedState
{
    protected bool hasMoved = false;
    protected bool hasAtttacked = false;
    protected bool animationFinished = false;

    public PlayerAttackState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName)
        : base(core, stateMachine, data, animName) { }

    public override void Enter()
    {
        base.Enter();

        core.ComboHandler.AdvanceCombo();
        core.Animator.SetFloat("comboTracker", core.ComboHandler.CurrentComboIndex);

        hasMoved = false;
        animationFinished = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Change state if animation is finished
        if (animationFinished)
        {
            // TODO: attack input buffer
            // if (core.Input.AttackPressed)
            // {
            //     stateMachine.ChangeState(core.AttackState);
            // }
            // else 
            if (core.Input.MovementInput.magnitude > 0.1f)
            {
                stateMachine.ChangeState(core.MoveState);
            }
            else
            {
                stateMachine.ChangeState(core.IdleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // Apply movement only once
        if (!hasMoved)
        {
            hasMoved = true;
            core.Movement.AttackMove(data.AttackMovementForce, core.transform.forward);
            // Debug.Log("Attack State Movement!");
        }
    }

    public void NotifyAttackAnimationEnded()
    {
        animationFinished = true;
        if (core.ComboHandler.CurrentComboIndex >= core.Data.ComboMaxLength)
        {
            core.ComboHandler.ResetCombo();
        }
    }

    public void NotifyAttackAnimationAttack() 
    {
        hasAtttacked = true;
    }
}
