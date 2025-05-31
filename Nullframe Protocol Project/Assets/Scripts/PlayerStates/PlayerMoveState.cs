using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName) : base(core, stateMachine, data, animName)
    { }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // If jump pressed, change to JumpState
        if (core.Input.JumpPressed && core.Movement.CanJump())
        {
            core.Input.UseJump();
            stateMachine.ChangeState(core.JumpState);
            return;
        }

        // If stopped moveing, change to IdleState
        if (core.Input.MovementInput.magnitude <= 0.1f)
        {
            stateMachine.ChangeState(core.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        core.Movement.Move();
    }
}
