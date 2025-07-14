using UnityEngine;
using System;

public class EnemyHealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public event Action<Transform> OnDeath;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("[" + gameObject.name + "] took " + amount + " damage. Remaining: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        OnDeath?.Invoke(transform);
        EnemyEvents.RaiseEnemyKilled();
        ParticleEvents.RaiseEnemyDeath(gameObject.transform.position);
        AudioEvents.RaiseEnemyDeath(gameObject.transform.position);  
        //TODO: Animation
        Destroy(gameObject);
    }
}
