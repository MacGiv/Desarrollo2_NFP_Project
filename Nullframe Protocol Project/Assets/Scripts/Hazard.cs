using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private int hazardDamage = 50;
    [SerializeField] private Transform repositionPlayerPos;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealthSystem playerHp = other.gameObject.GetComponent<PlayerHealthSystem>();
        playerHp.TakeDamage(hazardDamage);
        other.gameObject.transform.position = repositionPlayerPos.position;
    }
}
