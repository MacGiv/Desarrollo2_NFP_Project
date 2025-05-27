using UnityEngine;

/// <summary>
/// Controls the current State of the Player and transitions to different states.
/// </summary>
public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState.LogicUpdate();
    }

    public void FixedUpdate()
    {
        CurrentState.PhysicsUpdate();
    }
}
