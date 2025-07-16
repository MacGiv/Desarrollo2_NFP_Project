using System.Collections;
using UnityEngine;

/// <summary>
/// Handles tutorial flow for the Combat Tutorial level.
/// Explains basic attack, lock-on, charge system, and special attack usage.
/// Ends when all enemies are defeated.
/// </summary>
public class GameManager_TutorialCombat : MonoBehaviour
{
    [SerializeField] private float delayBeforeNextScene = 2f;
    [SerializeField] private string nextScene = "GameScene_3";

    private int _enemiesRemaining;
    private bool _lockOnUsed = false;
    private bool _specialUsed = false;

    private void Start()
    {
        _enemiesRemaining = FindObjectsByType<EnemyHealthSystem>(FindObjectsSortMode.None).Length;

        StartCoroutine(StartTutorialSequence());

        // Subscribe to all enemies' OnDeath event
        foreach (var enemy in FindObjectsByType<EnemyHealthSystem>(FindObjectsSortMode.None))
        {
            enemy.OnDeath += OnEnemyDied;
        }

        // Subscribe to lock-on and special attack events
        if (FindFirstObjectByType<PlayerInputHandler>() is PlayerInputHandler input)
        {
            input.OnToggleLockOn += OnLockOnUsed;
            input.OnSpecialAttackPressed += OnSpecialAttackUsed;
        }
    }

    private IEnumerator StartTutorialSequence()
    {
        yield return ShowMessage("<<SYSTEM ONLINE>>", 1.5f);
        yield return ShowMessage("Multiple hostiles detected.", 2f);
        yield return ShowMessage("Seems like they are deactivated, that's... convenient.", 2f);
        yield return ShowMessage("Attack them with [ Left Click ] / [ X Button ]", 3f);
        ShowMessagePersistent("Try locking on to an enemy with: [ E ] / [ Left Trigger ]");
        yield return new WaitUntil(() => _lockOnUsed);
        HideMessage();

        yield return ShowMessage("Enemies give you energy when defeated.", 3f);
        yield return ShowMessage("Collect 3 to unlock a SPECIAL ATTACK.", 3f);
        ShowMessagePersistent("Press [ Right Click ] / [ Y / Triangle ] to perform it.");
        yield return new WaitUntil(() => _specialUsed);
        HideMessage();
        _specialUsed = false;
        ShowMessagePersistent("Use double jump + special to reach the last enemy.");
        yield return new WaitUntil(() => _specialUsed);
        HideMessage();
        yield return ShowMessage("Eliminate all targets.", 2f);
    }

    private IEnumerator ShowMessage(string msg, float duration)
    {
        if (ServiceProvider.TryGetService<MessageHandlerUI>(out var handler))
        {
            yield return handler.ShowMessageForDuration(msg, duration);
        }
    }

    private void ShowMessagePersistent(string msg)
    {
        if (ServiceProvider.TryGetService<MessageHandlerUI>(out var handler))
        {
            handler.ShowPersistentMessage(msg);
        }
    }

    private void HideMessage()
    {
        if (ServiceProvider.TryGetService<MessageHandlerUI>(out var handler))
        {
            handler.HideMessage();
        }
    }

    private void OnEnemyDied(Transform enemy)
    {
        _enemiesRemaining--;

        if (_enemiesRemaining <= 0)
        {
            StartCoroutine(HandleVictory());
        }
    }

    private void OnLockOnUsed()
    {
        _lockOnUsed = true;
    }

    private void OnSpecialAttackUsed()
    {
        _specialUsed = true;
    }

    private IEnumerator HandleVictory()
    {
        yield return ShowMessage("Target area cleared.", 2f);
        yield return ShowMessage("Let me try get you out of here.", 2f);
        yield return ShowMessage("<<LOADING NEXT SECTOR>>", 2f);

        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var sceneFlow))
        {
            sceneFlow.LoadSceneReplacing(nextScene);
        }
    }

    private void OnDisable()
    {
        foreach (var enemy in FindObjectsByType<EnemyHealthSystem>(FindObjectsSortMode.None))
        {
            enemy.OnDeath -= OnEnemyDied;
        }

        if (FindFirstObjectByType<PlayerInputHandler>() is PlayerInputHandler input)
        {
            input.OnToggleLockOn -= OnLockOnUsed;
            input.OnSpecialAttackPressed -= OnSpecialAttackUsed;
        }
    }
}
