using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LastBlade : MonoBehaviour
{
    [SerializeField]
    private GameObject bitePrefab;

    private void Start()
    {
        if (TryGetComponent(out ParticleDamage particleDamage))
        {
            particleDamage.onTakePlayerDamage.AddListener((player) =>
            {
                player.TakeDamage(1f);
                VisualEffect effect = Instantiate(bitePrefab, player.transform.position + Vector3.up, Quaternion.identity).GetComponent<VisualEffect>();
                effect.Play();

                Destroy(effect.gameObject, 1.5f);
            });

            particleDamage.onTakeEnemyDamage.AddListener((enemy) =>
            {
                enemy.TakeDamage(20f);
                VisualEffect effect = Instantiate(bitePrefab, enemy.transform.position + Vector3.up, Quaternion.identity).GetComponent<VisualEffect>();
                effect.Play();

                Destroy(effect.gameObject, 1.5f);
            });
        }
    }
}
