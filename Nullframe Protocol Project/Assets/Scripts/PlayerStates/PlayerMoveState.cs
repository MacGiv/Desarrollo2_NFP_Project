using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data) : base(core, stateMachine, data)
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
        core.Movement.Move();
    }
}
