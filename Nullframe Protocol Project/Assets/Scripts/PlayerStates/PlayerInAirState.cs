using UnityEngine;

public class PlayerInAirState : PlayerState
{
    public PlayerInAirState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName) : base(core, stateMachine, data, animName)
    { }

    public override void LogicUpdate()
    {
        // If Landed, change to IdleState
        if (core.GroundChecker.IsGrounded)
        {
            stateMachine.ChangeState(core.IdleState);
            return;
        }

        // If Jump Pressed, change to JumpState
        if (core.Input.JumpPressed && core.Movement.CanJump())
        {
            core.Input.UseJump();
            stateMachine.ChangeState(core.JumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        core.GroundChecker.CheckGround();
        core.Movement.UpdateGrounded(core.GroundChecker.IsGrounded);
        core.Movement.Move(); // TODO: Limit the air movement control
        core.Movement.HoldJump(core.Input.JumpHeld); // Holded Jump
    }
}
