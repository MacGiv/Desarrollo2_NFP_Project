using UnityEngine;
using System;
using System.Collections;

public class GameManager_TutorialMovement : MonoBehaviour
{
    [SerializeField] private TutorialMessageUI tutorialUI;
    [SerializeField] private float delayBetweenMessages = 1.0f;
    [SerializeField] private string nextScene = "GameScene_2";

    private int currentStep = 0;

    private void Start()
    {
        StartCoroutine(HandleTutorialSequence());
    }

    private IEnumerator HandleTutorialSequence()
    {
        yield return ShowMessage("<<STARTING SYSTEM>>", 1.5f);
        yield return ShowMessage("Hello...", 1.5f);
        yield return ShowMessage("...you read me?", 1.5f);
        yield return ShowMessage("Try moving with: W A S D / Left Stick", 3f);
        yield return WaitForPlayerMoved();

        yield return ShowMessage("Jump with SPACE / A Button", 3f);
        yield return WaitForPlayerJumped();

        yield return ShowMessage("You can jump while mid-air. Try it.", 3f);
        yield return WaitForDoubleJump();

        yield return ShowMessage("Good. Now advance through the platforms...", 2f);
        yield return ShowMessage("Exit should be near...", 2f);
        yield return WaitForReachedCheckpoint("Checkpoint_Platforms");

        yield return ShowMessage("Avoid those lasers... be careful", 2f);
        yield return WaitForReachedCheckpoint("Checkpoint_Traps");

        yield return ShowMessage("Let's get to the next zone", 3f);
        yield return ShowMessage("<<LOADING NEXT SECTOR>>", 2f);

        // Proceed to next level
        if (ServiceProvider.TryGetService<SceneFlowHandler>(out var sceneFlow))
        {
            sceneFlow?.LoadSceneReplacing(nextScene);
        }
    }

    private IEnumerator ShowMessage(string text, float duration)
    {
        tutorialUI.ShowMessage(text);
        yield return new WaitForSeconds(duration);
        tutorialUI.HideMessage();
        yield return new WaitForSeconds(delayBetweenMessages);
    }

    // Coroutines to check tutorial's action triggers
    private IEnumerator WaitForPlayerMoved() { yield return new WaitUntil(() => PlayerHasMoved()); }
    private IEnumerator WaitForPlayerJumped() { yield return new WaitUntil(() => PlayerHasJumped()); }
    private IEnumerator WaitForDoubleJump() { yield return new WaitUntil(() => PlayerDidDoubleJump()); }
    private IEnumerator WaitForReachedCheckpoint(string tag) { yield return new WaitUntil(() => CheckpointReached(tag)); }

    // Methods to conect with inputs and triggers with tutorial's coroutines
    private bool PlayerHasMoved() => FindFirstObjectByType<PlayerInputHandler>()?.MovementInput.magnitude > 0.1f;
    private bool PlayerHasJumped() => FindFirstObjectByType<PlayerMovement>()?.GetYVelocity() > 1f; 
    private bool PlayerDidDoubleJump() => FindFirstObjectByType<PlayerMovement>()?.JumpCount >= 2;
    private bool CheckpointReached(string tag) => TutorialCheckpoint.Reached(tag);
}
