using UnityEngine;

public interface IDamageDealer
{
    void DealDamage(IDamageable target, float damage);
}
