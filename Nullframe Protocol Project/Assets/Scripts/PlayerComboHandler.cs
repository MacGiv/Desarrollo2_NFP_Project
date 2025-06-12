using UnityEngine;

public class PlayerComboHandler : MonoBehaviour
{
    private float resetTime;
    private float comboTimer;
    [SerializeField] private float currentComboIndex;

    public float CurrentComboIndex => currentComboIndex;

    private void OnEnable()
    {
        resetTime = GetComponent<PlayerCore>().Data.ComboResetTime;
    }

    private void Update()
    {
        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0f || currentComboIndex >= 3)
            {
                ResetCombo();
            }
        }
    }

    public void AdvanceCombo()
    {
        currentComboIndex++;
        comboTimer = resetTime;
    }

    public void ResetCombo()
    {
        currentComboIndex = 0;
        comboTimer = 0f;
    }
}
