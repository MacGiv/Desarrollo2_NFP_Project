using UnityEngine;

/// <summary>
/// Resolves the direction the player should attack towards.
/// Prioritizes nearby enemies, falls back to camera direction.
/// </summary>
public class AttackDirectionResolver : MonoBehaviour
{
    [SerializeField] private float enemyDetectRadius = 5f;
    [SerializeField] private LayerMask enemyLayer;

    private Transform camTransform;

    private void Awake()
    {
        camTransform = Camera.main.transform;
    }

    public Vector3 GetAttackDirection()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, enemyDetectRadius, enemyLayer);
        if (hits.Length > 0)
        {
            // Get closest enemy
            Transform closest = hits[0].transform;
            float minDist = Vector3.Distance(transform.position, closest.position);

            foreach (var hit in hits)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = hit.transform;
                }
            }

            Vector3 dirToEnemy = (closest.position - transform.position).normalized;
            dirToEnemy.y = 0f;

            Debug.DrawLine(transform.position + Vector3.up, closest.position, Color.red, 1f);
            Debug.Log("Attacking enemy: " + closest.name);

            return dirToEnemy;
        }

        // Fallback to camera forward
        Vector3 camDir = camTransform.forward;
        camDir.y = 0f;

        Debug.DrawRay(transform.position + Vector3.up, camDir * 5f, Color.blue, 1f);

        return camDir.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, enemyDetectRadius);
    }

}
