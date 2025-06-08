using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private PlayerHealthSystem healthSystem;

    
    private void Start()
    {
        healthSystem = Object.FindFirstObjectByType<PlayerHealthSystem>();

        UpdateHealthUI(healthSystem.MaxHealth, healthSystem.MaxHealth);

        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged += UpdateHealthUI;
        }
    }

    private void UpdateHealthUI(int current, int max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
    }

    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged -= UpdateHealthUI;
        }
    }
}
