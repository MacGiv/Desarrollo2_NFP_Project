using UnityEngine;

public class PlayerHurtState : PlayerState
{
    private float hurtDuration = 1.0f;
    private float timer;

    public PlayerHurtState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName)
        : base(core, stateMachine, data, animName) { }

    public override void Enter()
    {
        timer = hurtDuration;
        core.Animator.SetTrigger(animName);
        core.Movement.HurtMove(core.Data.ReceiveHitForce, core.transform.forward);
        
    }

    public override void LogicUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            stateMachine.ChangeState(core.IdleState);
        }
    }

    public override void Exit()
    {
        // Only override.
    }
}