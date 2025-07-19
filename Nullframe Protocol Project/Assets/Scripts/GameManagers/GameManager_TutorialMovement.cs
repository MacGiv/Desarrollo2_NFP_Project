using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the tutorial flow for movement and jumping mechanics.
/// Automatically advances if the player reaches the final checkpoint early.
/// </summary>
public class GameManager_TutorialMovement : MonoBehaviour
{
    [Header("Tutorial Settings")]
    [SerializeField] private float delayBetweenMessages = 1.0f;
    [SerializeField] private string nextScene = "GameScene_2";
    [SerializeField] private float timeoutForActions = 60f;

    private enum TutorialStep
    {
        Movement,
        Jump,
        DoubleJump,
        PlatformCheckpoint,
        TrapCheckpoint,
        Completed
    }

    private TutorialStep _currentStep = TutorialStep.Movement;

    private PlayerInputHandler _input;
    private PlayerMovement _movement;

    private void OnEnable()
    {
        ResetTutorialFlags();
        TutorialCheckpoint.OnReached += OnCheckpointReached;
    }

    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();
        _movement = FindFirstObjectByType<PlayerMovement>();

        if (_input == null || _movement == null)
        {
            Debug.LogError("Tutorial could not find PlayerInputHandler or PlayerMovement.");
            return;
        }

        StartCoroutine(HandleTutorialSequence());
    }

    private void OnDisable()
    {
        TutorialCheckpoint.OnReached -= OnCheckpointReached;
    }

    private IEnumerator HandleTutorialSequence()
    {
        yield return ShowMessage("<<STARTING SYSTEM>>", 1.5f);
        yield return ShowMessage("Hello...", 1.5f);
        yield return ShowMessage("...you read me?", 1.5f);

        yield return ShowMessage("Try moving with: W A S D / Left Stick", 3f);
        yield return WaitForPlayerMoved();
        if (_currentStep == TutorialStep.Completed) yield break;

        yield return ShowMessage("Jump with SPACE / A Button", 3f);
        yield return WaitForPlayerJumped();
        if (_currentStep == TutorialStep.Completed) yield break;

        yield return ShowMessage("You can jump while mid-air. Try it.", 3f);
        yield return WaitForDoubleJump();
        if (_currentStep == TutorialStep.Completed) yield break;

        yield return ShowMessage("Good. Now advance through the platforms...", 2f);
        yield return ShowMessage("Exit should be near...", 2f);
        yield return WaitForReachedCheckpoint("Checkpoint_Platforms");
        if (_currentStep == TutorialStep.Completed) yield break;

        yield return ShowMessage("Avoid those pointy things they'll damage you", 2f);
        yield return WaitForReachedCheckpoint("Checkpoint_Traps");
        if (_currentStep == TutorialStep.Completed) yield break;

        yield return ShowMessage("Let's get to the next zone", 3f);
        yield return ShowMessage("<<LOADING NEXT SECTOR>>", 2f);

        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var sceneFlow))
        {
            sceneFlow.LoadSceneReplacing(nextScene);
        }
    }

    private void OnCheckpointReached(string tag)
    {
        if (tag == "Checkpoint_EndOfLevel" && _currentStep < TutorialStep.Completed)
        {
            StopAllCoroutines();
            StartCoroutine(SkipToEndEarly());
        }
    }

    private IEnumerator SkipToEndEarly()
    {
        _currentStep = TutorialStep.Completed;

        if (ServiceProvider.TryGetService<MessageHandlerUI>(out var handler))
        {
            yield return handler.ShowMessageForDuration("Okay, seems like you don't need my help that much. Good.", 2.5f);
            yield return handler.ShowMessageForDuration("<<LOADING NEXT SECTOR>>", 2f);
        }

        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var sceneFlow))
        {
            sceneFlow.LoadSceneReplacing(nextScene);
        }
    }

    private IEnumerator ShowMessage(string msg, float duration)
    {
        if (ServiceProvider.TryGetService<MessageHandlerUI>(out var handler))
        {
            yield return handler.ShowMessageForDuration(msg, duration);
        }
    }

    private IEnumerator WaitForPlayerMoved()
    {
        _currentStep = TutorialStep.Movement;
        float timer = 0f;
        bool hintShown = false;

        while (true)
        {
            if (_input.MovementInput.magnitude > 0.1f || _currentStep == TutorialStep.Completed)
            {
                if (hintShown)
                    HideHint();

                yield break;
            }

            timer += Time.deltaTime;

            if (timer > timeoutForActions && !hintShown)
            {
                ShowRestartHint("Can't move? Press [R]/[Select] to restart.");
                hintShown = true;
            }

            yield return null;
        }
    }

    private IEnumerator WaitForPlayerJumped()
    {
        _currentStep = TutorialStep.Jump;
        float timer = 0f;
        bool hintShown = false;

        while (true)
        {
            if (_movement.GetYVelocity() > 1f || _currentStep == TutorialStep.Completed)
            {
                if (hintShown)
                    HideHint();

                yield break;
            }

            timer += Time.deltaTime;

            if (timer > timeoutForActions && !hintShown)
            {
                ShowRestartHint("Jump not working? Press [R]/[Select] to restart.");
                hintShown = true;
            }

            yield return null;
        }
    }

    private IEnumerator WaitForDoubleJump()
    {
        _currentStep = TutorialStep.DoubleJump;
        float timer = 0f;
        bool hintShown = false;

        while (true)
        {
            if (_movement.JumpCount >= 2 || _currentStep == TutorialStep.Completed)
            {
                if (hintShown)
                    HideHint();

                yield break;
            }

            timer += Time.deltaTime;

            if (timer > timeoutForActions && !hintShown)
            {
                ShowRestartHint("Double jump failed? Press [R]/[Select] to restart.");
                hintShown = true;
            }

            yield return null;
        }
    }

    private IEnumerator WaitForReachedCheckpoint(string tag)
    {
        if (tag == "Checkpoint_Platforms")
            _currentStep = TutorialStep.PlatformCheckpoint;
        else if (tag == "Checkpoint_Traps")
            _currentStep = TutorialStep.TrapCheckpoint;

        float timer = 0f;
        bool hintShown = false;

        while (true)
        {
            if (TutorialCheckpoint.Reached(tag) || _currentStep == TutorialStep.Completed)
            {
                if (hintShown)
                    HideHint();

                yield break;
            }

            timer += Time.deltaTime;

            if (timer > timeoutForActions && !hintShown)
            {
                ShowRestartHint("Lost? Press [R]/[Select] to restart from beginning.");
                hintShown = true;
            }

            yield return null;
        }
    }


    private void ShowRestartHint(string message)
    {
        if (ServiceProvider.TryGetService<MessageHandlerUI>(out var handler))
        {
            handler.ShowPersistentMessage(message);
        }
    }

    private void HideHint()
    {
        if (ServiceProvider.TryGetService<MessageHandlerUI>(out var handler))
        {
            handler.HideMessage();
        }
    }


    private void ResetTutorialFlags()
    {
        TutorialCheckpoint.ResetAll();
    }
}
