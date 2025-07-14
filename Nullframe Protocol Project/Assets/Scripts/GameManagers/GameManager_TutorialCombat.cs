using System.Collections;
using UnityEngine;

/// <summary>
/// Handles tutorial flow for the Combat Tutorial level.
/// Explains basic attack, lock-on, charge system, and special attack usage.
/// Ends when all enemies are defeated.
/// </summary>
public class GameManager_TutorialCombat : MonoBehaviour
{
    [SerializeField] private TutorialMessageUI tutorialUI;
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
        yield return ShowMessage("Attack with [ Left Click ] / [ X Button ]", 3f);
        yield return ShowMessage("Try locking on to an enemy: [ E ] / [ Left Trigger ]", 3f);
        yield return new WaitUntil(() => _lockOnUsed);

        yield return ShowMessage("Enemies give you energy when defeated.", 2.5f);
        yield return ShowMessage("Collect 3 to unlock a SPECIAL ATTACK.", 2.5f);
        yield return ShowMessage("Press [ Right Click ] / [ Y / Triangle ] to perform it.", 3f);
        yield return new WaitUntil(() => _specialUsed);

        yield return ShowMessage("Use double jump + special to reach the last enemy.", 3f);
        yield return ShowMessage("Eliminate all targets.", 2f);
    }

    private IEnumerator ShowMessage(string msg, float duration)
    {
        tutorialUI.ShowMessage(msg);
        yield return new WaitForSeconds(duration);
        tutorialUI.HideMessage();
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
