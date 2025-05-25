using UnityEngine;
/// <summary>
/// Base for all grounded States
/// </summary>
public abstract class PlayerGroundedState : PlayerState
{
    protected PlayerGroundedState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data)
        : base(core, stateMachine, data) { }

    public override void LogicUpdate()
    {
        // Checks if player left the ground
        if (!core.GroundChecker.IsGrounded)
        {
            stateMachine.ChangeState(core.InAirState);
        }
    }

}

