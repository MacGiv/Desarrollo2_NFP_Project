using System;

public static class LockOnEvents
{
    public static event Action OnLockOnEnabled;
    public static event Action OnLockOnDisabled;

    public static void RaiseLockOnEnabled() => OnLockOnEnabled?.Invoke();
    public static void RaiseLockOnDisabled() => OnLockOnDisabled?.Invoke();
}
