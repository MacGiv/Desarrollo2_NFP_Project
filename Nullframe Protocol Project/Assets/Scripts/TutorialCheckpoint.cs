using System.Collections.Generic;
using UnityEngine;

public class TutorialCheckpoint : MonoBehaviour
{
    [SerializeField] private string checkpointTag;

    private static HashSet<string> reachedCheckpoints = new();

    public static bool Reached(string tag)
    {
        return reachedCheckpoints.Contains(tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            reachedCheckpoints.Add(checkpointTag);
            Debug.Log("[TutorialCheckpoint] Alcanzado: " + checkpointTag);
        }

        gameObject.SetActive(false);
    }
}
