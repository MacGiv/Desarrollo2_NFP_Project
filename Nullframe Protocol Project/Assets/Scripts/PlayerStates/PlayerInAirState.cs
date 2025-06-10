using UnityEngine;

public class PlayerInAirState : PlayerState
{
    public PlayerInAirState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName) : base(core, stateMachine, data, animName)
    { }

    public override void LogicUpdate()
    {
        // If Landed, change to IdleState
        core.Movement.UpdateGrounded(core.GroundChecker.CheckGround());
        if (core.GroundChecker.IsGrounded)
        {
            if (core.Input.MovementInput.magnitude > data.MoveInputThreshold)
            {
                stateMachine.ChangeState(core.MoveState);
            }
            else
            {
                stateMachine.ChangeState(core.IdleState);
            }
            return;
        }

        // If Jump Pressed, change to JumpState
        if (core.Input.JumpPressed && core.Movement.CanJump())
        {
            core.Input.ResetJumpBuffer();
            stateMachine.ChangeState(core.JumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        Vector3 moveDir = core.Movement.GetCameraRelativeInput();
        core.Movement.Move(moveDir); 
        core.Movement.ApplyRotation(moveDir);
        core.Movement.HoldJump(core.Input.JumpHeld);
    }
}
