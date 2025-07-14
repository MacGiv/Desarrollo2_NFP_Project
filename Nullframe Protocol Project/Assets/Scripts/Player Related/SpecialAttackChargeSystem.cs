using UnityEngine;
using System;

public class SpecialAttackChargeSystem : MonoBehaviour
{
    [SerializeField] private int maxCharges = 3;
    private int currentCharges;

    public int CurrentCharges => currentCharges;
    public bool CanUseSpecial => currentCharges >= maxCharges;

    public event Action<int, int> OnChargeChanged;

    private void OnEnable()
    {
        EnemyEvents.OnEnemyKilled += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        EnemyEvents.OnEnemyKilled -= HandleEnemyKilled;
    }

    private void Awake()
    {
        currentCharges = 3;
        NotifyChange();
    }

    public void AddCharge()
    {
        currentCharges = Mathf.Clamp(currentCharges + 1, 0, maxCharges);
        NotifyChange();
    }

    public void ConsumeCharges()
    {
        currentCharges = 0;
        NotifyChange();
    }

    private void NotifyChange() => OnChargeChanged?.Invoke(currentCharges, maxCharges);

    /// <summary>
    /// Adds one charge when an enemy is killed.
    /// </summary>
    private void HandleEnemyKilled()
    {
        AddCharge();
    }
}
