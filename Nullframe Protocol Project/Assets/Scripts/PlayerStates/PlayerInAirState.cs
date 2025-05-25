using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private PlayerInputHandler inputHandler;
    private PlayerGroundChecker groundChecker;

    public PlayerInAirState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data) : base(core, stateMachine, data)
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
        core.Movement.Move(); // TODO: Limit the air movement control
        core.Movement.HoldJump(core.Input.JumpHeld); // Holded Jump
    }
}
