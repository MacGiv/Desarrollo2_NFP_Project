using UnityEngine;

public class PlayerComboHandler : MonoBehaviour
{
    private float resetTime;
    private float comboTimer;
    private int currentComboIndex;
    private PlayerData data;

    public int CurrentComboIndex => currentComboIndex;

    private void OnEnable()
    {
        currentComboIndex = 0;
        data = GetComponent<PlayerCore>().Data;
        resetTime = data.ComboResetTime;
        ResetCombo();
    }

    private void Update()
    {
        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0f && GetComponent<PlayerCore>().StateMachine.CurrentState.GetType() != typeof(PlayerAttackState))
            {
                ResetCombo();
            }
        }
    }

    public void AdvanceCombo()
    {
        currentComboIndex = Mathf.Min(currentComboIndex + 1, data.ComboMaxLength);
        comboTimer = resetTime;
        Debug.Log("[AdvanceCombo] Combo Index: " + currentComboIndex);
    }

    public void ResetCombo()
    {
        currentComboIndex = 1;
        comboTimer = 0f;
        GetComponent<PlayerCore>()?.Animator.SetInteger("comboStep", currentComboIndex);
        Debug.Log("[ResetCombo] Combo reseted!" + currentComboIndex);
    }

    public void StartComboTimer() => comboTimer = resetTime;
}
