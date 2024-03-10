using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IDamageDealer
{
    [Header("Weapon Properties")]
    [SerializeField]
    private string _weaponName;
    [SerializeField]
    private float _weaponLength;
    [SerializeField]
    private float _weaponDamage;

    [Header("References")]
    [SerializeField]
    private LayerMask _targetLayer;

    bool canDealDamage;
    List<IDamageable> dealtTargets = new List<IDamageable>();

    float damageAdjust;

    public string WeaponName => _weaponName;

    private void Start()
    {
        canDealDamage = false;
    }

    private void Update()
    {
        if (canDealDamage)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -transform.up, out hit, _weaponLength, _targetLayer))
            {
                var target = hit.transform.GetComponent<IDamageable>();
                var hitDir = hit.point - transform.position;

                if (target != null)
                {
                    if (!dealtTargets.Contains(target))
                    {
                        DealDamage(target, _weaponDamage + damageAdjust);
                        dealtTargets.Add(target);
                    }
                }            
            }        
        }
    }

    public void DealDamage(IDamageable target, float damage)
    {
        Debug.Log($"Dealing {damage} damages.");
        target.TakeDamage(damage);
    }

    public void StartDealDamage(float damageAdjust = 0f)
    {
        this.damageAdjust = damageAdjust;

        canDealDamage = true;
        dealtTargets.Clear();
    }

    public void EndDealDamage()
    {
        this.damageAdjust = 0f;
        canDealDamage = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * _weaponLength);
    }
}
