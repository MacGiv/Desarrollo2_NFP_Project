using UnityEngine;
using System.Collections;

/// <summary>
/// Handles logic for general levels (non-tutorial).
/// Progression is based on reaching the final checkpoint.
/// </summary>
public class GameManager_Level : MonoBehaviour
{
    [SerializeField] private string nextScene = "GameScene_4";

    private NarratorManager _narrator;

    private bool _levelCompleted = false;

    private void OnEnable()
    {
        TutorialCheckpoint.OnReached += OnCheckpointReached;
    }

    private void Start()
    {
        _narrator = FindFirstObjectByType<NarratorManager>();
        TutorialCheckpoint.ResetAll();

        StartCoroutine(IntroSequence());
    }

    private void OnDisable()
    {
        TutorialCheckpoint.OnReached -= OnCheckpointReached;
    }

    /// <summary>
    /// Called when any checkpoint is reached. We only care about the end.
    /// </summary>
    private void OnCheckpointReached(string tag)
    {
        if (_levelCompleted) return;

        if (tag == "Checkpoint_EndOfLevel")
        {
            _levelCompleted = true;
            StartCoroutine(VictorySequence());
        }
    }

    /// <summary>
    /// Intro dialogue for the level.
    /// </summary>
    private IEnumerator IntroSequence()
    {
        _narrator?.Say("<<RECONNECTING SYSTEM>>", 1.5f);
        yield return new WaitForSeconds(1.5f);

        _narrator?.Say("Move forward, be careful with the pointy things.", 3f);
        yield return new WaitForSeconds(3f);

        _narrator?.Say("Use everything you learned. Movement. Lock-ons. Special strikes.", 3f);
        yield return new WaitForSeconds(3f);

        _narrator?.SayPersistent("Find a way up. That last target’s high ground.");
    }

    /// <summary>
    /// Triggers when player reaches final area.
    /// </summary>
    private IEnumerator VictorySequence()
    {
        _narrator?.StopTalking();
        _narrator?.Say("Nice work.", 2f);
        yield return new WaitForSeconds(2f);

        _narrator?.Say("Opening the path ahead...", 2f);
        yield return new WaitForSeconds(2f);

        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var sceneFlow))
        {
            sceneFlow.LoadSceneReplacing(nextScene);
        }
    }
}
