using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName) : base(core, stateMachine, data, animName)
    { }
    public override void Enter()
    {
        base.Enter();
        core.Movement.Jump();
    }

    public override void PhysicsUpdate()
    {
        if (core.Movement.GetYVelocity() <= 0f)
        {
            stateMachine.ChangeState(core.InAirState);
        }
        // Air movement and rotation while jumping up
        Vector3 moveDir = core.Movement.GetCameraRelativeInput();
        core.Movement.Move(moveDir);
        core.Movement.ApplyRotation(moveDir);
        core.Movement.HoldJump(core.Input.JumpHeld);
    }



}
