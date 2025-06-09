using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName) : base(core, stateMachine, data, animName)
    { }

    public override void LogicUpdate()
    {
        // GroundCheck
        base.LogicUpdate();
        // Change to Jump
        if (core.Input.JumpPressed && core.Movement.CanJump())
        {
            core.Input.ResetJumpBuffer();
            stateMachine.ChangeState(core.JumpState);
            return;
        }

        // Change to Move
        if (core.Input.MovementInput.magnitude > 0.1f)
        {
            stateMachine.ChangeState(core.MoveState);
            return;
        }

        // Change to Attack
        if (core.Input.AttackPressed)
        {
            stateMachine.ChangeState(core.AttackState);
            return;
        }
    }

}
