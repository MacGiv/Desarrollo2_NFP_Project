using UnityEngine;

public class PlayerComboHandler : MonoBehaviour
{
    private float resetTime;
    private float comboTimer;
    private int currentComboIndex;

    public int CurrentComboIndex => currentComboIndex;

    private void OnEnable()
    {
        currentComboIndex = 0;
        resetTime = GetComponent<PlayerCore>().Data.ComboResetTime;
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
        currentComboIndex++;
        comboTimer = resetTime;
        Debug.Log("AdvanceCombo() called. Index: " + currentComboIndex);
    }

    public void ResetCombo()
    {
        currentComboIndex = 1;
        comboTimer = 0f;
        GetComponent<PlayerCore>().Animator.SetInteger("comboStep", 1);
        Debug.Log("Combo reseted! Index:" + currentComboIndex);
    }

    public void StartComboTimer() => comboTimer = resetTime;
}
