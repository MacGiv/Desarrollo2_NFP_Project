using UnityEngine;
/// <summary>
/// Base for all grounded States
/// </summary>
public abstract class PlayerGroundedState : PlayerState
{
    protected PlayerGroundedState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName)
        : base(core, stateMachine, data, animName) { }

    public override void LogicUpdate()
    {
        // Checks if player left the ground
        if (!core.GroundChecker.IsGrounded)
        {
            stateMachine.ChangeState(core.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        core.GroundChecker.CheckGround();
        core.Movement.UpdateGrounded(core.GroundChecker.IsGrounded);
    }

}

