using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private int hazardDamage = 50;
    [SerializeField] private GameObject hitParticles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHealthSystem>(out var hp))
        {
            hp.TakeDamage(hazardDamage);

            if (hitParticles != null)
                Instantiate(hitParticles, other.transform.position, Quaternion.identity);
        }
    }
}