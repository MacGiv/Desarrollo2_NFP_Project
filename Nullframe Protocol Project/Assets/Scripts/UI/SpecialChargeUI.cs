using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates the HUD to reflect current special attack charges.
/// </summary>
public class SpecialChargeUI : MonoBehaviour
{
    [Tooltip("Icons for each charge slot")]
    [SerializeField] private Image[] chargeIcons;

    [Tooltip("Color when charge is active")]
    [SerializeField] private Color activeColor = Color.white;

    [Tooltip("Color when charge is inactive")]
    [SerializeField] private Color inactiveColor = Color.gray;

    private void Start()
    {
        var chargeSystem = FindFirstObjectByType<SpecialAttackChargeSystem>();
        if (chargeSystem != null)
        {
            chargeSystem.OnChargeChanged += UpdateChargeIcons;
            UpdateChargeIcons(chargeSystem.CurrentCharges, 3); // Initial update
        }
        else
        {
            Debug.LogWarning("[SpecialChargeUI] No SpecialAttackChargeSystem found.");
        }
    }

    /// <summary>
    /// Visually updates the icons based on current charges.
    /// </summary>
    private void UpdateChargeIcons(int current, int max)
    {
        for (int i = 0; i < chargeIcons.Length; i++)
        {
            chargeIcons[i].color = (i < current)? activeColor : inactiveColor;
        }
    }
}
