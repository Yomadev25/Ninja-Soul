using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IDamageDealer
{
    [SerializeField]
    private LayerMask targetLayer;
    [SerializeField]
    private float weaponLength;
    [SerializeField]
    private float weaponDamage;

    bool canDealDamage;
    List<IDamageable> dealtTargets = new List<IDamageable>();

    private void Start()
    {
        canDealDamage = false;
    }

    private void Update()
    {
        if (canDealDamage)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, targetLayer))
            {
                var target = hit.transform.GetComponent<IDamageable>();
                var hitDir = hit.point - transform.position;

                if (target != null)
                {
                    if (!dealtTargets.Contains(target))
                    {
                        DealDamage(target, weaponDamage);
                        dealtTargets.Add(target);
                    }
                }            
            }        
        }
    }

    public void DealDamage(IDamageable target, float damage)
    {
        Debug.Log($"Deal {damage} damages");
        target.TakeDamage(damage);
    }

    public void StartDealDamage(float damageAdjust = 0f)
    {
        canDealDamage = true;
        dealtTargets.Clear();
    }

    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
