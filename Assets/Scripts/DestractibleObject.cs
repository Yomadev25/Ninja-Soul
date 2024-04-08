using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestractibleObject : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float _hp;

    [Header("Loot")]
    [SerializeField]
    private GameObject[] _lootItems;
    [SerializeField]
    private int _lootCount;

    [Header("Effects")]
    [SerializeField]
    private GameObject _hitFx;
    [SerializeField]
    private GameObject _destroyFx;

    public void TakeDamage(float damage, GameObject effect = null, bool impact = false)
    {
        if (_hp <= 0) return;
        _hp--;

        if (_hitFx != null)
            Instantiate(_hitFx, transform.position, Quaternion.identity);

        if (_hp <= 0)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        if (_destroyFx != null)
            Instantiate(_destroyFx, transform.position, Quaternion.identity);

        for (int i = 0; i < _lootCount; i++)
        {
            GameObject item = _lootItems[Random.Range(0, _lootItems.Length)];
            Instantiate(item, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
