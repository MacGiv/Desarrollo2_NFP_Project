using UnityEngine;

public class PlayerAttackState : PlayerGroundedState
{
    protected bool hasMoved = false;
    protected bool hasAttacked = false; 
    protected bool animationFinished = false;

    private bool attackBufferedDuringWindow = false;
    private bool inputBufferedBeforeWindow = false;

    public PlayerAttackState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName)
        : base(core, stateMachine, data, animName) { }

    public override void Enter()
    {
        base.Enter();
        core.Animator.SetInteger("comboStep", core.ComboHandler.CurrentComboIndex);

        hasMoved = false;
        animationFinished = false;
        attackBufferedDuringWindow = false;

        inputBufferedBeforeWindow = core.Input.AttackBufferedManually;
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (animationFinished)
        {
            if (attackBufferedDuringWindow && core.ComboHandler.CurrentComboIndex < core.Data.ComboMaxLength)
            {
                core.ComboHandler.AdvanceCombo();
                stateMachine.ChangeState(core.AttackState);
            }
            else if (core.Input.MovementInput.magnitude > 0.1f)
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
            Vector3 attackDir = core.DirectionResolver.GetAttackDirection();
            core.Movement.AttackMove(data.AttackMovementForce, attackDir);
            core.Movement.ApplyRotationInstant(attackDir);
            Debug.Log("Attack State Movement!");
        }
    }
    
    public override void Exit()
    {
        if(!attackBufferedDuringWindow)
        {
            base.Exit();
        }
    }

    public void NotifyAttackInputWindowStart()
    {
        if (core.ComboHandler.CurrentComboIndex < core.Data.ComboMaxLength)
        {
            if (inputBufferedBeforeWindow || core.Input.BufferedAttackPressed)
            {
                attackBufferedDuringWindow = true;
                core.Input.ResetAttackBuffer();
                core.Input.ConsumeManualAttackBuffer();
                Debug.Log("[Window] Buffered input consumed");
            }
        }
    }

    public void NotifyAttackAnimationAttack()
    {
        hasAttacked = true;
    }


    public void NotifyAttackAnimationEnded()
    {
        animationFinished = true;
        core.Animator.SetBool("attack", false);

        Debug.Log($"[Animation Ended] ComboIndex: {core.ComboHandler.CurrentComboIndex}, Buffered: {attackBufferedDuringWindow}");

        if (attackBufferedDuringWindow && core.ComboHandler.CurrentComboIndex < core.Data.ComboMaxLength+1)
        {
            core.ComboHandler.AdvanceCombo();
            stateMachine.ChangeState(core.AttackState); // Transición inmediata al siguiente ataque
        }
        else
        {
            core.ComboHandler.ResetCombo();
        }
    }


}
