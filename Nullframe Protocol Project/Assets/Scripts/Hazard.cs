using UnityEngine;

public class Hazard : MonoBehaviour
{
    int hazardDamage = 50;
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealthSystem playerHp = other.gameObject.GetComponent<PlayerHealthSystem>();
        playerHp.TakeDamage(hazardDamage);
    }
}
