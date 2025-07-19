using UnityEngine;

/// <summary>
/// Attach this to a trigger collider. It will notify the static TutorialCheckpoint system when the player enters it.
/// </summary>
public class CheckpointTrigger : MonoBehaviour
{
    [Tooltip("Name of the checkpoint to report to the tutorial system.")]
    [SerializeField] private string checkpointTag = "Checkpoint_Platforms";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialCheckpoint.RegisterReached(checkpointTag);
        }
    }
}
