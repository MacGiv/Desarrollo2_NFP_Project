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

    public PlayerState(PlayerCore core, PlayerStateMachine stateMachine, PlayerData data)
    {
        this.core = core;
        this.stateMachine = stateMachine;
        this.data = data;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() { }
}