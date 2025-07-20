using UnityEngine;

public class LockOnHandler : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float lockOnRadius = 10f;
    [Header("Offset")]
    [SerializeField] private float lockOnYOffset = 10f;
    [Header("Cached")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform cameraTransform;

    private Transform _currentTarget;
    private bool _isLockedOn = false;

    public Transform CurrentTarget => _currentTarget;
    public bool IsLockedOn => _isLockedOn;
    public float LockOnYOffset => lockOnYOffset;
    public void ToggleLockOn()
    {
        if (_isLockedOn)
        {
            _isLockedOn = false;
            _currentTarget = null;
            LockOnEvents.RaiseLockOnDisabled();
        }
        else
        {
            TryFindTarget();
        }

        Debug.Log("[LockOnHandler] Lock On Vision Toggled!");
    }

    private void TryFindTarget()
    {
        _currentTarget = null;
        _isLockedOn = false;

        Collider[] hits = Physics.OverlapSphere(transform.position, lockOnRadius, enemyLayer);
        float closest = Mathf.Infinity;

        foreach (var hit in hits)
        {
            Vector3 toEnemy = hit.transform.position - cameraTransform.position;
            float dot = Vector3.Dot(cameraTransform.forward, toEnemy.normalized);

            if (dot > 0.5f)
            {
                float dist = toEnemy.sqrMagnitude;
                if (dist < closest)
                {
                    _currentTarget = hit.transform;
                    closest = dist;
                }
            }
        }

        if (_currentTarget != null)
        {
            _isLockedOn = true;
            LockOnEvents.RaiseLockOnEnabled();

            if (_currentTarget.TryGetComponent(out EnemyHealthSystem enemyHp))
                enemyHp.OnDeath += OnLockedEnemyDestroyed;
        }
    }

    private void OnLockedEnemyDestroyed(Transform enemy)
    {
        if (_currentTarget == enemy)
        {
            _currentTarget = null;
            _isLockedOn = false;
            LockOnEvents.RaiseLockOnDisabled();
            Debug.Log("[LockOnHandler] Lock Off (Target destroyed via event)");
        }
    }


    public void ForceUnlock()
    {
        _isLockedOn = false;
        _currentTarget = null;
        LockOnEvents.RaiseLockOnDisabled();
        Debug.Log("[LockOnHandler] Force Lock Off");
    }

}
