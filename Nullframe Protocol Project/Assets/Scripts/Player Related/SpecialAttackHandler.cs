using UnityEngine;

public class SpecialAttackHandler : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    
    private float _range = 15f;

    private void Awake()
    {
        _range = GetComponent<PlayerCore>().Data.SpecialAttackRange;
    }

    public Transform FindSpecialTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _range, enemyLayer);

        float minDist = Mathf.Infinity;
        Transform target = null;

        foreach (var hit in hits)
        {
            float dist = (hit.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                target = hit.transform;
            }
        }

        return target;
    }

    public void PerformDamageOnTarget(Transform target)
    {
        if (target == null) return;

        if (target.TryGetComponent<EnemyHealthSystem>(out var health))
        {
            health.TakeDamage(GetComponent<PlayerCore>().Data.SpecialAttackDamage);
        }
    }

    public bool HasValidTarget()
    {
        return FindSpecialTarget() != null;
    }

}
