using UnityEngine;

public class PlayerJumpState : PlayerGroundedState
{
    public PlayerJumpState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data) : base(core, stateMachine, data)
    { }

    public override void Enter()
    {
        base.Enter();

        core.Movement.Jump();
        stateMachine.ChangeState(core.InAirState);
    }

}
