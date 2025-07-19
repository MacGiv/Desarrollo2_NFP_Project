using System.Collections;
using UnityEngine;

/// <summary>
/// Handles tutorial flow for the Combat Tutorial level.
/// Explains attack, lock-on, charge system, and special attack usage.
/// Ends when the player reaches the high ground using special + double jump.
/// </summary>
public class GameManager_TutorialCombat : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeoutForActions = 60f;
    [SerializeField] private float delayBeforeNextScene = 2f;
    [SerializeField] private string nextScene = "GameScene_3";

    private enum TutorialStep
    {
        LockOn,
        SpecialAttack,
        ReachHighGround,
        End
    }

    private TutorialStep _currentStep = TutorialStep.LockOn;
    private bool _lockOnUsed = false;
    private bool _specialUsed = false;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");

        // Subscribe to inputs
        if (FindFirstObjectByType<PlayerInputHandler>() is PlayerInputHandler input)
        {
            input.OnToggleLockOn += OnLockOnUsed;
            input.OnSpecialAttackPressed += OnSpecialAttackUsed;
        }

        // Subscribe to checkpoints
        TutorialCheckpoint.OnReached += OnCheckpointReached;

        StartCoroutine(HandleTutorialSequence());
    }

    private IEnumerator HandleTutorialSequence()
    {
        yield return ShowMessage("<<SYSTEM ONLINE>>", 1.5f);
        yield return ShowMessage("Multiple hostiles detected.", 2f);
        yield return ShowMessage("Seems like they are deactivated, that's... convenient.", 2f);
        yield return ShowMessage("Attack them with [ Left Click ] / [ X Button ]", 3f);

        yield return WaitForLockOn();
        if (_currentStep == TutorialStep.End) yield break;

        yield return ShowMessage("Enemies give you energy when defeated.", 3f);
        yield return ShowMessage("Collect 3 to unlock a SPECIAL ATTACK.", 3f);

        yield return WaitForSpecialAttack();
        if (_currentStep == TutorialStep.End) yield break;

        yield return WaitForSecondSpecialAttack();
    }

    private IEnumerator WaitForLockOn()
    {
        _currentStep = TutorialStep.LockOn;
        float timer = 0f;

        ShowMessagePersistent("Try locking on to an enemy with: [ E ] / [ Left Trigger ]");

        while (timer < timeoutForActions && !_lockOnUsed)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        HideMessage();

        if (!_lockOnUsed)
        {
            ShowRestartHint("Lock-On failed? Press [R]/[Select] to restart.");
        }
    }

    private IEnumerator WaitForSpecialAttack()
    {
        _currentStep = TutorialStep.SpecialAttack;
        float timer = 0f;

        ShowMessagePersistent("Press [ Right Click ] / [ Y / Triangle ] to perform it.");

        while (timer < timeoutForActions && !_specialUsed)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        HideMessage();

        if (!_specialUsed)
        {
            ShowRestartHint("Try killing enemies to charge Special Attack. Press [R] to restart.");
        }

        _specialUsed = false;
    }

    private IEnumerator WaitForSecondSpecialAttack()
    {
        _currentStep = TutorialStep.ReachHighGround;
        float timer = 0f;

        var chargeSystem = _player?.GetComponent<SpecialAttackChargeSystem>();

        ShowMessagePersistent("Use double jump + special to reach the last enemy.");

        bool reachedHighPlatform = false;

        void CheckReachedHighGround(string tag)
        {
            if (tag == "Checkpoint_HighGround")
                reachedHighPlatform = true;
        }

        TutorialCheckpoint.OnReached += CheckReachedHighGround;

        while (timer < timeoutForActions && !reachedHighPlatform)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        TutorialCheckpoint.OnReached -= CheckReachedHighGround;
        HideMessage();

        if (!reachedHighPlatform)
        {
            ShowMessagePersistent("That thing seems like has something to do with energy try getting close.");

            // Esperar hasta que lo logre
            reachedHighPlatform = false;
            TutorialCheckpoint.OnReached += CheckReachedHighGround;

            yield return new WaitUntil(() => reachedHighPlatform);
            TutorialCheckpoint.OnReached -= CheckReachedHighGround;

            HideMessage();
        }

        _currentStep = TutorialStep.End;
        StartCoroutine(HandleVictory());
    }

    private IEnumerator HandleVictory()
    {
        yield return ShowMessage("Nice! I knew you'd figure it out.", 2f);
        yield return ShowMessage("<<LOADING NEXT SECTOR>>", 2f);

        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var sceneFlow))
        {
            sceneFlow.LoadSceneReplacing(nextScene);
        }
    }

    private void OnLockOnUsed()
    {
        _lockOnUsed = true;
        if (_currentStep == TutorialStep.LockOn)
            HideMessage();
    }

    private void OnSpecialAttackUsed()
    {
        _specialUsed = true;
        if (_currentStep is TutorialStep.SpecialAttack or TutorialStep.ReachHighGround)
            HideMessage();
    }

    private void OnCheckpointReached(string tag)
    {
        if (tag == "Checkpoint_HighGround" && _currentStep != TutorialStep.End)
        {
            _currentStep = TutorialStep.End;
            StopAllCoroutines();
            StartCoroutine(HandleVictory());
        }
    }

    private void GivePlayerEnergy()
    {
        var chargeSystem = _player?.GetComponent<SpecialAttackChargeSystem>();

        if (chargeSystem == null) return;

        while (!chargeSystem.CanUseSpecial)
        {
            chargeSystem.AddCharge();
        }
    }

    private IEnumerator ShowMessage(string msg, float duration)
    {
        if (ServiceProvider.TryGetService<MessageHandlerUI>(out var handler))
            yield return handler.ShowMessageForDuration(msg, duration);
    }

    private void ShowMessagePersistent(string msg)
    {
        if (ServiceProvider.TryGetService<MessageHandlerUI>(out var handler))
            handler.ShowPersistentMessage(msg);
    }

    private void HideMessage()
    {
        if (ServiceProvider.TryGetService<MessageHandlerUI>(out var handler))
            handler.HideMessage();
    }

    private void ShowRestartHint(string message)
    {
        if (ServiceProvider.TryGetService<MessageHandlerUI>(out var handler))
            handler.ShowPersistentMessage(message);
    }

    private void OnDisable()
    {
        TutorialCheckpoint.OnReached -= OnCheckpointReached;

        if (FindFirstObjectByType<PlayerInputHandler>() is PlayerInputHandler input)
        {
            input.OnToggleLockOn -= OnLockOnUsed;
            input.OnSpecialAttackPressed -= OnSpecialAttackUsed;
        }
    }
}
