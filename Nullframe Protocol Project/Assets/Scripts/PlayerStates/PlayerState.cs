using UnityEngine;

/// <summary>
/// Player State Base, other States are going to inherit from this one
/// Abstract class because is not going to be instantiated, it serves only as a base/template class
/// </summary>
public abstract class PlayerState
{
    protected PlayerCore core;
    protected PlayerStateMachine stateMachine;
    protected PlayerData data;
    protected string animName;

    public PlayerState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data, string animName)
    {
        this.core = core;
        this.stateMachine = stateMachine;
        this.data = data;
        this.animName = animName;
    }

    public virtual void Enter() 
    { 
        core.Animator.SetBool(animName, true);
    }

    public virtual void Exit() 
    {
        core.Animator.SetBool(animName, false);
    }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() { }
}