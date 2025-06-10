using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatHandler : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private PlayerData playerData;
    
    // HashSet<> is a collection type that stores a set of unique elements.
    private HashSet<Collider> alreadyHitThisAttack = new HashSet<Collider>();

    private void OnEnable()
    {
        playerData = GetComponent<PlayerCore>().Data;
    }

    /// <summary>
    /// Method to be called at Attack animation's impact frame. 
    /// </summary>
    public void PerformAttack()
    {
        alreadyHitThisAttack.Clear();

        Vector3 origin = GetComponentInParent<Transform>().position; // Animation correction
        Vector3 direction = transform.forward * playerData.AttackRange;

        Collider[] hits = Physics.OverlapSphere(origin + direction * playerData.AttackRange, playerData.AttackRadius, enemyLayer);

        foreach (var hit in hits)
        {
            if (alreadyHitThisAttack.Contains(hit)) continue; // Skip to next iteration if hit target already was processed

            alreadyHitThisAttack.Add(hit);

            if (hit.TryGetComponent(out EnemyHealthSystem enemy))
            {
                enemy.TakeDamage(playerData.AttackDamage);
            }
        }
    }

    // Visual debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 origin = GetComponentInParent<Transform>().position;
        Vector3 direction = transform.forward * playerData.AttackRange;
        Gizmos.DrawWireSphere(origin + direction * playerData.AttackRange, playerData.AttackRadius);
    }
}
