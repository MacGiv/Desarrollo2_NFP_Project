using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName)
        : base(core, stateMachine, data, animName) { }

    public override void Enter()
    {
        base.Enter();
        core.Animator.SetTrigger(animName);
        core.Movement.StopMovement();
        core.Input.enabled = false;
    }

    public override void LogicUpdate()
    {
        // TODO: Esperar animación, o mostrar pantalla de derrota desde otro sistema.
    }

    public override void Exit()
    {
        // Only override.
    }
}