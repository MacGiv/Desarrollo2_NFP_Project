using System.Collections;
using UnityEngine;

public class GameManager_TutorialCombat : MonoBehaviour
{
    [SerializeField] private TutorialMessageUI tutorialUI;
    [SerializeField] private float delayBeforeNextScene = 2f;
    [SerializeField] private string nextScene = "GameScene_3";

    private int _enemiesRemaining;

    private void Start()
    {
        // Count start enemies
        _enemiesRemaining = FindObjectsByType<EnemyHealthSystem>(FindObjectsSortMode.None).Length;

        StartCoroutine(StartTutorialSequence());

        // Subscribe to OnDeath enemie's action 
        foreach (var enemy in FindObjectsByType<EnemyHealthSystem>(FindObjectsSortMode.None))
        {
            enemy.OnDeath += OnEnemyDied;
        }
    }

    private IEnumerator StartTutorialSequence()
    {
        yield return ShowMessage("<<SYSTEM ONLINE>>", 1.5f);
        yield return ShowMessage("Multiple hostiles detected.", 2f);
        yield return ShowMessage("Attack with [ Right Click ] / [ A Button ]", 3f);
        yield return ShowMessage("Eliminate them all.", 2f);
    }

    private IEnumerator ShowMessage(string msg, float duration)
    {
        tutorialUI.ShowMessage(msg);
        yield return new WaitForSeconds(duration);
        tutorialUI.HideMessage();
    }

    private void OnEnemyDied()
    {
        _enemiesRemaining--;

        if (_enemiesRemaining <= 0)
        {
            StartCoroutine(HandleVictory());
        }
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
    }
}
