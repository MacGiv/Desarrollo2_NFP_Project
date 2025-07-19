using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static system that keeps track of tutorial checkpoint activations and raises events.
/// </summary>
public static class TutorialCheckpoint
{
    private static HashSet<string> reachedCheckpoints = new HashSet<string>();

    /// <summary>
    /// Event raised when a checkpoint is reached.
    /// </summary>
    public static event Action<string> OnReached;

    /// <summary>
    /// Marks the checkpoint as reached and notifies listeners.
    /// </summary>
    public static void RegisterReached(string tag)
    {
        if (reachedCheckpoints.Contains(tag))
            return;

        reachedCheckpoints.Add(tag);
        Debug.Log($"[Checkpoint] Reached: {tag}");

        OnReached?.Invoke(tag);
    }

    /// <summary>
    /// Checks whether a checkpoint has already been reached.
    /// </summary>
    public static bool Reached(string tag)
    {
        return reachedCheckpoints.Contains(tag);
    }

    /// <summary>
    /// Clears all reached checkpoint data.
    /// </summary>
    public static void ResetAll()
    {
        reachedCheckpoints.Clear();
    }
}
