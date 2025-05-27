using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName) : base(core, stateMachine, data, animName)
    { }

    public override void Enter()
    {
        base.Enter();

        core.Movement.Jump();
        stateMachine.ChangeState(core.InAirState);
    }

}
