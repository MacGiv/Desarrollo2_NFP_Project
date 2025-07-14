using System;

/// <summary>
/// Global event broadcaster for enemy-related events.
/// </summary>
public static class EnemyEvents
{
    public static event Action OnEnemyKilled;

    public static void RaiseEnemyKilled()
    {
        OnEnemyKilled?.Invoke();
    }
}
