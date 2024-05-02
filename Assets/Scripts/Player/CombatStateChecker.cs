using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateChecker : MonoBehaviour
{
    [SerializeField]
    private LayerMask _targetLayer;
    private SphereCollider _sphereCollider;
    private bool _isCombat;

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (_isCombat)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _sphereCollider.radius, _targetLayer);
            if (colliders.Length == 0)
            {
                AudioManager.Instance.StopOverrideBGM();
                _isCombat = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (_isCombat) return;
            if (other.TryGetComponent(out Boss boss)) return;
            if (AudioManager.Instance.currentBgm == "Boss") return;

            AudioManager.Instance.PlayOverrideBGM("Combat");
            _isCombat = true;
        }
    }
}
