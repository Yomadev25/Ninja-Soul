using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentFactory : MonoBehaviour
{
    [SerializeField]
    private Weapon _weapon;

    void Start()
    {
        if (_weapon == null)
        {
            Debug.LogError("This character doesn't equip weapon");
        }
    }

    public void StartDealWeaponDamage()
    {
        _weapon.StartDealDamage();
    }

    public void EndDealWeaponDamage()
    {
        _weapon.EndDealDamage();
    }
}
