using UnityEngine;

/// <summary>
/// Automatically charges the player's energy to full if they enter the trigger area.
/// Can be used in tutorials or hidden in levels as emergency recovery.
/// </summary>
public class EnergyCharger : MonoBehaviour
{
    [SerializeField] private bool onlyOnce = false;
    [SerializeField] private bool hideAfterUse = false;

    private bool _alreadyUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyUsed) return;

        if (other.CompareTag("Player"))
        {
            var chargeSystem = other.GetComponent<SpecialAttackChargeSystem>();
            if (chargeSystem != null && !chargeSystem.CanUseSpecial)
            {
                while (!chargeSystem.CanUseSpecial)
                {
                    chargeSystem.AddCharge();
                }

                Debug.Log("[EnergyCharger] Special energy fully recharged!");

                if (onlyOnce)
                    _alreadyUsed = true;

                if (hideAfterUse)
                    gameObject.SetActive(false);
            }
        }
    }
}
