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
            core.Input.ResetJumpBuffer();
            stateMachine.ChangeState(core.JumpState);
            return;
        }

        // If stopped moving, change to IdleState
        if (core.Input.MovementInput.magnitude <= 0.1f)
        {
            stateMachine.ChangeState(core.IdleState);
        }

        // If attack pressed Change to Attack 
        if (core.Input.AttackPressed)
        {
            stateMachine.ChangeState(core.AttackState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Vector3 moveDir = core.Movement.GetCameraRelativeInput();
        core.Movement.Move(moveDir);
        core.Movement.ApplyRotation(moveDir);
    }
}
