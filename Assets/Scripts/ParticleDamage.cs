using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public string targetTag;
    public float damage;

    private ParticleSystem particle;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(targetTag))
        {
            if (targetTag == "Player")
            {
                if (other.TryGetComponent<PlayerManager>(out PlayerManager player))
                {
                    player.TakeDamage(1);
                }
            }
            else if (targetTag == "Enemy")
            {
                if (other.TryGetComponent<EnemyManager>(out EnemyManager enemy))
                {
                    enemy.TakeDamage(damage);
                }
            }

            Destroy(gameObject);
        }
    }
}
