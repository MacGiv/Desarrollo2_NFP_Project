using UnityEngine;
using System.Collections;

/// <summary>
/// Handles logic for general levels (non-tutorial).
/// Controls progression, victory conditions, and communicates with the narrator.
/// </summary>
public class GameManager_Level : MonoBehaviour
{
    [SerializeField] private string nextScene = "GameScene_4";

    private NarratorManager _narrator;
    private int _remainingEnemies;

    private void Start()
    {
        _narrator = FindFirstObjectByType<NarratorManager>();

        EnemyHealthSystem[] enemies = FindObjectsByType<EnemyHealthSystem>(FindObjectsSortMode.None);
        _remainingEnemies = enemies.Length;

        foreach (var enemy in enemies)
        {
            enemy.OnDeath += OnEnemyDied;
        }

        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        _narrator?.Say("<<RECONNECTING SYSTEM>>", 1.5f);
        yield return new WaitForSeconds(1f);

        _narrator?.Say("I'm pretty sure that purple thing is no good.", 3f);
        yield return new WaitForSeconds(3f);

        _narrator?.Say("Use everything you learned. Movement. Lock-ons. Special strikes.", 3f);
        yield return new WaitForSeconds(3f);

        _narrator?.SayPersistent("Find a way up. That last target’s high ground.");
    }

    private void OnEnemyDied(Transform _)
    {
        _remainingEnemies--;

        if (_remainingEnemies <= 0)
        {
            StartCoroutine(VictorySequence());
        }
    }

    private IEnumerator VictorySequence()
    {
        _narrator?.StopTalking();
        _narrator?.Say("Nice work. All targets neutralized.", 2f);
        yield return new WaitForSeconds(2f);

        _narrator?.Say("Opening the path ahead...", 2f);
        yield return new WaitForSeconds(2f);

        if (ServiceProvider.TryGetService(out SceneFlowHandler sceneFlow))
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
    }
}
