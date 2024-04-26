using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleDamage : MonoBehaviour
{
    public string targetTag;
    public float damage;

    public UnityEvent<PlayerManager> onTakePlayerDamage;
    public UnityEvent<EnemyManager> onTakeEnemyDamage;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(targetTag))
        {
            if (targetTag == "Player")
            {
                if (other.TryGetComponent(out PlayerManager player))
                {
                    player.TakeDamage(1);
                    onTakePlayerDamage?.Invoke(player);
                    Destroy(gameObject);
                }
            }
            else if (targetTag == "Enemy")
            {
                if (other.TryGetComponent(out EnemyManager enemy))
                {
                    enemy.TakeDamage(damage);
                    onTakeEnemyDamage?.Invoke(enemy);
                    Destroy(gameObject);
                }
            }          
        }
    }
}
