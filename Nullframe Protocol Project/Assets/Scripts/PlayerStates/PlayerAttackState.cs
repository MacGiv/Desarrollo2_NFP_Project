using UnityEngine;

public class PlayerAttackState : PlayerGroundedState
{
    protected bool hasMoved = false;
    protected bool hasAttacked = false; 
    protected bool animationFinished = false;

    private bool canAcceptNextAttack = false;
    private bool attackBufferedDuringWindow = false;

    public PlayerAttackState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName)
        : base(core, stateMachine, data, animName) { }

    public override void Enter()
    {
        base.Enter();
        core.Animator.SetInteger("comboStep", core.ComboHandler.CurrentComboIndex);

        hasMoved = false;
        animationFinished = false;
        canAcceptNextAttack = false;
        attackBufferedDuringWindow = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        // Change state if animation is finished
        if (animationFinished)
        {
            Debug.Log("Animation finished! " + animationFinished);
            if (attackBufferedDuringWindow && core.ComboHandler.CurrentComboIndex < core.Data.ComboMaxLength)
            {
                core.Input.ResetAttackBuffer();
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
            core.Movement.AttackMove(data.AttackMovementForce, core.transform.forward);
            // Debug.Log("Attack State Movement!");
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
            canAcceptNextAttack = true;
            attackBufferedDuringWindow = false;

            if (core.Input.BufferedAttackPressed)
            {
                attackBufferedDuringWindow = true;
                core.Input.ResetAttackBuffer();
                Debug.Log("[Window] Attack buffered dentro de ventana!");
            }
        }
    }

    public void NotifyAttackAnimationAttack()
    {
        hasAttacked = true;

        if (canAcceptNextAttack && core.Input.NewAttackInput)
        {
            attackBufferedDuringWindow = true;
            core.Input.ResetAttackBuffer(); // Avoid multiple inputs
        }

    }

    public void NotifyAttackAnimationEnded()
    {
        animationFinished = true;
        core.Animator.SetBool("attack", false);

        Debug.Log("[Attack Animation Ended] ComboIndex: " + core.ComboHandler.CurrentComboIndex + "Buffered: " + attackBufferedDuringWindow);

        if (attackBufferedDuringWindow && core.ComboHandler.CurrentComboIndex < core.Data.ComboMaxLength)
        {
            //core.ComboHandler.AdvanceCombo();
            Debug.Log("[AdvanceCombo] ComboIndex is now: " + core.ComboHandler.CurrentComboIndex);
        }
        else
        {
            Debug.Log("[Combo Reset]");
            core.ComboHandler.ResetCombo();
        }
    }

}
