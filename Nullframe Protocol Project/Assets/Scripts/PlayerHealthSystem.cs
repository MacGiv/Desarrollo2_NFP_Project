using UnityEngine;
using System;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    private bool isGodMode = false;
    private bool isDead = false;

    public int MaxHealth => maxHealth;

    // Delegate to notify damage
    public event Action<int, int> OnHealthChanged; // (current, max)
    public event Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        NotifyHealthChanged();
    }

    public void TakeDamage(int amount)
    {
        if (!isGodMode && !isDead)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            

            NotifyHealthChanged();

            if (currentHealth <= 0)
            {
                Die();
            }
            else if (TryGetComponent<PlayerCore>(out var core))
            {
                core.StateMachine.ChangeState(core.HurtState);
            }
        }
    }

    public void Heal(int amount)
    {
        if (!isDead)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            NotifyHealthChanged();
        }
    }

    public void ToggleGodMode()
    {
        isGodMode = !isGodMode;
    }

    private void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
        if (TryGetComponent<PlayerCore>(out var core))
        {
            core.StateMachine.ChangeState(core.DeathState);
        }
        // TODO: Reiniciar escena, mostrar pantalla de derrota, etc.
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
