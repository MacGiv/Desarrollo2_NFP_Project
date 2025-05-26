using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SerializeField] private int currentHealth;
    private bool isGodMode = false;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (!isGodMode || !isDead)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            // Debug.Log("Player took damage. Amount: " + amount);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Heal(int amount)
    {
        if (!isDead)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }

    public void ToggleGodMode()
    {
        isGodMode = !isGodMode;
    }

    private void Die()
    {
        isDead = true;
        // Debug.Log("Player has died.");
        // TODO: Reiniciar escena
    }
}
